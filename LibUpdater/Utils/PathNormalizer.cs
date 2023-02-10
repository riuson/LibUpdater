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
    public static string AdjustDirSeparator(this string path)
    {
        var fileInfo = new FileInfo(path);
        return fileInfo.FullName;
    }
}