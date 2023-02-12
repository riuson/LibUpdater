namespace AppUpdater.UpdateUI;

public class ResultErrorPage : ResultPage
{
    public ResultErrorPage(Exception exc)
    {
        Heading = "Возникла ошибка.";
        Text = exc.Message;
        Expander = new TaskDialogExpander
        {
            CollapsedButtonText = "Подробнее",
            ExpandedButtonText = "Скрыть",
            Expanded = false,
            Position = TaskDialogExpanderPosition.AfterText,
            Text = exc.StackTrace
        };
        Icon = TaskDialogIcon.Error;
    }
}