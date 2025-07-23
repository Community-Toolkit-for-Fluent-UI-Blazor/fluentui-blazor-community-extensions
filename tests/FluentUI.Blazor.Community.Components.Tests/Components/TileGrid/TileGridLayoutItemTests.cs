using FluentUI.Blazor.Community.Components;

namespace FluentUI.Blazor.Community.Components.Tests.Components.TileGrid;

public class TileGridLayoutItemTests
{
    [Fact]
    public void TileGridLayoutItem_Constructor_CreatesInstance()
    {
        // Act
        var item = new TileGridLayoutItem();

        // Assert
        Assert.NotNull(item);
        Assert.Equal(0, item.ColumnSpan);
        Assert.Equal(0, item.RowSpan);
        Assert.Equal(0, item.Index);
        Assert.Null(item.Key);
    }

    [Fact]
    public void TileGridLayoutItem_InheritsFromGridLayoutBaseItem()
    {
        // Act
        var item = new TileGridLayoutItem();

        // Assert
        Assert.IsAssignableFrom<GridLayoutBaseItem>(item);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 3)]
    [InlineData(5, 7)]
    [InlineData(0, 0)]
    [InlineData(-1, -2)]
    [InlineData(int.MaxValue, int.MinValue)]
    public void TileGridLayoutItem_ColumnSpan_SetAndGetCorrectly(int columnSpan, int rowSpan)
    {
        // Arrange
        var item = new TileGridLayoutItem();

        // Act
        item.ColumnSpan = columnSpan;
        item.RowSpan = rowSpan;

        // Assert
        Assert.Equal(columnSpan, item.ColumnSpan);
        Assert.Equal(rowSpan, item.RowSpan);
    }

    [Theory]
    [InlineData("test-key", 5)]
    [InlineData("", 0)]
    [InlineData("UPPERCASE", -1)]
    [InlineData("item-123", 999)]
    public void TileGridLayoutItem_BaseProperties_SetAndGetCorrectly(string key, int index)
    {
        // Arrange
        var item = new TileGridLayoutItem();

        // Act
        item.Key = key;
        item.Index = index;

        // Assert
        Assert.Equal(key, item.Key);
        Assert.Equal(index, item.Index);
    }

    [Fact]
    public void TileGridLayoutItem_AllProperties_SetIndependently()
    {
        // Arrange
        var item = new TileGridLayoutItem();

        // Act
        item.Key = "test-item";
        item.Index = 42;
        item.ColumnSpan = 3;
        item.RowSpan = 2;

        // Assert
        Assert.Equal("test-item", item.Key);
        Assert.Equal(42, item.Index);
        Assert.Equal(3, item.ColumnSpan);
        Assert.Equal(2, item.RowSpan);
    }

    [Fact]
    public void TileGridLayoutItem_NullKey_HandledCorrectly()
    {
        // Arrange
        var item = new TileGridLayoutItem();

        // Act
        item.Key = null;

        // Assert
        Assert.Null(item.Key);
    }

    [Fact]
    public void TileGridLayoutItem_MultiplePropertyChanges_AllPersist()
    {
        // Arrange
        var item = new TileGridLayoutItem()
        {
            Key = "initial",
            Index = 1,
            ColumnSpan = 1,
            RowSpan = 1
        };

        // Act - Multiple changes
        item.Key = "updated";
        item.Index = 99;
        item.ColumnSpan = 5;
        item.RowSpan = 3;

        // Assert
        Assert.Equal("updated", item.Key);
        Assert.Equal(99, item.Index);
        Assert.Equal(5, item.ColumnSpan);
        Assert.Equal(3, item.RowSpan);
    }
}