using System.IO;
using System.Threading.Tasks;

namespace LibUpdater.Utils;

public interface IDownloader
{
    string DownloadString(string uri);
    Task<string> DownloadStringAsync(string uri);
    Task<Stream> OpenReadStreamAsync(string uri);
    void DownloadFile(string uri, string path);
    Task DownloadFileAsync(string uri, string path);
}