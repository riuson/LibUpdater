namespace LibUpdater.Utils
{
    internal class ArchiveItem : IArchiveItem
    {
        public string Path { get; set; }
        public long Size { get; set; }
        public string Hash { get; set; }
        public string UnpackPath { get; set; }
        public long ArchiveSize { get; set; }
        public string ArchiveHash { get; set; }
    }
}