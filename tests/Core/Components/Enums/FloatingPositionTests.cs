using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Enums;

public class FloatingPositionTests
{
    [Fact]
    public void Values_AreExpected()
    {
        Assert.Equal(0, (int)FloatingPosition.TopLeft);
        Assert.Equal(1, (int)FloatingPosition.TopCenter);
        Assert.Equal(2, (int)FloatingPosition.TopRight);
        Assert.Equal(3, (int)FloatingPosition.MiddleLeft);
        Assert.Equal(4, (int)FloatingPosition.MiddleCenter);
        Assert.Equal(5, (int)FloatingPosition.MiddleRight);
        Assert.Equal(6, (int)FloatingPosition.BottomLeft);
        Assert.Equal(7, (int)FloatingPosition.BottomCenter);
        Assert.Equal(8, (int)FloatingPosition.BottomRight);
    }
}
