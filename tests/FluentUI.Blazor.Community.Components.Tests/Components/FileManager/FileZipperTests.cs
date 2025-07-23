using System.IO.Compression;

namespace FluentUI.Blazor.Community.Components.Tests.Components.FileManager;

public class DummyItem { }

public class FileZipperTests
{
    [Fact]
    public async Task ZipAsync_ReturnsNull_WhenEntriesIsEmpty()
    {
        var result = await FileZipper.ZipAsync<DummyItem>([]);
        Assert.Null(result);
    }

    [Fact]
    public async Task ZipAsync_CreatesZipWithSingleFile()
    {
        var fileData = new byte[] { 1, 2, 3, 4 };
        var fileEntry = FileManagerEntry<DummyItem>.CreateEntry(fileData, "test.txt", fileData.LongLength);

        var result = await FileZipper.ZipAsync([fileEntry]);

        Assert.NotNull(result);
        Assert.Equal($"archive_{DateTime.Now:yyyyMMdd_HHmmss}.zip", result.Name);
        var zippedBytes = await result.GetBytesAsync();
        Assert.NotNull(zippedBytes);
        Assert.True(zippedBytes.Length > 0);

        // Vérification du contenu du zip
        using var ms = new MemoryStream(zippedBytes);
        using var archive = new ZipArchive(ms, ZipArchiveMode.Read);
        var entry = archive.GetEntry("test.txt");
        Assert.NotNull(entry);
        using var entryStream = entry.Open();
        var buffer = new byte[fileData.Length];
        entryStream.ReadExactly(buffer);
        Assert.Equal(fileData, buffer);
    }

    [Fact]
    public async Task ZipAsync_CreatesZipWithDirectoryAndFiles()
    {
        var root = FileManagerEntry<DummyItem>.Home;
        var dir =  root.CreateDirectory("subdir");
        root.CreateDefaultFileEntry([10, 20], "file1.txt", 2);
        dir.CreateDefaultFileEntry([30, 40], "file2.txt", 2);

        var result = await FileZipper.ZipAsync([root]);

        Assert.NotNull(result);
        var zippedBytes = await result.GetBytesAsync();
        Assert.NotNull(zippedBytes);

        using var ms = new MemoryStream(zippedBytes);
        using var archive = new ZipArchive(ms, ZipArchiveMode.Read);
        Assert.NotNull(archive.GetEntry("Home/file1.txt"));
        Assert.NotNull(archive.GetEntry("Home/subdir/file2.txt"));
    }
}
