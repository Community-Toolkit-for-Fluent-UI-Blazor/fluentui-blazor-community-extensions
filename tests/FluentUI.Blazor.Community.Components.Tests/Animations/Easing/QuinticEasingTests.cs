using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Animations;

public class QuinticEasingTests
{
    [Theory]
    [InlineData(0, 10, 20, 5, 10)]
    [InlineData(5, 10, 20, 5, 30)]
    [InlineData(2.5, 10, 20, 5, 10 + 20 * 0.03125)]
    public void EaseIn_ReturnsExpected(double t, double b, double c, double d, double expected)
    {
        var result = QuinticEasing.EaseIn(t, b, c, d);
        Assert.Equal(expected, result, precision: 6);
    }

    [Theory]
    [InlineData(0, 10, 20, 5, 10)]
    [InlineData(5, 10, 20, 5, 30)]
    [InlineData(2.5, 10, 20, 5, 10 + 20 * 0.96875)]
    public void EaseOut_ReturnsExpected(double t, double b, double c, double d, double expected)
    {
        var result = QuinticEasing.EaseOut(t, b, c, d);
        Assert.Equal(expected, result, precision: 6);
    }

    [Theory]
    [InlineData(0, 10, 20, 5, 10)] 
    [InlineData(2.5, 10, 20, 5, 20)]
    [InlineData(5, 10, 20, 5, 30)]
    public void EaseInOut_ReturnsExpected(double t, double b, double c, double d, double expected)
    {
        var result = QuinticEasing.EaseInOut(t, b, c, d);
        Assert.Equal(expected, result, precision: 6);
    }
}
