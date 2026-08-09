using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Animations.Engine;

public class MotionTransitionStateTests
{
    [Fact]
    public void Reset_ClearsValues()
    {
        var state = new MotionTransitionState();
        state.Source.X = 5;
        state.Target.Opacity = 0.4;

        state.Reset();

        Assert.Equal(0.0, state.Source.X, 6);
        Assert.Equal(1.0, state.Target.Opacity, 6);
    }
}
