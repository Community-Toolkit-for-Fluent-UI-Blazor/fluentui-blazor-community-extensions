using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Animations.Interpolators;

public class DoubleInterpolatorTests
{
    [Theory]
    [InlineData(0.0, 10.0, 0.0, 0.0)]
    [InlineData(0.0, 10.0, 1.0, 10.0)]
    [InlineData(0.0, 10.0, 0.5, 5.0)]
    public void Lerp_ReturnsExpectedValue(double start, double end, double amount, double expected)
    {
        var interpolator = new DoubleInterpolator();

        var result = interpolator.Lerp(start, end, amount);

        Assert.Equal(expected, result, 6);
    }
}
