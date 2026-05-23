using FluentUI.Blazor.Community.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;
using Xunit;

namespace Components.Tests.Components.TileGrid;

public class TileGridResizeHandlerTests
{
    [Fact]
    public void BeginResize_SetsStateAndInitialSpans()
    {
        var items = CreateItems();
        var handler = CreateHandler(items, 4, out var state);
        var item = items[0];

        handler.BeginResize(item, TileGridItemResizeHandle.Right, 10, 20);

        Assert.Equal(item, state.ResizeSource);
        Assert.Equal(TileGridItemResizeHandle.Right, state.ResizeHandle);
        Assert.Equal(10, state.ResizeStartX);
        Assert.Equal(20, state.ResizeStartY);
        Assert.Equal(item.ColumnSpan, state.ResizeInitialColumnSpan);
        Assert.Equal(item.RowSpan, state.ResizeInitialRowSpan);
        Assert.True(state.IsResizing);
    }

    [Fact]
    public void Move_WithRightHandle_UpdatesColumnSpan()
    {
        var items = CreateItems();
        var handler = CreateHandler(items, 4, out var state);
        var item = items[0];

        handler.BeginResize(item, TileGridItemResizeHandle.Right, 0, 0);
        state.CellWidth = 10;
        state.CellHeight = 10;

        handler.Move(new MouseEventArgs { ClientX = 15, ClientY = 0 });

        Assert.Equal(4, state.ResizePreviewColumnSpan);
        Assert.Equal(1, state.ResizePreviewRowSpan);
        Assert.Equal(4, item.ColumnSpan);
        Assert.Equal(1, item.RowSpan);
    }

    [Fact]
    public void Move_WithBottomHandle_UpdatesRowSpan()
    {
        var items = CreateItems();
        var handler = CreateHandler(items, 4, out var state);
        var item = items[0];

        handler.BeginResize(item, TileGridItemResizeHandle.Bottom, 0, 0);
        state.CellWidth = 10;
        state.CellHeight = 10;

        handler.Move(new MouseEventArgs { ClientX = 0, ClientY = 18 });

        Assert.Equal(1, state.ResizePreviewColumnSpan);
        Assert.Equal(3, state.ResizePreviewRowSpan);
        Assert.Equal(1, item.ColumnSpan);
        Assert.Equal(3, item.RowSpan);
    }

    [Fact]
    public void Move_WhenResizeIsInvalid_DoesNotUpdate()
    {
        var items = CreateItems();
        var handler = CreateHandler(items, 1, out var state);
        var item = items[0];

        handler.BeginResize(item, TileGridItemResizeHandle.Right, 0, 0);
        state.CellWidth = 10;
        state.CellHeight = 10;

        handler.Move(new MouseEventArgs { ClientX = 10, ClientY = 0 });

        Assert.Equal(0, state.ResizePreviewColumnSpan);
        Assert.Equal(0, state.ResizePreviewRowSpan);
        Assert.Equal(1, item.ColumnSpan);
        Assert.Equal(1, item.RowSpan);
    }

    [Fact]
    public void End_ResetsResizeState()
    {
        var items = CreateItems();
        var handler = CreateHandler(items, 4, out var state);

        handler.BeginResize(items[0], TileGridItemResizeHandle.BottomRight, 0, 0);
        handler.End();

        Assert.False(state.IsResizing);
        Assert.Null(state.ResizeSource);
        Assert.Equal(TileGridItemResizeHandle.None, state.ResizeHandle);
    }

    private static TileGridResizeHandler<TestTileGridItem> CreateHandler(
        List<TestTileGridItem> items,
        int columns,
        out TileGridState<TestTileGridItem> state)
    {
        state = new TileGridState<TestTileGridItem>();
        var tileGrid = new FluentCxTileGrid<TestTileGridItem>(new LibraryConfiguration());
        tileGrid.SetParametersAsync(ParameterView.FromDictionary(new Dictionary<string, object?>
        {
            ["Columns"] = columns,
            ["Items"] = items
        })).GetAwaiter().GetResult();
        var layout = new TileGridLayoutEngine<TestTileGridItem>(tileGrid);

        return new TileGridResizeHandler<TestTileGridItem>(state, layout);
    }

    private static List<TestTileGridItem> CreateItems()
    {
        return
        [
            new TestTileGridItem { Index = 0, RowSpan = 1, ColumnSpan = 1 },
            new TestTileGridItem { Index = 1, RowSpan = 1, ColumnSpan = 1 }
        ];
    }

    private sealed class TestTileGridItem : ITileGridItem
    {
        public string Key { get; } = "test-key";

        public long Index { get; set; }

        public int RowSpan { get; set; }

        public int ColumnSpan { get; set; }

        public bool IsDragging { get; set; }

        public bool IsPreviewTarget { get; set; }

        public int Row { get; set; }

        public int Column { get; set; }
    }
}
