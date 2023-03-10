using LibUpdater.Utils;

namespace LibUpdater.Tests.Utils;

internal class RemoverTests
{
    [Flags]
    public enum CleanChildsItem
    {
        None = 0,
        RootDir = 1 << 1,
        SubDir1 = 1 << 2,
        SubDir2 = 1 << 3
    }

    [Flags]
    public enum KeepItem
    {
        None = 0,
        File = 1 << 0,
        File1 = 1 << 1,
        File2 = 1 << 2,
        RootDir = 1 << 3,
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

    [TestCaseSource(nameof(TestItemsSelectiveDelete))]
    public void RemoveFileShould((RemoveItem remove, KeepItem keep) item)
    {
        var remover = new Remover();

        if ((item.remove & RemoveItem.File) == RemoveItem.File)
            remover.RemoveFile(_file.FullName);

        if ((item.remove & RemoveItem.File1) == RemoveItem.File1)
            remover.RemoveFile(_file1.FullName);

        if ((item.remove & RemoveItem.File2) == RemoveItem.File2)
            remover.RemoveFile(_file2.FullName);

        Assert.That(_file.Exists, Is.EqualTo((item.keep & KeepItem.File) == KeepItem.File));
        Assert.That(_file1.Exists, Is.EqualTo((item.keep & KeepItem.File1) == KeepItem.File1));
        Assert.That(_file2.Exists, Is.EqualTo((item.keep & KeepItem.File2) == KeepItem.File2));
    }

    [TestCaseSource(nameof(TestItemsSelectiveDelete))]
    public async Task RemoveFileAsyncShould((RemoveItem remove, KeepItem keep) item)
    {
        var remover = new Remover();

        if ((item.remove & RemoveItem.File) == RemoveItem.File)
            await remover.RemoveFileAsync(_file.FullName);

        if ((item.remove & RemoveItem.File1) == RemoveItem.File1)
            await remover.RemoveFileAsync(_file1.FullName);

        if ((item.remove & RemoveItem.File2) == RemoveItem.File2)
            await remover.RemoveFileAsync(_file2.FullName);

        Assert.That(_file.Exists, Is.EqualTo((item.keep & KeepItem.File) == KeepItem.File));
        Assert.That(_file1.Exists, Is.EqualTo((item.keep & KeepItem.File1) == KeepItem.File1));
        Assert.That(_file2.Exists, Is.EqualTo((item.keep & KeepItem.File2) == KeepItem.File2));
    }

    [TestCaseSource(nameof(TestItemsSelectiveDelete))]
    public void RemoveEmptyDirsShould((RemoveItem remove, KeepItem keep) item)
    {
        var remover = new Remover();

        if ((item.remove & RemoveItem.File) == RemoveItem.File)
            remover.RemoveFile(_file.FullName);

        if ((item.remove & RemoveItem.File1) == RemoveItem.File1)
            remover.RemoveFile(_file1.FullName);

        if ((item.remove & RemoveItem.File2) == RemoveItem.File2)
            remover.RemoveFile(_file2.FullName);

        remover.RemoveEmptyDirs(_tempDir.FullName);

        Assert.That(_tempDir.Exists, Is.EqualTo((item.keep & KeepItem.RootDir) == KeepItem.RootDir));
        Assert.That(_subDir1.Exists, Is.EqualTo((item.keep & KeepItem.SubDir1) == KeepItem.SubDir1));
        Assert.That(_subDir2.Exists, Is.EqualTo((item.keep & KeepItem.SubDir2) == KeepItem.SubDir2));
    }

    [TestCaseSource(nameof(TestItemsSelectiveDelete))]
    public async Task RemoveEmptyDirsAsyncShould((RemoveItem remove, KeepItem keep) item)
    {
        var remover = new Remover();

        if ((item.remove & RemoveItem.File) == RemoveItem.File)
            await remover.RemoveFileAsync(_file.FullName);

        if ((item.remove & RemoveItem.File1) == RemoveItem.File1)
            await remover.RemoveFileAsync(_file1.FullName);

        if ((item.remove & RemoveItem.File2) == RemoveItem.File2)
            await remover.RemoveFileAsync(_file2.FullName);

        await remover.RemoveEmptyDirsAsync(_tempDir.FullName);

        Assert.That(_tempDir.Exists, Is.EqualTo((item.keep & KeepItem.RootDir) == KeepItem.RootDir));
        Assert.That(_subDir1.Exists, Is.EqualTo((item.keep & KeepItem.SubDir1) == KeepItem.SubDir1));
        Assert.That(_subDir2.Exists, Is.EqualTo((item.keep & KeepItem.SubDir2) == KeepItem.SubDir2));
    }

