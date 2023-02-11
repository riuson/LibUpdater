using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibUpdater.Data;
using LibUpdater.Tests.Utils;
using LibUpdater.Utils;

namespace LibUpdater;

public class Updater
{
    public async Task Update(
        UpdateOptions options,
        Func<IActualVersionInfo, Task<bool>> confirmVersion,
        Func<IEnumerable<IArchiveItem>, Task<bool>> confirmIndex,
        Func<IAnalyzeResult, Task<bool>> confirmAnalyzed,
        Func<IAnalyzeResult, Task<bool>> confirmApply)
    {
        var api = new UpdaterAPI();

        var versionInfo = await api.GetActualVersionAsync(options);

        if (!await confirmVersion(versionInfo)) return;

        var indexItems = await api.GetIndexAsync(options, versionInfo.Path);

        if (!await confirmIndex(indexItems)) return;

        var scanner = new TreeScanner();
        var localItems = await scanner.ScanTreeAsync(options.TargetDir, options.DegreeOfParallelism);

        var analyzer = new TreeAnalyzer();
        var analyzed = analyzer.Analyze(
            options.TargetDir,
            localItems,
            indexItems);

        if (!await confirmAnalyzed(analyzed)) return;

        await api.GetArchiveItemsAsync(options, versionInfo.Path, analyzed.Added);

        if (!await confirmApply(analyzed)) return;

        await api.CleanupObsoleteItemsAsync(options, analyzed.Obsolete);

        await api.ApplyArchiveItemsAsync(options, analyzed.Added);
    }
}