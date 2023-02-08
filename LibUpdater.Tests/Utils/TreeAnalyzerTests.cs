using System.Reflection;
using Moq;
using LibUpdater.Data;

namespace LibUpdater.Tests.Utils
{
    internal class TreeAnalyzerTests
    {
        private string _samplesPath;

        [SetUp]
        public void Setup()
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            this._samplesPath = Path.Combine(assemblyPath, "Samples2", "DirStruct");
        }

        [Test]
        public void AnalyzerShouldDetectEquality()
        {
            var analyzer = new TreeAnalyzer();

            IEnumerable<IFileItem> localItems = new[]
            {
                new Mock<IFileItem>()
                    .Do(x => x.SetupGet(y => y.Path).Returns("file1.txt"))
                    .Do(x => x.SetupGet(y => y.Hash).Returns("1234"))
                    .Do(x => x.SetupGet(y => y.Size).Returns(1234L))
                    .Object,
                new Mock<IFileItem>()
                    .Do(x => x.SetupGet(y => y.Path).Returns("dir\\file2.txt"))
                    .Do(x => x.SetupGet(y => y.Hash).Returns("123456"))
                    .Do(x => x.SetupGet(y => y.Size).Returns(123456L))
                    .Object,
            };

            IEnumerable<IArchiveItem> remoteItems = new[]
            {
                new Mock<IArchiveItem>()
                    .Do(x => x.SetupGet(y => y.Path).Returns("file1.txt"))
                    .Do(x => x.SetupGet(y => y.Hash).Returns("1234"))
                    .Do(x => x.SetupGet(y => y.Size).Returns(1234L))
                    .Object,
                new Mock<IArchiveItem>()
                    .Do(x => x.SetupGet(y => y.Path).Returns("dir/file2.txt"))
                    .Do(x => x.SetupGet(y => y.Hash).Returns("123456"))
                    .Do(x => x.SetupGet(y => y.Size).Returns(123456L))
                    .Object,
            };

            var result = analyzer.Analyze(
                targetDirectory: this._samplesPath,
                localItems: localItems,
                remoteItems: remoteItems);

            Assert.That(result.IsEquals, Is.True);
        }

        [Test]
        public void AnalyzerShouldDetectEqualityUnordered()
        {
            var analyzer = new TreeAnalyzer();

            IEnumerable<IFileItem> localItems = new[]
            {
                new Mock<IFileItem>()
                    .Do(x => x.SetupGet(y => y.Path).Returns("file1.txt"))
                    .Do(x => x.SetupGet(y => y.Hash).Returns("1234"))
                    .Do(x => x.SetupGet(y => y.Size).Returns(1234L))
                    .Object,
                new Mock<IFileItem>()
                    .Do(x => x.SetupGet(y => y.Path).Returns("dir\\file2.txt"))
                    .Do(x => x.SetupGet(y => y.Hash).Returns("123456"))
                    .Do(x => x.SetupGet(y => y.Size).Returns(123456L))
                    .Object,
            };

            IEnumerable<IArchiveItem> remoteItems = new[]
            {
                new Mock<IArchiveItem>()
                    .Do(x => x.SetupGet(y => y.Path).Returns("dir/file2.txt"))
                    .Do(x => x.SetupGet(y => y.Hash).Returns("123456"))
                    .Do(x => x.SetupGet(y => y.Size).Returns(123456L))
                    .Object,
                new Mock<IArchiveItem>()
                    .Do(x => x.SetupGet(y => y.Path).Returns("file1.txt"))
                    .Do(x => x.SetupGet(y => y.Hash).Returns("1234"))
                    .Do(x => x.SetupGet(y => y.Size).Returns(1234L))
                    .Object,
            };

            var result = analyzer.Analyze(
                targetDirectory: this._samplesPath,
                localItems: localItems,
                remoteItems: remoteItems);

            Assert.That(result.IsEquals, Is.True);
        }

