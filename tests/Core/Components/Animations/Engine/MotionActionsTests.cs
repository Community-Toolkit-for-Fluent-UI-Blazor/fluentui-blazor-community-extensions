using System;
using System.Threading.Tasks;
using FluentUI.Blazor.Community.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Xunit;

namespace Components.Tests.Components.Animations.Engine;

public class MotionActionsTests
{
    [Fact]
    public async Task FadeOutAsync_UpdatesOpacity()
    {
        var node = new TestMotionNode();
        node.State.Opacity = 1.0;

        var task = node.Actions.FadeOutAsync(TimeSpan.FromMilliseconds(1));
        node.Timeline.Update(TimeSpan.FromMilliseconds(1));
        await task;

        Assert.Equal(0.0, node.State.Opacity, 6);
    }

    [Fact]
    public async Task MoveToAsync_UpdatesPosition()
    {
        var node = new TestMotionNode();

        var task = node.Actions.MoveToAsync(10, 20, TimeSpan.FromMilliseconds(1));
        node.Timeline.Update(TimeSpan.FromMilliseconds(1));
        await task;

        Assert.Equal(10.0, node.State.X, 6);
        Assert.Equal(20.0, node.State.Y, 6);
    }

    private sealed class TestMotionNode : MotionNode
    {
        public TestMotionNode()
            : base(new LibraryConfiguration())
        {
        }
    }
}
