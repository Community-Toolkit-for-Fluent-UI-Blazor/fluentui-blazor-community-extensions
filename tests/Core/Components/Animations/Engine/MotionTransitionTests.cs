using System;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Animations.Engine;

public class MotionTransitionTests
{
    [Fact]
    public void ToCurve_UsesSettings()
    {
        var transition = new MotionTransition
        {
            Duration = TimeSpan.FromMilliseconds(150),
            Function = EasingFunction.Back,
            Mode = EasingMode.InOut
        };

        var curve = transition.ToCurve();

        Assert.Equal(TimeSpan.FromMilliseconds(150), curve.Duration);
        Assert.Equal(EasingFunction.Back, curve.Function);
        Assert.Equal(EasingMode.InOut, curve.Mode);
    }
}
