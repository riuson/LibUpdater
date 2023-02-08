using System;
using System.Linq;
using System.Collections.Generic;
using LibUpdater.Data;

namespace LibUpdater.Tests.Utils;

public class TreeAnalyzer
{
    public AnalyzeResult Analyze(
        string targetDirectory,
        IEnumerable<IFileItem> localItems,
        IEnumerable<IArchiveItem> remoteItems)
    {
        var comparer = new ItemsEqualityComparer(targetDirectory);
        var localItemsOrdered = localItems.OrderBy(x => x.Path);
        var remoteItemsOrdered = remoteItems.OrderBy(x => x.Path);

        if (localItemsOrdered.SequenceEqual(remoteItemsOrdered.Cast<IFileItem>(), comparer))
        {
            return new AnalyzeResult()
            {
                IsEquals = true,
            };
        }

        return new AnalyzeResult()
        {
            IsEquals = false,
        };
    }
}