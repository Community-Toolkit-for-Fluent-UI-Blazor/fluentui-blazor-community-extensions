using Components.Tests.Components.Charts.TestHelpers;
using FluentUI.Blazor.Community.Components.Charts;
using FluentUI.Blazor.Community.Components.Charts.Composers;
using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Charts.Composers;

public class ChartLayoutComposerTests
{
    [Fact]
    public void ChartLayoutComposer_Compose_AddsLayoutLayer()
    {
        var context = new ChartContext
        {
            TitleArea = new ChartRect(0, 0, 100, 10),
            SubtitleArea = new ChartRect(0, 10, 100, 10),
            LegendArea = new ChartRect(0, 20, 200, 50),
            LegendItemCount = 1
        };

        var serie = new BarSerie { Name = "Serie" };
        serie.UpdateItems([new CategoryItem { Category = "A", Value = 1 }]);

        var composer = new ChartLayoutComposer(
            () => context,
            () => "Title",
            () => "Subtitle",
            () => ChartLegendItemShape.Circle,
            () => new[] { serie });

        var target = new ChartTestRenderTarget();

        composer.Compose(target, new ChartOptions());

        Assert.Single(target.Layers);
        Assert.Equal("chartlayout", target.Layers[0].Key);
    }

    [Fact]
    public void ChartLayoutComposer_Compose_SkipsWhenNoPayloads()
    {
        var context = new ChartContext();

        var composer = new ChartLayoutComposer(
            () => context,
            () => null,
            () => null,
            () => ChartLegendItemShape.Circle,
            () => Array.Empty<ChartSerie>());

        var target = new ChartTestRenderTarget();

        composer.Compose(target, new ChartOptions());

        Assert.Empty(target.Layers);
    }
}
