using LibUpdater.Data;

namespace AppUpdater.UpdateUI;

public class InstallingUpdatesPage : TaskDialogPage
{
    private readonly IProgress<ProgressEventArgs> _progress;

    public InstallingUpdatesPage()
    {
        AllowCancel = true;
        AllowMinimize = false;
        Buttons = new TaskDialogButtonCollection { TaskDialogButton.Cancel };
        Caption = "Обновление";
        DefaultButton = TaskDialogButton.Cancel;
        Heading = "Производится установка обновлений...";
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