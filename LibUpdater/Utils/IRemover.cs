namespace LibUpdater.Utils;

public interface IRemover
{
    /// <summary>
    ///     Remove file on path.
    ///     If it was a last file, directory also removed.
    /// </summary>
    /// <param name="path">Path to file.</param>
    void Remove(string path);
}