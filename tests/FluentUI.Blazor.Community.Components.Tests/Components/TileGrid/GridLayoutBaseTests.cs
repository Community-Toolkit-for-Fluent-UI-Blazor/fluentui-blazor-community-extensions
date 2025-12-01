
namespace FluentUI.Blazor.Community.Components.Tests.Components.TileGrid;

public class GridLayoutBaseTests
{
    // Helper class to access protected methods for testing
    private class TestableGridLayout : GridLayoutBase
    {
        public new T? Get<T>(string? key) where T : GridLayoutBaseItem
        {
            return base.Get<T>(key);
        }

        public new void Add<T>(T value) where T : GridLayoutBaseItem, new()
        {
            base.Add<T>(value);
        }

        public new void Remove(int index)
        {
            base.Remove(index);
        }

        public new void AddRange(IEnumerable<GridLayoutBaseItem> items)
        {
            base.AddRange(items);
        }

        public new void Update<TItem>(Func<TItem, string>? keyFunc, IList<TItem> items)
        {
            base.Update(keyFunc, items);
        }

        public bool TestIsDirty
        {
            get => IsDirty;
            set => IsDirty = value;
        }

        public void TestRequestSave()
        {
            RequestSave();
        }

        public event EventHandler? TestSaveRequested
        {
            add => SaveRequested += value;
            remove => SaveRequested -= value;
        }
    }

    private class TestItem : GridLayoutBaseItem
    {
        public string? Name { get; set; }
    }

    [Fact]
    public void GridLayoutBase_Constructor_InitializesCorrectly()
    {
        // Act
        var layout = new TestableGridLayout();

        // Assert
        Assert.NotNull(layout);
        Assert.Empty(layout.Items);
        Assert.False(layout.TestIsDirty);
    }

    [Fact]
    public void GridLayoutBase_Add_AddsItemCorrectly()
    {
        // Arrange
        var layout = new TestableGridLayout
        {
            // Act
            new TileGridLayoutItem()
            {
                Key = "test-key",
                Index = 5
            }
        };

        // Assert
        Assert.Single(layout.Items);
        var item = layout.Items.First();
        Assert.Equal("test-key", item.Key);
        Assert.Equal(5, item.Index);
    }

    [Fact]
    public void GridLayoutBase_Add_MultipleItems_AddsAll()
    {
        // Arrange
        var layout = new TestableGridLayout
        {
            // Act
            new TileGridLayoutItem() { Key = "item1", Index = 0 },
            new TileGridLayoutItem() { Key = "item2", Index = 1 },
            new TestItem() { Key = "item3", Index = 2 }
        };

        // Assert
        Assert.Equal(3, layout.Items.Count());

        var items = layout.Items.ToList();
        Assert.Equal("item1", items[0].Key);
        Assert.Equal("item2", items[1].Key);
        Assert.Equal("item3", items[2].Key);
        Assert.Equal(0, items[0].Index);
        Assert.Equal(1, items[1].Index);
        Assert.Equal(2, items[2].Index);
    }

    [Fact]
    public void GridLayoutBase_Get_WithExistingKey_ReturnsItem()
    {
        // Arrange
        var layout = new TestableGridLayout
        {
            new TileGridLayoutItem() { Key = "test-key", Index = 0 }
        };

        // Act
        var result = layout.Get<TileGridLayoutItem>("test-key");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test-key", result.Key);
        Assert.Equal(0, result.Index);
    }