    [TestCaseSource(nameof(TestItemsChildsDelete))]
    public void RemoveChildsShould((CleanChildsItem clean, KeepItem keep) item)
    {
        var remover = new Remover();

        if ((item.clean & CleanChildsItem.SubDir1) == CleanChildsItem.SubDir1)
            remover.RemoveChilds(_subDir1.FullName);

        if ((item.clean & CleanChildsItem.SubDir2) == CleanChildsItem.SubDir2)
            remover.RemoveChilds(_subDir2.FullName);

        if ((item.clean & CleanChildsItem.RootDir) == CleanChildsItem.RootDir)
            remover.RemoveChilds(_tempDir.FullName);

        Assert.That(_tempDir.Exists, Is.EqualTo((item.keep & KeepItem.RootDir) == KeepItem.RootDir));
        Assert.That(_subDir1.Exists, Is.EqualTo((item.keep & KeepItem.SubDir1) == KeepItem.SubDir1));
        Assert.That(_subDir2.Exists, Is.EqualTo((item.keep & KeepItem.SubDir2) == KeepItem.SubDir2));
    }

    [TestCaseSource(nameof(TestItemsChildsDelete))]
    public async Task RemoveChildsAsyncShould((CleanChildsItem clean, KeepItem keep) item)
    {
        var remover = new Remover();

        if ((item.clean & CleanChildsItem.SubDir1) == CleanChildsItem.SubDir1)
            remover.RemoveChilds(_subDir1.FullName);

        if ((item.clean & CleanChildsItem.SubDir2) == CleanChildsItem.SubDir2)
            remover.RemoveChilds(_subDir2.FullName);

        if ((item.clean & CleanChildsItem.RootDir) == CleanChildsItem.RootDir)
            remover.RemoveChilds(_tempDir.FullName);

        Assert.That(_tempDir.Exists, Is.EqualTo((item.keep & KeepItem.RootDir) == KeepItem.RootDir));
        Assert.That(_subDir1.Exists, Is.EqualTo((item.keep & KeepItem.SubDir1) == KeepItem.SubDir1));
        Assert.That(_subDir2.Exists, Is.EqualTo((item.keep & KeepItem.SubDir2) == KeepItem.SubDir2));
    }

    private static IEnumerable<(RemoveItem remove, KeepItem keep)> TestItemsSelectiveDelete()
    {
        yield return (RemoveItem.None,
            KeepItem.RootDir | KeepItem.SubDir1 | KeepItem.SubDir2 | KeepItem.File | KeepItem.File1 | KeepItem.File2);
        yield return (RemoveItem.File,
            KeepItem.RootDir | KeepItem.SubDir1 | KeepItem.SubDir2 | KeepItem.File1 | KeepItem.File2);
        yield return (RemoveItem.File1, KeepItem.RootDir | KeepItem.SubDir2 | KeepItem.File | KeepItem.File2);
        yield return (RemoveItem.File2, KeepItem.RootDir | KeepItem.SubDir1 | KeepItem.File | KeepItem.File1);
        yield return (RemoveItem.File1 | RemoveItem.File2, KeepItem.RootDir | KeepItem.File);
        yield return (RemoveItem.File | RemoveItem.File1 | RemoveItem.File2, KeepItem.RootDir);
    }

    private static IEnumerable<(CleanChildsItem clean, KeepItem keep)> TestItemsChildsDelete()
    {
        yield return (CleanChildsItem.None,
            KeepItem.RootDir | KeepItem.SubDir1 | KeepItem.SubDir2 | KeepItem.File | KeepItem.File1 | KeepItem.File2);
        yield return (CleanChildsItem.RootDir, KeepItem.RootDir);
        yield return (CleanChildsItem.SubDir1,
            KeepItem.RootDir | KeepItem.SubDir1 | KeepItem.SubDir2 | KeepItem.File | KeepItem.File2);
        yield return (CleanChildsItem.SubDir2,
            KeepItem.RootDir | KeepItem.SubDir1 | KeepItem.SubDir2 | KeepItem.File | KeepItem.File1);
        yield return (CleanChildsItem.SubDir1 | CleanChildsItem.SubDir2,
            KeepItem.RootDir | KeepItem.SubDir1 | KeepItem.SubDir2 | KeepItem.File);
    }
}