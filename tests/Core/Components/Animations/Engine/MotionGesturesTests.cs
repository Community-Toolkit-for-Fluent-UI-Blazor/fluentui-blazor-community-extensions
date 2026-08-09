using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Animations.Engine;

public class MotionGesturesTests
{
    [Fact]
    public void Properties_AreSettable()
    {
        var gestures = new MotionGestures
        {
            Hover = new MotionVariant().Set("x", 1.0),
            Press = new MotionVariant().Set("y", 2.0),
            Tap = new MotionVariant().Set("o", 0.5),
            Drag = new MotionVariant().Set("s", 1.2)
        };

        Assert.NotNull(gestures.Hover);
        Assert.NotNull(gestures.Press);
        Assert.NotNull(gestures.Tap);
        Assert.NotNull(gestures.Drag);
    }
}
