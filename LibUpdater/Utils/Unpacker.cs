using System;
using System.IO;
using System.Threading.Tasks;
using SevenZipExtractor;

namespace LibUpdater.Utils;

internal class Unpacker : IUnpacker
{
    public void Unpack(Stream sourceStream, Stream targetStream)
    {
        using var archive = new ArchiveFile(sourceStream, SevenZipFormat.SevenZip);

        if (archive.Entries.Count != 1)
            throw new ArgumentException("The archive with only one file inside was expected.");

        var entry = archive.Entries[0];
        entry.Extract(targetStream);
    }

    public Task UnpackAsync(Stream sourceStream, Stream targetStream)
    {
        return Task.Run(() => Unpack(sourceStream, targetStream));
    }

    public void Unpack(string sourcePath, string targetPath)
    {
        using var sourceStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        using var targetStream = new FileStream(targetPath, FileMode.Create, FileAccess.Write, FileShare.Read);

        Unpack(sourceStream, targetStream);
    }

    public Task UnpackAsync(string sourcePath, string targetPath)
    {
        return Task.Run(() => Unpack(sourcePath, targetPath));
    }
}