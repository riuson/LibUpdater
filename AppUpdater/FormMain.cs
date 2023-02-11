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

    private void buttonGo_Click(object sender, EventArgs e)
    {
        if (!Directory.Exists(textBoxTargetDir.Text)) return;

        var options = new UpdateOptions
        {
            TempDir = _tempDir,
            TargetDir = textBoxTargetDir.Text,
            DegreeOfParallelism = 1,
            UpdatesUri = Convert.ToString(comboBoxUri.SelectedItem)
        };
        var scanner = new TreeScanner();
        var analyzer = new TreeAnalyzer();

        var latestVersion = _updater.GetLatestVersion(options);

        var archiveItems = _updater.GetIndex(options, latestVersion);

        var localItems = scanner.ScanTree(options.TargetDir, options.DegreeOfParallelism);

        var analyzed = analyzer.Analyze(options.TargetDir, localItems, archiveItems);

        _updater.GetArchiveItems(options, latestVersion, archiveItems);

        _updater.CleanupObsoleteItems(options, analyzed.Obsolete);

        _updater.ApplyArchiveItems(options, archiveItems);
    }

    private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (Directory.Exists(_tempDir))
            Directory.Delete(_tempDir, true);
    }
}