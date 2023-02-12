namespace AppUpdater.UpdateUI;

public class ConfirmInstallPage : ConfirmableTaskDialogPage
{
    public ConfirmInstallPage()
    {
        Heading = "Всё готово для установки обновления.";
        Footnote = new TaskDialogFootnote
        {
            Text = "Отменить процесс будет невозможно!",
            Icon = TaskDialogIcon.Warning
        };
        Icon = TaskDialogIcon.Information;
    }
}