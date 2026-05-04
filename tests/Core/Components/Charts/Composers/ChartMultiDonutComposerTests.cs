using Components.Tests.Components.Charts.TestHelpers;
using FluentUI.Blazor.Community.Components.Charts;
using FluentUI.Blazor.Community.Components.Charts.Composers;
using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Charts.Series;
using Xunit;

namespace Components.Tests.Components.Charts.Composers;

public class ChartMultiDonutComposerTests
{
    [Fact]
    public void ChartMultiDonutComposer_Compose_AddsMultiDonutLayer()
    {
        var context = new ChartContext { PlotArea = new ChartRect(0, 0, 100, 100) };
        var donut = new DonutSerie { Name = "Donut" };
        donut.UpdateItems([new RadialSlice { Name = "A", Value = 10 }]);

        var multi = new MultiDonutSerie { Name = "Multi" };
        multi.Series.Add(donut);

        var composer = new ChartMultiDonutComposer("chart", () => context, () => new[] { multi });
        var target = new ChartTestRenderTarget();

        composer.Compose(target, new ChartOptions());

        Assert.Single(target.Layers);
        Assert.Equal("multi-donut", target.Layers[0].Key);
    }

    [Fact]
    public void ChartMultiDonutComposer_Compose_SkipsWhenNoSeries()
    {
        var context = new ChartContext { PlotArea = new ChartRect(0, 0, 100, 100) };
        var multi = new MultiDonutSerie { Name = "Multi" };

        var composer = new ChartMultiDonutComposer("chart", () => context, () => new[] { multi });
        var target = new ChartTestRenderTarget();

        composer.Compose(target, new ChartOptions());

        Assert.Empty(target.Layers);
    }
}
