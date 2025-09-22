using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Animations;
public class BackEasingTests
{
    [Theory]
    [InlineData(0, 0, 10, 1, 1.70158, 0)]
    [InlineData(0.5, 0, 10, 1, 1.70158, -0.877)]
    [InlineData(1, 0, 10, 1, 1.70158, 10)]
    public void EaseIn_ReturnsExpected(double t, double b, double c, double d, double s, double expected)
    {
        var result = BackEasing.EaseIn(t, b, c, d, s);
        Assert.Equal(expected, result, 4);
    }

    [Theory]
    [InlineData(0, 0, 10, 1, 1.70158, 0)] 
    [InlineData(0.5, 0, 10, 1, 1.70158, 10.877)] 
    [InlineData(1, 0, 10, 1, 1.70158, 10)] 
    public void EaseOut_ReturnsExpected(double t, double b, double c, double d, double s, double expected)
    {
        var result = BackEasing.EaseOut(t, b, c, d, s);
        Assert.Equal(expected, result, 4);
    }

    [Theory]
    [InlineData(0, 0, 10, 1, 1.70158, 0)] 
    [InlineData(0.5, 0, 10, 1, 1.70158, 5)] 
    [InlineData(1, 0, 10, 1, 1.70158, 10)] 
    public void EaseInOut_ReturnsExpected(double t, double b, double c, double d, double s, double expected)
    {
        var result = BackEasing.EaseInOut(t, b, c, d, s);
        Assert.Equal(expected, result, 4);
    }
}
