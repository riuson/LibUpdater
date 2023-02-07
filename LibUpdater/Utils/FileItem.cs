namespace LibUpdater.Utils
{
    internal class FileItem : IFileItem
    {
        public FileItem(string path)
        {
            this.Path = path;
        }

        public string Path { get; }
    }
}