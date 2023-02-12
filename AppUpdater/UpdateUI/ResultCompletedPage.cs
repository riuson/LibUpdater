namespace AppUpdater.UpdateUI;

public class ResultCompletedPage : TaskDialogPage
{
    public ResultCompletedPage()
    {
        AllowCancel = true;
        AllowMinimize = false;
        Buttons = new TaskDialogButtonCollection { TaskDialogButton.OK };
        DefaultButton = TaskDialogButton.OK;
        Heading = "Установка обновлений завершена.";
        Icon = TaskDialogIcon.Information;
    }
}