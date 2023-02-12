namespace AppUpdater.UpdateUI;

public class ConfirmableTaskDialogPage : CancellableTaskDialogPage
{
    protected readonly TaskDialogButton _buttonContinue;

    public ConfirmableTaskDialogPage()
    {
        _buttonContinue = new TaskDialogButton("Продолжить", true, false);
        _buttonContinue.Click += (sender, args) =>
        {
            _decision = UserDecisions.Continue;
            _semaphoreDecision.Release();
        };
        Buttons.Clear();
        Buttons.Add(_buttonContinue);
        Buttons.Add(_buttonCancel);
    }
}