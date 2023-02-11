namespace LibUpdater.Data;

internal class FileItem : IFileItem
{
    public string Path { get; set; }
    public long Size { get; set; }
    public string Hash { get; set; }
}