using System.Net;

namespace LibUpdater.Utils;

internal class Downloader : IDownloader
{
    public string DownloadString(string uri)
    {
        using var client = new WebClient();
        return client.DownloadString(uri);
    }

    public void DownloadFile(string uri, string path)
    {
        using var client = new WebClient();
        client.DownloadFile(uri, path);
    }
}