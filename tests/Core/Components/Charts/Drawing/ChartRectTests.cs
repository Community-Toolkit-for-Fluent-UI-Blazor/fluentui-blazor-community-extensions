using FluentUI.Blazor.Community.Components.Charts.Drawing;
using Xunit;

namespace Components.Tests.Components.Charts.Drawing;

public class ChartRectTests
{
    [Fact]
    public void ChartRect_CalculatesMarginsAgainstParent()
    {
        var parent = new ChartRect(0, 0, 100, 100);
        var rect = new ChartRect(10, 20, 30, 40, parent);

        Assert.Equal(10, rect.LeftMargin);
        Assert.Equal(60, rect.RightMargin);
        Assert.Equal(20, rect.TopMargin);
        Assert.Equal(40, rect.BottomMargin);
    }

    [Fact]
    public void ChartRect_Empty_IsZeroed()
    {
        var empty = ChartRect.Empty;

        Assert.Equal(0, empty.X);
        Assert.Equal(0, empty.Y);
        Assert.Equal(0, empty.Width);
        Assert.Equal(0, empty.Height);
    }
}
