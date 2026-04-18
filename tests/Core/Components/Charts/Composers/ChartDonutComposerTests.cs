using Components.Tests.Components.Charts.TestHelpers;
using FluentUI.Blazor.Community.Components.Charts;
using FluentUI.Blazor.Community.Components.Charts.Composers;
using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Charts.Series;
using Xunit;

namespace Components.Tests.Components.Charts.Composers;

public class ChartDonutComposerTests
{
    [Fact]
    public void ChartDonutComposer_Compose_AddsDonutLayer()
    {
        var context = new ChartContext { PlotArea = new ChartRect(0, 0, 100, 100) };
        var serie = new DonutSerie { Name = "Donut" };
        serie.UpdateItems([new RadialSlice { Name = "A", Value = 10 }]);

        var composer = new ChartDonutComposer("chart", () => context, () => new[] { serie });
        var target = new ChartTestRenderTarget();

        composer.Compose(target, new ChartOptions());

        Assert.Single(target.Layers);
        Assert.Equal("donut", target.Layers[0].Key);
    }

    [Fact]
    public void ChartDonutComposer_Compose_SkipsMultiDonutSeries()
    {
        var context = new ChartContext { PlotArea = new ChartRect(0, 0, 100, 100) };
        var serie = new DonutSerie { Name = "Donut" };
        serie.UpdateItems([new RadialSlice { Name = "A", Value = 10 }]);
        serie.IsPartOfMultiDonut = true;

        var composer = new ChartDonutComposer("chart", () => context, () => new[] { serie });
        var target = new ChartTestRenderTarget();

        composer.Compose(target, new ChartOptions());

        Assert.Empty(target.Layers);
    }
}
