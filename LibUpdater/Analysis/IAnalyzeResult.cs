using System.Collections.Generic;
using LibUpdater.Data;

namespace LibUpdater.Tests.Utils;

public interface IAnalyzeResult
{
    bool IsEquals { get; }
    IEnumerable<IFileItem> Obsolete { get; }
    IEnumerable<IArchiveItem> Added { get; }
}