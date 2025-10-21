using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Animations;

public class BounceEasingTests
{
    [Theory]
    [InlineData(0, 0, 10, 10, 0.00000)]
    [InlineData(5, 0, 10, 10, 7.65625)]
    [InlineData(10, 0, 10, 10, 10.00000)]
    public void EaseOut_ReturnsExpected(double t, double b, double c, double d, double expected)
    {
        var result = BounceEasing.EaseOut(t, b, c, d);
        Assert.Equal(expected, result, 5);
    }

    [Theory]
    [InlineData(0, 0, 10, 10, 0.00000)]
    [InlineData(5, 0, 10, 10, 2.34375)] 
    [InlineData(10, 0, 10, 10, 10.00000)]
    public void EaseIn_ReturnsExpected(double t, double b, double c, double d, double expected)
    {
        var result = BounceEasing.EaseIn(t, b, c, d);
        Assert.Equal(expected, result, 5);
    }

    [Theory]
    [InlineData(0, 0, 10, 10, 0.00000)]
    [InlineData(5, 0, 10, 10, 5.00000)]
    [InlineData(10, 0, 10, 10, 10.00000)]
    public void EaseInOut_ReturnsExpected(double t, double b, double c, double d, double expected)
    {
        var result = BounceEasing.EaseInOut(t, b, c, d);
        Assert.Equal(expected, result, 5);
    }
}