        [Test]
        public void AnalyzerShouldDetectNonEqualityName()
        {
            var analyzer = new TreeAnalyzer();

            IEnumerable<IFileItem> localItems = new[]
            {
                new Mock<IFileItem>()
                    .Do(x => x.SetupGet(y => y.Path).Returns("file1.txt"))
                    .Do(x => x.SetupGet(y => y.Hash).Returns("1234"))
                    .Do(x => x.SetupGet(y => y.Size).Returns(1234L))
                    .Object,
                new Mock<IFileItem>()
                    .Do(x => x.SetupGet(y => y.Path).Returns("dir\\file2.txt"))
                    .Do(x => x.SetupGet(y => y.Hash).Returns("123456"))
                    .Do(x => x.SetupGet(y => y.Size).Returns(123456L))
                    .Object,
            };

            IEnumerable<IArchiveItem> remoteItems = new[]
            {
                new Mock<IArchiveItem>()
                    .Do(x => x.SetupGet(y => y.Path).Returns("filel.txt"))
                    .Do(x => x.SetupGet(y => y.Hash).Returns("1234"))
                    .Do(x => x.SetupGet(y => y.Size).Returns(1234L))
                    .Object,
                new Mock<IArchiveItem>()
                    .Do(x => x.SetupGet(y => y.Path).Returns("dir/file2.txt"))
                    .Do(x => x.SetupGet(y => y.Hash).Returns("123456"))
                    .Do(x => x.SetupGet(y => y.Size).Returns(123456L))
                    .Object,
            };

            var result = analyzer.Analyze(
                targetDirectory: this._samplesPath,
                localItems: localItems,
                remoteItems: remoteItems);

            Assert.That(result.IsEquals, Is.False);
        }

        [Test]
        public void AnalyzerShouldDetectNonEqualitySize()
        {
            var analyzer = new TreeAnalyzer();

            IEnumerable<IFileItem> localItems = new[]
            {
                new Mock<IFileItem>()
                    .Do(x => x.SetupGet(y => y.Path).Returns("file1.txt"))
                    .Do(x => x.SetupGet(y => y.Hash).Returns("1234"))
                    .Do(x => x.SetupGet(y => y.Size).Returns(1234L))
                    .Object,
                new Mock<IFileItem>()
                    .Do(x => x.SetupGet(y => y.Path).Returns("dir\\file2.txt"))
                    .Do(x => x.SetupGet(y => y.Hash).Returns("123456"))
                    .Do(x => x.SetupGet(y => y.Size).Returns(123455L))
                    .Object,
            };

            IEnumerable<IArchiveItem> remoteItems = new[]
            {
                new Mock<IArchiveItem>()
                    .Do(x => x.SetupGet(y => y.Path).Returns("file1.txt"))
                    .Do(x => x.SetupGet(y => y.Hash).Returns("1234"))
                    .Do(x => x.SetupGet(y => y.Size).Returns(1236L))
                    .Object,
                new Mock<IArchiveItem>()
                    .Do(x => x.SetupGet(y => y.Path).Returns("dir/file2.txt"))
                    .Do(x => x.SetupGet(y => y.Hash).Returns("123456"))
                    .Do(x => x.SetupGet(y => y.Size).Returns(123456L))
                    .Object,
            };

            var result = analyzer.Analyze(
                targetDirectory: this._samplesPath,
                localItems: localItems,
                remoteItems: remoteItems);

            Assert.That(result.IsEquals, Is.False);
        }

        [Test]
        public void AnalyzerShouldDetectNonEqualityHash()
        {
            var analyzer = new TreeAnalyzer();

            IEnumerable<IFileItem> localItems = new[]
            {
                new Mock<IFileItem>()
                    .Do(x => x.SetupGet(y => y.Path).Returns("file1.txt"))
                    .Do(x => x.SetupGet(y => y.Hash).Returns("1234a"))
                    .Do(x => x.SetupGet(y => y.Size).Returns(1234L))
                    .Object,
                new Mock<IFileItem>()
                    .Do(x => x.SetupGet(y => y.Path).Returns("dir\\file2.txt"))
                    .Do(x => x.SetupGet(y => y.Hash).Returns("123456"))
                    .Do(x => x.SetupGet(y => y.Size).Returns(123456L))
                    .Object,
            };

            IEnumerable<IArchiveItem> remoteItems = new[]
            {
                new Mock<IArchiveItem>()
                    .Do(x => x.SetupGet(y => y.Path).Returns("filel.txt"))
                    .Do(x => x.SetupGet(y => y.Hash).Returns("1234b"))
                    .Do(x => x.SetupGet(y => y.Size).Returns(1234L))
                    .Object,
                new Mock<IArchiveItem>()
                    .Do(x => x.SetupGet(y => y.Path).Returns("dir/file2.txt"))
                    .Do(x => x.SetupGet(y => y.Hash).Returns("123456"))
                    .Do(x => x.SetupGet(y => y.Size).Returns(123456L))
                    .Object,
            };

            var result = analyzer.Analyze(
                targetDirectory: this._samplesPath,
                localItems: localItems,
                remoteItems: remoteItems);

            Assert.That(result.IsEquals, Is.False);
        }

