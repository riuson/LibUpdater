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
    /// <param name="path">Path to root directory.</param>
    void RemoveEmptyDirs(string path);

    Task RemoveEmptyDirsAsync(string path);

    /// <summary>
    /// Remove all child files and subdirectories.
    /// </summary>
    /// <param name="path">Path to root directory.</param>
    void RemoveChilds(string path);
    
    Task RemoveChildsAsync(string path);
}