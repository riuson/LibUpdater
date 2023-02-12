using LibUpdater.Data;

namespace AppUpdater.UpdateUI;

public class DownloadingArchivesPage : CancellableTaskDialogPage
{
    private readonly IProgress<ProgressEventArgs> _progress;

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
    }

    public void ProgressHandler(object? sender, ProgressEventArgs e)
    {
        _progress.Report(e);
    }
}