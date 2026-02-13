using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components;

public class AnchorLogicalPositionTests
{
    [Fact]
    public void Values_AreExpected()
    {
        Assert.Equal(0, (int)AnchorLogicalPosition.TopLeft);
        Assert.Equal(1, (int)AnchorLogicalPosition.TopCenter);
        Assert.Equal(2, (int)AnchorLogicalPosition.TopRight);
        Assert.Equal(3, (int)AnchorLogicalPosition.MiddleLeft);
        Assert.Equal(4, (int)AnchorLogicalPosition.MiddleCenter);
        Assert.Equal(5, (int)AnchorLogicalPosition.MiddleRight);
        Assert.Equal(6, (int)AnchorLogicalPosition.BottomLeft);
        Assert.Equal(7, (int)AnchorLogicalPosition.BottomCenter);
        Assert.Equal(8, (int)AnchorLogicalPosition.BottomRight);
    }
}
