using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Animations;

public class ElasticEasingTests
{
    [Theory]
    [InlineData(0, 10, 20, 100, 10)]
    [InlineData(100, 10, 20, 100, 30)]
    public void EaseIn_BoundaryValues_ReturnsExpected(double t, double b, double c, double d, double expected)
    {
        var result = ElasticEasing.EaseIn(t, b, c, d);
        Assert.Equal(expected, result, 6);
    }

    [Fact]
    public void EaseIn_MiddleValue_ReturnsOscillating()
    {
        double t = 50, b = 10, c = 20, d = 100;
        var result = ElasticEasing.EaseIn(t, b, c, d);

        Assert.InRange(result, b - c, b + c);
    }

    [Theory]
    [InlineData(0, 10, 20, 100, 10)]
    [InlineData(100, 10, 20, 100, 30)]
    public void EaseOut_BoundaryValues_ReturnsExpected(double t, double b, double c, double d, double expected)
    {
        var result = ElasticEasing.EaseOut(t, b, c, d);
        Assert.Equal(expected, result, 6);
    }

    [Fact]
    public void EaseOut_MiddleValue_ReturnsOscillating()
    {
        double t = 50, b = 10, c = 20, d = 100;
        var result = ElasticEasing.EaseOut(t, b, c, d);
        Assert.InRange(result, -20, 40);
    }

    [Theory]
    [InlineData(0, 10, 20, 100, 10)] 
    [InlineData(100, 10, 20, 100, 30)]
    public void EaseInOut_BoundaryValues_ReturnsExpected(double t, double b, double c, double d, double expected)
    {
        var result = ElasticEasing.EaseInOut(t, b, c, d);
        Assert.Equal(expected, result, 6);
    }

    [Fact]
    public void EaseInOut_MiddleValue_ReturnsOscillating()
    {
        double t = 50, b = 10, c = 20, d = 100;
        var result = ElasticEasing.EaseInOut(t, b, c, d);
        Assert.InRange(result, b - c, b + c);
    }

    [Theory]
    [InlineData(-10, 10, 20, 100)]
    [InlineData(110, 10, 20, 100)]
    public void EaseIn_OutOfRangeTime_ReturnsOscillating(double t, double b, double c, double d)
    {
        var result = ElasticEasing.EaseIn(t, b, c, d);
        Assert.True(double.IsFinite(result));
    }

    [Theory]
    [InlineData(-10, 10, 20, 100)]
    [InlineData(110, 10, 20, 100)]
    public void EaseOut_OutOfRangeTime_ReturnsOscillating(double t, double b, double c, double d)
    {
        var result = ElasticEasing.EaseOut(t, b, c, d);
        Assert.True(double.IsFinite(result));
    }

    [Theory]
    [InlineData(-10, 10, 20, 100)]
    [InlineData(110, 10, 20, 100)]
    public void EaseInOut_OutOfRangeTime_ReturnsOscillating(double t, double b, double c, double d)
    {
        var result = ElasticEasing.EaseInOut(t, b, c, d);
        Assert.True(double.IsFinite(result));
    }
}
