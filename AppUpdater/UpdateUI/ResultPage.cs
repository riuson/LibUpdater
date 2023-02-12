namespace AppUpdater.UpdateUI;

public class ResultPage : TaskDialogPage
{
    public ResultPage()
    {
        AllowCancel = true;
        AllowMinimize = false;
        Caption = "Обновление";
        Buttons = new TaskDialogButtonCollection { TaskDialogButton.OK };
        DefaultButton = TaskDialogButton.OK;
    }
}