using System.Drawing;
using FluentUI.Blazor.Community.Components;
using PopupPlacementEnum = FluentUI.Blazor.Community.Components.PopupPlacement;
using Xunit;

namespace Components.Tests.Components;

public class PopupPlacementResultTests
{
    [Fact]
    public void Properties_AreAssigned()
    {
        var result = new PopupPlacementResult
        {
            Position = new Point(12, 34),
            Placement = PopupPlacementEnum.BottomRight,
            PopupSize = new Size(50, 60),
            AnchorSize = new Size(20, 30)
        };

        Assert.Equal(new Point(12, 34), result.Position);
        Assert.Equal(PopupPlacementEnum.BottomRight, result.Placement);
        Assert.Equal(new Size(50, 60), result.PopupSize);
        Assert.Equal(new Size(20, 30), result.AnchorSize);
    }
}
