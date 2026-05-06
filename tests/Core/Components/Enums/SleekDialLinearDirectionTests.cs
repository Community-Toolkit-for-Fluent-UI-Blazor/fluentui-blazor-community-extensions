using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.SleekDial;

public class SleekDialLinearDirectionTests
{
    [Fact]
    public void Values_AreExpected()
    {
        Assert.Equal(0, (int)SleekDialLinearDirection.Default);
        Assert.Equal(1, (int)SleekDialLinearDirection.Up);
        Assert.Equal(2, (int)SleekDialLinearDirection.Down);
        Assert.Equal(3, (int)SleekDialLinearDirection.Left);
        Assert.Equal(4, (int)SleekDialLinearDirection.Right);
    }
}
