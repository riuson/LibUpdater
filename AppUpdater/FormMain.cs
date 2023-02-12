using AppUpdater.UpdateUI;
using LibUpdater;
using LibUpdater.Analysis;
using LibUpdater.Data;
using LibUpdater.Utils;

namespace AppUpdater;

public partial class FormMain : Form
{
    private readonly string _targetDir = Path.Combine(
        Path.GetDirectoryName(Application.ExecutablePath),
        "target");

    private readonly string _tempDir = Path.Combine(
        Path.GetDirectoryName(Application.ExecutablePath),
        "temp");

    private readonly UpdaterAPI _updater = new();

    public FormMain()
    {
        InitializeComponent();

        textBoxTargetDir.Text = _targetDir;

        if (!Directory.Exists(_tempDir))
            Directory.CreateDirectory(_tempDir);

        if (!Directory.Exists(_targetDir))
            Directory.CreateDirectory(_targetDir);

        comboBoxUri.SelectedIndex = 0;
    }

    private void buttonBrowse_Click(object sender, EventArgs e)
    {
        using var dialog = new FolderBrowserDialog();

        if (dialog.ShowDialog(this) == DialogResult.OK) textBoxTargetDir.Text = dialog.SelectedPath;
    }

    private async void buttonGo_Click(object sender, EventArgs e)
    {
        buttonGo.Enabled = false;
        var token = new CancellationTokenSource();

        var options = new UpdateOptions
        {
            TempDir = _tempDir,
            TargetDir = textBoxTargetDir.Text,
            DegreeOfParallelism = 1,
            UpdatesUri = Convert.ToString(comboBoxUri.SelectedItem),
            Token = token.Token
        };
        var updater = new Updater();

        var updateRequestingServerPage = new RequestingIndexPage();

        TaskDialogPage currentPage = updateRequestingServerPage;

        async Task<bool> confirmVersion(IActualVersionInfo versionInfo)
        {
            var decision =
                await updateRequestingServerPage.WaitDecisionAsync(
                    2000);

            if (decision == UserDecisions.Cancel) return false;

            var updateConfirmVersionPage = new ConfirmVersionPage(versionInfo);


            currentPage.Navigate(updateConfirmVersionPage);
            currentPage = updateConfirmVersionPage;

            decision = await updateConfirmVersionPage.WaitDecisionAsync();

            if (decision != UserDecisions.Continue) return false;

            updateRequestingServerPage = new RequestingIndexPage();
            currentPage.Navigate(updateRequestingServerPage);
            currentPage = updateRequestingServerPage;

            return true;
        }

        async Task<bool> confirmIndex(IEnumerable<IArchiveItem> archiveItems)
        {
            var decision =
                await updateRequestingServerPage.WaitDecisionAsync(
                    2000);

            if (decision == UserDecisions.Cancel) return false;

            var updateConfirmIndexPage = new ConfirmIndexPage();
            currentPage.Navigate(updateConfirmIndexPage);
            currentPage = updateConfirmIndexPage;
            decision = await updateConfirmIndexPage.WaitDecisionAsync();
            return decision == UserDecisions.Continue;
        }

        async Task<bool> confirmAnalysis(IAnalysisResult analysisResult)
        {
            var updateConfirmAnalysisPage = new ConfirmAnalysisPage(
                analysisResult);
            currentPage.Navigate(updateConfirmAnalysisPage);
            currentPage = updateConfirmAnalysisPage;
            var decision = await updateConfirmAnalysisPage.WaitDecisionAsync();

            if (decision != UserDecisions.Continue) return false;

            var updateDownloadingArchivesPage = new DownloadingArchivesPage();
            updater.Progress += updateDownloadingArchivesPage.ProgressHandler;

            currentPage.Navigate(updateDownloadingArchivesPage);
            currentPage = updateDownloadingArchivesPage;

            return true;
        }

        async Task<bool> confirmApply(IAnalysisResult analysisResult)
        {
            if (currentPage.BoundDialog is null) return false;

            var confirmInstallPage = new ConfirmInstallPage();
            currentPage.Navigate(confirmInstallPage);
            currentPage = confirmInstallPage;
            var decision = await confirmInstallPage.WaitDecisionAsync();

            if (decision != UserDecisions.Continue) return false;

            var updateApplyChangesPage = new InstallingUpdatesPage();
            updater.Progress += updateApplyChangesPage.ProgressHandler;

            currentPage.Navigate(updateApplyChangesPage);
            currentPage = updateApplyChangesPage;


            return true;
        }

        async Task notifyComplete()
        {
            if (currentPage.BoundDialog is null) return;

            var updateCompletePage = new ResultCompletedPage();

            currentPage.Navigate(updateCompletePage);
            currentPage = updateCompletePage;

            await Task.Delay(1);
        }

        async Task notifyNoChanges()
        {
            if (currentPage.BoundDialog is null) return;

            var updateNoChangesPage = new ResultNoChangesPage();

            currentPage.Navigate(updateNoChangesPage);
            currentPage = updateNoChangesPage;

            await Task.Delay(1);
        }

        async Task notifyCancel()
        {
            if (currentPage.BoundDialog is null) return;

            var updateCancelPage = new ResultCancelledPage();

            currentPage.Navigate(updateCancelPage);
            currentPage = updateCancelPage;

            await Task.Delay(1000);
        }

        var taskUpdate = updater.Update(
            options,
            confirmVersion,
            confirmIndex,
            confirmAnalysis,
            confirmApply,
            notifyComplete,
            notifyCancel,
            notifyNoChanges);

        TaskDialog.ShowDialog(this, updateRequestingServerPage);

        await taskUpdate;

        buttonGo.Enabled = true;
    }
}