using LibUpdater.Utils;

namespace LibUpdater.Tests.Utils;

[Ignore("Requires server")]
internal class DownloaderTests
{
    private string _tempFile;

    [SetUp]
    public void SetUp()
    {
        _tempFile = Path.GetTempFileName();
    }

    [TearDown]
    public void TearDown()
    {
        File.Delete(_tempFile);
    }

    [TestCase("https://updates1.riuson.com/version.json", "0.1.0.0")]
    public void DownloadStringShouldGetString(string uri, string expectedValue)
    {
        var downloader = new Downloader();

        var value = downloader.DownloadString(uri, CancellationToken.None);

        Assert.That(value.Contains(expectedValue), Is.True);
    }

    [TestCase("https://updates1.riuson.com/version.json", "0.1.0.0")]
    public async Task DownloadStringShouldGetStringAsync(string uri, string expectedValue)
    {
        var downloader = new Downloader();

        var value = await downloader.DownloadStringAsync(uri, CancellationToken.None);

        Assert.That(value.Contains(expectedValue), Is.True);
    }

    [TestCase("https://updates1.riuson.com/version.json", "0.1.0.0")]
    public void DownloadFileShouldGetFile(string uri, string expectedValue)
    {
        var downloader = new Downloader();

        downloader.DownloadFile(uri, _tempFile, CancellationToken.None);

        var value = File.ReadAllText(_tempFile);

        Assert.That(value.Contains(expectedValue), Is.True);
    }

    [TestCase("https://updates1.riuson.com/version.json", "0.1.0.0")]
    public void DownloadFileShouldOverwriteFile(string uri, string expectedValue)
    {
        var downloader = new Downloader();

        File.WriteAllText(_tempFile, "some content");

        downloader.DownloadFile(uri, _tempFile, CancellationToken.None);

        var value = File.ReadAllText(_tempFile);

        Assert.That(value.Contains(expectedValue), Is.True);
    }
}