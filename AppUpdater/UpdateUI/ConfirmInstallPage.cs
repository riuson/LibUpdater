namespace AppUpdater.UpdateUI;

public class ConfirmInstallPage : ConfirmableTaskDialogPage
{
    public ConfirmInstallPage()
    {
        Heading = "Всё готово для установки обновления.";
        Footnote = "Отменить процесс будет невозможно!";
        Icon = TaskDialogIcon.Warning;
    }
}