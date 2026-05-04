using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Charts.Builders;
using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Charts.Themes;
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using Xunit;

namespace Components.Tests.Components.Charts.Builders;

public class ChartMultiDonutSvgBuilderTests
{
    [Fact]
    public void ChartMultiDonutSvgBuilder_Build_RendersRingsAndLabels()
    {
        var svg = new SvgBuilder();
        var ring = new DonutPayloadCollection
        {
            Center = new ChartPoint(50, 50),
            Radius = 40,
            InnerRadius = 20,
            ShowLabels = true,
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
                    Label = "A",
                    LabelPosition = new ChartPoint(60, 60),
                    Value = 10,
                    IsMultiDonutSlice = true
                }
            ]
        };

        var payload = new MultiDonutPayloadCollection
        {
            Center = new ChartPoint(50, 50),
            Rings = [ring]
        };

        var context = new ChartThemeContext
        {
            Theme = new ChartTheme
            {
                Palette = new ChartPalette
                {
                    Series = [new Srgb8(10, 20, 30)],
                    StrokeSeries = [new Srgb8(5, 5, 5)]
                },
                Typography = new ChartTypography
                {
                    Label = new ChartTextStyle { FontFamily = "Segoe", FontSize = 12, Color = new Srgb8(1, 1, 1) }
                }
            },
            ComputedValues = new ChartComputedValues { FinalStrokeThickness = 2 }
        };

        ChartMultiDonutSvgBuilder.Build(svg, payload, context);

        var markup = svg.Build();

        Assert.Contains("chart-donut", markup);
        Assert.Contains("chart-donut-labels", markup);
        Assert.Contains("slice-1", markup);
    }
}
