using LibUpdater.Data;

namespace AppUpdater.UpdateUI;

public class DownloadingArchivesPage : CancellableTaskDialogPage
{
    private readonly EventHandler _cancelHandler;
    private readonly IProgress<ProgressEventArgs> _progress;

    public DownloadingArchivesPage(EventHandler cancelHandler)
        : this()
    {
        _cancelHandler = cancelHandler;
    }

    public DownloadingArchivesPage()
    {
        Heading = "Производится загрузка обновлений...";
        Icon = TaskDialogIcon.Information;
        ProgressBar = new TaskDialogProgressBar
        {
            Minimum = 0,
            Maximum = 100,
            Value = 0,
            State = TaskDialogProgressBarState.Normal
        };

        var noMoreUpdates = false;

        _progress = new Progress<ProgressEventArgs>(args =>
        {
            try
            {
                if (!noMoreUpdates)
                {
                    ProgressBar.Value = args.Percentage;
                    Text = $"{args.Current:n0} / {args.Total:n0}";
                }
            }
            catch
            {
                noMoreUpdates = true;
            }
        });

        _buttonCancel.Click += buttonCancel_Click;
    }

    private void buttonCancel_Click(object? sender, EventArgs e)
    {
        _cancelHandler?.Invoke(this, EventArgs.Empty);
    }

    public void ProgressHandler(object? sender, ProgressEventArgs e)
    {
        _progress.Report(e);
    }
}