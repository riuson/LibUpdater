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
                    .Do(x => x.SetupGet(y => y.UnpackPath).Returns("file1.txt"))
                    .Do(x => x.SetupGet(y => y.ArchiveHash).Returns("abcd"))
                    .Do(x => x.SetupGet(y => y.ArchiveSize).Returns(564564L))
                    .Object,
                new Mock<IArchiveItem>()
                    .Do(x => x.SetupGet(y => y.Path).Returns("dir/file2.txt"))
                    .Do(x => x.SetupGet(y => y.Hash).Returns("123456"))
                    .Do(x => x.SetupGet(y => y.Size).Returns(123456L))
                    .Do(x => x.SetupGet(y => y.UnpackPath).Returns("dir/file2.txt"))
                    .Do(x => x.SetupGet(y => y.ArchiveHash).Returns("abcd"))
                    .Do(x => x.SetupGet(y => y.ArchiveSize).Returns(564564L))
                    .Object,
            };

            var result = analyzer.Analyze(
                targetDirectory: this._samplesPath,
                localItems: localItems,
                remoteItems: remoteItems);

            Assert.That(result.IsEquals, Is.True);
        }
    }
}