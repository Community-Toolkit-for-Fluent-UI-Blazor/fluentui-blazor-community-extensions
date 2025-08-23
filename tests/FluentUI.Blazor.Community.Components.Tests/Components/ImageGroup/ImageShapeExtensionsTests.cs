using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.ImageGroup;

public class ImageShapeExtensionsTests
{
    [Theory]
    [InlineData(ImageShape.Square, "0px")]
    [InlineData(ImageShape.RoundSquare, "8px")]
    [InlineData(ImageShape.Circle, "100000px")]
    public void ToBorderRadius_ReturnsExpectedCssValue(ImageShape shape, string expected)
    {
        // Act
        var result = shape.ToBorderRadius();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToBorderRadius_ThrowsArgumentOutOfRangeException_ForInvalidValue()
    {
        // Arrange
        var invalidShape = (ImageShape)999;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => invalidShape.ToBorderRadius());
    }
}
