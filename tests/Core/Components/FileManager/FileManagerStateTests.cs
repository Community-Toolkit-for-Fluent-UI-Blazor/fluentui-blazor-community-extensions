using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class FileManagerStateTests
{
    [Fact]
    public void Defaults_AreExpected()
    {
        var state = new FileManagerState();

        Assert.Equal(FileSortBy.Name, state.SortBy);
        Assert.Equal(FileSortMode.Ascending, state.SortMode);
        Assert.Equal(FileView.List, state.View);
        Assert.Equal(FileSortLayout.Folders, state.SortLayout);
    }

    [Fact]
    public void SortChanges_RaiseEventsOnce()
    {
        var state = new FileManagerState();
        var sortChangedCount = 0;
        var viewChangedCount = 0;
        var layoutChangedCount = 0;

        state.SortChanged += (_, _) => sortChangedCount++;
        state.ViewChanged += (_, _) => viewChangedCount++;
        state.SortLayoutChanged += (_, _) => layoutChangedCount++;

        state.SortBy = FileSortBy.Extension;
        state.SortBy = FileSortBy.Extension;
        state.SortMode = FileSortMode.Descending;
        state.View = FileView.Mosaic;
        state.SortLayout = FileSortLayout.Files;

        Assert.Equal(2, sortChangedCount);
        Assert.Equal(1, viewChangedCount);
        Assert.Equal(1, layoutChangedCount);
    }
}
