using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components;

public class PreferredPopupDirectionTests
{
    [Fact]
    public void Values_AreExpected()
    {
        Assert.Equal(0, (int)PreferredPopupDirection.Default);
        Assert.Equal(1, (int)PreferredPopupDirection.Left);
        Assert.Equal(2, (int)PreferredPopupDirection.Right);
        Assert.Equal(3, (int)PreferredPopupDirection.Up);
        Assert.Equal(4, (int)PreferredPopupDirection.Down);
    }
}
