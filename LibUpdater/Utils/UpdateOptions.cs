namespace LibUpdater.Utils;

public class UpdateOptions
{
    public string TargetDir { get; set; }
    public string TempDir { get; set; }
    public string UpdatesUri { get; set; }
    public string VersionFile { get; set; } = "version.json";
    public string IndexFile { get; set; } = "index.json";
}