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
        Func<IActualVersionInfo, bool> confirmVersion,
        Func<IEnumerable<IArchiveItem>, bool> confirmIndex,
        Func<IAnalyzeResult, bool> confirmAnalyzed,
        Func<IAnalyzeResult, bool> confirmApply)
    {
        var api = new UpdaterAPI();

        var versionInfo = await api.GetActualVersionAsync(options);

        if (!confirmVersion(versionInfo)) return;

        var indexItems = await api.GetIndexAsync(options, versionInfo.Path);

        if (!confirmIndex(indexItems)) return;

        var scanner = new TreeScanner();
        var localItems = await scanner.ScanTreeAsync(options.TargetDir, options.DegreeOfParallelism);

        var analyzer = new TreeAnalyzer();
        var analyzed = analyzer.Analyze(
            options.TargetDir,
            localItems,
            indexItems);

        if (!confirmAnalyzed(analyzed)) return;

        await api.GetArchiveItemsAsync(options, versionInfo.Path, analyzed.Added);

        if (!confirmApply(analyzed)) return;

        await api.CleanupObsoleteItemsAsync(options, analyzed.Obsolete);

        await api.ApplyArchiveItemsAsync(options, analyzed.Added);
    }
}