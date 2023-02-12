namespace AppUpdater.UpdateUI;

public class ResultCancelledPage : TaskDialogPage
{
    public ResultCancelledPage()
    {
        AllowCancel = true;
        Buttons = new TaskDialogButtonCollection { TaskDialogButton.OK };
        DefaultButton = TaskDialogButton.OK;
        Heading = "Установка обновлений отменена.";
        Icon = TaskDialogIcon.Error;
    }
}