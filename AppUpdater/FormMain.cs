using LibUpdater;
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

        await updater.Update(
            options,
            _ => true,
            _ => true,
            _ => true,
            _ => true);

        buttonGo.Enabled = true;
    }

    private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (Directory.Exists(_tempDir))
            Directory.Delete(_tempDir, true);
    }
}