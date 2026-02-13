using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.SleekDial;

public class SleekDialEasingTests
{
    [Fact]
    public void Values_AreExpected()
    {
        Assert.Equal(0, (int)SleekDialEasing.Linear);
        Assert.Equal(1, (int)SleekDialEasing.EaseIn);
        Assert.Equal(2, (int)SleekDialEasing.EaseOut);
        Assert.Equal(3, (int)SleekDialEasing.EaseInOut);
        Assert.Equal(4, (int)SleekDialEasing.Spring);
    }
}
