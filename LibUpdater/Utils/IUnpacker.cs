using System.IO;

namespace LibUpdater.Utils;

public interface IUnpacker
{
    void Unpack(Stream sourceStream, Stream targetStream);
    void Unpack(string sourcePath, string targetPath);
}