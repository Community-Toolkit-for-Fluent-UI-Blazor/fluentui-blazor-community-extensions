using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.SleekDial;

public class SleekDialAnimationTypeTests
{
    [Fact]
    public void Values_AreExpected()
    {
        Assert.Equal(0, (int)SleekDialAnimationType.None);
        Assert.Equal(1, (int)SleekDialAnimationType.Fade);
        Assert.Equal(2, (int)SleekDialAnimationType.Scale);
        Assert.Equal(3, (int)SleekDialAnimationType.Slide);
        Assert.Equal(4, (int)SleekDialAnimationType.RadialSweep);
        Assert.Equal(5, (int)SleekDialAnimationType.Orbit);
        Assert.Equal(6, (int)SleekDialAnimationType.Staggered);
    }
}
