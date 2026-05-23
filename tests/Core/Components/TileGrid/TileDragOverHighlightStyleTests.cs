using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.TileGrid;

public class TileDragOverHighlightStyleTests
{
    [Fact]
    public void TileDragOverHighlightStyle_ContainsExpectedValues()
    {
        var values = Enum.GetValues<TileDragOverHighlightStyle>();

        Assert.Equal(4, values.Length);
        Assert.Contains(TileDragOverHighlightStyle.Glow, values);
        Assert.Contains(TileDragOverHighlightStyle.SideBar, values);
        Assert.Contains(TileDragOverHighlightStyle.FocusRing, values);
        Assert.Contains(TileDragOverHighlightStyle.AnimatedGlow, values);
    }
}
