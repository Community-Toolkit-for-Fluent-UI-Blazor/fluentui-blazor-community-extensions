using Components.Tests.Components.Charts.TestHelpers;
using FluentUI.Blazor.Community.Components.Charts;
using FluentUI.Blazor.Community.Components.Charts.Composers;
using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Charts.Series;
using Xunit;

namespace Components.Tests.Components.Charts.Composers;

public class ChartPieComposerTests
{
    [Fact]
    public void ChartPieComposer_Compose_AddsPieLayer()
    {
        var context = new ChartContext { PlotArea = new ChartRect(0, 0, 100, 100) };
        var serie = new PieSerie { Name = "Pie" };
        serie.UpdateItems([new RadialSlice { Name = "A", Value = 10 }]);

        var composer = new ChartPieComposer("chart", () => context, () => new[] { serie });
        var target = new ChartTestRenderTarget();

        composer.Compose(target, new ChartOptions());

        Assert.Single(target.Layers);
        Assert.Equal("pie", target.Layers[0].Key);
    }

    [Fact]
    public void ChartPieComposer_Compose_IgnoresInvisibleSeries()
    {
        var context = new ChartContext { PlotArea = new ChartRect(0, 0, 100, 100) };
        var serie = new PieSerie { Name = "Pie", IsVisible = false };

        var composer = new ChartPieComposer("chart", () => context, () => new[] { serie });
        var target = new ChartTestRenderTarget();

        composer.Compose(target, new ChartOptions());

        Assert.Empty(target.Layers);
    }
}
