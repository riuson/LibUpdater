namespace AppUpdater.UpdateUI;

public class RequestingIndexPage : TaskDialogPage
{
    public RequestingIndexPage()
    {
        AllowCancel = true;
        AllowMinimize = false;
        Buttons = new TaskDialogButtonCollection { TaskDialogButton.Cancel };
        Caption = "Обновление";
        DefaultButton = TaskDialogButton.Cancel;
        Heading = "Производится запрос на сервер обновлений...";
        Icon = TaskDialogIcon.Information;
        ProgressBar = new TaskDialogProgressBar(TaskDialogProgressBarState.Marquee);
    }
}