using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Animations.Engine;

public class MotionPresenceTests
{
    [Fact]
    public void Properties_AreSettable()
    {
        var presence = new MotionPresence
        {
            Enter = new MotionVariant().Set("o", 1.0),
            Exit = new MotionVariant().Set("o", 0.0),
            Transition = new MotionTransition { Duration = TimeSpan.FromMilliseconds(200) }
        };

        Assert.NotNull(presence.Enter);
        Assert.NotNull(presence.Exit);
        Assert.Equal(TimeSpan.FromMilliseconds(200), presence.Transition?.Duration);
    }
}
