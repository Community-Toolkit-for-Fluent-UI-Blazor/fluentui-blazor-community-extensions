using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.TileGrid;

public class TileGridItemResizeHandleSizeTests
{
    [Fact]
    public void TileGridItemResizeHandleSize_ContainsExpectedValues()
    {
        var values = Enum.GetValues<TileGridItemResizeHandleSize>();

        Assert.Equal(6, values.Length);
        Assert.Contains(TileGridItemResizeHandleSize.ExtraSmall, values);
        Assert.Contains(TileGridItemResizeHandleSize.Small, values);
        Assert.Contains(TileGridItemResizeHandleSize.Medium, values);
        Assert.Contains(TileGridItemResizeHandleSize.Large, values);
        Assert.Contains(TileGridItemResizeHandleSize.ExtraLarge, values);
        Assert.Contains(TileGridItemResizeHandleSize.Full, values);
    }
}
