using LibUpdater.Utils;

namespace LibUpdater.Tests.Utils;

internal class PathNormalizerTests
{
#if _WINDOWS
    [TestCase(@"C:\Temp\file.txt", @"C:\Temp\file.txt")]
    [TestCase(@"C:\Temp\\file.txt", @"C:\Temp\file.txt")]
    [TestCase(@"C:\Temp/file.txt", @"C:\Temp\file.txt")]
    [TestCase(@"C:\Temp//file.txt", @"C:\Temp\file.txt")]
    [TestCase(@"C:/Temp///file.txt", @"C:\Temp\file.txt")]
    [TestCase(@"C:/Temp/../Temp/file.txt", @"C:\Temp\file.txt")]
#else
    [Ignore("Not implemented.")]
#endif
    public void AdjustSeparatorShould(string source, string expected)
    {
        var actual = source.AdjustSeparator();

        Assert.That(expected, Is.EqualTo(actual));
    }

    [TestCase(@"C:\Temp\file.txt", @"C:\Users", @"C:\Temp\file.txt")]
    [TestCase(@"file.txt", @"C:\Users", @"C:\Users\file.txt")]
    public void AdjustParentShould(string value, string parent, string expected)
    {
        var actual = value.AdjustParent(parent);

        Assert.That(expected, Is.EqualTo(actual));
    }
}