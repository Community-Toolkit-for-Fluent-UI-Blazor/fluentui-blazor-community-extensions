using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.SleekDial;

public class SleekDialRadialDirectionTests
{
    [Fact]
    public void Values_AreExpected()
    {
        Assert.Equal(0, (int)SleekDialRadialDirection.Clockwise);
        Assert.Equal(1, (int)SleekDialRadialDirection.Counterclockwise);
    }
}
