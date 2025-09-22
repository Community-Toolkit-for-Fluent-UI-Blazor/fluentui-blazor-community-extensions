using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Animations;

public class QuadraticEasingTests
{
    [Theory]
    [InlineData(0, 10, 20, 5, 10)]
    [InlineData(2.5, 10, 20, 5, 15)]
    [InlineData(5, 10, 20, 5, 30)]
    public void EaseIn_ReturnsExpected(double t, double b, double c, double d, double expected)
    {
        var result = QuadraticEasing.EaseIn(t, b, c, d);
        Assert.Equal(expected, result, 5);
    }

    [Theory]
    [InlineData(0, 10, 20, 5, 10)]
    [InlineData(2.5, 10, 20, 5, 25)]
    [InlineData(5, 10, 20, 5, 30)]
    public void EaseOut_ReturnsExpected(double t, double b, double c, double d, double expected)
    {
        var result = QuadraticEasing.EaseOut(t, b, c, d);
        Assert.Equal(expected, result, 5);
    }

    [Theory]
    [InlineData(0, 10, 20, 5, 10)]
    [InlineData(2.5, 10, 20, 5, 20)]
    [InlineData(5, 10, 20, 5, 30)]
    public void EaseInOut_ReturnsExpected(double t, double b, double c, double d, double expected)
    {
        var result = QuadraticEasing.EaseInOut(t, b, c, d);
        Assert.Equal(expected, result, 5);
    }
}
