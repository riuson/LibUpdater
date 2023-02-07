using System.Reflection;

namespace LibUpdater.Tests.Utils
{
    internal class DirectoryScannerTests
    {
        [Test]
        public void DirectoryScannerShouldScanTree()
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