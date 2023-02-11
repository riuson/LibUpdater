using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LibUpdater.Utils;

internal class Remover : IRemover
{
    public void RemoveFile(string path)
    {
        if (File.Exists(path))
            File.Delete(path);
    }

    public Task RemoveFileAsync(string path)
    {
        return Task.Run(() => RemoveFile(path));
    }

    public void RemoveEmptyDirs(string path)
    {
        var directoryRoot = new DirectoryInfo(path);
        var subDirectories = directoryRoot.GetDirectories("*", SearchOption.AllDirectories);
        var list = new List<DirectoryInfo>();

        bool isAnyChild(DirectoryInfo directoryInfo)
        {
            return directoryInfo.EnumerateFiles("*", SearchOption.TopDirectoryOnly).Any() ||
                   directoryInfo.EnumerateDirectories("*", SearchOption.TopDirectoryOnly).Any();
        }

        foreach (var directoryInfo in subDirectories)
            if (!isAnyChild(directoryInfo))
                list.Add(directoryInfo);

        foreach (var directoryInfo in list.OrderByDescending(x => x.FullName))
            if (directoryInfo.Exists)
                directoryInfo.Delete();

        //if (!isAnyChild(directoryRoot))
        //    directoryRoot.Delete();
    }

    public Task RemoveEmptyDirsAsync(string path)
    {
        return Task.Run(() => RemoveEmptyDirs(path));
    }
}