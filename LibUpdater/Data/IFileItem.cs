namespace LibUpdater.Data;

/// <summary>
///     Interface for local file.
/// </summary>
public interface IFileItem
{
    /// <summary>
    ///     Gets absolute local path to file.
    /// </summary>
    string Path { get; }

    /// <summary>
    ///     Gets size of file in bytes.
    /// </summary>
    long Size { get; }

    /// <summary>
    ///     Gets SHA-1 hash of file.
    /// </summary>
    string Hash { get; }
}