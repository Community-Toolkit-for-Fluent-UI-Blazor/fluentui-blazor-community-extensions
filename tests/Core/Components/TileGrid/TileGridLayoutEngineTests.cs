using FluentUI.Blazor.Community.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Xunit;

namespace Components.Tests.Components.TileGrid;

public class TileGridLayoutEngineTests
{
    [Fact]
    public void ApplyLayout_AssignsRowsAndColumns()
    {
        var items = new List<TestTileGridItem>
        {
            new() { Index = 0, RowSpan = 1, ColumnSpan = 1 },
            new() { Index = 1, RowSpan = 1, ColumnSpan = 2 },
            new() { Index = 2, RowSpan = 1, ColumnSpan = 1 }
        };
        var layout = CreateLayoutEngine(items, 2);

        layout.ApplyLayout();

        Assert.Equal(0, items[0].Row);
        Assert.Equal(0, items[0].Column);
        Assert.Equal(1, items[0].RowSpan);
        Assert.Equal(1, items[0].ColumnSpan);

        Assert.Equal(1, items[1].Row);
        Assert.Equal(0, items[1].Column);
        Assert.Equal(1, items[1].RowSpan);
        Assert.Equal(2, items[1].ColumnSpan);

        Assert.Equal(0, items[2].Row);
        Assert.Equal(1, items[2].Column);
        Assert.Equal(1, items[2].RowSpan);
        Assert.Equal(1, items[2].ColumnSpan);
    }

    [Fact]
    public void ApplyLayout_WithPreview_DoesNotChangeSpans()
    {
        var items = new List<TestTileGridItem>
        {
            new() { Index = 0, RowSpan = 2, ColumnSpan = 2 },
            new() { Index = 1, RowSpan = 1, ColumnSpan = 1 }
        };
        var layout = CreateLayoutEngine(items, 2);

        layout.ApplyLayout(preview: true);

        Assert.Equal(2, items[0].RowSpan);
        Assert.Equal(2, items[0].ColumnSpan);
        Assert.Equal(0, items[0].Row);
        Assert.Equal(0, items[0].Column);
    }

    [Theory]
    [InlineData(3, 2, 2, true)]
    [InlineData(3, 4, 1, false)]
    [InlineData(3, 0, 1, false)]
    public void IsResizeValid_ValidatesAgainstColumnCount(int columns, int newColSpan, int newRowSpan, bool expected)
    {
        var items = new List<TestTileGridItem>
        {
            new() { Index = 0, RowSpan = 1, ColumnSpan = 1 }
        };
        var layout = CreateLayoutEngine(items, columns);

        var result = layout.IsResizeValid(newColSpan, newRowSpan);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void IsResizeValid_WhenColumnCountIsZero_ReturnsTrue()
    {
        var items = new List<TestTileGridItem>
        {
            new() { Index = 0, RowSpan = 1, ColumnSpan = 1 }
        };
        var layout = CreateLayoutEngine(items, 0);

        var result = layout.IsResizeValid(10, 10);

        Assert.True(result);
    }

    [Fact]
    public void Reset_RestoresInitialLayout()
    {
        var initialItems = new List<TestTileGridItem>
        {
            new() { Index = 0, RowSpan = 1, ColumnSpan = 1 },
            new() { Index = 1, RowSpan = 2, ColumnSpan = 1 }
        };
        var layout = CreateLayoutEngine(initialItems, 2);

        initialItems.Clear();
        initialItems.Add(new TestTileGridItem { Index = 5, RowSpan = 3, ColumnSpan = 3 });

        layout.Reset();

        Assert.Equal(2, initialItems.Count);
        Assert.Collection(initialItems,
            item =>
            {
                Assert.Equal(0, item.Index);
                Assert.Equal(1, item.RowSpan);
                Assert.Equal(1, item.ColumnSpan);
            },
            item =>
            {
                Assert.Equal(1, item.Index);
                Assert.Equal(2, item.RowSpan);
                Assert.Equal(1, item.ColumnSpan);
            });
    }

    private static TileGridLayoutEngine<TestTileGridItem> CreateLayoutEngine(List<TestTileGridItem> items, int columns)
    {
        var tileGrid = new FluentCxTileGrid<TestTileGridItem>(new LibraryConfiguration());
        tileGrid.SetParametersAsync(ParameterView.FromDictionary(new Dictionary<string, object?>
        {
            ["Columns"] = columns,
            ["Items"] = items
        })).GetAwaiter().GetResult();

        return new TileGridLayoutEngine<TestTileGridItem>(tileGrid);
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
