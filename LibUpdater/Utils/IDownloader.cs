using System;
using System.Threading.Tasks;
using LibUpdater.Data;

namespace LibUpdater.Utils;

public interface IDownloader
{
    string DownloadString(string uri);
    Task<string> DownloadStringAsync(string uri);
    void DownloadFile(string uri, string path, long size = -1);
    Task DownloadFileAsync(string uri, string path, long size = -1);
    event EventHandler<ProgressEventArgs> Progress;
}