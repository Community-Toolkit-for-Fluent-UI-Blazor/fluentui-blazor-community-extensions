namespace FluentUI.Blazor.Community.Components.Tests.Components.FileManager;

public class FileManagerEntryTests
{
    private class Dummy { }

    [Fact]
    public void HomeDirectory_IsDirectoryAndNamedHome()
    {
        var home = FileManagerEntry<Dummy>.Home;
        Assert.True(home.IsDirectory);
        Assert.Equal("Home", home.Name);
        Assert.Equal("Home", home.RelativePath);
    }

    [Fact]
    public void CreateDirectory_AddsDirectory()
    {
        var root = FileManagerEntry<Dummy>.Home;
        var dir = root.CreateDirectory("TestDir");
        Assert.True(dir.IsDirectory);
        Assert.Contains(dir, root.GetDirectories());
        Assert.Equal(root, dir.Parent);
    }

    [Fact]
    public void CreateDefaultFileEntry_AddsFile()
    {
        var root = FileManagerEntry<Dummy>.Home;
        var data = new byte[] { 1, 2, 3 };
        var file = root.CreateDefaultFileEntry(data, "file.txt", data.Length);
        Assert.False(file.IsDirectory);
        Assert.Contains(file, root.GetFiles());
        Assert.Equal(root, file.Parent);
        Assert.Equal("file.txt", file.Name);
    }

    [Fact]
    public async Task GetBytesAsync_ReturnsData()
    {
        var root = FileManagerEntry<Dummy>.Home;
        var data = new byte[] { 42, 84, 68, 23, 32, 78, 99, 00 };
        var file = root.CreateDefaultFileEntry(data, "file.bin", data.Length);
        var bytes = await file.GetBytesAsync();
        Assert.Equal(data, bytes);
    }

    [Fact]
    public async Task GetBytesAsync_ReturnsDataFromFunc()
    {
        var root = FileManagerEntry<Dummy>.Home;
        var file = root.CreateDefaultFileEntry(async () => await Task.FromResult(new byte[] { 42, 84, 68, 23, 32, 78, 99, 00 }).ConfigureAwait(false), "file.bin", 1);
        var bytes = await file.GetBytesAsync();
        Assert.Equal(new byte[] { 42, 84, 68, 23, 32, 78, 99, 00 }, bytes);
    }

    [Fact]
    public void AddRange_AddsMultipleEntries()
    {
        var root = FileManagerEntry<Dummy>.Home;
        var dir1 = root.CreateDirectory("Dir1");
        var dir2 = root.CreateDirectory("Dir2");
        var file1 = root.CreateDefaultFileEntry([1], "f1.txt", 1);
        var file2 = root.CreateDefaultFileEntry([2], "f2.txt", 1);

        var newRoot = FileManagerEntry<Dummy>.Home;
        newRoot.AddRange(dir1, dir2, file1, file2);

        Assert.Contains(dir1, newRoot.GetDirectories());
        Assert.Contains(dir2, newRoot.GetDirectories());
        Assert.Contains(file1, newRoot.GetFiles());
        Assert.Contains(file2, newRoot.GetFiles());
    }

    [Fact]
    public void Remove_RemovesFileAndDirectory()
    {
        var root = FileManagerEntry<Dummy>.Home;
        var dir = root.CreateDirectory("Dir");
        var file = root.CreateDefaultFileEntry([1], "f.txt", 1);

        root.Remove(dir);
        root.Remove(file);

        Assert.DoesNotContain(dir, root.GetDirectories());
        Assert.DoesNotContain(file, root.GetFiles());
    }

    [Fact]
    public void Remove_MultipleEntries()
    {
        var root = FileManagerEntry<Dummy>.Home;
        var dir1 = root.CreateDirectory("Dir1");
        var dir2 = root.CreateDirectory("Dir2");
        root.Remove([dir1, dir2]);
        Assert.Empty(root.GetDirectories());
    }

    [Fact]
    public void Find_ByIdAndPredicate()
    {
        var root = FileManagerEntry<Dummy>.Home;
        var dir = root.CreateDirectory("Dir");
        var file = root.CreateDefaultFileEntry([1], "f.txt", 1);

        var foundById = FileManagerEntry<Dummy>.Find(root, file.ViewId);
        Assert.Equal(file, foundById);

        var foundByPredicate = FileManagerEntry<Dummy>.Find(root, e => e.Name == "f.txt");
        Assert.Equal(file, foundByPredicate);
    }

    [Fact]
    public void FindByName_ReturnsMatchingEntries()
    {
        var root = FileManagerEntry<Dummy>.Home;
        var file1 = root.CreateDefaultFileEntry([1], "abc.txt", 1);
        var file2 = root.CreateDefaultFileEntry([2], "abcd.txt", 1);

        var found = FileManagerEntry<Dummy>.FindByName(root, "abc").ToList();
        Assert.Contains(file1, found);
        Assert.Contains(file2, found);
    }

    [Fact]
    public void SetName_ChangesNameWithExtension()
    {
        var root = FileManagerEntry<Dummy>.Home;
        var file = root.CreateDefaultFileEntry([1], "old.txt", 1);
        file.SetName("new");
        Assert.Equal("new.txt", file.Name);
    }

    [Fact]
    public void Sort_SortsEntries()
    {
        var root = FileManagerEntry<Dummy>.Home;
        var fileA = root.CreateDefaultFileEntry([1], "a.txt", 1);
        var fileB = root.CreateDefaultFileEntry([2], "b.txt", 1);

        root.InvalidateMerge();
        root.Sort(FileSortMode.Descending, FileSortBy.Name);

        var sorted = root.Enumerate();
        Assert.Equal(fileB.Name, sorted.First().Name);
    }

    [Fact]
    public void EqualsAndHashCode_Work()
    {
        var entry1 = FileManagerEntry<Dummy>.CreateEntry([1], "file.txt", 1);

        Assert.NotNull(entry1);
        Assert.False(entry1.Equals(null));
        Assert.True(entry1.Equals(entry1, entry1));
        Assert.False(entry1.Equals(entry1, null));
        Assert.NotEqual(0, entry1.GetHashCode());
    }

    [Fact]
    public void HasExtensionAndExtension_Work()
    {
        var entry = FileManagerEntry<Dummy>.CreateEntry([1], "file.txt", 1);
        Assert.True(entry.HasExtension);
        Assert.Equal(".txt", entry.Extension);
    }

    [Theory]
    [InlineData("file.txt", "text/plain")]
    public void ContentType_ReturnsExpected(string name, string contentType)
    {
        var entry = FileManagerEntry<Dummy>.CreateEntry([1], name, 1);
        Assert.Equal(contentType, entry.ContentType);
    }
}
