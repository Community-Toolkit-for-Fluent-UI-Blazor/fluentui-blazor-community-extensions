using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Animations;

public class CubicEasingTests
{
    [Theory]
    [InlineData(0, 0, 10, 10, 0)]
    [InlineData(5, 0, 10, 10, 1.25)]
    [InlineData(10, 0, 10, 10, 10)]
    public void EaseIn_ReturnsExpected(double t, double b, double c, double d, double expected)
    {
        var result = CubicEasing.EaseIn(t, b, c, d);
        Assert.Equal(expected, result, 5);
    }

    [Theory]
    [InlineData(0, 0, 10, 10, 0)]
    [InlineData(5, 0, 10, 10, 8.75)]
    [InlineData(10, 0, 10, 10, 10)]
    public void EaseOut_ReturnsExpected(double t, double b, double c, double d, double expected)
    {
        var result = CubicEasing.EaseOut(t, b, c, d);
        Assert.Equal(expected, result, 5);
    }

    [Theory]
    [InlineData(0, 0, 10, 10, 0)]
    [InlineData(5, 0, 10, 10, 5)]
    [InlineData(10, 0, 10, 10, 10)]
    public void EaseInOut_ReturnsExpected(double t, double b, double c, double d, double expected)
    {
        var result = CubicEasing.EaseInOut(t, b, c, d);
        Assert.Equal(expected, result, 5);
    }
}
