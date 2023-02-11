using System.Threading;

namespace LibUpdater.Utils;

public class UpdateOptions
{
    public string TargetDir { get; set; }
    public string TempDir { get; set; }
    public string UpdatesUri { get; set; }
    public string VersionFile { get; set; } = "version.json";
    public string IndexFile { get; set; } = "index.json";
    public int DegreeOfParallelism { get; set; } = 1;
    public CancellationToken Token { get; set; } = CancellationToken.None;
}