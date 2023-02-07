using System.Reflection;
using LibUpdater.Utils;

namespace LibUpdater.Tests.Utils
{
    internal class DirectoryScannerTests
    {
        private string samplesPath;

        [SetUp]
        public void Setup()
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            this.samplesPath = Path.Combine(assemblyPath, "Samples2", "DirStruct");
        }

        [Test]
        public void DirectoryScannerShouldScan()
        {
            var scanner = new DirectoryScanner();

            var items = scanner.ScanTree(this.samplesPath);

            Assert.That(items.Count(), Is.EqualTo(4));
            Assert.That(items.Any(x => x.Path.Contains(@"DirStruct\file.txt")), Is.True);
            Assert.That(items.Any(x => x.Path.Contains(@"DirStruct\sub1\file1.txt")), Is.True);
            Assert.That(items.Any(x => x.Path.Contains(@"DirStruct\sub2\file2.txt")), Is.True);
            Assert.That(items.Any(x => x.Path.Contains(@"DirStruct\sub2\sub3\file3.txt")), Is.True);
        }

        [Test]
        public void DirectoryScannerShouldSort()
        {
            var scanner = new DirectoryScanner();

            var items = scanner.ScanTree(this.samplesPath);

            var sortedItems = items.OrderBy(x => x.Path);

            Assert.That(items, Is.EqualTo(sortedItems));
        }

        [Test]
        public void DirectoryScannerShouldGetFileSize()
        {
            var scanner = new DirectoryScanner();

            var items = scanner.ScanTree(this.samplesPath);

            Assert.That(items.All(x => x.Size == 1), Is.True);
        }

        [Test]
        public void DirectoryScannerShouldGetFileHash()
        {
            var scanner = new DirectoryScanner();
            var expectedHash = "356a192b7913b04c54574d18c28d46e6395428ab"; // File with content "1".

            var items = scanner.ScanTree(this.samplesPath);

            Assert.That(items.All(x => x.Hash == expectedHash), Is.True);
        }
    }
}