using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LibUpdater.Data;

namespace LibUpdater.Utils;

public class UpdaterAPI
{
    private readonly IDownloader _downloader;
    private readonly IRemover _remover;
    private readonly IUnpacker _unpacker;

    public UpdaterAPI()
    {
        _downloader = new Downloader();
        _unpacker = new Unpacker();
        _remover = new Remover();
    }

    public UpdaterAPI(
        IDownloader downloader,
        IUnpacker unpacker,
        IRemover remover)
    {
        _downloader = downloader;
        _unpacker = unpacker;
        _remover = remover;
    }

    public IActualVersionInfo GetActualVersion(UpdateOptions options)
    {
        var versionUri = CombineUrl(options.UpdatesUri, options.VersionFile);

        try
        {
            _downloader.Progress += ProgressHandler;
            var latestVersionJson = _downloader.DownloadString(versionUri);
            var decoder = new JsonDecoder();
            return decoder.DecodeVersion(latestVersionJson);
        }
        finally
        {
            _downloader.Progress -= ProgressHandler;
        }
    }

    public async Task<IActualVersionInfo> GetActualVersionAsync(UpdateOptions options)
    {
        try
        {
            _downloader.Progress += ProgressHandler;
            var versionUri = CombineUrl(options.UpdatesUri, options.VersionFile);
            var latestVersionJson = await _downloader.DownloadStringAsync(versionUri);
            var decoder = new JsonDecoder();
            return decoder.DecodeVersion(latestVersionJson);
        }
        finally
        {
            _downloader.Progress -= ProgressHandler;
        }
    }

    public async Task<IEnumerable<IArchiveItem>> GetIndexAsync(
        UpdateOptions options,
        string version)
    {
        try
        {
            _downloader.Progress += ProgressHandler;
            var indexUri = CombineUrl(options.UpdatesUri, version, options.IndexFile);
            var indexJson = await _downloader.DownloadStringAsync(indexUri);
            var decoder = new JsonDecoder();
            var result = decoder.DecodeIndex(indexJson);
            return result;
        }
        finally
        {
            _downloader.Progress -= ProgressHandler;
        }
    }

    public IEnumerable<IArchiveItem> GetIndex(
        UpdateOptions options,
        string version)
    {
        try
        {
            _downloader.Progress += ProgressHandler;
            var indexUri = CombineUrl(options.UpdatesUri, version, options.IndexFile);
            var indexJsonString = _downloader.DownloadString(indexUri);
            var decoder = new JsonDecoder();
            var result = decoder.DecodeIndex(indexJsonString);
            return result;
        }
        finally
        {
            _downloader.Progress -= ProgressHandler;
        }
    }

