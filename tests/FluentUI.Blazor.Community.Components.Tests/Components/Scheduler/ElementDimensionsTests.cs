using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class ElementDimensionsTests
{
    [Fact]
    public void Equality_SameValues_AreEqual()
    {
        // Arrange
        var a = new ElementDimensions(10.5f, 20.25f);
        var b = new ElementDimensions(10.5f, 20.25f);

        // Act / Assert
        Assert.Equal(a, b);
        Assert.True(a == b);
        Assert.False(ReferenceEquals(a, b));
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void Deconstruct_ReturnsWidthAndHeight()
    {
        // Arrange
        var dims = new ElementDimensions(5f, 7.5f);

        // Act
        var (w, h) = dims;

        // Assert
        Assert.Equal(5f, w);
        Assert.Equal(7.5f, h);
    }

    [Fact]
    public void WithExpression_ProducesNewInstance_OriginalUnchanged()
    {
        // Arrange
        var original = new ElementDimensions(1f, 2f);

        // Act
        var modified = original with { Width = 3f };

        // Assert
        Assert.Equal(1f, original.Width);
        Assert.Equal(2f, original.Height);
        Assert.Equal(3f, modified.Width);
        Assert.Equal(2f, modified.Height);
        Assert.NotEqual(original, modified);
    }

    [Fact]
    public void ToString_ContainsWidthAndHeight()
    {
        // Arrange
        var dims = new ElementDimensions(12f, 34f);

        // Act
        var s = dims.ToString();

        // Assert
        Assert.Contains("Width", s, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Height", s, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("12", s);
        Assert.Contains("34", s);
    }
}
