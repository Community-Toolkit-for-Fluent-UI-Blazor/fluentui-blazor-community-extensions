using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components;

public class PopupPlacementTests
{
    [Fact]
    public void Values_AreExpected()
    {
        Assert.Equal(0, (int)PopupPlacement.Auto);
        Assert.Equal(1, (int)PopupPlacement.TopLeft);
        Assert.Equal(2, (int)PopupPlacement.TopCenter);
        Assert.Equal(3, (int)PopupPlacement.TopRight);
        Assert.Equal(4, (int)PopupPlacement.BottomLeft);
        Assert.Equal(5, (int)PopupPlacement.BottomCenter);
        Assert.Equal(6, (int)PopupPlacement.BottomRight);
        Assert.Equal(7, (int)PopupPlacement.LeftTop);
        Assert.Equal(8, (int)PopupPlacement.LeftCenter);
        Assert.Equal(9, (int)PopupPlacement.LeftBottom);
        Assert.Equal(10, (int)PopupPlacement.RightTop);
        Assert.Equal(11, (int)PopupPlacement.RightCenter);
        Assert.Equal(12, (int)PopupPlacement.RightBottom);
    }
}
