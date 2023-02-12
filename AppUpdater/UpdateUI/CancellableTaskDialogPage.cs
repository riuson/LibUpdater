namespace AppUpdater.UpdateUI;

public class CancellableTaskDialogPage : TaskDialogPage
{
    protected readonly TaskDialogButton _buttonCancel;
    protected readonly SemaphoreSlim _semaphoreDecision;
    protected UserDecisions _decision = UserDecisions.None;

    public CancellableTaskDialogPage()
    {
        _semaphoreDecision = new SemaphoreSlim(0, 1);
        _buttonCancel = new TaskDialogButton("Отменить", true, false);
        _buttonCancel.Click += (sender, args) =>
        {
            _decision = UserDecisions.Cancel;
            _semaphoreDecision.Release();
        };

        AllowCancel = false;
        AllowMinimize = false;
        Buttons = new TaskDialogButtonCollection { _buttonCancel };
        Caption = "Обновление";
        DefaultButton = _buttonCancel;
        //ProgressBar = new TaskDialogProgressBar(TaskDialogProgressBarState.None);
    }

    public async Task<UserDecisions> WaitDecisionAsync(
        int milliseconds = int.MaxValue)
    {
        await _semaphoreDecision.WaitAsync(milliseconds);
        return _decision;
    }
}