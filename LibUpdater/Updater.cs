using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibUpdater.Analysis;
using LibUpdater.Data;
using LibUpdater.Utils;

namespace LibUpdater;

public class Updater
{
    public async Task Update(
        UpdateOptions options,
        Func<IActualVersionInfo, Task<bool>> confirmVersion,
        Func<IEnumerable<IArchiveItem>, Task<bool>> confirmIndex,
        Func<IAnalysisResult, Task<bool>> confirmAnalysis,
        Func<IAnalysisResult, Task<bool>> confirmApply,
        Func<Task> notifyComplete,
        Func<Task> notifyCancel,
        Func<Task> notifyNoChanges,
        Func<Exception, Task> notifyError)
    {
        var api = new UpdaterAPI();
        api.Progress += OnProgress;

        try
        {
            var versionInfo = await api.GetActualVersionAsync(options);

            if (!await confirmVersion(versionInfo))
            {
                await notifyCancel();
                return;
            }

            var indexItems = await api.GetIndexAsync(options, versionInfo.Path);

            if (!await confirmIndex(indexItems))
            {
                await notifyCancel();
                return;
            }

            var scanner = new TreeScanner();
            var localItems = await scanner.ScanTreeAsync(
                options.TargetDir,
                options.Token,
                options.DegreeOfParallelism);

            var analyzer = new TreeAnalyzer();
            var analysisResult = analyzer.Analyze(
                options.TargetDir,
                localItems,
                indexItems);

            if (!analysisResult.IsEquals)
            {
                if (!await confirmAnalysis(analysisResult))
                {
                    await notifyCancel();
                    return;
                }

                await api.GetArchiveItemsAsync(options, versionInfo.Path, analysisResult.Added);

                if (!await confirmApply(analysisResult))
                {
                    await notifyCancel();
                    return;
                }

                await api.CleanupObsoleteItemsAsync(options, analysisResult.Obsolete);

                await api.ApplyArchiveItemsAsync(options, analysisResult.Added);
            }
            else
            {
                await notifyNoChanges();
            }

            await notifyComplete();
        }
        catch (OperationCanceledException)
        {
            if (options.Token.IsCancellationRequested)
            {
                await notifyCancel();
            }
        }
        catch (Exception exc)
        {
            await notifyError(exc);
        }
        finally
        {
            var remover = new Remover();
            await remover.RemoveChildsAsync(options.TempDir);
        }

        api.Progress -= OnProgress;
    }

    public event EventHandler<ProgressEventArgs> Progress;

    private void OnProgress(object sender, ProgressEventArgs e)
    {
        Progress?.Invoke(this, e);
    }
}