using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.TileGrid;

public class GhostTileTests
{
    [Fact]
    public void Activate_SetsStateAndStartsDragging()
    {
        var item = new TestTileGridItem
        {
            Index = 5,
            RowSpan = 2,
            ColumnSpan = 3,
            IsDragging = false
        };
        var ghost = new GhostTile();

        ghost.Activate(item);

        Assert.Equal(item.Index, ghost.Index);
        Assert.Equal(item.RowSpan, ghost.RowSpan);
        Assert.Equal(item.ColumnSpan, ghost.ColumnSpan);
        Assert.True(item.IsDragging);
        Assert.True(ghost.IsActive);
    }

    [Fact]
    public void Clear_ResetsStateAndStopsDragging()
    {
        var item = new TestTileGridItem
        {
            Index = 2,
            RowSpan = 4,
            ColumnSpan = 1,
            IsDragging = false
        };
        var ghost = new GhostTile();

        ghost.Activate(item);
        ghost.Clear();

        Assert.Null(ghost.Index);
        Assert.Equal(0, ghost.RowSpan);
        Assert.Equal(0, ghost.ColumnSpan);
        Assert.False(item.IsDragging);
        Assert.False(ghost.IsActive);
    }

    [Fact]
    public void Clear_WhenNotActive_ResetsState()
    {
        var ghost = new GhostTile();

        ghost.Clear();

        Assert.Null(ghost.Index);
        Assert.Equal(0, ghost.RowSpan);
        Assert.Equal(0, ghost.ColumnSpan);
        Assert.False(ghost.IsActive);
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
