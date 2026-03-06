using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components.Tests.Components.FileManager.TestDoubles;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class FileManagerEngineTests
{
    private sealed class DummyItem
    {
    }

    [Fact]
    public async Task InitializeAsync_LoadsChildren()
    {
        var provider = CreateProvider(new Dictionary<string, IReadOnlyList<EntryDescriptor<DummyItem>>>
        {
            ["home"] = [EntryDescriptor<DummyItem>.Directory("dir", "Dir", "home", DateTime.UtcNow, DateTime.UtcNow)]
        });
        var engine = new FileManagerEngine<DummyItem>(provider, new FileManagerState());

        await engine.InitializeAsync();

        Assert.Single(engine.MasterRoot.Children);
        Assert.Equal("Dir", engine.MasterRoot.Children[0].Name);
    }

    [Fact]
    public async Task FindById_ReturnsNestedEntry()
    {
        var provider = CreateProvider(new Dictionary<string, IReadOnlyList<EntryDescriptor<DummyItem>>>
        {
            ["home"] = [EntryDescriptor<DummyItem>.Directory("dir", "Dir", "home", DateTime.UtcNow, DateTime.UtcNow)],
            ["dir"] = [EntryDescriptor<DummyItem>.File("file", "file.txt", "dir", 10, DateTime.UtcNow, DateTime.UtcNow, () => Task.FromResult(Array.Empty<byte>()))]
        });
        var engine = new FileManagerEngine<DummyItem>(provider, new FileManagerState());

        await engine.LoadChildrenRecursiveAsync(engine.MasterRoot);

        Assert.NotNull(engine.FindById("file"));
    }

    [Fact]
    public void ApplyRename_UpdatesNameAndModifiedDate()
    {
        var entry = CreateFile("file.txt", 0);
        var before = entry.ModifiedDate;

        FileManagerEngine<DummyItem>.ApplyRename(entry, "renamed");

        Assert.Equal("renamed.txt", entry.Name);
        Assert.True(entry.ModifiedDate >= before);
    }

    [Fact]
    public void ApplyDelete_RemovesEntryAndUpdatesSizes()
    {
        var parent = CreateDirectory("parent");
        var fileA = CreateFile("a.txt", 5);
        var fileB = CreateFile("b.txt", 7);

        parent.AddChild(fileA);
        parent.AddChild(fileB);
        parent.RecalculateSizes();

        FileManagerEngine<DummyItem>.ApplyDelete([fileA]);

        Assert.Single(parent.Children);
        Assert.Equal(7, parent.Size);
    }

    [Fact]
    public void GetSortedEntry_ReturnsSortedClone()
    {
        var state = new FileManagerState
        {
            SortBy = FileSortBy.Name,
            SortMode = FileSortMode.Ascending,
            SortLayout = FileSortLayout.None
        };
        var engine = new FileManagerEngine<DummyItem>(CreateProvider([]), state);
        var parent = CreateDirectory("parent");
        var beta = CreateFile("beta.txt", 1);
        var alpha = CreateFile("alpha.txt", 1);

        parent.AddChild(beta);
        parent.AddChild(alpha);

        var sorted = engine.GetSortedEntry(parent);

        Assert.NotSame(parent, sorted);
        Assert.Equal(["alpha.txt", "beta.txt"], sorted!.Children.Select(c => c.Name).ToArray());
    }

    private static FileEntry<DummyItem> CreateFile(string name, long size)
    {
        return new FileEntry<DummyItem>(
            id: Guid.NewGuid().ToString(),
            name: name,
            isDirectory: false,
            size: size,
            createdDate: DateTime.UtcNow,
            modifiedDate: DateTime.UtcNow,
            item: new DummyItem());
    }

    private static FileEntry<DummyItem> CreateDirectory(string name)
    {
        return new FileEntry<DummyItem>(
            id: Guid.NewGuid().ToString(),
            name: name,
            isDirectory: true,
            size: 0,
            createdDate: DateTime.UtcNow,
            modifiedDate: DateTime.UtcNow,
            item: new DummyItem());
    }

    private static TestFileProvider<DummyItem> CreateProvider(Dictionary<string, IReadOnlyList<EntryDescriptor<DummyItem>>> map)
    {
        return new TestFileProvider<DummyItem>(map);
    }
}
