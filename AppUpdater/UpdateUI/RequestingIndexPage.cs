namespace AppUpdater.UpdateUI;

public class RequestingIndexPage : CancellableTaskDialogPage
{
    public RequestingIndexPage()
    {
        Heading = "Производится запрос на сервер обновлений...";
        Icon = TaskDialogIcon.Information;
        ProgressBar = new TaskDialogProgressBar(TaskDialogProgressBarState.Marquee);
    }
}