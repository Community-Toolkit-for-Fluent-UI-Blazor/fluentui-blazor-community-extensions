using FluentUI.Blazor.Community.Components;

namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class GridLayoutBaseItemTests
{
    [Fact]
    public void GridLayoutBaseItem_Constructor_CreatesInstance()
    {
        // Act
        var item = new GridLayoutBaseItem();

        // Assert
        Assert.NotNull(item);
        Assert.Equal(0, item.Index);
        Assert.Null(item.Key);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(42)]
    [InlineData(-1)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public void GridLayoutBaseItem_Index_SetAndGetCorrectly(int index)
    {
        // Arrange
        var item = new GridLayoutBaseItem();

        // Act
        item.Index = index;

        // Assert
        Assert.Equal(index, item.Index);
    }

    [Theory]
    [InlineData("test-key")]
    [InlineData("")]
    [InlineData("UPPERCASE")]
    [InlineData("lowercase")]
    [InlineData("123-numeric")]
    [InlineData("special!@#$%")]
    [InlineData("very-long-key-with-many-characters-and-numbers-123456789")]
    public void GridLayoutBaseItem_Key_SetAndGetCorrectly(string key)
    {
        // Arrange
        var item = new GridLayoutBaseItem();

        // Act
        item.Key = key;

        // Assert
        Assert.Equal(key, item.Key);
    }

    [Fact]
    public void GridLayoutBaseItem_Key_NullValue_HandledCorrectly()
    {
        // Arrange
        var item = new GridLayoutBaseItem();

        // Act
        item.Key = null;

        // Assert
        Assert.Null(item.Key);
    }

    [Fact]
    public void GridLayoutBaseItem_PropertiesSetTogether_BothPersist()
    {
        // Arrange
        var item = new GridLayoutBaseItem();
        var expectedKey = "test-item";
        var expectedIndex = 99;

        // Act
        item.Key = expectedKey;
        item.Index = expectedIndex;

        // Assert
        Assert.Equal(expectedKey, item.Key);
        Assert.Equal(expectedIndex, item.Index);
    }

    [Fact]
    public void GridLayoutBaseItem_MultiplePropertyChanges_AllPersist()
    {
        // Arrange
        var item = new GridLayoutBaseItem()
        {
            Key = "initial",
            Index = 1
        };

        // Act - Multiple changes
        item.Key = "updated";
        item.Index = 999;
        item.Key = "final";
        item.Index = -42;

        // Assert
        Assert.Equal("final", item.Key);
        Assert.Equal(-42, item.Index);
    }

    [Fact]
    public void GridLayoutBaseItem_ObjectInitializer_SetsProperties()
    {
        // Act
        var item = new GridLayoutBaseItem()
        {
            Key = "initialized-key",
            Index = 123
        };

        // Assert
        Assert.Equal("initialized-key", item.Key);
        Assert.Equal(123, item.Index);
    }
}