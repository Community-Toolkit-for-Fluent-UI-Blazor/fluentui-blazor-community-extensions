using System;
using System.Threading.Tasks;
using FluentUI.Blazor.Community.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Xunit;

namespace Components.Tests.Components.Animations.Engine;

public class MotionSequenceTests
{
    [Fact]
    public async Task StartAsync_RaisesCompleted()
    {
        var item = new MotionItem(new LibraryConfiguration());
        var sequence = new MotionSequence(item);
        var completed = false;

        sequence.Completed += (_, _) => completed = true;

        await sequence.StartAsync();

        Assert.True(completed);
    }

    [Fact]
    public void WithRepeat_ConfiguresTimeline()
    {
        var item = new MotionItem(new LibraryConfiguration());
        var sequence = new MotionSequence(item);

        sequence.WithRepeat(2);

        Assert.Equal(MotionTimelineLoopMode.Repeat, item.Timeline.LoopMode);
        Assert.Equal(2, item.Timeline.LoopCount);
    }
}
