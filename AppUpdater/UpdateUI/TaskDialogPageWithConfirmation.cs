namespace AppUpdater.UpdateUI;

public class TaskDialogPageWithConfirmation : TaskDialogPage
{
    private readonly SemaphoreSlim _semaphore;

    public TaskDialogPageWithConfirmation()
    {
        _semaphore = new SemaphoreSlim(0, 1);
        Confirm = false;

        ButtonContinue = new TaskDialogButton("Продолжить", true, false);
        ButtonCancel = new TaskDialogButton("Отменить");
        ButtonContinue.Click += (sender, args) =>
        {
            Confirm = true;
            _semaphore.Release();
        };
        ButtonCancel.Click += (sender, args) =>
        {
            Confirm = false;
            _semaphore.Release();
        };
    }

    public TaskDialogButton ButtonContinue { get; }
    public TaskDialogButton ButtonCancel { get; }

    public bool Confirm { get; private set; }

    public Task WaitDecisionAsync()
    {
        return _semaphore.WaitAsync();
    }
}