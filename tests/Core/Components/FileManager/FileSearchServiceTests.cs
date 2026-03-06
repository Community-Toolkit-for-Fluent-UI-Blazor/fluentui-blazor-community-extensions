using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components.Tests.Components.FileManager.TestDoubles;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class FileSearchServiceTests
{
    private sealed class DummyItem
    {
        public string Metadata { get; set; } = string.Empty;
    }

    [Fact]
    public async Task SearchAsync_MatchesByName()
    {
        var provider = new TestFileProvider<DummyItem>(new Dictionary<string, IReadOnlyList<EntryDescriptor<DummyItem>>>
        {
            ["root"] = [EntryDescriptor<DummyItem>.File("file", "report.txt", "root", 1, DateTime.UtcNow, DateTime.UtcNow, () => Task.FromResult(Array.Empty<byte>()), new DummyItem())]
        });
        var service = new FileSearchService<DummyItem>(provider);
        var root = CreateRoot();
        var options = new FileSearchOptions { SearchInNames = true };

        var results = await CollectAsync(service.SearchAsync(root, "report", item => item.Metadata, options));

        Assert.Single(results);
        Assert.Equal("report.txt", results[0].Name);
    }

    [Fact]
    public async Task SearchAsync_MatchesByContent()
    {
        var provider = new TestFileProvider<DummyItem>(new Dictionary<string, IReadOnlyList<EntryDescriptor<DummyItem>>>
        {
            ["root"] = [EntryDescriptor<DummyItem>.File(
                "file",
                "notes.txt",
                "root",
                1,
                DateTime.UtcNow,
                DateTime.UtcNow,
                () => Task.FromResult(System.Text.Encoding.UTF8.GetBytes("hello world")),
                new DummyItem())]
        });
        var service = new FileSearchService<DummyItem>(provider);
        var root = CreateRoot();
        var options = new FileSearchOptions { SearchInNames = false, SearchInContent = true };

        var results = await CollectAsync(service.SearchAsync(root, "hello", item => item.Metadata, options));

        Assert.Single(results);
    }

    private static FileEntry<DummyItem> CreateRoot()
    {
        return new FileEntry<DummyItem>(
            id: "root",
            name: "Root",
            isDirectory: true,
            size: 0,
            createdDate: DateTime.UtcNow,
            modifiedDate: DateTime.UtcNow,
            item: new DummyItem());
    }

    private static async Task<List<FileEntry<DummyItem>>> CollectAsync(IAsyncEnumerable<FileEntry<DummyItem>> entries)
    {
        var list = new List<FileEntry<DummyItem>>();
        await foreach (var entry in entries)
        {
            list.Add(entry);
        }

        return list;
    }
}
