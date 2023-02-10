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
    public void AdjustShould(string source, string expected)
    {
        var actual = source.AdjustDirSeparator();

        Assert.That(expected, Is.EqualTo(actual));
    }
}