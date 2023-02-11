using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace LibUpdater.Utils;

internal class Downloader : IDownloader
{
    public string DownloadString(string uri)
    {
        using var client = new WebClient();
        return client.DownloadString(uri);
    }

    public Task<string> DownloadStringAsync(string uri)
    {
        using var client = new WebClient();
        return client.DownloadStringTaskAsync(new Uri(uri));
    }

    public Task<Stream> OpenReadStreamAsync(string uri)
    {
        using var client = new WebClient();
        return client.OpenReadTaskAsync(new Uri(uri));
    }

    public void DownloadFile(string uri, string path)
    {
        using var client = new WebClient();
        client.DownloadFile(uri, path);
    }

    public Task DownloadFileAsync(string uri, string path)
    {
        using var client = new WebClient();
        return client.DownloadFileTaskAsync(uri, path);
    }
}