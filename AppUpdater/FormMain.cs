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
            var updateConfirmVersionPage = new ConfirmVersionPage(versionInfo);


#if DEBUG
            // To illustrate the process.
            await Task.Delay(2000);
#endif

            currentPage.Navigate(updateConfirmVersionPage);
            currentPage = updateConfirmVersionPage;

            await updateConfirmVersionPage.WaitDecisionAsync();

            currentPage.Navigate(updateRequestingServerPage);
            currentPage = updateRequestingServerPage;

#if DEBUG
            // To illustrate the process.
            await Task.Delay(2000);
#endif

            return updateConfirmVersionPage.Confirm;
        }

        async Task<bool> confirmIndex(IEnumerable<IArchiveItem> archiveItems)
        {
            var updateConfirmIndexPage = new ConfirmIndexPage();
            currentPage.Navigate(updateConfirmIndexPage);
            currentPage = updateConfirmIndexPage;
            await updateConfirmIndexPage.WaitDecisionAsync();
            return updateConfirmIndexPage.Confirm;
        }

        async Task<bool> confirmAnalysis(IAnalysisResult analysisResult)
        {
            var updateConfirmAnalysisPage = new ConfirmAnalysisPage(
                analysisResult);
            currentPage.Navigate(updateConfirmAnalysisPage);
            currentPage = updateConfirmAnalysisPage;
            await updateConfirmAnalysisPage.WaitDecisionAsync();

            var updateDownloadingArchivesPage = new DownloadingArchivesPage();
            updater.Progress += updateDownloadingArchivesPage.ProgressHandler;

            currentPage.Navigate(updateDownloadingArchivesPage);
            currentPage = updateDownloadingArchivesPage;

            return updateConfirmAnalysisPage.Confirm;
        }

        async Task<bool> confirmApply(IAnalysisResult analysisResult)
        {
            var confirmInstallPage = new ConfirmInstallPage();
            currentPage.Navigate(confirmInstallPage);
            currentPage = confirmInstallPage;
            await confirmInstallPage.WaitDecisionAsync();


            var updateApplyChangesPage = new InstallingUpdatesPage();
            updater.Progress += updateApplyChangesPage.ProgressHandler;

            currentPage.Navigate(updateApplyChangesPage);
            currentPage = updateApplyChangesPage;


            return confirmInstallPage.Confirm;
        }

        async Task confirmComplete()
        {
            var updateCompletePage = new CompletedPage();

            currentPage.Navigate(updateCompletePage);
            currentPage = updateCompletePage;

            await Task.Delay(1);
        }

        var taskUpdate = updater.Update(
            options,
            confirmVersion,
            confirmIndex,
            confirmAnalysis,
            confirmApply,
            confirmComplete);

        TaskDialog.ShowDialog(this, updateRequestingServerPage);

        await taskUpdate;

        buttonGo.Enabled = true;
    }
}