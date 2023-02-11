using System.Threading.Tasks;

namespace LibUpdater.Utils;

public interface IRemover
{
    /// <summary>
    ///     Remove file on path.
    /// </summary>
    /// <param name="path">Path to file.</param>
    void RemoveFile(string path);

    Task RemoveFileAsync(string path);

    /// <summary>
    ///     Remove all empty subdirectories in directory specified by <paramref name="path" />.
    /// </summary>
    /// <param name="path"></param>
    void RemoveEmptyDirs(string path);

    Task RemoveEmptyDirsAsync(string path);
}