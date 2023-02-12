namespace AppUpdater.UpdateUI;

public class ResultCancelledPage : ResultPage
{
    public ResultCancelledPage()
    {
        Heading = "Установка обновлений отменена.";
        Icon = TaskDialogIcon.Error;
    }
}