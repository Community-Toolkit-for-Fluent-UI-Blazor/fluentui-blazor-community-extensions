using Components.Tests.Components.Charts.TestHelpers;
using FluentUI.Blazor.Community.Components.Charts;
using FluentUI.Blazor.Community.Components.Charts.Builders;
using FluentUI.Blazor.Community.Components.Charts.Composers;
using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Charts.Composers;

public class ChartAxesComposerTests
{
    private sealed class TestSerie : ChartSerie<CategoryItem, CategorySerieOptions>
    {
        public override ChartType ChartType => ChartType.Bar;

        protected internal override IEnumerable<double> Values => Items.Select(i => i.Value);
    }

    [Fact]
    public void ChartAxesComposer_Compose_AddsAxesLayer()
    {
        var context = new ChartContext { PlotArea = new ChartRect(0, 0, 100, 100) };
        var axisOptions = new ChartAxisOptions { Show = true };
        var options = new ChartOptions { DefaultAxisOptions = new ChartAxisOptions { Show = true } };
        var series = new TestSerie { Name = "Serie" };
        series.UpdateItems([new CategoryItem { Category = "A", Value = 1 }]);

        ChartAxesBuilder.BuildCategoryAxes(context, new[] { series });

        var composer = new ChartAxesComposer(() => context, () => axisOptions);
        var target = new ChartTestRenderTarget();

        composer.Compose(target, options);

        Assert.Single(target.Layers);
        Assert.Equal("axes", target.Layers[0].Key);
    }

    [Fact]
    public void ChartAxesComposer_Compose_DoesNotAddLayerWhenHidden()
    {
        var context = new ChartContext { PlotArea = new ChartRect(0, 0, 100, 100) };
        var axisOptions = new ChartAxisOptions { Show = false };
        var options = new ChartOptions { DefaultAxisOptions = new ChartAxisOptions { Show = false } };

        var composer = new ChartAxesComposer(() => context, () => axisOptions);
        var target = new ChartTestRenderTarget();

        composer.Compose(target, options);

        Assert.Empty(target.Layers);
    }
}
