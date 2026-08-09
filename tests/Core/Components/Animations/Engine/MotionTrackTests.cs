using System;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Animations.Engine;

public class MotionTrackTests
{
    [Fact]
    public void Update_InterpolatesAndCompletes()
    {
        var state = new MotionState();
        var track = new MotionTrack<double>(
            "x",
            state,
            0,
            10,
            new MotionCurve(TimeSpan.FromMilliseconds(10), EasingFunction.Linear, EasingMode.In),
            new DoubleInterpolator());

        track.Update(TimeSpan.FromMilliseconds(5));
        Assert.False(track.IsCompleted);
        Assert.True(state.X > 0);

        var completed = false;
        track.Completed += (_, _) => completed = true;

        track.Update(TimeSpan.FromMilliseconds(10));

        Assert.True(track.IsCompleted);
        Assert.True(completed);
        Assert.Equal(10.0, state.X, 6);
    }
}
