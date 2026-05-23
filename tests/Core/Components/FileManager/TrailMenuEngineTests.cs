using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components.Tests.Components.FileManager.TestDoubles;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class TrailMenuEngineTests
{
    private sealed class DummyItem
    {
    }

    [Fact]
    public async Task BuildAsync_ReturnsNullForMissingEntry()
    {
        var engine = new TrailMenuEngine<DummyItem>(new TestFileProvider<DummyItem>([]));

        var result = await engine.BuildAsync(null);

        Assert.Null(result);
    }

    [Fact]
    public async Task BuildAsync_BuildsTrailWithDirectoryItems()
    {
        var provider = new TestFileProvider<DummyItem>(new Dictionary<string, IReadOnlyList<EntryDescriptor<DummyItem>>>
        {
            ["root"] = [EntryDescriptor<DummyItem>.Directory("child", "Child", "root", DateTime.UtcNow, DateTime.UtcNow)],
            ["child"] = []
        });
        var engine = new TrailMenuEngine<DummyItem>(provider);

        var root = CreateDirectory("root", "Root", null);
        var child = CreateDirectory("child", "Child", root);
        var file = CreateFile("file", "File.txt", child);

        var result = await engine.BuildAsync(file);

        Assert.NotNull(result);
        Assert.Equal("Root", result!.Label);
        Assert.Equal("Child", result.Next?.Label);
        Assert.True(result.Items.Any());
    }

    private static FileEntry<DummyItem> CreateDirectory(string id, string name, FileEntry<DummyItem>? parent)
    {
        return new FileEntry<DummyItem>(
            id: id,
            name: name,
            isDirectory: true,
            size: 0,
            createdDate: DateTime.UtcNow,
            modifiedDate: DateTime.UtcNow,
            item: new DummyItem(),
            parent: parent);
    }

    private static FileEntry<DummyItem> CreateFile(string id, string name, FileEntry<DummyItem> parent)
    {
        return new FileEntry<DummyItem>(
            id: id,
            name: name,
            isDirectory: false,
            size: 0,
            createdDate: DateTime.UtcNow,
            modifiedDate: DateTime.UtcNow,
            item: new DummyItem(),
            parent: parent);
    }
}
