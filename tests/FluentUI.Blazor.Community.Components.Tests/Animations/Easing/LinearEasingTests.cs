using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Animations;

public class LinearEasingTests
{
    [Theory]
    [InlineData(0.0)]
    [InlineData(0.5)]
    [InlineData(1.0)]
    [InlineData(-1.0)]
    [InlineData(100.0)]
    public void Ease_ReturnsInputValue(double input)
    {
        // Act
        var result = LinearEasing.Ease(input);

        // Assert
        Assert.Equal(input, result);
    }
}
