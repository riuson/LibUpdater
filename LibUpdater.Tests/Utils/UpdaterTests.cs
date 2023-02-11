using LibUpdater.Data;
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

        var updater = new Updater(downloaderMock.Object, null, null);

        var options = new UpdateOptions();
        options.UpdatesUri = "https://localhost:81";
        options.VersionFile = "version_file.txt";

        var version = updater.GetLatestVersion(options);

        Assert.That(version, Is.EqualTo("version1"));
        downloaderMock.Verify(t => t.DownloadString("https://localhost:81/version_file.txt"));
    }

    [Test]
    public async Task GetLatestVersionAsyncShould()
    {
        var downloaderMock = new Mock<IDownloader>();
        downloaderMock.Setup(mock => mock.DownloadStringAsync("https://localhost:81/version_file.txt"))
            .Returns(Task.FromResult("version1"))
            .Verifiable();

        var updater = new Updater(downloaderMock.Object, null, null);

        var options = new UpdateOptions();
        options.UpdatesUri = "https://localhost:81";
        options.VersionFile = "version_file.txt";

        var version = await updater.GetLatestVersionAsync(options);

        Assert.That(version, Is.EqualTo("version1"));
        downloaderMock.Verify(t => t.DownloadStringAsync("https://localhost:81/version_file.txt"));
    }

    [TestCaseSource(nameof(TestJsonIndexResources))]
    public void GetIndexShould(string jsonIndexResource)
    {
        var json = AssemblyExtension.ReadResource(jsonIndexResource);

        var downloaderMock = new Mock<IDownloader>();
        downloaderMock.Setup(mock => mock.DownloadString("https://localhost/version2/index_file.json"))
            .Returns(json)
            .Verifiable();

        var updater = new Updater(downloaderMock.Object, null, null);

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

    [TestCaseSource(nameof(TestJsonIndexResources))]
    public async Task GetIndexAsyncShould(string jsonIndexResource)
    {
        var json = AssemblyExtension.ReadResource(jsonIndexResource);

        var downloaderMock = new Mock<IDownloader>();
        downloaderMock.Setup(mock => mock.DownloadStringAsync("https://localhost/version2/index_file.json"))
            .Returns(Task.FromResult(json))
            .Verifiable();

        var updater = new Updater(downloaderMock.Object, null, null);

        var options = new UpdateOptions();
        options.UpdatesUri = "https://localhost";
        options.IndexFile = "index_file.json";

        var archiveItems = await updater.GetIndexAsync(options, "version2");

        downloaderMock.Verify(t => t.DownloadStringAsync("https://localhost/version2/index_file.json"));

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

    [Test]
    public void GetArchiveItemsShouldReceive()
    {
        IEnumerable<IArchiveItem> archiveItems = new[]
        {
            new Mock<IArchiveItem>()
                .Do(x => x.SetupGet(y => y.Path).Returns("file1.txt"))
                .Do(x => x.SetupGet(y => y.Hash).Returns("1234"))
                .Do(x => x.SetupGet(y => y.Size).Returns(1234L))
                .Object,
            new Mock<IArchiveItem>()
                .Do(x => x.SetupGet(y => y.Path).Returns("dir/file2.txt"))
                .Do(x => x.SetupGet(y => y.Hash).Returns("123456"))
                .Do(x => x.SetupGet(y => y.Size).Returns(123456L))
                .Object
        };

        var downloaderMock = new Mock<IDownloader>();
        downloaderMock.Setup(mock => mock.DownloadFile(It.IsAny<string>(), It.IsAny<string>(), -1))
            .Verifiable();

        var updater = new Updater(downloaderMock.Object, null, null);

        var options = new UpdateOptions();
        options.UpdatesUri = "https://localhost";
        options.TempDir = Path.GetTempPath();
        options.DegreeOfParallelism = 2;

        updater.GetArchiveItems(options, "version1", archiveItems);

        downloaderMock.Verify(t =>
            t.DownloadFile("https://localhost/version1/1234", Path.Combine(options.TempDir, "1234"), -1));
        downloaderMock.Verify(t =>
            t.DownloadFile("https://localhost/version1/123456", Path.Combine(options.TempDir, "123456"), -1));
    }

    [Test]
    public async Task GetArchiveItemsAsyncShouldReceive()
    {
        IEnumerable<IArchiveItem> archiveItems = new[]
        {
            new Mock<IArchiveItem>()
                .Do(x => x.SetupGet(y => y.Path).Returns("file1.txt"))
                .Do(x => x.SetupGet(y => y.Hash).Returns("1234"))
                .Do(x => x.SetupGet(y => y.Size).Returns(1234L))
                .Object,
            new Mock<IArchiveItem>()
                .Do(x => x.SetupGet(y => y.Path).Returns("dir/file2.txt"))
                .Do(x => x.SetupGet(y => y.Hash).Returns("123456"))
                .Do(x => x.SetupGet(y => y.Size).Returns(123456L))
                .Object
        };

        var downloaderMock = new Mock<IDownloader>();
        downloaderMock.Setup(mock => mock.DownloadFileAsync(It.IsAny<string>(), It.IsAny<string>(), -1))
            .Verifiable();

        var updater = new Updater(downloaderMock.Object, null, null);

        var options = new UpdateOptions();
        options.UpdatesUri = "https://localhost";
        options.TempDir = Path.GetTempPath();
        options.DegreeOfParallelism = 2;

        await updater.GetArchiveItemsAsync(options, "version1", archiveItems);

        downloaderMock.Verify(t =>
            t.DownloadFileAsync("https://localhost/version1/1234", Path.Combine(options.TempDir, "1234"), -1));
        downloaderMock.Verify(t =>
            t.DownloadFileAsync("https://localhost/version1/123456", Path.Combine(options.TempDir, "123456"), -1));
    }

    [Test]
    public void GetArchiveItemsShouldSkipDuplicates()
    {
        IEnumerable<IArchiveItem> archiveItems = new[]
        {
            new Mock<IArchiveItem>()
                .Do(x => x.SetupGet(y => y.Path).Returns("file1.txt"))
                .Do(x => x.SetupGet(y => y.Hash).Returns("1234"))
                .Do(x => x.SetupGet(y => y.Size).Returns(1234L))
                .Object,
            new Mock<IArchiveItem>()
                .Do(x => x.SetupGet(y => y.Path).Returns("dir/file2.txt"))
                .Do(x => x.SetupGet(y => y.Hash).Returns("123456"))
                .Do(x => x.SetupGet(y => y.Size).Returns(123456L))
                .Object,
            new Mock<IArchiveItem>()
                .Do(x => x.SetupGet(y => y.Path).Returns("file1_another.txt"))
                .Do(x => x.SetupGet(y => y.Hash).Returns("1234"))
                .Do(x => x.SetupGet(y => y.Size).Returns(1234L))
                .Object,
            new Mock<IArchiveItem>()
                .Do(x => x.SetupGet(y => y.Path).Returns("dir/file2_new.txt"))
                .Do(x => x.SetupGet(y => y.Hash).Returns("123456"))
                .Do(x => x.SetupGet(y => y.Size).Returns(123456L))
                .Object,
            new Mock<IArchiveItem>()
                .Do(x => x.SetupGet(y => y.Path).Returns("dir/file3.txt"))
                .Do(x => x.SetupGet(y => y.Hash).Returns("12345678"))
                .Do(x => x.SetupGet(y => y.Size).Returns(123456L))
                .Object
        };

        var downloaderMock = new Mock<IDownloader>();
        downloaderMock.Setup(mock => mock.DownloadFile(It.IsAny<string>(), It.IsAny<string>(), -1))
            .Verifiable();

        var updater = new Updater(downloaderMock.Object, null, null);

        var options = new UpdateOptions();
        options.UpdatesUri = "https://localhost";
        options.TempDir = Path.GetTempPath();
        options.DegreeOfParallelism = 2;

        updater.GetArchiveItems(options, "version1", archiveItems);

        downloaderMock.Verify(t =>
                t.DownloadFile("https://localhost/version1/1234", Path.Combine(options.TempDir, "1234"), -1),
            Times.Once);
        downloaderMock.Verify(t =>
                t.DownloadFile("https://localhost/version1/123456", Path.Combine(options.TempDir, "123456"), -1),
            Times.Once);
        downloaderMock.Verify(t =>
                t.DownloadFile("https://localhost/version1/12345678", Path.Combine(options.TempDir, "12345678"), -1),
            Times.Once);
    }

    [Test]
    public void ApplyArchiveItemsShould()
    {
        IEnumerable<IArchiveItem> archiveItems = new[]
        {
            new Mock<IArchiveItem>()
                .Do(x => x.SetupGet(y => y.Path).Returns("file1.txt"))
                .Do(x => x.SetupGet(y => y.Hash).Returns("1234"))
                .Do(x => x.SetupGet(y => y.Size).Returns(1234L))
                .Object,
            new Mock<IArchiveItem>()
                .Do(x => x.SetupGet(y => y.Path).Returns("dir/file2.txt"))
                .Do(x => x.SetupGet(y => y.Hash).Returns("123456"))
                .Do(x => x.SetupGet(y => y.Size).Returns(123456L))
                .Object
        };

        var unpackerMock = new Mock<IUnpacker>();
        unpackerMock.Setup(mock => mock.Unpack(It.IsAny<string>(), It.IsAny<string>()))
            .Verifiable();

        var updater = new Updater(null, unpackerMock.Object, null);

        var options = new UpdateOptions
        {
            UpdatesUri = "https://localhost",
            TargetDir = @"C:\Target".AdjustSeparator(),
            TempDir = @"C:\Temp".AdjustSeparator()
        };

        updater.ApplyArchiveItems(options, archiveItems);

        unpackerMock.Verify(t =>
            t.Unpack(
                @"C:\Temp\1234".AdjustSeparator(),
                @"C:\Target\file1.txt".AdjustSeparator()));
        unpackerMock.Verify(t =>
            t.Unpack(
                @"C:\Temp\123456".AdjustSeparator(),
                @"C:\Target\dir\file2.txt".AdjustSeparator()));
    }

    [Test]
    public async Task ApplyArchiveItemsAsyncShould()
    {
        IEnumerable<IArchiveItem> archiveItems = new[]
        {
            new Mock<IArchiveItem>()
                .Do(x => x.SetupGet(y => y.Path).Returns("file1.txt"))
                .Do(x => x.SetupGet(y => y.Hash).Returns("1234"))
                .Do(x => x.SetupGet(y => y.Size).Returns(1234L))
                .Object,
            new Mock<IArchiveItem>()
                .Do(x => x.SetupGet(y => y.Path).Returns("dir/file2.txt"))
                .Do(x => x.SetupGet(y => y.Hash).Returns("123456"))
                .Do(x => x.SetupGet(y => y.Size).Returns(123456L))
                .Object
        };

        var unpackerMock = new Mock<IUnpacker>();
        unpackerMock.Setup(mock => mock.UnpackAsync(It.IsAny<string>(), It.IsAny<string>()))
            .Verifiable();

        var updater = new Updater(null, unpackerMock.Object, null);

        var options = new UpdateOptions
        {
            UpdatesUri = "https://localhost",
            TargetDir = @"C:\Target".AdjustSeparator(),
            TempDir = @"C:\Temp".AdjustSeparator()
        };

        await updater.ApplyArchiveItemsAsync(options, archiveItems);

        unpackerMock.Verify(t =>
            t.UnpackAsync(
                @"C:\Temp\1234".AdjustSeparator(),
                @"C:\Target\file1.txt".AdjustSeparator()));
        unpackerMock.Verify(t =>
            t.UnpackAsync(
                @"C:\Temp\123456".AdjustSeparator(),
                @"C:\Target\dir\file2.txt".AdjustSeparator()));
    }

    [Test]
    public void CleanupObsoleteShould()
    {
        IEnumerable<IFileItem> obsoleteItems = new[]
        {
            new Mock<IFileItem>()
                .Do(x => x.SetupGet(y => y.Path).Returns("file1.txt"))
                .Do(x => x.SetupGet(y => y.Hash).Returns("1234"))
                .Do(x => x.SetupGet(y => y.Size).Returns(1234L))
                .Object,
            new Mock<IFileItem>()
                .Do(x => x.SetupGet(y => y.Path).Returns("dir/file2.txt"))
                .Do(x => x.SetupGet(y => y.Hash).Returns("123456"))
                .Do(x => x.SetupGet(y => y.Size).Returns(123456L))
                .Object
        };

        var removerMock = new Mock<IRemover>();
        removerMock.Setup(mock => mock.RemoveFile(It.IsAny<string>()))
            .Verifiable();
        removerMock.Setup(mock => mock.RemoveEmptyDirs(It.IsAny<string>()))
            .Verifiable();

        var updater = new Updater(null, null, removerMock.Object);

        var options = new UpdateOptions();
        options.TargetDir = @"C:\Windows\Temp".AdjustSeparator();

        updater.CleanupObsoleteItems(options, obsoleteItems);

        removerMock.Verify(t =>
            t.RemoveFile(@"C:\Windows\Temp\file1.txt".AdjustSeparator()));
        removerMock.Verify(t =>
            t.RemoveFile(@"C:\Windows\Temp\dir\file2.txt".AdjustSeparator()));
        removerMock.Verify(t =>
            t.RemoveEmptyDirs(options.TargetDir));
    }

    [Test]
    public async Task CleanupObsoleteAsyncShould()
    {
        IEnumerable<IFileItem> obsoleteItems = new[]
        {
            new Mock<IFileItem>()
                .Do(x => x.SetupGet(y => y.Path).Returns("file1.txt"))
                .Do(x => x.SetupGet(y => y.Hash).Returns("1234"))
                .Do(x => x.SetupGet(y => y.Size).Returns(1234L))
                .Object,
            new Mock<IFileItem>()
                .Do(x => x.SetupGet(y => y.Path).Returns("dir/file2.txt"))
                .Do(x => x.SetupGet(y => y.Hash).Returns("123456"))
                .Do(x => x.SetupGet(y => y.Size).Returns(123456L))
                .Object
        };

        var removerMock = new Mock<IRemover>();
        removerMock.Setup(mock => mock.RemoveFileAsync(It.IsAny<string>()))
            .Verifiable();
        removerMock.Setup(mock => mock.RemoveEmptyDirsAsync(It.IsAny<string>()))
            .Verifiable();

        var updater = new Updater(null, null, removerMock.Object);

        var options = new UpdateOptions();
        options.TargetDir = @"C:\Windows\Temp".AdjustSeparator();

        await updater.CleanupObsoleteItemsAsync(options, obsoleteItems);

        removerMock.Verify(t =>
            t.RemoveFileAsync(@"C:\Windows\Temp\file1.txt".AdjustSeparator()));
        removerMock.Verify(t =>
            t.RemoveFileAsync(@"C:\Windows\Temp\dir\file2.txt".AdjustSeparator()));
        removerMock.Verify(t =>
            t.RemoveEmptyDirsAsync(options.TargetDir));
    }

    private static IEnumerable<string> TestJsonIndexResources()
    {
        var names = AssemblyExtension.GetEmbeddedResourceNames(x => x.Contains("Samples.JsonIndex"));
        return names;
    }
}