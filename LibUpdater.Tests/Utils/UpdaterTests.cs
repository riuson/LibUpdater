using LibUpdater.Utils;
using Moq;

namespace LibUpdater.Tests.Utils;

internal class UpdaterTests
{
    [Test]
    public void GetLatestVersionShould()
    {
        var downloaderMock = new Mock<IDownloader>();
        downloaderMock.Setup(mock => mock.DownloadString("https://localhost:81/version_file.txt")).Returns("version1").Verifiable();

        var updater = new Updater(downloaderMock.Object);

        var options = new UpdateOptions();
        options.UpdatesUri = "https://localhost:81";
        options.VersionFile = "version_file.txt";

        var version = updater.GetLatestVersion(options);

        Assert.That(version, Is.EqualTo("version1"));
        downloaderMock.Verify(t => t.DownloadString("https://localhost:81/version_file.txt"));
    }
}