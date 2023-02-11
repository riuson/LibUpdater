﻿using System.Collections.Generic;
using System.Linq;
using LibUpdater.Data;

namespace LibUpdater.Tests.Utils;

public class TreeAnalyzer
{
    public IAnalyzeResult Analyze(
        string targetDirectory,
        IEnumerable<IFileItem> localItems,
        IEnumerable<IArchiveItem> remoteItems)
    {
        var comparer = new ItemsEqualityComparer(targetDirectory);
        var localItemsOrdered = localItems.OrderBy(x => x.Path);
        var remoteItemsOrdered = remoteItems.OrderBy(x => x.Path);

        var isEquals = localItemsOrdered.SequenceEqual(remoteItemsOrdered, comparer);

        var obsoleteItems = localItemsOrdered
            .Except(remoteItemsOrdered, comparer)
            .ToArray();
        var addedItems = remoteItemsOrdered
            .Except(localItemsOrdered, comparer)
            .Cast<IArchiveItem>()
            .ToArray();

        return new AnalyzeResult
        {
            IsEquals = isEquals,
            Added = addedItems,
            Obsolete = obsoleteItems
        };
    }
}