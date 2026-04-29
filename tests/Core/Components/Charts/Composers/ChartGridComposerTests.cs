using Components.Tests.Components.Charts.TestHelpers;
using FluentUI.Blazor.Community.Components.Charts;
using FluentUI.Blazor.Community.Components.Charts.Composers;
using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Options;
using Xunit;

namespace Components.Tests.Components.Charts.Composers;

public class ChartGridComposerTests
{
    [Fact]
    public void ChartGridComposer_Compose_AddsGridLayer()
    {
        var context = new ChartContext { PlotArea = new ChartRect(0, 0, 100, 100) };
        var gridOptions = new ChartGridOptions { ShowHorizontal = true, ShowVertical = true };
        var chartOptions = new ChartOptions { GridOptions = new ChartGridOptions { ShowHorizontal = true, ShowVertical = true } };

        var composer = new ChartGridComposer(() => context, () => gridOptions);
        var target = new ChartTestRenderTarget();

        composer.Compose(target, chartOptions);

        Assert.Single(target.Layers);
        Assert.Equal("grid", target.Layers[0].Key);
    }

    [Fact]
    public void ChartGridComposer_Compose_SkipsWhenHidden()
    {
        var context = new ChartContext { PlotArea = new ChartRect(0, 0, 100, 100) };
        var gridOptions = new ChartGridOptions { ShowHorizontal = false, ShowVertical = false };
        var chartOptions = new ChartOptions { GridOptions = new ChartGridOptions { ShowHorizontal = false, ShowVertical = false } };

        var composer = new ChartGridComposer(() => context, () => gridOptions);
        var target = new ChartTestRenderTarget();

        composer.Compose(target, chartOptions);

        Assert.Empty(target.Layers);
    }
}
