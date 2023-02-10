﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibUpdater.Data;

namespace LibUpdater.Utils;

public class Updater
{
    private readonly IDownloader _downloader;
    private readonly IUnpacker _unpacker;

    public Updater(
        IDownloader downloader,
        IUnpacker unpacker)
    {
        _downloader = downloader;
        _unpacker = unpacker;
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
        string archiveItemUri(IArchiveItem item)
        {
            return CombineUrl(options.UpdatesUri, version, item.Hash);
        }

        string archiveItemPath(IArchiveItem item)
        {
            return Path.Combine(options.TempDir, item.Hash);
        }

        archiveItems.AsParallel()
            .WithDegreeOfParallelism(options.DegreeOfParallelism)
            .ForAll(item => _downloader.DownloadFile(archiveItemUri(item), archiveItemPath(item)));
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
            _unpacker.Unpack(
                archiveItemSourcePath(archiveItem),
                archiveItemTargetPath(archiveItem));
    }

    private static string CombineUrl(params string[] segments)
    {
        return string.Join("/", segments.Select(x => x.TrimEnd('/')));
    }
}