    public void GetArchiveItems(
        UpdateOptions options,
        string version,
        IEnumerable<IArchiveItem> archiveItems)
    {
        var totalSize = archiveItems.Sum(x => x.ArchiveSize);
        var progressMap = new Dictionary<Guid, ProgressEventArgs>();
        var id = Guid.NewGuid();

        void progressHandler(object sender, ProgressEventArgs args)
        {
            progressMap[args.Id] = args;
            var currentSize = progressMap.Values.Sum(x => x.Current);
            ReportProgress(currentSize, totalSize, id);
        }

        try
        {
            _downloader.Progress += progressHandler;

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
        finally
        {
            _downloader.Progress -= progressHandler;
        }
    }

    public async Task GetArchiveItemsAsync(
        UpdateOptions options,
        string version,
        IEnumerable<IArchiveItem> archiveItems)
    {
        var totalSize = archiveItems.Sum(x => x.ArchiveSize);
        var progressMap = new ConcurrentDictionary<Guid, ProgressEventArgs>();
        var id = Guid.NewGuid();

        void progressHandler(object sender, ProgressEventArgs args)
        {
            progressMap[args.Id] = args;
            var currentSize = progressMap.Values.Sum(x => x.Current);
            ReportProgress(currentSize, totalSize, id);
        }

        try
        {
            _downloader.Progress += progressHandler;

            string archiveItemUri(string hash)
            {
                return CombineUrl(options.UpdatesUri, version, hash);
            }

            string archiveItemPath(string hash)
            {
                return Path.Combine(options.TempDir, hash);
            }

            var uniqueHashes = archiveItems
                .Select(x => x.Hash)
                .Distinct();

            await uniqueHashes.ForEachAsync(
                options.DegreeOfParallelism,
                hash => _downloader.DownloadFileAsync(
                    archiveItemUri(hash),
                    archiveItemPath(hash)));
        }
        finally
        {
            _downloader.Progress -= progressHandler;
        }
    }

    public void ApplyArchiveItems(
        UpdateOptions options,
        IEnumerable<IArchiveItem> archiveItems)
    {
        var totalSize = archiveItems.Sum(x => x.ArchiveSize);
        var progressMap = new Dictionary<Guid, ProgressEventArgs>();
        var id = Guid.NewGuid();

        void progressHandler(object sender, ProgressEventArgs args)
        {
            progressMap[args.Id] = args;
            var currentSize = progressMap.Values.Sum(x => x.Current);
            ReportProgress(currentSize, totalSize, id);
        }

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

            // Simulate progress.
            progressHandler(_unpacker, new ProgressEventArgs
            {
                Current = archiveItem.Size,
                Total = archiveItem.Size,
                Id = Guid.NewGuid()
            });
        }
    }

    public async Task ApplyArchiveItemsAsync(
        UpdateOptions options,
        IEnumerable<IArchiveItem> archiveItems)
    {
        var totalSize = archiveItems.Sum(x => x.ArchiveSize);
        var progressMap = new ConcurrentDictionary<Guid, ProgressEventArgs>();
        var id = Guid.NewGuid();

        void progressHandler(object sender, ProgressEventArgs args)
        {
            progressMap[args.Id] = args;
            var currentSize = progressMap.Values.Sum(x => x.Current);
            ReportProgress(currentSize, totalSize, id);
        }

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

            // Simulate progress.
            progressHandler(_unpacker, new ProgressEventArgs
            {
                Current = archiveItem.Size,
                Total = archiveItem.Size,
                Id = Guid.NewGuid()
            });
        }
    }

    public void CleanupObsoleteItems(UpdateOptions options, IEnumerable<IFileItem> obsoleteItems)
    {
        var totalCount = obsoleteItems.Count();
        var current = 0;
        var id = Guid.NewGuid();

        foreach (var obsoleteItem in obsoleteItems)
        {
            _remover.RemoveFile(
                obsoleteItem.Path.AdjustParent(options.TargetDir).AdjustSeparator());
            current++;
            ReportProgress(current, totalCount, id);
        }

        _remover.RemoveEmptyDirs(options.TargetDir);
    }

    public async Task CleanupObsoleteItemsAsync(UpdateOptions options, IEnumerable<IFileItem> obsoleteItems)
    {
        var totalCount = obsoleteItems.Count();
        var current = 0;
        var id = Guid.NewGuid();

        foreach (var obsoleteItem in obsoleteItems)
        {
            await _remover.RemoveFileAsync(
                obsoleteItem.Path.AdjustParent(options.TargetDir).AdjustSeparator());
            current++;
            ReportProgress(current, totalCount, id);
        }

        await _remover.RemoveEmptyDirsAsync(options.TargetDir);
    }

    public event EventHandler<ProgressEventArgs> Progress;

    private static string CombineUrl(params string[] segments)
    {
        return string.Join("/", segments.Select(x => x.TrimEnd('/')));
    }

    private void ReportProgress(long current, long total, Guid id)
    {
        Progress?.Invoke(
            this,
            new ProgressEventArgs
            {
                Current = current,
                Total = total,
                Id = id
            });
    }

    private void ProgressHandler(object sender, ProgressEventArgs args)
    {
        ReportProgress(args.Current, args.Total, args.Id);
    }
}