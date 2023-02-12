namespace AppUpdater.UpdateUI;

public class ConfirmIndexPage : ConfirmableTaskDialogPage
{
    public ConfirmIndexPage()
    {
        DefaultButton = _buttonContinue;
        Heading = "Получен перечень файлов с сервера.\nТребуется сравнить с локальными файлами.";
        Icon = TaskDialogIcon.Information;
    }
}