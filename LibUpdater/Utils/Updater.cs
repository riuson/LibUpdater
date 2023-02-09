using System;

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
        var versionUri = new Uri(
            new Uri(options.UpdatesUri),
            options.VersionFile);

        var latestVersionString = _downloader
            .DownloadString(versionUri.AbsoluteUri)
            .Trim();

        return latestVersionString;
    }
}