using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Charts.Builders;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Themes;
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using Xunit;

namespace Components.Tests.Components.Charts.Builders;

public class ChartColumnSvgBuilderTests
{
    [Fact]
    public void ChartColumnSvgBuilder_Build_RendersColumns()
    {
        var svg = new SvgBuilder();
        var payload = new ColumnPayloadCollection(
        [
            new ColumnPayload
            {
                Id = "col-1",
                ChartId = "chart",
                GroupId = "group",
                Index = 0,
                SerieIndex = 0,
                Normal = new FluentUI.Blazor.Community.Components.Charts.Styles.ChartVisualStateStyle(),
                X = 10,
                Y = 20,
                Width = 30,
                Height = 40,
                CategoryIndex = 0,
                Value = 5
            }
        ]);

        var themeContext = new ChartThemeContext
        {
            Theme = new ChartTheme
            {
                Palette = new ChartPalette
                {
                    Series = [new Srgb8(10, 20, 30)],
                    StrokeSeries = [new Srgb8(5, 5, 5)]
                }
            },
            ComputedValues = new ChartComputedValues { FinalBarRadius = 2, FinalAxisThickness = 1 }
        };

        var chartContext = new FluentUI.Blazor.Community.Components.Charts.ChartContext { ClipPathId = "clip" };

        ChartColumnSvgBuilder.Build(svg, payload, chartContext, themeContext);

        var markup = svg.Build();

        Assert.Contains("chart-columns", markup);
        Assert.Contains("data-id=\"col-1\"", markup);
        Assert.Contains("clip", markup);
    }

    [Fact]
    public void ChartColumnSvgBuilder_Build_SkipsEmptyCollection()
    {
        var svg = new SvgBuilder();
        var payload = new ColumnPayloadCollection(Array.Empty<ColumnPayload>());
        var themeContext = new ChartThemeContext
        {
            Theme = new ChartTheme
            {
                Palette = new ChartPalette
                {
                    Series = [new Srgb8(10, 20, 30)],
                    StrokeSeries = [new Srgb8(5, 5, 5)]
                }
            },
            ComputedValues = new ChartComputedValues { FinalBarRadius = 2, FinalAxisThickness = 1 }
        };

        ChartColumnSvgBuilder.Build(svg, payload, new FluentUI.Blazor.Community.Components.Charts.ChartContext(), themeContext);

        Assert.DoesNotContain("chart-columns", svg.Build());
    }
}
