using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        var latestVersionString = _downloader.DownloadString(versionUri);
        return latestVersionString.Trim();
    }

    public async Task<string> GetLatestVersionAsync(UpdateOptions options)
    {
        var versionUri = CombineUrl(options.UpdatesUri, options.VersionFile);
        var latestVersionString = await _downloader.DownloadStringAsync(versionUri);
        return latestVersionString.Trim();
    }

    public async Task<IEnumerable<IArchiveItem>> GetIndexAsync(
        UpdateOptions options,
        string version)
    {
        var indexUri = CombineUrl(options.UpdatesUri, version, options.IndexFile);
        var indexJsonStream = await _downloader.OpenReadStreamAsync(indexUri);
        var decoder = new IndexDecoder();
        var result = await decoder.DecodeAsync(indexJsonStream);
        return result;
    }

    public IEnumerable<IArchiveItem> GetIndex(
        UpdateOptions options,
        string version)
    {
        var indexUri = CombineUrl(options.UpdatesUri, version, options.IndexFile);
        var indexJsonString = _downloader.DownloadString(indexUri);
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

    public Task GetArchiveItemsAsync(
        UpdateOptions options,
        string version,
        IEnumerable<IArchiveItem> archiveItems)
    {
        return Task.Run(() => GetArchiveItems(options, version, archiveItems));
    }

    public void ApplyArchiveItems(
        UpdateOptions options,
        IEnumerable<IArchiveItem> archiveItems)
    {
        string archiveItemSourcePath(IArchiveItem item)
        {
            return Path.Combine(options.TempDir, item.Hash)
                .AdjustSeparator();
        }

        string archiveItemTargetPath(IArchiveItem item)
        {
            return Path.Combine(options.TargetDir, item.Path)
                .AdjustSeparator();
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

    public async Task ApplyArchiveItemsAsync(
        UpdateOptions options,
        IEnumerable<IArchiveItem> archiveItems)
    {
        string archiveItemSourcePath(IArchiveItem item)
        {
            return Path.Combine(options.TempDir, item.Hash)
                .AdjustSeparator();
        }

        string archiveItemTargetPath(IArchiveItem item)
        {
            return Path.Combine(options.TargetDir, item.Path)
                .AdjustSeparator();
        }

        foreach (var archiveItem in archiveItems)
        {
            var targetPath = archiveItemTargetPath(archiveItem);
            var targetDir = Path.GetDirectoryName(targetPath);

            if (!Directory.Exists(targetDir)) Directory.CreateDirectory(targetDir);

            await _unpacker.UnpackAsync(
                archiveItemSourcePath(archiveItem),
                targetPath);
        }
    }

    public async Task CleanupObsoleteItemsAsync(UpdateOptions options, IEnumerable<IFileItem> obsoleteItems)
    {
        foreach (var obsoleteItem in obsoleteItems)
            await _remover.RemoveFileAsync(
                obsoleteItem.Path.AdjustParent(options.TargetDir).AdjustSeparator());
    }

    public void CleanupObsoleteItems(UpdateOptions options, IEnumerable<IFileItem> obsoleteItems)
    {
        foreach (var obsoleteItem in obsoleteItems) _remover.RemoveFile(obsoleteItem.Path);
    }

    private static string CombineUrl(params string[] segments)
    {
        return string.Join("/", segments.Select(x => x.TrimEnd('/')));
    }
}