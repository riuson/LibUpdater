using LibUpdater.Utils;

namespace LibUpdater.Tests.Utils;

internal class DownloaderTests
{
    private string _tempFile;

    [SetUp]
    public void SetUp()
    {
        this._tempFile = Path.GetTempFileName();
    }

    [TearDown]
    public void TearDown()
    {
        File.Delete(this._tempFile);
    }
    
    [Ignore("Requires server")]
    [TestCase("https://updates1.riuson.com/version.txt", "ver1")]
    public void DownloadStringShouldGetString(string uri, string expectedValue)
    {
        var downloader = new Downloader();

        var value = downloader.DownloadString(uri);

        Assert.That(value.Trim(), Is.EqualTo(expectedValue.Trim()));
    }

    [Ignore("Requires server")]
    [TestCase("https://updates1.riuson.com/version.txt", "ver1")]
    public void DownloadFileShouldGetFile(string uri, string expectedValue)
    {
        var downloader = new Downloader();

        downloader.DownloadFile(uri, this._tempFile);

        var value = File.ReadAllText(this._tempFile);

        Assert.That(value.Trim(), Is.EqualTo(expectedValue.Trim()));
    }

    [Ignore("Requires server")]
    [TestCase("https://updates1.riuson.com/version.txt", "ver1")]
    public void DownloadFileShouldOverwriteFile(string uri, string expectedValue)
    {
        var downloader = new Downloader();

        File.WriteAllText(this._tempFile, "some content");

        downloader.DownloadFile(uri, this._tempFile);

        var value = File.ReadAllText(this._tempFile);

        Assert.That(value.Trim(), Is.EqualTo(expectedValue.Trim()));
    }
}