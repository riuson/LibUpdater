namespace AppUpdater.UpdateUI;

public class ResultNoChangesPage : TaskDialogPage
{
    public ResultNoChangesPage()
    {
        AllowCancel = true;
        AllowMinimize = false;
        Buttons = new TaskDialogButtonCollection { TaskDialogButton.OK };
        DefaultButton = TaskDialogButton.OK;
        Heading = "Обновление не требуется.";
        Icon = TaskDialogIcon.Information;
    }
}