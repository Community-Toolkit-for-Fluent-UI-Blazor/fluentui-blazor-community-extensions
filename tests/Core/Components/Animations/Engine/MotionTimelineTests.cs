using System;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Animations.Engine;

public class MotionTimelineTests
{
    [Fact]
    public void StartAndStop_UpdateState()
    {
        var timeline = new MotionTimeline();

        timeline.Start();
        Assert.Equal(MotionTimelineState.Running, timeline.State);

        timeline.Pause();
        Assert.Equal(MotionTimelineState.Paused, timeline.State);

        timeline.Resume();
        Assert.Equal(MotionTimelineState.Running, timeline.State);

        timeline.Stop();
        Assert.Equal(MotionTimelineState.Stopped, timeline.State);
        Assert.Equal(TimeSpan.Zero, timeline.Elapsed);
    }

    [Fact]
    public void Update_CompletesTrack()
    {
        var timeline = new MotionTimeline();
        var state = new MotionState();
        var track = new MotionTrack<double>(
            "x",
            state,
            0,
            10,
            new MotionCurve(TimeSpan.FromMilliseconds(1), EasingFunction.Linear, EasingMode.In),
            new DoubleInterpolator());

        timeline.AddTrack(track);
        timeline.Start();
        timeline.Update(TimeSpan.FromMilliseconds(1));

        Assert.True(track.IsCompleted);
        Assert.Equal(10.0, state.X, 6);
    }
}