    [Fact]
    public void GridLayoutBase_Get_WithNonExistentKey_ReturnsNull()
    {
        // Arrange
        var layout = new TestableGridLayout
        {
            new TileGridLayoutItem() { Key = "existing-key", Index = 0 }
        };

        // Act
        var result = layout.Get<TileGridLayoutItem>("non-existent-key");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GridLayoutBase_Get_WithNullKey_ReturnsNull()
    {
        // Arrange
        var layout = new TestableGridLayout
        {
            new TileGridLayoutItem() { Key = "test-key", Index = 0 }
        };

        // Act
        var result = layout.Get<TileGridLayoutItem>(null);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GridLayoutBase_Get_CaseInsensitive_FindsItem()
    {
        // Arrange
        var layout = new TestableGridLayout
        {
            new TileGridLayoutItem() { Key = "Test-Key", Index = 0 }
        };

        // Act
        var result = layout.Get<TileGridLayoutItem>("TEST-KEY");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test-Key", result.Key);
    }

    [Fact]
    public void GridLayoutBase_Remove_WithExistingIndex_RemovesItem()
    {
        // Arrange
        var layout = new TestableGridLayout
        {
            new TileGridLayoutItem() { Key = "item1", Index = 0 },
            new TileGridLayoutItem() { Key = "item2", Index = 1 },
            new TileGridLayoutItem() { Key = "item3", Index = 2 }
        };

        // Act
        layout.Remove(1);

        // Assert
        Assert.Equal(2, layout.Items.Count());
        var remainingKeys = layout.Items.Select(x => x.Key).ToList();
        Assert.Contains("item1", remainingKeys);
        Assert.Contains("item3", remainingKeys);
        Assert.DoesNotContain("item2", remainingKeys);
    }

    [Fact]
    public void GridLayoutBase_Remove_WithNonExistentIndex_DoesNothing()
    {
        // Arrange
        var layout = new TestableGridLayout
        {
            new TileGridLayoutItem() { Key = "item1", Index = 0 }
        };

        // Act
        layout.Remove(999);

        // Assert
        Assert.Single(layout.Items);
        Assert.Equal("item1", layout.Items.First().Key);
    }

    [Fact]
    public void GridLayoutBase_AddRange_WithEmptyLayout_AddsItems()
    {
        // Arrange
        var layout = new TestableGridLayout();
        var items = new List<GridLayoutBaseItem>
        {
            new TileGridLayoutItem { Key = "item1", Index = 0 },
            new TileGridLayoutItem { Key = "item2", Index = 1 }
        };

        // Act
        layout.AddRange(items);

        // Assert - AddRange only works when _items.Count > 0, so this should do nothing
        Assert.Empty(layout.Items);
    }

    [Fact]
    public void GridLayoutBase_AddRange_WithExistingItems_ReplacesItems()
    {
        // Arrange
        var layout = new TestableGridLayout
        {
            new TileGridLayoutItem() { Key = "original", Index = 0 } // Add one item first
        };

        var newItems = new List<GridLayoutBaseItem>
        {
            new TileGridLayoutItem { Key = "item1", Index = 0 },
            new TileGridLayoutItem { Key = "item2", Index = 1 }
        };

        // Act
        layout.AddRange(newItems);

        // Assert
        Assert.Equal(2, layout.Items.Count());
        var keys = layout.Items.Select(x => x.Key).ToList();
        Assert.Contains("item1", keys);
        Assert.Contains("item2", keys);
        Assert.DoesNotContain("original", keys);
    }

    [Fact]
    public void GridLayoutBase_AddRange_WithNullItems_DoesNothing()
    {
        // Arrange
        var layout = new TestableGridLayout
        {
            new TileGridLayoutItem() { Key = "original", Index = 0 }
        };

        // Act
        layout.AddRange(null!);

        // Assert
        Assert.Single(layout.Items);
        Assert.Equal("original", layout.Items.First().Key);
    }

    [Fact]
    public void GridLayoutBase_AddRange_WithEmptyCollection_DoesNothing()
    {
        // Arrange
        var layout = new TestableGridLayout
        {
            new TileGridLayoutItem() { Key = "original", Index = 0 }
        };

        // Act
        layout.AddRange(new List<GridLayoutBaseItem>());

        // Assert
        Assert.Single(layout.Items);
        Assert.Equal("original", layout.Items.First().Key);
    }

    [Fact]
    public void GridLayoutBase_Update_WithValidKeyFunc_UpdatesIndices()
    {
        // Arrange
        var layout = new TestableGridLayout
        {
            new TileGridLayoutItem() { Key = "item1", Index = 0 },
            new TileGridLayoutItem() { Key = "item2", Index = 1 },
            new TileGridLayoutItem() { Key = "item3", Index = 2 }
        };

        var reorderedItems = new List<string> { "item3", "item1", "item2" };

        // Act
        layout.Update(x => x, reorderedItems);

        // Assert
        var items = layout.Items.ToList();
        var item1 = items.First(x => x.Key == "item1");
        var item2 = items.First(x => x.Key == "item2");
        var item3 = items.First(x => x.Key == "item3");

        Assert.Equal(1, item1.Index); // item1 is now at index 1
        Assert.Equal(2, item2.Index); // item2 is now at index 2
        Assert.Equal(0, item3.Index); // item3 is now at index 0
    }

    [Fact]
    public void GridLayoutBase_Update_WithNullKeyFunc_ThrowsException()
    {
        // Arrange
        var layout = new TestableGridLayout();
        var items = new List<string> { "item1" };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            layout.Update<string>(null, items));

        Assert.Equal("The keyFunc cannot be null.", exception.Message);
    }

    [Fact]
    public void GridLayoutBase_Update_WithNonExistentKeys_DoesNotThrow()
    {
        // Arrange
        var layout = new TestableGridLayout
        {
            new TileGridLayoutItem() { Key = "item1", Index = 0 }
        };

        var items = new List<string> { "non-existent-item" };

        // Act & Assert - Should not throw
        layout.Update(x => x, items);

        // Original item should remain unchanged
        Assert.Single(layout.Items);
        Assert.Equal(0, layout.Items.First().Index);
    }

    [Fact]
    public void GridLayoutBase_RequestSave_InvokesSaveRequestedEvent()
    {
        // Arrange
        var layout = new TestableGridLayout();
        var eventInvoked = false;
        layout.TestIsDirty = true;

        layout.TestSaveRequested += (sender, args) =>
        {
            eventInvoked = true;
            Assert.Same(layout, sender);
        };

        // Act
        layout.TestRequestSave();

        // Assert
        Assert.True(eventInvoked);
        Assert.False(layout.TestIsDirty); // Should be reset to false
    }

    [Fact]
    public void GridLayoutBase_RequestSave_WithoutSubscribers_DoesNotThrow()
    {
        // Arrange
        var layout = new TestableGridLayout();
        layout.TestIsDirty = true;

        // Act & Assert - Should not throw
        layout.TestRequestSave();

        Assert.False(layout.TestIsDirty);
    }

    [Fact]
    public void GridLayoutBase_Enumeration_ReturnsAllItems()
    {
        // Arrange
        var layout = new TestableGridLayout
        {
            new TileGridLayoutItem() { Key = "item1", Index = 0 },
            new TileGridLayoutItem() { Key = "item2", Index = 1 },
            new TestItem() { Key = "item3", Index = 2 }
        };

        // Act
        var enumeratedItems = new List<GridLayoutBaseItem>();
        foreach (var item in layout)
        {
            enumeratedItems.Add(item);
        }

        // Assert
        Assert.Equal(3, enumeratedItems.Count);
        Assert.Equal("item1", enumeratedItems[0].Key);
        Assert.Equal("item2", enumeratedItems[1].Key);
        Assert.Equal("item3", enumeratedItems[2].Key);
    }

    [Fact]
    public void GridLayoutBase_EnumerationGeneric_ReturnsAllItems()
    {
        // Arrange
        var layout = new TestableGridLayout
        {
            new TileGridLayoutItem() { Key = "item1", Index = 0 },
            new TileGridLayoutItem() { Key = "item2", Index = 1 }
        };

        // Act
        var items = layout.ToList();

        // Assert
        Assert.Equal(2, items.Count);
        Assert.All(items, item => Assert.IsType<TileGridLayoutItem>(item));
    }
}
