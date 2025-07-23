using System.Drawing;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Resizer;

public class ResizedEventArgsTests
{
    [Fact]
    public void Constructor_CreatesInstanceWithDefaultValues()
    {
        // Act
        var eventArgs = new ResizedEventArgs();

        // Assert
        Assert.NotNull(eventArgs);
        Assert.Null(eventArgs.Id);
        Assert.Equal(default, eventArgs.Orientation);
        Assert.Equal(default, eventArgs.OriginalSize);
        Assert.Equal(default, eventArgs.NewSize);
        Assert.Equal(0, eventArgs.ColumnSpan);
        Assert.Equal(0, eventArgs.RowSpan);
    }

    [Fact]
    public void Id_CanBeSetAndRetrieved()
    {
        // Arrange
        var eventArgs = new ResizedEventArgs();
        var testId = "test-component-id";

        // Act
        eventArgs.Id = testId;

        // Assert
        Assert.Equal(testId, eventArgs.Id);
    }

    [Fact]
    public void Id_CanBeSetToNull()
    {
        // Arrange
        var eventArgs = new ResizedEventArgs { Id = "initial" };

        // Act
        eventArgs.Id = null;

        // Assert
        Assert.Null(eventArgs.Id);
    }

    [Theory]
    [InlineData(ResizerHandler.Horizontally)]
    [InlineData(ResizerHandler.Vertically)]
    [InlineData(ResizerHandler.Both)]
    public void Orientation_CanBeSetAndRetrieved(ResizerHandler orientation)
    {
        // Arrange
        var eventArgs = new ResizedEventArgs();

        // Act
        eventArgs.Orientation = orientation;

        // Assert
        Assert.Equal(orientation, eventArgs.Orientation);
    }

    [Fact]
    public void OriginalSize_CanBeSetAndRetrieved()
    {
        // Arrange
        var eventArgs = new ResizedEventArgs();
        var originalSize = new SizeF(100.5f, 200.75f);

        // Act
        eventArgs.OriginalSize = originalSize;

        // Assert
        Assert.Equal(originalSize, eventArgs.OriginalSize);
    }

    [Fact]
    public void NewSize_CanBeSetAndRetrieved()
    {
        // Arrange
        var eventArgs = new ResizedEventArgs();
        var newSize = new SizeF(150.25f, 300.5f);

        // Act
        eventArgs.NewSize = newSize;

        // Assert
        Assert.Equal(newSize, eventArgs.NewSize);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(0)]
    [InlineData(-1)] // Edge case: negative values
    public void ColumnSpan_CanBeSetAndRetrieved(int columnSpan)
    {
        // Arrange
        var eventArgs = new ResizedEventArgs();

        // Act
        eventArgs.ColumnSpan = columnSpan;

        // Assert
        Assert.Equal(columnSpan, eventArgs.ColumnSpan);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(8)]
    [InlineData(0)]
    [InlineData(-1)] // Edge case: negative values
    public void RowSpan_CanBeSetAndRetrieved(int rowSpan)
    {
        // Arrange
        var eventArgs = new ResizedEventArgs();

        // Act
        eventArgs.RowSpan = rowSpan;

        // Assert
        Assert.Equal(rowSpan, eventArgs.RowSpan);
    }

    [Fact]
    public void AllProperties_CanBeSetTogether()
    {
        // Arrange
        var eventArgs = new ResizedEventArgs();
        var id = "test-id";
        var orientation = ResizerHandler.Both;
        var originalSize = new SizeF(100f, 200f);
        var newSize = new SizeF(150f, 250f);
        var columnSpan = 3;
        var rowSpan = 2;

        // Act
        eventArgs.Id = id;
        eventArgs.Orientation = orientation;
        eventArgs.OriginalSize = originalSize;
        eventArgs.NewSize = newSize;
        eventArgs.ColumnSpan = columnSpan;
        eventArgs.RowSpan = rowSpan;

        // Assert
        Assert.Equal(id, eventArgs.Id);
        Assert.Equal(orientation, eventArgs.Orientation);
        Assert.Equal(originalSize, eventArgs.OriginalSize);
        Assert.Equal(newSize, eventArgs.NewSize);
        Assert.Equal(columnSpan, eventArgs.ColumnSpan);
        Assert.Equal(rowSpan, eventArgs.RowSpan);
    }

    [Fact]
    public void Equality_WithSameValues_ReturnsTrue()
    {
        // Arrange
        var eventArgs1 = new ResizedEventArgs
        {
            Id = "test",
            Orientation = ResizerHandler.Both,
            OriginalSize = new SizeF(100f, 200f),
            NewSize = new SizeF(150f, 250f),
            ColumnSpan = 2,
            RowSpan = 3
        };

        var eventArgs2 = new ResizedEventArgs
        {
            Id = "test",
            Orientation = ResizerHandler.Both,
            OriginalSize = new SizeF(100f, 200f),
            NewSize = new SizeF(150f, 250f),
            ColumnSpan = 2,
            RowSpan = 3
        };

        // Act & Assert
        Assert.Equal(eventArgs1, eventArgs2);
    }

    [Fact]
    public void Equality_WithDifferentValues_ReturnsFalse()
    {
        // Arrange
        var eventArgs1 = new ResizedEventArgs
        {
            Id = "test1",
            Orientation = ResizerHandler.Horizontally,
            ColumnSpan = 1,
            RowSpan = 1
        };

        var eventArgs2 = new ResizedEventArgs
        {
            Id = "test2",
            Orientation = ResizerHandler.Vertically,
            ColumnSpan = 2,
            RowSpan = 2
        };

        // Act & Assert
        Assert.NotEqual(eventArgs1, eventArgs2);
    }

    [Fact]
    public void GetHashCode_WithSameValues_ReturnsSameHashCode()
    {
        // Arrange
        var eventArgs1 = new ResizedEventArgs
        {
            Id = "test",
            Orientation = ResizerHandler.Both,
            ColumnSpan = 2,
            RowSpan = 3
        };

        var eventArgs2 = new ResizedEventArgs
        {
            Id = "test",
            Orientation = ResizerHandler.Both,
            ColumnSpan = 2,
            RowSpan = 3
        };

        // Act & Assert
        Assert.Equal(eventArgs1.GetHashCode(), eventArgs2.GetHashCode());
    }

    [Fact]
    public void ToString_ReturnsExpectedFormat()
    {
        // Arrange
        var eventArgs = new ResizedEventArgs
        {
            Id = "test-component",
            Orientation = ResizerHandler.Both,
            ColumnSpan = 2,
            RowSpan = 3
        };

        // Act
        var result = eventArgs.ToString();

        // Assert
        Assert.Contains("ResizedEventArgs", result);
    }
}
