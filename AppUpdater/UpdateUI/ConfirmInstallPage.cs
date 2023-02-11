namespace AppUpdater.UpdateUI;

public class ConfirmInstallPage : TaskDialogPage
{
    public ConfirmInstallPage(
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
        Heading = "Далее будет произведена установка обновления.\nОтменить процесс будет невозможно.";
        Icon = TaskDialogIcon.Warning;
    }

    public TaskDialogButton ButtonContinue { get; }
    public TaskDialogButton ButtonCancel { get; }
}