using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LibUpdater.Data;

namespace LibUpdater.Utils;

internal class Downloader : IDownloader
{
    public string DownloadString(string uri)
    {
        using var client = new WebClient();
        using var stream = client.OpenRead(uri);
        using var streamReader = new StreamReader(stream, Encoding.UTF8);
        var buffer = new char[1024];
        var result = new StringBuilder();
        var totalReaded = 0;
        var id = Guid.NewGuid();

        while (true)
        {
            var readed = streamReader.Read(buffer, 0, buffer.Length);
            totalReaded += readed;

            if (readed > 0)
            {
                result.Append(buffer, 0, readed);
                ReportProgress(totalReaded, -1, id);
            }
            else
            {
                ReportProgress(totalReaded, totalReaded, id);
                return result.ToString();
            }
        }
    }

    public async Task<string> DownloadStringAsync(string uri)
    {
        using var client = new WebClient();
        using var stream = await client.OpenReadTaskAsync(uri);
        using var streamReader = new StreamReader(stream, Encoding.UTF8);
        var buffer = new char[1024];
        var result = new StringBuilder();
        var totalReaded = 0;
        var id = Guid.NewGuid();

        while (true)
        {
            var readed = await streamReader.ReadBlockAsync(buffer, 0, buffer.Length);
            totalReaded += readed;

            if (readed > 0)
            {
                result.Append(buffer, 0, readed);
                ReportProgress(totalReaded, -1, id);
            }
            else
            {
                ReportProgress(totalReaded, totalReaded, id);
                return result.ToString();
            }
        }
    }

    public void DownloadFile(string uri, string path, long size = -1)
    {
        using var client = new WebClient();
        var webStream = client.OpenRead(uri);
        using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read);
        var buffer = new byte[1024 * 1024];
        var totalReaded = 0;
        var id = Guid.NewGuid();

        while (true)
        {
            var readed = webStream.Read(buffer, 0, buffer.Length);
            totalReaded += readed;

            if (readed > 0)
            {
                fileStream.Write(buffer, 0, readed);
                ReportProgress(totalReaded, size, id);
            }
            else
            {
                ReportProgress(totalReaded, size, id);
                break;
            }
        }
    }

    public async Task DownloadFileAsync(string uri, string path, long size = -1)
    {
        using var client = new WebClient();
        var webStream = await client.OpenReadTaskAsync(uri);
        using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read);
        var buffer = new byte[1024 * 1024];
        var totalReaded = 0;
        var id = Guid.NewGuid();

        while (true)
        {
            var readed = await webStream.ReadAsync(buffer, 0, buffer.Length);
            totalReaded += readed;

            if (readed > 0)
            {
                await fileStream.WriteAsync(buffer, 0, readed);
                ReportProgress(totalReaded, size, id);
            }
            else
            {
                ReportProgress(totalReaded, size, id);
                break;
            }
        }
    }

    public event EventHandler<ProgressEventArgs> Progress;

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
}