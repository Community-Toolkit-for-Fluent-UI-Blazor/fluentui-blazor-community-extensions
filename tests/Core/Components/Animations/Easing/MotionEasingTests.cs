using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Animations.Easing;

public class MotionEasingTests
{
    [Theory]
    [InlineData(-0.5, 0.0)]
    [InlineData(1.5, 1.0)]
    public void Evaluate_ClampsInput(double t, double expected)
    {
        var result = MotionEasing.Evaluate(EasingFunction.Linear, EasingMode.In, t);

        Assert.Equal(expected, result, 6);
    }

    [Fact]
    public void Evaluate_UsesSelectedFunction()
    {
        var result = MotionEasing.Evaluate(EasingFunction.Cubic, EasingMode.In, 0.5);
        var expected = CubicEasing.EaseIn(0.5, 0, 1, 1);

        Assert.Equal(expected, result, 6);
    }
}
