using LibUpdater.Utils;

namespace LibUpdater.Tests.Utils;

internal class IndexDecoderTests
{
    [TestCaseSource(nameof(TestItems))]
    public void IndexDecodeShouldDecodeJson(string resourceName)
    {
        var json = AssemblyExtension.ReadResource(resourceName);
        var decoder = new IndexDecoder();

        var items = decoder.Decode(json);

        Assert.That(items.Count(), Is.EqualTo(4));

        Assert.That(items.ElementAt(0).Path, Is.EqualTo("LibUpdater.dll"));
        Assert.That(items.ElementAt(0).Size, Is.EqualTo(10240));
        Assert.That(items.ElementAt(0).Hash, Is.EqualTo("6146e3bcd55200cdd8de2c8ba207ebb1ff717322"));
        Assert.That(items.ElementAt(0).ArchiveSize, Is.EqualTo(4278));
        Assert.That(items.ElementAt(0).ArchiveHash, Is.EqualTo("c150297e6efd6e8b59957d7e2544e9b874b806f7"));

        Assert.That(items.ElementAt(3).Path, Is.EqualTo("LibUpdater.Tests.pdb"));
        Assert.That(items.ElementAt(3).Size, Is.EqualTo(17932));
        Assert.That(items.ElementAt(3).Hash, Is.EqualTo("15b16d08f2e3bb1655f3f4aaf39429b856efc262"));
        Assert.That(items.ElementAt(3).ArchiveSize, Is.EqualTo(8929));
        Assert.That(items.ElementAt(3).ArchiveHash, Is.EqualTo("366c5aa66c6b31233ddf6c88c13561ab327b541a"));
    }

    private static IEnumerable<string> TestItems()
    {
        var names = AssemblyExtension.GetEmbeddedResourceNames(x => x.Contains("Samples.Json"));
        return names;
    }
}