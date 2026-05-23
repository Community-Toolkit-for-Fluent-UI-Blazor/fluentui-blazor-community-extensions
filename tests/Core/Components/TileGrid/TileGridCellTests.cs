using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.TileGrid;

public class TileGridCellTests
{
    [Fact]
    public void Constructor_SetsDefaultValues()
    {
        var cell = new TileGridCell();

        Assert.Equal(1, cell.ColumnCount);
        Assert.Equal(1, cell.CellWidth);
        Assert.Equal(1, cell.CellHeight);
        Assert.NotNull(cell.RowHeights);
        Assert.Empty(cell.RowHeights);
    }

    [Fact]
    public void Properties_CanBeUpdated()
    {
        var cell = new TileGridCell
        {
            ColumnCount = 4,
            CellWidth = 120.5,
            CellHeight = 80.25,
            RowHeights = [25.5, 30.0]
        };

        Assert.Equal(4, cell.ColumnCount);
        Assert.Equal(120.5, cell.CellWidth);
        Assert.Equal(80.25, cell.CellHeight);
        Assert.Equal([25.5, 30.0], cell.RowHeights);
    }
}
