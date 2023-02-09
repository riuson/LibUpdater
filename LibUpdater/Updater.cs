using LibUpdater.Utils;
using System;

namespace LibUpdater;

public class Updater
{
    private readonly IDownloader _downloader;

    public Updater(IDownloader downloader)
    {
        _downloader = downloader;
    }

    public void Update(UpdateOptions options)
    {
        var versionUri = new Uri(
            new Uri(options.UpdatesUri),
            options.VersionFile);

        var latestVersionString = this._downloader
            .DownloadString(options.UpdatesUri)
            .Trim();
    }
}