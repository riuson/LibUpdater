using LibUpdater.Utils;
using Moq;

namespace LibUpdater.Tests.Utils;

internal class UpdaterTests
{
    [Test]
    public void GetLatestVersionShould()
    {
        var downloaderMock = new Mock<IDownloader>();
        downloaderMock.Setup(mock => mock.DownloadString("https://localhost:81/version_file.txt"))
            .Returns("version1")
            .Verifiable();

        var updater = new Updater(downloaderMock.Object);

        var options = new UpdateOptions();
        options.UpdatesUri = "https://localhost:81";
        options.VersionFile = "version_file.txt";

        var version = updater.GetLatestVersion(options);

        Assert.That(version, Is.EqualTo("version1"));
        downloaderMock.Verify(t => t.DownloadString("https://localhost:81/version_file.txt"));
    }

    [TestCaseSource(nameof(TestJsonIndexResources))]
    public void GetIndexShould(string jsonIndexResource)
    {
        var json = AssemblyExtension.ReadResource(jsonIndexResource);

        var downloaderMock = new Mock<IDownloader>();
        downloaderMock.Setup(mock => mock.DownloadString("https://localhost/version2/index_file.json"))
            .Returns(json)
            .Verifiable();

        var updater = new Updater(downloaderMock.Object);

        var options = new UpdateOptions();
        options.UpdatesUri = "https://localhost";
        options.IndexFile = "index_file.json";

        var archiveItems = updater.GetIndex(options, "version2");

        downloaderMock.Verify(t => t.DownloadString("https://localhost/version2/index_file.json"));

        Assert.That(archiveItems.Count(), Is.EqualTo(4));

        Assert.That(archiveItems.ElementAt(0).Path, Is.EqualTo("LibUpdater.dll"));
        Assert.That(archiveItems.ElementAt(0).Size, Is.EqualTo(10240));
        Assert.That(archiveItems.ElementAt(0).Hash, Is.EqualTo("6146e3bcd55200cdd8de2c8ba207ebb1ff717322"));
        Assert.That(archiveItems.ElementAt(0).ArchiveSize, Is.EqualTo(4278));
        Assert.That(archiveItems.ElementAt(0).ArchiveHash, Is.EqualTo("c150297e6efd6e8b59957d7e2544e9b874b806f7"));

        Assert.That(archiveItems.ElementAt(3).Path, Is.EqualTo("LibUpdater.Tests.pdb"));
        Assert.That(archiveItems.ElementAt(3).Size, Is.EqualTo(17932));
        Assert.That(archiveItems.ElementAt(3).Hash, Is.EqualTo("15b16d08f2e3bb1655f3f4aaf39429b856efc262"));
        Assert.That(archiveItems.ElementAt(3).ArchiveSize, Is.EqualTo(8929));
        Assert.That(archiveItems.ElementAt(3).ArchiveHash, Is.EqualTo("366c5aa66c6b31233ddf6c88c13561ab327b541a"));
    }

    private static IEnumerable<string> TestJsonIndexResources()
    {
        var names = AssemblyExtension.GetEmbeddedResourceNames(x => x.Contains("Samples.JsonIndex"));
        return names;
    }
}