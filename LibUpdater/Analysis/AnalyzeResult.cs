using System.Collections.Generic;
using LibUpdater.Data;

namespace LibUpdater.Tests.Utils;

internal class AnalyzeResult : IAnalyzeResult
{
    public bool IsEquals { get; set; } = false;
    public IEnumerable<IFileItem> Obsolete { get; set; } = new IFileItem[] { };
    public IEnumerable<IArchiveItem> Added { get; set; } = new IArchiveItem[] { };
}