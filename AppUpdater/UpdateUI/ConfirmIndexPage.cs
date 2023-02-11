namespace AppUpdater.UpdateUI;

public class ConfirmIndexPage : TaskDialogPage
{
    public ConfirmIndexPage(
        EventHandler continueHandler,
        EventHandler cancelHandler)
    {
        ButtonContinue = new TaskDialogButton("Продолжить", true, false);
        ButtonCancel = new TaskDialogButton("Отменить");
        ButtonContinue.Click += continueHandler;
        ButtonCancel.Click += cancelHandler;

        AllowCancel = true;
        AllowMinimize = false;
        Buttons = new TaskDialogButtonCollection { ButtonContinue, ButtonCancel };
        Caption = "Обновление";
        DefaultButton = ButtonContinue;
        Heading = "Получен перечень файлов с сервера.\nТребуется сравнить с локальными файлами.";
        Icon = TaskDialogIcon.Information;
    }

    public TaskDialogButton ButtonContinue { get; }
    public TaskDialogButton ButtonCancel { get; }
}