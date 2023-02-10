using LibUpdater.Utils;

namespace LibUpdater.Tests.Utils;

internal class RemoverTests
{
    [Flags]
    public enum KeepItem
    {
        None = 0,
        File = 1 << 0,
        File1 = 1 << 1,
        File2 = 1 << 2,
        TempDir = 1 << 3,
        SubDir1 = 1 << 4,
        SubDir2 = 1 << 5
    }

    [Flags]
    public enum RemoveItem
    {
        None = 0,
        File = 1 << 0,
        File1 = 1 << 1,
        File2 = 1 << 2
    }

    private FileInfo _file;
    private FileInfo _file1;
    private FileInfo _file2;
    private DirectoryInfo _subDir1;
    private DirectoryInfo _subDir2;
    private DirectoryInfo _tempDir;

    [SetUp]
    public void SetUp()
    {
        _tempDir = new DirectoryInfo(
            Path.Combine(
                Path.GetTempPath(),
                Guid.NewGuid().ToString()));

        _tempDir.Create();

        _subDir1 = _tempDir.CreateSubdirectory("sub1");
        _subDir2 = _tempDir.CreateSubdirectory("sub2");

        _file = new FileInfo(
            Path.Combine(
                _tempDir.FullName,
                "file.txt"));
        using (var stream = _file.CreateText())
        {
            stream.WriteLine("file");
        }

        _file1 = new FileInfo(
            Path.Combine(
                _subDir1.FullName,
                "file1.txt"));
        using (var stream = _file1.CreateText())
        {
            stream.WriteLine("file1");
        }

        _file2 = new FileInfo(
            Path.Combine(
                _subDir2.FullName,
                "file2.txt"));
        using (var stream = _file2.CreateText())
        {
            stream.WriteLine("file2");
        }
    }

    [TearDown]
    public void TearDown()
    {
        if (_tempDir.Exists)
            _tempDir.Delete(true);
    }

    [TestCaseSource(nameof(TestItems))]
    public void ShouldRemove(
        RemoveItem remove,
        KeepItem keep)
    {
        var remover = new Remover();

        if ((remove & RemoveItem.File) == RemoveItem.File)
            remover.Remove(_file.FullName);

        if ((remove & RemoveItem.File1) == RemoveItem.File1)
            remover.Remove(_file1.FullName);

        if ((remove & RemoveItem.File2) == RemoveItem.File2)
            remover.Remove(_file2.FullName);

        Assert.That(_file.Exists, Is.EqualTo((keep & KeepItem.File) == KeepItem.File));
        Assert.That(_file1.Exists, Is.EqualTo((keep & KeepItem.File1) == KeepItem.File1));
        Assert.That(_file2.Exists, Is.EqualTo((keep & KeepItem.File2) == KeepItem.File2));

        // Order-depended. TODO Fix.
        Assert.That(_tempDir.Exists, Is.EqualTo((keep & KeepItem.TempDir) == KeepItem.TempDir));
        Assert.That(_subDir1.Exists, Is.EqualTo((keep & KeepItem.SubDir1) == KeepItem.SubDir1));
        Assert.That(_subDir2.Exists, Is.EqualTo((keep & KeepItem.SubDir2) == KeepItem.SubDir2));
    }

    [TestCaseSource(nameof(TestItems))]
    public async Task ShouldRemoveAsync(
        RemoveItem remove,
        KeepItem keep)
    {
        var remover = new Remover();

        if ((remove & RemoveItem.File) == RemoveItem.File)
            await remover.RemoveAsync(_file.FullName);

        if ((remove & RemoveItem.File1) == RemoveItem.File1)
            await remover.RemoveAsync(_file1.FullName);

        if ((remove & RemoveItem.File2) == RemoveItem.File2)
            await remover.RemoveAsync(_file2.FullName);

        Assert.That(_file.Exists, Is.EqualTo((keep & KeepItem.File) == KeepItem.File));
        Assert.That(_file1.Exists, Is.EqualTo((keep & KeepItem.File1) == KeepItem.File1));
        Assert.That(_file2.Exists, Is.EqualTo((keep & KeepItem.File2) == KeepItem.File2));

        // Order-depended. TODO Fix.
        Assert.That(_tempDir.Exists, Is.EqualTo((keep & KeepItem.TempDir) == KeepItem.TempDir));
        Assert.That(_subDir1.Exists, Is.EqualTo((keep & KeepItem.SubDir1) == KeepItem.SubDir1));
        Assert.That(_subDir2.Exists, Is.EqualTo((keep & KeepItem.SubDir2) == KeepItem.SubDir2));
    }

    private static IEnumerable<(RemoveItem remove, KeepItem keep)> TestItems()
    {
        yield return (RemoveItem.None,
            KeepItem.TempDir | KeepItem.SubDir1 | KeepItem.SubDir2 | KeepItem.File | KeepItem.File1 | KeepItem.File2);
        yield return (RemoveItem.File,
            KeepItem.TempDir | KeepItem.SubDir1 | KeepItem.SubDir2 | KeepItem.File1 | KeepItem.File2);
        yield return (RemoveItem.File1, KeepItem.TempDir | KeepItem.SubDir2 | KeepItem.File | KeepItem.File2);
        yield return (RemoveItem.File2, KeepItem.TempDir | KeepItem.SubDir1 | KeepItem.File | KeepItem.File1);
        yield return (RemoveItem.File1 | RemoveItem.File2, KeepItem.TempDir | KeepItem.File);
        yield return (RemoveItem.File | RemoveItem.File1 | RemoveItem.File2, KeepItem.None);
    }
}