namespace AppUpdater.UpdateUI;

public class CompletedPage : TaskDialogPage
{
    public CompletedPage()
    {
        AllowCancel = true;
        AllowMinimize = false;
        Buttons = new TaskDialogButtonCollection { TaskDialogButton.OK };
        Caption = "Обновление";
        DefaultButton = TaskDialogButton.OK;
        Heading = "Установка обновлений завершена.";
        Icon = TaskDialogIcon.Information;
    }
}