using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components.Tests.Components.FileManager.TestDoubles;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class FileEntryFlattenServiceTests
{
    private sealed class DummyItem
    {
    }

    [Fact]
    public async Task EnumerateAsync_ReturnsRootAndChildren()
    {
        var provider = new TestFileProvider<DummyItem>(new Dictionary<string, IReadOnlyList<EntryDescriptor<DummyItem>>>
        {
            ["root"] = [EntryDescriptor<DummyItem>.File("file", "file.txt", "root", 1, DateTime.UtcNow, DateTime.UtcNow, () => Task.FromResult(Array.Empty<byte>()))]
        });
        var service = new FileEntryFlattenService<DummyItem>(provider);
        var root = new FileEntry<DummyItem>(
            id: "root",
            name: "Root",
            isDirectory: true,
            size: 0,
            createdDate: DateTime.UtcNow,
            modifiedDate: DateTime.UtcNow,
            item: new DummyItem());

        var entries = new List<FileEntry<DummyItem>>();
        await foreach (var entry in service.EnumerateAsync(root))
        {
            entries.Add(entry);
        }

        Assert.Equal(["Root", "file.txt"], entries.Select(e => e.Name).ToArray());
    }
}
