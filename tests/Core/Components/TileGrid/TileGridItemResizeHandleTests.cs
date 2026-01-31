using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.TileGrid;

public class TileGridItemResizeHandleTests
{
    [Fact]
    public void TileGridItemResizeHandle_ContainsExpectedValues()
    {
        var values = Enum.GetValues<TileGridItemResizeHandle>();

        Assert.Equal(4, values.Length);
        Assert.Contains(TileGridItemResizeHandle.None, values);
        Assert.Contains(TileGridItemResizeHandle.Bottom, values);
        Assert.Contains(TileGridItemResizeHandle.Right, values);
        Assert.Contains(TileGridItemResizeHandle.BottomRight, values);
    }
}
