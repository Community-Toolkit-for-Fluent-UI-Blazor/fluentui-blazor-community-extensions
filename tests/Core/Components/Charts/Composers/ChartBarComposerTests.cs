using Components.Tests.Components.Charts.TestHelpers;
using FluentUI.Blazor.Community.Components.Charts;
using FluentUI.Blazor.Community.Components.Charts.Composers;
using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Charts.Series;
using Xunit;

namespace Components.Tests.Components.Charts.Composers;

public class ChartBarComposerTests
{
    [Fact]
    public void ChartBarComposer_Compose_AddsLayerPerSerie()
    {
        var context = new ChartContext { PlotArea = new ChartRect(0, 0, 100, 100) };
        var serie = new BarSerie { Name = "Serie" };
        serie.UpdateItems([new CategoryItem { Name = "A", Value = 5 }]);

        var composer = new ChartBarComposer("chart", () => context, () => new[] { serie });
        var target = new ChartTestRenderTarget();
        var options = new ChartOptions { BarStyles = new FluentUI.Blazor.Community.Components.Charts.Styles.ChartBarStyle() };

        composer.Compose(target, options);

        Assert.Single(target.Layers);
        Assert.Equal("bar", target.Layers[0].Key);
    }

    [Fact]
    public void ChartBarComposer_Compose_IgnoresInvisibleSeries()
    {
        var context = new ChartContext { PlotArea = new ChartRect(0, 0, 100, 100) };
        var serie = new BarSerie { Name = "Serie", IsVisible = false };

        var composer = new ChartBarComposer("chart", () => context, () => new[] { serie });
        var target = new ChartTestRenderTarget();

        composer.Compose(target, new ChartOptions());

        Assert.Empty(target.Layers);
    }
}
