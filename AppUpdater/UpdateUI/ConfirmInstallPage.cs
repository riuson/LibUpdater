namespace AppUpdater.UpdateUI;

public class ConfirmInstallPage : TaskDialogPageWithConfirmation
{
    public ConfirmInstallPage()
    {
        AllowCancel = true;
        AllowMinimize = false;
        Buttons = new TaskDialogButtonCollection { ButtonContinue, ButtonCancel };
        Caption = "Обновление";
        DefaultButton = ButtonContinue;
        Heading = "Далее будет произведена установка обновления.\nОтменить процесс будет невозможно.";
        Icon = TaskDialogIcon.Warning;
    }
}