using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.SleekDial;

public class SleekDialHideModeTests
{
    [Fact]
    public void Values_AreExpected()
    {
        Assert.Equal(0, (int)SleekDialHideMode.None);
        Assert.Equal(1, (int)SleekDialHideMode.WhenEmpty);
        Assert.Equal(2, (int)SleekDialHideMode.WhenNoVisible);
        Assert.Equal(3, (int)SleekDialHideMode.WhenEmptyOrNoVisible);
    }
}
