﻿using System;
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
        Func<IAnalysisResult, Task<bool>> confirmApply)
    {
        var api = new UpdaterAPI();

        var versionInfo = await api.GetActualVersionAsync(options);

        if (!await confirmVersion(versionInfo)) return;

        var indexItems = await api.GetIndexAsync(options, versionInfo.Path);

        if (!await confirmIndex(indexItems)) return;

        var scanner = new TreeScanner();
        var localItems = await scanner.ScanTreeAsync(options.TargetDir, options.DegreeOfParallelism);

        var analyzer = new TreeAnalyzer();
        var analysisResult = analyzer.Analyze(
            options.TargetDir,
            localItems,
            indexItems);

        if (!await confirmAnalysis(analysisResult)) return;

        await api.GetArchiveItemsAsync(options, versionInfo.Path, analysisResult.Added);

        if (!await confirmApply(analysisResult)) return;

        await api.CleanupObsoleteItemsAsync(options, analysisResult.Obsolete);

        await api.ApplyArchiveItemsAsync(options, analysisResult.Added);
    }
}