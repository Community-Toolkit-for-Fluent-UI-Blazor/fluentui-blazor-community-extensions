using Components.Tests.Components.Charts.TestHelpers;
using FluentUI.Blazor.Community.Components.Charts;
using FluentUI.Blazor.Community.Components.Charts.Composers;
using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Charts.Series;
using Xunit;

namespace Components.Tests.Components.Charts.Composers;

public class ChartColumnComposerTests
{
    [Fact]
    public void ChartColumnComposer_Compose_AddsLayerPerSerie()
    {
        var context = new ChartContext { PlotArea = new ChartRect(0, 0, 100, 100) };
        var serie = new ColumnSerie { Name = "Serie" };
        serie.UpdateItems([new CategoryItem { Category = "A", Value = 5 }]);

        var composer = new ChartColumnComposer("chart", () => context, () => new[] { serie });
        var target = new ChartTestRenderTarget();

        composer.Compose(target, new ChartOptions());

        Assert.Single(target.Layers);
        Assert.Equal("column", target.Layers[0].Key);
    }

    [Fact]
    public void ChartColumnComposer_Compose_IgnoresInvisibleSeries()
    {
        var context = new ChartContext { PlotArea = new ChartRect(0, 0, 100, 100) };
        var serie = new ColumnSerie { Name = "Serie", IsVisible = false };

        var composer = new ChartColumnComposer("chart", () => context, () => new[] { serie });
        var target = new ChartTestRenderTarget();

        composer.Compose(target, new ChartOptions());

        Assert.Empty(target.Layers);
    }
}
