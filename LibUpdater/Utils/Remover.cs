using System.IO;
using System.Threading.Tasks;

namespace LibUpdater.Utils;

internal class Remover : IRemover
{
    public void Remove(string path)
    {
        if (File.Exists(path))
            File.Delete(path);

        var directory = new DirectoryInfo(Path.GetDirectoryName(path));

        var filesCount = directory.GetFiles("*.*", SearchOption.TopDirectoryOnly).Length;
        var dirsCount = directory.GetDirectories("*", SearchOption.TopDirectoryOnly).Length;

        if (filesCount == 0 && dirsCount == 0) directory.Delete(true);
    }

    public Task RemoveAsync(string path)
    {
        return Task.Run(() => Remove(path));
    }
}