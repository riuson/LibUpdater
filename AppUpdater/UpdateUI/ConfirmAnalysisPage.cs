using LibUpdater.Analysis;

namespace AppUpdater.UpdateUI;

public class ConfirmAnalysisPage : ConfirmableTaskDialogPage
{
    public ConfirmAnalysisPage(IAnalysisResult analysisResult)
    {
        Expander = new TaskDialogExpander
        {
            CollapsedButtonText = "Подробная информация",
            ExpandedButtonText = "Скрыть",
            Expanded = false,
            Position = TaskDialogExpanderPosition.AfterText,
            Text = string.Format(
                "Новых файлов: {0}\n" +
                "Устаревших файлов: {1}\n" +
                "К загрузке: {2:n0} байт\n" +
                "К распаковке: {3:n0} байт\n" +
                "К удалению: {4:n0} байт\n" +
                "Баланс: {5:n0} байт\n",
                analysisResult.FilesToAddCount,
                analysisResult.FilesToRemoveCount,
                analysisResult.BytesToDownload,
                analysisResult.BytesToAdd,
                analysisResult.BytesToRemove,
                analysisResult.BytesChanged)
        };
        Heading = $"Потребуется загрузить {analysisResult.BytesToDownload:n0} байт.";
        Icon = TaskDialogIcon.Information;
    }
}