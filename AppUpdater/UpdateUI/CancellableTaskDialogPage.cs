namespace AppUpdater.UpdateUI;

public class CancellableTaskDialogPage : TaskDialogPage
{
    protected readonly TaskDialogButton _buttonCancel;
    protected readonly AutoResetEvent _eventDecision;
    protected UserDecisions _decision = UserDecisions.None;

    public CancellableTaskDialogPage()
    {
        _eventDecision = new AutoResetEvent(false);
        _buttonCancel = new TaskDialogButton("Отменить", true, false);
        _buttonCancel.Click += (sender, args) =>
        {
            _decision = UserDecisions.Cancel;
            _eventDecision.Set();
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
        await Task.Run(() => _eventDecision.WaitOne(milliseconds));
        return _decision;
    }
}