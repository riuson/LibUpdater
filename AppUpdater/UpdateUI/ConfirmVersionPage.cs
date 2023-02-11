using LibUpdater.Data;

namespace AppUpdater.UpdateUI;

public class ConfirmVersionPage : TaskDialogPageWithConfirmation
{
    public ConfirmVersionPage(IActualVersionInfo versionInfo)
    {
        AllowCancel = true;
        AllowMinimize = false;
        Buttons = new TaskDialogButtonCollection { ButtonContinue, ButtonCancel };
        Caption = "Обновление";
        DefaultButton = ButtonContinue;
        Heading = $"Доступна версия {versionInfo.Version}\n{versionInfo.Description}";
        Icon = TaskDialogIcon.Information;
    }
}