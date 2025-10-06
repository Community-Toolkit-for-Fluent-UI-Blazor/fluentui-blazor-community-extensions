using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Animations;

public class QuarticEasingTests
{
    [Theory]
    [InlineData(0, 10, 20, 5, 10)]
    [InlineData(2.5, 10, 20, 5, 10 + 20 * 0.5 * 0.5 * 0.5 * 0.5)]
    [InlineData(5, 10, 20, 5, 30)]
    public void EaseIn_ReturnsExpected(double t, double b, double c, double d, double expected)
    {
        var result = QuarticEasing.EaseIn(t, b, c, d);
        Assert.Equal(expected, result, precision: 6);
    }

    [Theory]
    [InlineData(0, 10, 20, 5, 10)]
    [InlineData(2.5, 10, 20, 5, 10 + 20 * 0.9375)]
    [InlineData(5, 10, 20, 5, 30)]
    public void EaseOut_ReturnsExpected(double t, double b, double c, double d, double expected)
    {
        var result = QuarticEasing.EaseOut(t, b, c, d);
        Assert.Equal(expected, result, precision: 6);
    }

    [Theory]
    [InlineData(0, 10, 20, 5, 10)]
    [InlineData(1.25, 10, 20, 5, 10.625)]
    [InlineData(2.5, 10, 20, 5, 10 + 20 / 2 * 1)]
    [InlineData(5, 10, 20, 5, 30)]
    public void EaseInOut_ReturnsExpected(double t, double b, double c, double d, double expected)
    {
        var result = QuarticEasing.EaseInOut(t, b, c, d);
        Assert.Equal(expected, result, precision: 6);
    }
}
