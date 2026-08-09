using System;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Animations.Engine;

public class MotionCurveTests
{
    [Fact]
    public void Evaluate_ClampsToRange()
    {
        var curve = new MotionCurve(TimeSpan.FromMilliseconds(100), EasingFunction.Linear, EasingMode.In);

        Assert.Equal(0.0, curve.Evaluate(TimeSpan.Zero), 6);
        Assert.Equal(1.0, curve.Evaluate(TimeSpan.FromMilliseconds(200)), 6);
    }
}
