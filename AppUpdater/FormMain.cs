using LibUpdater;
using LibUpdater.Data;
using LibUpdater.Tests.Utils;
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

        var options = new UpdateOptions
        {
            TempDir = _tempDir,
            TargetDir = textBoxTargetDir.Text,
            DegreeOfParallelism = 1,
            UpdatesUri = Convert.ToString(comboBoxUri.SelectedItem)
        };
        var updater = new Updater();

        var updateInitialPage = new TaskDialogPage
        {
            AllowCancel = true,
            AllowMinimize = false,
            Buttons = new TaskDialogButtonCollection { TaskDialogButton.Cancel },
            Caption = "Обновление",
            DefaultButton = TaskDialogButton.Cancel,
            Heading = "Производится запрос на сервер обновлений...",
            Icon = TaskDialogIcon.Information,
            ProgressBar = new TaskDialogProgressBar(TaskDialogProgressBarState.Marquee)
        };

        var currentPage = updateInitialPage;

        async Task<bool> confirmVersion(IActualVersionInfo versionInfo)
        {
            var buttonContinue = new TaskDialogButton("Продолжить", true, false);
            var buttonCancel = new TaskDialogButton("Отменить");
            var updateConfirmVersionPage = new TaskDialogPage
            {
                AllowCancel = true,
                AllowMinimize = false,
                Buttons = new TaskDialogButtonCollection { buttonContinue, buttonCancel },
                Caption = "Обновление",
                DefaultButton = buttonContinue,
                Heading = $"Доступна версия {versionInfo.Version}\n{versionInfo.Description}",
                Icon = TaskDialogIcon.Information
            };

            var confirm = false;
            var semaphoreConfirm = new SemaphoreSlim(0, 1);
            buttonContinue.Click += (o, args) =>
            {
                confirm = true;
                semaphoreConfirm.Release();
            };
            buttonCancel.Click += (o, args) => { semaphoreConfirm.Release(); };
            currentPage.Navigate(updateConfirmVersionPage);
            currentPage = updateConfirmVersionPage;
            await semaphoreConfirm.WaitAsync();
            return confirm;
        }

        async Task<bool> confirmIndex(IEnumerable<IArchiveItem> archiveItems)
        {
            var buttonContinue = new TaskDialogButton("Продолжить", true, false);
            var buttonCancel = new TaskDialogButton("Отменить");
            var updateConfirmIndexPage = new TaskDialogPage
            {
                AllowCancel = true,
                AllowMinimize = false,
                Buttons = new TaskDialogButtonCollection { buttonContinue, buttonCancel },
                Caption = "Обновление",
                DefaultButton = buttonContinue,
                Heading = "Получен перечень файлов с сервера.\nТребуется сравнить с локальными файлами.",
                Icon = TaskDialogIcon.Information
            };

            var confirm = false;
            var semaphoreConfirm = new SemaphoreSlim(0, 1);
            buttonContinue.Click += (o, args) =>
            {
                confirm = true;
                semaphoreConfirm.Release();
            };
            buttonCancel.Click += (o, args) => { semaphoreConfirm.Release(); };
            currentPage.Navigate(updateConfirmIndexPage);
            currentPage = updateConfirmIndexPage;
            await semaphoreConfirm.WaitAsync();
            return confirm;
        }

        async Task<bool> confirmAnalysis(IAnalyzeResult analysisResult)
        {
            var buttonContinue = new TaskDialogButton("Продолжить", true, false);
            var buttonCancel = new TaskDialogButton("Отменить");
            var updateConfirmAnalysisPage = new TaskDialogPage
            {
                AllowCancel = true,
                AllowMinimize = false,
                Buttons = new TaskDialogButtonCollection { buttonContinue, buttonCancel },
                Caption = "Обновление",
                DefaultButton = buttonContinue,
                Expander = new TaskDialogExpander
                {
                    CollapsedButtonText = "Подробная информация",
                    ExpandedButtonText = "Скрыть",
                    Expanded = false,
                    Position = TaskDialogExpanderPosition.AfterText,
                    Text = string.Format(
                        "Новых файлов: {0}\n" +
                        "Устаревших файлов: {1}\n" +
                        "К загрузке: {2:n0} байт\n" +
                        "К распаковке: {3:n0} байт\n" +
                        "К удалению: {4:n0} байт\n" +
                        "Баланс: {5:n0} байт\n",
                        analysisResult.FilesToAddCount,
                        analysisResult.FilesToRemoveCount,
                        analysisResult.BytesToDownload,
                        analysisResult.BytesToAdd,
                        analysisResult.BytesToRemove,
                        analysisResult.BytesChanged)
                },
                Heading = $"Потребуется загрузить {analysisResult.BytesToDownload:n0} байт.",
                Icon = TaskDialogIcon.Information
            };

            var confirm = false;
            var semaphoreConfirm = new SemaphoreSlim(0, 1);
            buttonContinue.Click += (o, args) =>
            {
                confirm = true;
                semaphoreConfirm.Release();
            };
            buttonCancel.Click += (o, args) => { semaphoreConfirm.Release(); };
            currentPage.Navigate(updateConfirmAnalysisPage);
            currentPage = updateConfirmAnalysisPage;
            await semaphoreConfirm.WaitAsync();
            return confirm;
        }

        async Task<bool> confirmApply(IAnalyzeResult analysisResult)
        {
            var buttonContinue = new TaskDialogButton("Продолжить", true, false);
            var buttonCancel = new TaskDialogButton("Отменить");
            var updateConfirmAnalysisPage = new TaskDialogPage
            {
                AllowCancel = true,
                AllowMinimize = false,
                Buttons = new TaskDialogButtonCollection { buttonContinue, buttonCancel },
                Caption = "Обновление",
                DefaultButton = buttonContinue,
                Heading = "Далее будет произведена установка обновления. Отменить процесс будет невозможно.",
                Icon = TaskDialogIcon.Information
            };

            var confirm = false;
            var semaphoreConfirm = new SemaphoreSlim(0, 1);
            buttonContinue.Click += (o, args) =>
            {
                confirm = true;
                semaphoreConfirm.Release();
            };
            buttonCancel.Click += (o, args) => { semaphoreConfirm.Release(); };
            currentPage.Navigate(updateConfirmAnalysisPage);
            currentPage = updateConfirmAnalysisPage;
            await semaphoreConfirm.WaitAsync();
            return confirm;
        }

        var taskUpdate = updater.Update(
            options,
            confirmVersion,
            confirmIndex,
            confirmAnalysis,
            confirmApply);

        TaskDialog.ShowDialog(this, updateInitialPage);

        buttonGo.Enabled = true;
    }

    private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (Directory.Exists(_tempDir))
            Directory.Delete(_tempDir, true);
    }
}