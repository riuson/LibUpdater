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

        if (localItems.SequenceEqual(remoteItems.Cast<IFileItem>(), comparer))
        {
            return new AnalyzeResult()
            {
                IsEquals = true,
            };
        }

        throw new NotImplementedException();
    }
}