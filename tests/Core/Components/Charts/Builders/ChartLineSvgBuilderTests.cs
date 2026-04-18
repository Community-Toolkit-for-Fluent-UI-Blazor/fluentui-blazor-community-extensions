using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Charts.Builders;
using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Themes;
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using Xunit;

namespace Components.Tests.Components.Charts.Builders;

public class ChartLineSvgBuilderTests
{
    [Fact]
    public void ChartLineSvgBuilder_Build_RendersPathAndPoints()
    {
        var svg = new SvgBuilder();
        var payload = new LinePayload
        {
            ChartId = "chart",
            GroupId = "group",
            Id = "line",
            Index = 0,
            SerieIndex = 0,
            Normal = new FluentUI.Blazor.Community.Components.Charts.Styles.ChartVisualStateStyle(),
            Path = new LinePathPayload
            {
                Id = "path",
                Smooth = false,
                Points =
                [
                    new ChartPoint(0, 0),
                    new ChartPoint(10, 10)
                ]
            },
            Points =
            [
                new LinePointPayload
                {
                    Id = "pt-1",
                    GroupId = "group",
                    ChartId = "chart",
                    Index = 0,
                    SerieIndex = 0,
                    Normal = new FluentUI.Blazor.Community.Components.Charts.Styles.ChartVisualStateStyle(),
                    X = 0,
                    Y = 0,
                    Value = 1,
                    CategoryIndex = 0
                }
            ]
        };

        var context = new ChartThemeContext
        {
            Theme = new ChartTheme
            {
                Palette = new ChartPalette
                {
                    Series = [new Srgb8(10, 20, 30)],
                    StrokeSeries = [new Srgb8(20, 30, 40)]
                }
            },
            ComputedValues = new ChartComputedValues { FinalStrokeThickness = 2 }
        };

        ChartLineSvgBuilder.Build(svg, payload, context);

        var markup = svg.Build();

        Assert.Contains("line-", markup, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("data-id=\"path\"", markup);
        Assert.Contains("data-id=\"pt-1\"", markup);
    }

    [Fact]
    public void ChartLineSvgBuilder_Build_HandlesEmptyPoints()
    {
        var svg = new SvgBuilder();
        var payload = new LinePayload
        {
            ChartId = "chart",
            GroupId = "group",
            Id = "line",
            Index = 0,
            SerieIndex = 0,
            Normal = new FluentUI.Blazor.Community.Components.Charts.Styles.ChartVisualStateStyle(),
            Path = new LinePathPayload
            {
                Id = "path",
                Smooth = false,
                Points = []
            },
            Points = []
        };

        var context = new ChartThemeContext
        {
            Theme = new ChartTheme
            {
                Palette = new ChartPalette
                {
                    Series = [new Srgb8(10, 20, 30)],
                    StrokeSeries = [new Srgb8(20, 30, 40)]
                }
            },
            ComputedValues = new ChartComputedValues { FinalStrokeThickness = 2 }
        };

        ChartLineSvgBuilder.Build(svg, payload, context);

        var markup = svg.Build();

        Assert.Contains("data-id=\"path\"", markup);
    }
}
