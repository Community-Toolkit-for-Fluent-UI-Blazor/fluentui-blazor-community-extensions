using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Animations;

public class ExponentialEasingTests
{
    [Theory]
    [InlineData(0, 10, 20, 100, 10)]
    [InlineData(100, 10, 20, 100, 30)]
    [InlineData(50, 10, 20, 100, 10 + 20 * 0.03125)]
    public void EaseIn_ReturnsExpected(double t, double b, double c, double d, double expected)
    {
        var result = ExponentialEasing.EaseIn(t, b, c, d);
        Assert.Equal(expected, result, 6);
    }

    [Theory]
    [InlineData(0, 10, 20, 100, 10 + 20 * 0)]
    [InlineData(100, 10, 20, 100, 30)]
    [InlineData(50, 10, 20, 100, 10 + 20 * 0.96875)]
    public void EaseOut_ReturnsExpected(double t, double b, double c, double d, double expected)
    {
        var result = ExponentialEasing.EaseOut(t, b, c, d);
        Assert.Equal(expected, result, 6);
    }

    [Theory]
    [InlineData(0, 10, 20, 100, 10)]
    [InlineData(100, 10, 20, 100, 30)]
    [InlineData(50, 10, 20, 100, 10 + 10 * 1)]
    [InlineData(25, 10, 20, 100, 10 + 10 * 0.03125)]
    [InlineData(75, 10, 20, 100, 10 + 10 * 1.96875)]
    public void EaseInOut_ReturnsExpected(double t, double b, double c, double d, double expected)
    {
        var result = ExponentialEasing.EaseInOut(t, b, c, d);
        Assert.Equal(expected, result, 6);
    }
}
