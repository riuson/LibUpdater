namespace AppUpdater.UpdateUI;

public class ResultNoChangesPage : ResultPage
{
    public ResultNoChangesPage()
    {
        Heading = "Обновление не требуется.";
        Icon = TaskDialogIcon.Information;
    }
}