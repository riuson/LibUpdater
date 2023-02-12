using LibUpdater.Data;

namespace AppUpdater.UpdateUI;

public class ConfirmVersionPage : ConfirmableTaskDialogPage
{
    public ConfirmVersionPage(IActualVersionInfo versionInfo)
    {
        Heading = $"Доступна версия {versionInfo.Version}\n{versionInfo.Description}";
        Icon = TaskDialogIcon.Information;
    }
}