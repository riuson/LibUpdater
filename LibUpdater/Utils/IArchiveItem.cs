namespace LibUpdater.Utils
{
    /// <summary>
    /// Interface for archived file.
    /// </summary>
    public interface IArchiveItem : IFileItem
    {
        /// <summary>
        /// Gets relative path for unpacking.
        /// </summary>
        string UnpackPath { get; }

        /// <summary>
        /// Gets size of archive in bytes.
        /// </summary>
        long ArchiveSize { get; }

        /// <summary>
        /// Gets SHA-1 hash of archive.
        /// </summary>
        string ArchiveHash { get; }
    }
}