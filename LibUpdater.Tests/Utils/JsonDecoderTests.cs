using LibUpdater.Utils;

namespace LibUpdater.Tests.Utils;

internal class JsonDecoderTests
{
    [TestCaseSource(nameof(TestJsonIndexResources))]
    public void DecodeIndexShould(string jsonIndexResource)
    {
        var json = AssemblyExtension.ReadResource(jsonIndexResource);
        var decoder = new JsonDecoder();

        var archiveItems = decoder.DecodeIndex(json);

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
    public async Task DecodeIndexAsyncShould(string jsonIndexResource)
    {
        var jsonStream = AssemblyExtension.GetEmbeddedResource(jsonIndexResource);
        var decoder = new JsonDecoder();

        var archiveItems = await decoder.DecodeIndexAsync(jsonStream);

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

    [TestCaseSource(nameof(TestJsonVersionResources))]
    public void DecodeVersionShould(string jsonVersionResource)
    {
        var json = AssemblyExtension.ReadResource(jsonVersionResource);
        var decoder = new JsonDecoder();

        var versionInfo = decoder.DecodeVersion(json);

        Assert.That(versionInfo.Version, Is.EqualTo(new Version(0, 1, 0, 0)));
        Assert.That(versionInfo.Path, Is.EqualTo("ver1"));
        Assert.That(versionInfo.Description, Is.EqualTo("Initial release"));
    }

    [TestCaseSource(nameof(TestJsonVersionResources))]
    public async Task DecodeVersionAsyncShould(string jsonVersionResource)
    {
        var jsonStream = AssemblyExtension.GetEmbeddedResource(jsonVersionResource);
        var decoder = new JsonDecoder();

        var versionInfo = await decoder.DecodeVersionAsync(jsonStream);

        Assert.That(versionInfo.Version, Is.EqualTo(new Version(0, 1, 0, 0)));
        Assert.That(versionInfo.Path, Is.EqualTo("ver1"));
        Assert.That(versionInfo.Description, Is.EqualTo("Initial release"));
    }

    private static IEnumerable<string> TestJsonIndexResources()
    {
        var names = AssemblyExtension.GetEmbeddedResourceNames(x => x.Contains("Samples.JsonIndex"));
        return names;
    }

    private static IEnumerable<string> TestJsonVersionResources()
    {
        var names = AssemblyExtension.GetEmbeddedResourceNames(x => x.Contains("Samples.JsonVersion"));
        return names;
    }
}