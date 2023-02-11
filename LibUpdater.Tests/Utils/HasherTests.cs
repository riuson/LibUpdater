using System.Text.RegularExpressions;
using LibUpdater.Utils;

namespace LibUpdater.Tests.Utils;

internal class HasherTests
{
    [TestCaseSource(nameof(TestItems))]
    public void HasherShouldCalcHash((string resourceName, string hash) testItem)
    {
        var hasher = new Hasher();
        var resourceStream = AssemblyExtension.GetEmbeddedResource(testItem.resourceName);

        var hash = hasher.HashStream(resourceStream);

        Assert.That(hash, Is.EqualTo(testItem.hash));
    }

    [TestCaseSource(nameof(TestItems))]
    public async Task HasherShouldCalcHashAsync((string resourceName, string hash) testItem)
    {
        var hasher = new Hasher();
        var resourceStream = AssemblyExtension.GetEmbeddedResource(testItem.resourceName);

        var hash = await hasher.HashStreamAsync(resourceStream);

        Assert.That(hash, Is.EqualTo(testItem.hash));
    }

    private static IEnumerable<(string resourceName, string hash)> TestItems()
    {
        var names = AssemblyExtension.GetEmbeddedResourceNames(x => x.Contains("Samples.Hashing"));
        var reg = new Regex(@"[0-9a-f]{40}");

        foreach (var name in names)
        {
            var match = reg.Match(name);

            if (match.Success) yield return (name, match.Value.ToLower());
        }
    }
}