using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Charts.Builders;
using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Themes;
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using Xunit;

namespace Components.Tests.Components.Charts.Builders;

public class ChartDonutSvgBuilderTests
{
    [Fact]
    public void ChartDonutSvgBuilder_Build_RendersSlicesAndLabels()
    {
        var svg = new SvgBuilder();
        var payload = new DonutPayloadCollection
        {
            Center = new ChartPoint(50, 50),
            Radius = 40,
            InnerRadius = 20,
            ShowLabels = true,
            ShowPercentages = true,
            Slices =
            [
                new PiePayload
                {
                    Id = "slice-1",
                    GroupId = "group",
                    ChartId = "chart",
                    Index = 0,
                    SerieIndex = 0,
                    Normal = new FluentUI.Blazor.Community.Components.Charts.Styles.ChartVisualStateStyle(),
                    ColorIndex = 0,
                    StartAngle = 0,
                    EndAngle = 180,
                    MidAngle = 90,
                    IsMultiDonutSlice = false,
                    Label = "A",
                    LabelPosition = new ChartPoint(60, 60),
                    Value = 10
                }
            ]
        };

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
            ComputedValues = new ChartComputedValues { FinalStrokeThickness = 2 }
        };

        ChartDonutSvgBuilder.Build(svg, payload, themeContext, renderLabelsDirect: true);

        var markup = svg.Build();

        Assert.Contains("chart-donut", markup);
        Assert.Contains("slice-1", markup);
        Assert.Contains("chart-donut-labels", markup);
        Assert.Contains("A", markup);
    }

    [Fact]
    public void ChartDonutSvgBuilder_Build_SkipsLabelsWhenDisabled()
    {
        var svg = new SvgBuilder();
        var payload = new DonutPayloadCollection
        {
            Center = new ChartPoint(50, 50),
            Radius = 40,
            InnerRadius = 20,
            ShowLabels = false,
            ShowPercentages = false,
            Slices =
            [
                new PiePayload
                {
                    Id = "slice-1",
                    GroupId = "group",
                    ChartId = "chart",
                    Index = 0,
                    SerieIndex = 0,
                    Normal = new FluentUI.Blazor.Community.Components.Charts.Styles.ChartVisualStateStyle(),
                    ColorIndex = 0,
                    StartAngle = 0,
                    EndAngle = 180,
                    MidAngle = 90,
                    IsMultiDonutSlice = false,
                    Label = "A",
                    LabelPosition = new ChartPoint(60, 60),
                    Value = 10
                }
            ]
        };

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
            ComputedValues = new ChartComputedValues { FinalStrokeThickness = 2 }
        };

        ChartDonutSvgBuilder.Build(svg, payload, themeContext, renderLabelsDirect: true);

        var markup = svg.Build();

        Assert.DoesNotContain("chart-donut-labels", markup);
    }
}
