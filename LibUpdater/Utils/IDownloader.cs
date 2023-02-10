namespace LibUpdater.Utils;

public interface IDownloader
{
    string DownloadString(string uri);
    void DownloadFile(string uri, string path);
}