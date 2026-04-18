using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Charts.Builders;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Styles;
using FluentUI.Blazor.Community.Components.Charts.Themes;
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using Xunit;

namespace Components.Tests.Components.Charts.Builders;

public class ChartBarSvgBuilderTests
{
    [Fact]
    public void ChartBarSvgBuilder_Build_RendersBars()
    {
        var svg = new SvgBuilder();
        var payload = new BarPayloadCollection(Guid.NewGuid().ToString(),
        [
            new BarPayload
            {
                Id = "bar-1",
                ChartId = "chart",
                GroupId = "group",
                Index = 0,
                SerieIndex = 0,
                X = 10,
                Y = 20,
                Width = 30,
                Height = 40,
                CategoryIndex = 0,
                Value = 5,
                Normal = new ChartVisualStateStyle()
            }
        ]);

        var themeContext = new ChartThemeContext
        {
            Theme = new ChartTheme
            {
                Palette = new ChartPalette
                {
                    Series = [new Srgb8(10, 20, 30)]
                }
            },
            ComputedValues = new ChartComputedValues { FinalBarRadius = 2, FinalAxisThickness = 1 }
        };

        var chartContext = new FluentUI.Blazor.Community.Components.Charts.ChartContext { ClipPathId = "clip" };

        ChartBarSvgBuilder.Build(svg, payload, themeContext, chartContext);

        var markup = svg.Build();

        Assert.Contains("chart-bars", markup);
        Assert.Contains("data-id=\"bar-1\"", markup);
        Assert.Contains("clip", markup);
    }

    [Fact]
    public void ChartBarSvgBuilder_Build_SkipsEmptyCollection()
    {
        var svg = new SvgBuilder();
        var payload = new BarPayloadCollection(Guid.NewGuid().ToString(), Array.Empty<BarPayload>());
        var themeContext = new ChartThemeContext
        {
            Theme = new ChartTheme
            {
                Palette = new ChartPalette
                {
                    Series = [new Srgb8(10, 20, 30)]
                }
            },
            ComputedValues = new ChartComputedValues { FinalBarRadius = 2, FinalAxisThickness = 1 }
        };

        ChartBarSvgBuilder.Build(svg, payload, themeContext, new FluentUI.Blazor.Community.Components.Charts.ChartContext());

        Assert.DoesNotContain("chart-bars", svg.Build());
    }
}
