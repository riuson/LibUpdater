namespace LibUpdater.Utils
{
    public interface IFileItem
    {
        string Path { get; }
        long Size { get; set; }
        string Hash { get; set; }
    }
}