using LibUpdater.Data;

namespace AppUpdater.UpdateUI;

public class ConfirmVersionPage : TaskDialogPage
{
    public ConfirmVersionPage(
        IActualVersionInfo versionInfo,
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
        Heading = $"Доступна версия {versionInfo.Version}\n{versionInfo.Description}";
        Icon = TaskDialogIcon.Information;
    }

    public TaskDialogButton ButtonContinue { get; }
    public TaskDialogButton ButtonCancel { get; }
}