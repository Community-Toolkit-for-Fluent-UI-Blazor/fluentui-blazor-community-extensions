using FluentUI.Blazor.Community.Components.Charts;
using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Options;
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
            new CategoryItem { Name = "A", Value = 10 },
            new CategoryItem { Name = "B", Value = 20 }
        ]);
        var options = new ChartOptions();

        var bars = BarLayoutEngine.Layout(serie, new[] { serie }, 10, 20, 0, context, options);

        Assert.Equal(2, bars.Count);
        Assert.True(bars[0].Width >= 0);
    }
}
