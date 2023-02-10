using LibUpdater.Utils;

namespace LibUpdater.Tests.Utils;

internal class UnpackerTests
{
    [Test]
    public void ShouldUnpackStream()
    {
        var names = AssemblyExtension.GetEmbeddedResourceNames(x => x.Contains("Samples.Archives"));

        var unpackedResourceName = names.First(x => x.Contains(".txt"));
        var packedResourceName = names.First(x => x.Contains(".7z"));
        var unpackedStreamActual = new MemoryStream();
        var unpacker = new Unpacker();

        unpacker.Unpack(
            AssemblyExtension.GetEmbeddedResource(packedResourceName),
            unpackedStreamActual);

        var unpackedActual = new byte[unpackedStreamActual.Length];
        unpackedStreamActual.Position = 0;
        unpackedStreamActual.Read(unpackedActual, 0, unpackedActual.Length);

        var unpackedStreamExpected = AssemblyExtension.GetEmbeddedResource(unpackedResourceName);
        var unpackedExpected = new byte[unpackedStreamExpected.Length];
        unpackedStreamExpected.Read(unpackedExpected, 0, unpackedExpected.Length);

        Assert.That(unpackedActual, Is.EqualTo(unpackedExpected));
    }

    [Test]
    public async Task ShouldUnpackStreamAsync()
    {
        var names = AssemblyExtension.GetEmbeddedResourceNames(x => x.Contains("Samples.Archives"));

        var unpackedResourceName = names.First(x => x.Contains(".txt"));
        var packedResourceName = names.First(x => x.Contains(".7z"));
        var unpackedStreamActual = new MemoryStream();
        var unpacker = new Unpacker();

        await unpacker.UnpackAsync(
            AssemblyExtension.GetEmbeddedResource(packedResourceName),
            unpackedStreamActual);

        var unpackedActual = new byte[unpackedStreamActual.Length];
        unpackedStreamActual.Position = 0;
        unpackedStreamActual.Read(unpackedActual, 0, unpackedActual.Length);

        var unpackedStreamExpected = AssemblyExtension.GetEmbeddedResource(unpackedResourceName);
        var unpackedExpected = new byte[unpackedStreamExpected.Length];
        unpackedStreamExpected.Read(unpackedExpected, 0, unpackedExpected.Length);

        Assert.That(unpackedActual, Is.EqualTo(unpackedExpected));
    }
}