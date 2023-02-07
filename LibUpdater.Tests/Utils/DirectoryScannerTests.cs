using System.Reflection;
using LibUpdater.Utils;

namespace LibUpdater.Tests.Utils
{
    internal class DirectoryScannerTests
    {
        [Test]
        public void DirectoryScannerShouldScan()
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var samplesPath = Path.Combine(assemblyPath, "Samples2", "DirStruct");

            var scanner = new DirectoryScanner();

            var items = scanner.ScanTree(samplesPath);

            Assert.That(items.Count(), Is.EqualTo(4));
            Assert.That(items.Any(x => x.Path.Contains(@"DirStruct\file.txt")), Is.True);
            Assert.That(items.Any(x => x.Path.Contains(@"DirStruct\sub1\file1.txt")), Is.True);
            Assert.That(items.Any(x => x.Path.Contains(@"DirStruct\sub2\file2.txt")), Is.True);
            Assert.That(items.Any(x => x.Path.Contains(@"DirStruct\sub2\sub3\file3.txt")), Is.True);
        }

        [Test]
        public void DirectoryScannerShouldSort()
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var samplesPath = Path.Combine(assemblyPath, "Samples2", "DirStruct");

            var scanner = new DirectoryScanner();

            var items = scanner.ScanTree(samplesPath);

            var sortedItems = items.OrderBy(x => x.Path);

            Assert.That(items, Is.EqualTo(sortedItems));
        }
    }
}