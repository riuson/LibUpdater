﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibUpdater.Data;

namespace LibUpdater.Utils;

public class Updater
{
    private readonly IDownloader _downloader;

    public Updater(IDownloader downloader)
    {
        _downloader = downloader;
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

        archiveItems.AsParallel()
            .WithDegreeOfParallelism(options.DegreeOfParallelism)
            .ForAll(item => _downloader.DownloadFile(archiveItemUri(item.Hash), archiveItemPath(item.Hash)));
    }

    private static string CombineUrl(params string[] segments)
    {
        return string.Join("/", segments.Select(x => x.TrimEnd('/')));
    }
}