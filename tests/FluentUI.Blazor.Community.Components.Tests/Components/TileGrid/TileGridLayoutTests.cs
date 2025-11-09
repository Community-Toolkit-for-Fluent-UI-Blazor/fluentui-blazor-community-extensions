
namespace FluentUI.Blazor.Community.Components.Tests.Components.TileGrid;

public class TileGridLayoutTests
{
    [Fact]
    public void TileGridLayout_Constructor_CreatesInstance()
    {
        // Act
        var layout = new TileGridLayout();

        // Assert
        Assert.NotNull(layout);
        Assert.Empty(layout.Items);
    }

    [Fact]
    public void TileGridLayout_UpdateSpan_WithExistingItem_UpdatesSpan()
    {
        // Arrange
        var layout = new TileGridLayout();
        var key = "test-item";

        // Add an item first using the protected Add method
        layout.Add(new TileGridLayoutItem() { Key = key, Index = 0 });

        // Act
        layout.UpdateSpan(key, 3, 2);

        // Assert
        var item = layout.Items.Cast<TileGridLayoutItem>().First();
        Assert.Equal(3, item.ColumnSpan);
        Assert.Equal(2, item.RowSpan);
        Assert.Equal(key, item.Key);
        Assert.Equal(0, item.Index);
    }

    [Fact]
    public void TileGridLayout_UpdateSpan_WithNonExistentItem_DoesNotThrow()
    {
        // Arrange
        var layout = new TileGridLayout();

        // Act & Assert - Should not throw
        layout.UpdateSpan("non-existent-key", 3, 2);

        // Verify no items were added
        Assert.Empty(layout.Items);
    }

    [Fact]
    public void TileGridLayout_UpdateSpan_WithNullKey_DoesNotThrow()
    {
        // Arrange
        var layout = new TileGridLayout();
        layout.Add(new TileGridLayoutItem() { Key = "test", Index = 0 });

        // Act & Assert - Should not throw
        layout.UpdateSpan(null!, 3, 2);
    }

    [Fact]
    public void TileGridLayout_UpdateSpan_WithEmptyKey_DoesNotThrow()
    {
        // Arrange
        var layout = new TileGridLayout();
        layout.Add(new TileGridLayoutItem() { Key = "test", Index = 0 });

        // Act & Assert - Should not throw
        layout.UpdateSpan("", 3, 2);
    }

    [Fact]
    public void TileGridLayout_UpdateSpan_WithZeroSpans_SetsCorrectly()
    {
        // Arrange
        var layout = new TileGridLayout();
        var key = "test-item";
        layout.Add(new TileGridLayoutItem() { Key = key, Index = 0 });

        // Act
        layout.UpdateSpan(key, 0, 0);

        // Assert
        var item = layout.Items.Cast<TileGridLayoutItem>().First();
        Assert.Equal(0, item.ColumnSpan);
        Assert.Equal(0, item.RowSpan);
    }

    [Fact]
    public void TileGridLayout_UpdateSpan_WithNegativeSpans_SetsCorrectly()
    {
        // Arrange
        var layout = new TileGridLayout();
        var key = "test-item";
        layout.Add(new TileGridLayoutItem() { Key = key, Index = 0 });

        // Act
        layout.UpdateSpan(key, -1, -2);

        // Assert
        var item = layout.Items.Cast<TileGridLayoutItem>().First();
        Assert.Equal(-1, item.ColumnSpan);
        Assert.Equal(-2, item.RowSpan);
    }

    [Fact]
    public void TileGridLayout_UpdateSpan_CaseInsensitiveKey_FindsItem()
    {
        // Arrange
        var layout = new TileGridLayout();
        var key = "Test-Item";
        layout.Add(new TileGridLayoutItem() { Key = key, Index = 0 });

        // Act - Use different case
        layout.UpdateSpan("TEST-ITEM", 5, 4);

        // Assert
        var item = layout.Items.Cast<TileGridLayoutItem>().First();
        Assert.Equal(5, item.ColumnSpan);
        Assert.Equal(4, item.RowSpan);
    }

    [Fact]
    public void TileGridLayout_UpdateSpan_MultipleItems_UpdatesCorrectItem()
    {
        // Arrange
        var layout = new TileGridLayout
        {
            new TileGridLayoutItem() { Key = "item1", Index = 0 },
            new TileGridLayoutItem() { Key = "item2", Index = 1 },
            new TileGridLayoutItem() { Key = "item3", Index = 2 }
        };

        // Act
        layout.UpdateSpan("item2", 7, 8);

        // Assert
        var items = layout.Items.Cast<TileGridLayoutItem>().ToList();
        Assert.Equal(3, items.Count);

        var item2 = items.First(x => x.Key == "item2");
        Assert.Equal(7, item2.ColumnSpan);
        Assert.Equal(8, item2.RowSpan);

        // Verify other items unchanged
        var item1 = items.First(x => x.Key == "item1");
        var item3 = items.First(x => x.Key == "item3");
        Assert.Equal(0, item1.ColumnSpan);
        Assert.Equal(0, item1.RowSpan);
        Assert.Equal(0, item3.ColumnSpan);
        Assert.Equal(0, item3.RowSpan);
    }

    [Fact]
    public void TileGridLayout_UpdateSpan_MultipleCallsSameItem_UpdatesCorrectly()
    {
        // Arrange
        var layout = new TileGridLayout();
        var key = "test-item";
        layout.Add(new TileGridLayoutItem() { Key = key, Index = 0 });

        // Act - Multiple updates
        layout.UpdateSpan(key, 1, 1);
        layout.UpdateSpan(key, 2, 3);
        layout.UpdateSpan(key, 5, 7);

        // Assert
        var item = layout.Items.Cast<TileGridLayoutItem>().First();
        Assert.Equal(5, item.ColumnSpan);
        Assert.Equal(7, item.RowSpan);
        Assert.Single(layout.Items); // Still only one item
    }
}