        [Test]
        public void AnalyzerShouldDetectObsoleteFiles()
        {
            var analyzer = new TreeAnalyzer();

            IEnumerable<IFileItem> localItems = new[]
            {
                new Mock<IFileItem>()
                    .Do(x => x.SetupGet(y => y.Path).Returns("file1.txt"))
                    .Do(x => x.SetupGet(y => y.Hash).Returns("1234"))
                    .Do(x => x.SetupGet(y => y.Size).Returns(1234L))
                    .Object,
                new Mock<IFileItem>()
                    .Do(x => x.SetupGet(y => y.Path).Returns("dir\\file2.txt"))
                    .Do(x => x.SetupGet(y => y.Hash).Returns("123456"))
                    .Do(x => x.SetupGet(y => y.Size).Returns(123456L))
                    .Object,
            };

            IEnumerable<IArchiveItem> remoteItems = new[]
            {
                new Mock<IArchiveItem>()
                    .Do(x => x.SetupGet(y => y.Path).Returns("dir/file2.txt"))
                    .Do(x => x.SetupGet(y => y.Hash).Returns("123456"))
                    .Do(x => x.SetupGet(y => y.Size).Returns(123456L))
                    .Object,
            };

            var result = analyzer.Analyze(
                targetDirectory: this._samplesPath,
                localItems: localItems,
                remoteItems: remoteItems);

            Assert.That(result.IsEquals, Is.False);
            Assert.That(result.Added.Count(), Is.EqualTo(0));
            Assert.That(result.Obsolete.Count(), Is.EqualTo(1));
            Assert.That(result.Obsolete.ElementAt(0), Is.EqualTo(localItems.ElementAt(0)));
        }

        [Test]
        public void AnalyzerShouldDetectNewFiles()
        {
            var analyzer = new TreeAnalyzer();

            IEnumerable<IFileItem> localItems = new[]
            {
                new Mock<IFileItem>()
                    .Do(x => x.SetupGet(y => y.Path).Returns("file1.txt"))
                    .Do(x => x.SetupGet(y => y.Hash).Returns("1234"))
                    .Do(x => x.SetupGet(y => y.Size).Returns(1234L))
                    .Object,
                new Mock<IFileItem>()
                    .Do(x => x.SetupGet(y => y.Path).Returns("dir\\file2.txt"))
                    .Do(x => x.SetupGet(y => y.Hash).Returns("123456"))
                    .Do(x => x.SetupGet(y => y.Size).Returns(123456L))
                    .Object,
            };

            IEnumerable<IArchiveItem> remoteItems = new[]
            {
                new Mock<IArchiveItem>()
                    .Do(x => x.SetupGet(y => y.Path).Returns("file1.txt"))
                    .Do(x => x.SetupGet(y => y.Hash).Returns("1234"))
                    .Do(x => x.SetupGet(y => y.Size).Returns(1234L))
                    .Object,
                new Mock<IArchiveItem>()
                    .Do(x => x.SetupGet(y => y.Path).Returns("dir/file2.txt"))
                    .Do(x => x.SetupGet(y => y.Hash).Returns("123456"))
                    .Do(x => x.SetupGet(y => y.Size).Returns(123456L))
                    .Object,
                new Mock<IArchiveItem>()
                    .Do(x => x.SetupGet(y => y.Path).Returns("dir2/file3.txt"))
                    .Do(x => x.SetupGet(y => y.Hash).Returns("146546546"))
                    .Do(x => x.SetupGet(y => y.Size).Returns(23131L))
                    .Object,
            };

            var result = analyzer.Analyze(
                targetDirectory: this._samplesPath,
                localItems: localItems,
                remoteItems: remoteItems);

            Assert.That(result.IsEquals, Is.False);
            Assert.That(result.Added.Count(), Is.EqualTo(1));
            Assert.That(result.Obsolete.Count(), Is.EqualTo(0));
            Assert.That(result.Added.ElementAt(0), Is.EqualTo(remoteItems.ElementAt(2)));
        }
    }
}