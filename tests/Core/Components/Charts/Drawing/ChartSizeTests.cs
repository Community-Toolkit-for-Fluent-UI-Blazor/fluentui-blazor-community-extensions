using FluentUI.Blazor.Community.Components.Charts;
using Xunit;

namespace Components.Tests.Components.Charts.Drawing;

public class ChartSizeTests
{
    [Fact]
    public void ChartSize_AssignsValues()
    {
        var size = new ChartSize(10, 20);

        Assert.Equal(10, size.Width);
        Assert.Equal(20, size.Height);
    }
}
