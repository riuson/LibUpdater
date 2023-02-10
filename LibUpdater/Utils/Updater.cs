using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibUpdater.Data;

namespace LibUpdater.Utils;

public class Updater
{
    private readonly IDownloader _downloader;
    private readonly IRemover _remover;
    private readonly IUnpacker _unpacker;

    public Updater()
    {
        _downloader = new Downloader();
        _unpacker = new Unpacker();
        _remover = new Remover();
    }

    public Updater(
        IDownloader downloader,
        IUnpacker unpacker,
        IRemover remover)
    {
        _downloader = downloader;
        _unpacker = unpacker;
        _remover = remover;
    }

    public string GetLatestVersion(UpdateOptions options)
    {
        var versionUri = CombineUrl(options.UpdatesUri, options.VersionFile);

        var latestVersionString = _downloader
            .DownloadString(versionUri)
            .Trim();

        return latestVersionString;
    }

    public IEnumerable<IArchiveItem> GetIndex(
        UpdateOptions options,
        string version)
    {
        var indexUri = CombineUrl(options.UpdatesUri, version, options.IndexFile);

        var indexJsonString = _downloader
            .DownloadString(indexUri)
            .Trim();

        var decoder = new IndexDecoder();
        var result = decoder.Decode(indexJsonString);

        return result;
    }

    public void GetArchiveItems(
        UpdateOptions options,
        string version,
        IEnumerable<IArchiveItem> archiveItems)
    {
        string archiveItemUri(string hash)
        {
            return CombineUrl(options.UpdatesUri, version, hash);
        }

        string archiveItemPath(string hash)
        {
            return Path.Combine(options.TempDir, hash);
        }

        archiveItems
            .Select(x => x.Hash)
            .Distinct()
            .AsParallel()
            .WithDegreeOfParallelism(options.DegreeOfParallelism)
            .ForAll(hash => _downloader.DownloadFile(archiveItemUri(hash), archiveItemPath(hash)));
    }

    public void ApplyArchiveItems(
        UpdateOptions options,
        IEnumerable<IArchiveItem> archiveItems)
    {
        string archiveItemSourcePath(IArchiveItem item)
        {
            return Path.Combine(options.TempDir, item.Hash)
                .AdjustDirSeparator();
        }

        string archiveItemTargetPath(IArchiveItem item)
        {
            return Path.Combine(options.TargetDir, item.Path)
                .AdjustDirSeparator();
        }

        foreach (var archiveItem in archiveItems)
        {
            var targetPath = archiveItemTargetPath(archiveItem);
            var targetDir = Path.GetDirectoryName(targetPath);

            if (!Directory.Exists(targetDir)) Directory.CreateDirectory(targetDir);

            _unpacker.Unpack(
                archiveItemSourcePath(archiveItem),
                targetPath);
        }
    }

    public void CleanupObsoleteItems(UpdateOptions options, IEnumerable<IFileItem> obsoleteItems)
    {
        foreach (var obsoleteItem in obsoleteItems) _remover.Remove(obsoleteItem.Path);
    }

    private static string CombineUrl(params string[] segments)
    {
        return string.Join("/", segments.Select(x => x.TrimEnd('/')));
    }
}