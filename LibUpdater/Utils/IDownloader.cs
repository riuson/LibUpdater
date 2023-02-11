using System;
using System.Threading;
using System.Threading.Tasks;
using LibUpdater.Data;

namespace LibUpdater.Utils;

public interface IDownloader
{
    string DownloadString(string uri, CancellationToken token);
    Task<string> DownloadStringAsync(string uri, CancellationToken token);
    void DownloadFile(string uri, string path, CancellationToken token, long size = -1);
    Task DownloadFileAsync(string uri, string path, CancellationToken token, long size = -1);
    event EventHandler<ProgressEventArgs> Progress;
}