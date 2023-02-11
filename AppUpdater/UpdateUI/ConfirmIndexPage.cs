namespace AppUpdater.UpdateUI;

public class ConfirmIndexPage : TaskDialogPageWithConfirmation
{
    public ConfirmIndexPage()
    {
        AllowCancel = true;
        AllowMinimize = false;
        Buttons = new TaskDialogButtonCollection { ButtonContinue, ButtonCancel };
        Caption = "Обновление";
        DefaultButton = ButtonContinue;
        Heading = "Получен перечень файлов с сервера.\nТребуется сравнить с локальными файлами.";
        Icon = TaskDialogIcon.Information;
    }
}