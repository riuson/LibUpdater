using LibUpdater.Utils;

namespace LibUpdater.Tests.Utils;

internal class IndexDecoderTests
{
    [TestCaseSource(nameof(TestJsonIndexResources))]
    public void IndexDecodeShouldDecodeJson(string jsonIndexResource)
    {
        var json = AssemblyExtension.ReadResource(jsonIndexResource);
        var decoder = new IndexDecoder();

        var archiveItems = decoder.Decode(json);

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
    public async Task IndexDecodeShouldDecodeJsonAsync(string jsonIndexResource)
    {
        var jsonStream = AssemblyExtension.GetEmbeddedResource(jsonIndexResource);
        var decoder = new IndexDecoder();

        var archiveItems = await decoder.DecodeAsync(jsonStream);

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