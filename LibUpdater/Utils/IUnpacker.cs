using System.IO;
using System.Threading.Tasks;

namespace LibUpdater.Utils;

public interface IUnpacker
{
    void Unpack(Stream sourceStream, Stream targetStream);
    Task UnpackAsync(Stream sourceStream, Stream targetStream);
    void Unpack(string sourcePath, string targetPath);
    Task UnpackAsync(string sourcePath, string targetPath);
}