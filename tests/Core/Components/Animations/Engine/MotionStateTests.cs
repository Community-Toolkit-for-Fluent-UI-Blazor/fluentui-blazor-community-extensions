using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Animations.Engine;

public class MotionStateTests
{
    [Fact]
    public void Defaults_AreExpected()
    {
        var state = new MotionState();

        Assert.Equal(0.0, state.X, 6);
        Assert.Equal(0.0, state.Y, 6);
        Assert.Equal(1.0, state.ScaleX, 6);
        Assert.Equal(1.0, state.ScaleY, 6);
        Assert.Equal(1.0, state.Opacity, 6);
    }

    [Fact]
    public void Scale_SetsAxes()
    {
        var state = new MotionState { Scale = 2.0 };

        Assert.Equal(2.0, state.ScaleX, 6);
        Assert.Equal(2.0, state.ScaleY, 6);
    }

    [Fact]
    public void Clone_CopiesValues()
    {
        var state = new MotionState { X = 3.0, Opacity = 0.5 };

        var clone = state.Clone();

        Assert.NotSame(state, clone);
        Assert.Equal(3.0, clone.X, 6);
        Assert.Equal(0.5, clone.Opacity, 6);
    }
}
