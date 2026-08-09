using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Animations.Easing;

public class QuinticEasingTests
{
    [Theory]
    [InlineData(0.0, 0.0)]
    [InlineData(1.0, 1.0)]
    public void EaseIn_RespectsEndpoints(double t, double expected)
    {
        var result = QuinticEasing.EaseIn(t, 0, 1, 1);

        Assert.Equal(expected, result, 6);
    }

    [Theory]
    [InlineData(0.0, 0.0)]
    [InlineData(1.0, 1.0)]
    public void EaseOut_RespectsEndpoints(double t, double expected)
    {
        var result = QuinticEasing.EaseOut(t, 0, 1, 1);

        Assert.Equal(expected, result, 6);
    }

    [Theory]
    [InlineData(0.0, 0.0)]
    [InlineData(1.0, 1.0)]
    public void EaseInOut_RespectsEndpoints(double t, double expected)
    {
        var result = QuinticEasing.EaseInOut(t, 0, 1, 1);

        Assert.Equal(expected, result, 6);
    }
}
