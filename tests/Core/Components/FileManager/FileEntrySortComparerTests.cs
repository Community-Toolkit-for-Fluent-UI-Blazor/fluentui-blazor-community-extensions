using System;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class FileEntrySortComparerTests
{
    private sealed class DummyItem
    {
    }

    [Fact]
    public void Compare_FoldersFirst_WhenConfigured()
    {
        var state = new FileManagerState
        {
            SortLayout = FileSortLayout.Folders,
            SortBy = FileSortBy.Name,
            SortMode = FileSortMode.Ascending
        };
        var comparer = new FileEntrySortComparer<DummyItem>(state);
        var folder = CreateEntry("folder", isDirectory: true);
        var file = CreateEntry("file.txt", isDirectory: false);

        Assert.True(comparer.Compare(folder, file) < 0);
    }

    [Fact]
    public void Compare_FilesFirst_WhenConfigured()
    {
        var state = new FileManagerState
        {
            SortLayout = FileSortLayout.Files,
            SortBy = FileSortBy.Name,
            SortMode = FileSortMode.Ascending
        };
        var comparer = new FileEntrySortComparer<DummyItem>(state);
        var folder = CreateEntry("folder", isDirectory: true);
        var file = CreateEntry("file.txt", isDirectory: false);

        Assert.True(comparer.Compare(folder, file) > 0);
    }

    [Fact]
    public void Compare_RespectsSortByAndMode()
    {
        var state = new FileManagerState
        {
            SortLayout = FileSortLayout.None,
            SortBy = FileSortBy.Name,
            SortMode = FileSortMode.Descending
        };
        var comparer = new FileEntrySortComparer<DummyItem>(state);
        var alpha = CreateEntry("alpha.txt", isDirectory: false);
        var beta = CreateEntry("beta.txt", isDirectory: false);

        Assert.True(comparer.Compare(alpha, beta) > 0);
    }

    private static FileEntry<DummyItem> CreateEntry(string name, bool isDirectory)
    {
        return new FileEntry<DummyItem>(
            id: Guid.NewGuid().ToString(),
            name: name,
            isDirectory: isDirectory,
            size: 0,
            createdDate: DateTimeOffset.UtcNow,
            modifiedDate: DateTimeOffset.UtcNow,
            item: new DummyItem());
    }
}
