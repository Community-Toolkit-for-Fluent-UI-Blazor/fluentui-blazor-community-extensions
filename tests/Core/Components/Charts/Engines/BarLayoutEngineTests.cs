using FluentUI.Blazor.Community.Components.Charts;
using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Series;
using Xunit;

namespace Components.Tests.Components.Charts.Engines;

public class BarLayoutEngineTests
{
    [Fact]
    public void BarLayoutEngine_Layout_ComputesBars()
    {
        var context = new ChartContext { PlotArea = new ChartRect(0, 0, 100, 100) };
        var serie = new BarSerie { Name = "Serie" };
        serie.UpdateItems([
            new CategoryItem { Category = "A", Value = 10 },
            new CategoryItem { Category = "B", Value = 20 }
        ]);

        var bars = BarLayoutEngine.Layout(serie, new[] { serie }, 10, 20, 0, context);

        Assert.Equal(2, bars.Count);
        Assert.True(bars[0].Width >= 0);
    }
}
