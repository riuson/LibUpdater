using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibUpdater.Data;

namespace LibUpdater.Utils;

public class TreeScanner
{
    private readonly Hasher _hasher = new();

    /// TODO: Not implemented progress.
    public event EventHandler<ProgressEventArgs> Progress;

    public IEnumerable<IFileItem> ScanTree(
        string path,
        CancellationToken token,
        int degreeOfParallelism = 1)
    {
        var directoryInfo = new DirectoryInfo(path);
        var progressMap = new Dictionary<Guid, ProgressEventArgs>();
        var id = Guid.NewGuid();

        void hashProgress(object sender, ProgressEventArgs e)
        {
            progressMap[e.Id] = e;
            var totalProgress = new ProgressEventArgs
            {
                Total = progressMap.Values.Sum(x => x.Total),
                Current = progressMap.Values.Sum(x => x.Current),
                Id = id
            };
        }

        try
        {
            _hasher.Progress += hashProgress;

            var result = directoryInfo.EnumerateFiles("*", SearchOption.AllDirectories)
                .Select(x => new FileItem { Path = x.FullName, Size = x.Length })
                .AsParallel()
                .WithDegreeOfParallelism(degreeOfParallelism)
                .WithCancellation(token)
                .Select(x => CalculateHash(x, token))
                .OrderBy(x => x.Path)
                .ToArray();
            return result;
        }
        finally
        {
            _hasher.Progress -= hashProgress;
        }
    }

    public async Task<IEnumerable<IFileItem>> ScanTreeAsync(
        string path,
        CancellationToken token,
        int degreeOfParallelism = 1)
    {
        var directoryInfo = new DirectoryInfo(path);
        var progressMap = new ConcurrentDictionary<Guid, ProgressEventArgs>();
        var id = Guid.NewGuid();

        void hashProgress(object sender, ProgressEventArgs e)
        {
            progressMap[e.Id] = e;
            var totalProgress = new ProgressEventArgs
            {
                Total = progressMap.Values.Sum(x => x.Total),
                Current = progressMap.Values.Sum(x => x.Current),
                Id = id
            };
        }

        try
        {
            _hasher.Progress += hashProgress;

            var fileItems = directoryInfo.EnumerateFiles("*", SearchOption.AllDirectories)
                .Select(x => new FileItem { Path = x.FullName, Size = x.Length });

            var result = new ConcurrentBag<IFileItem>();

            await fileItems.ForEachAsync(
                degreeOfParallelism,
                async fileItem => result.Add(await CalculateHashAsync(fileItem, token)));

            return result.ToArray();
        }
        finally
        {
            _hasher.Progress -= hashProgress;
        }
    }

    private FileItem CalculateHash(FileItem item, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        using var stream = new FileStream(item.Path, FileMode.Open, FileAccess.Read, FileShare.Read);
        var hash = _hasher.HashStream(stream, token, stream.Length);
        item.Hash = hash;

        return item;
    }

    private async Task<FileItem> CalculateHashAsync(FileItem item, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        using var stream = new FileStream(item.Path, FileMode.Open, FileAccess.Read, FileShare.Read);
        var hash = await _hasher.HashStreamAsync(stream, token, stream.Length);
        item.Hash = hash;

        return item;
    }
}