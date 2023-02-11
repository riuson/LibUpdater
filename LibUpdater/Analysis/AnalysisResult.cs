using System.Collections.Generic;
using System.Linq;
using LibUpdater.Data;

namespace LibUpdater.Analysis;

internal class AnalysisResult : IAnalysisResult
{
    public bool IsEquals { get; set; } = false;
    public IEnumerable<IFileItem> Obsolete { get; set; } = new IFileItem[] { };
    public IEnumerable<IArchiveItem> Added { get; set; } = new IArchiveItem[] { };
    public int FilesToAddCount => Added.Count();
    public int FilesToRemoveCount => Obsolete.Count();
    public long BytesToRemove => Obsolete.Sum(x => x.Size);
    public long BytesToAdd => Added.Sum(x => x.Size);
    public long BytesToDownload => Added.Sum(x => x.ArchiveSize);
    public long BytesChanged => BytesToAdd - BytesToRemove;
}