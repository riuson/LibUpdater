using System.Collections.Generic;
using LibUpdater.Data;

namespace LibUpdater.Analysis;

public interface IAnalysisResult
{
    bool IsEquals { get; }
    IEnumerable<IFileItem> Obsolete { get; }
    IEnumerable<IArchiveItem> Added { get; }

    int FilesToAddCount { get; }
    int FilesToRemoveCount { get; }
    long BytesToRemove { get; }
    long BytesToAdd { get; }
    long BytesToDownload { get; }
    long BytesChanged { get; }
}