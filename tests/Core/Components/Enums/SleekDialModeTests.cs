using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.SleekDial;

public class SleekDialModeTests
{
    [Fact]
    public void Values_AreExpected()
    {
        Assert.Equal(0, (int)SleekDialMode.Linear);
        Assert.Equal(1, (int)SleekDialMode.Radial);
    }
}
