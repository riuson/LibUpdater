using System.IO;

namespace LibUpdater.Utils;

public static class PathNormalizerExtension
{
    /// <summary>
    ///     Normalize directory separators in path.
    /// </summary>
    /// <remarks>The character case is not adjusted!</remarks>
    /// <param name="path">Path to file.</param>
    /// <returns>Normalized path to file.</returns>
    public static string AdjustSeparator(this string path)
    {
        var fileInfo = new FileInfo(path);
        return fileInfo.FullName;
    }

    /// <summary>
    ///     Add missing root path to relative path. Absolute path is not changed.
    /// </summary>
    /// <param name="path">Path to file.</param>
    /// <param name="parent">Optional path to root directory.</param>
    /// <returns>Path with root.</returns>
    public static string AdjustParent(this string path, string parent)
    {
        if (Path.IsPathRooted(path)) return path;

        return Path.Combine(parent, path);
    }
}