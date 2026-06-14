using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Animations.Easing;

public class BackEasingTests
{
    [Theory]
    [InlineData(0.0, 0.0)]
    [InlineData(1.0, 1.0)]
    public void EaseIn_RespectsEndpoints(double t, double expected)
    {
        var result = BackEasing.EaseIn(t, 0, 1, 1);

        Assert.Equal(expected, result, 6);
    }

    [Theory]
    [InlineData(0.0, 0.0)]
    [InlineData(1.0, 1.0)]
    public void EaseOut_RespectsEndpoints(double t, double expected)
    {
        var result = BackEasing.EaseOut(t, 0, 1, 1);

        Assert.Equal(expected, result, 6);
    }

    [Theory]
    [InlineData(0.0, 0.0)]
    [InlineData(1.0, 1.0)]
    public void EaseInOut_RespectsEndpoints(double t, double expected)
    {
        var result = BackEasing.EaseInOut(t, 0, 1, 1);

        Assert.Equal(expected, result, 6);
    }
}
