using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Charts.Builders;
using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Themes;
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using FluentUI.Blazor.Community.Components.Surface.Payloads;
using Xunit;

namespace Components.Tests.Components.Charts.Builders;

public class ChartGridSvgBuilderTests
{
    [Fact]
    public void ChartGridSvgBuilder_Build_RendersLines()
    {
        var builder = new SvgBuilder();
        var payload = new GridPayload
        {
            DisplayMode = GridDisplayMode.Lines,
            Opacity = 0.5,
            HorizontalLines =
            [
                new GridLinePayload
                {
                    StartPoint = new ChartPoint(0, 0),
                    EndPoint = new ChartPoint(10, 0)
                }
            ],
            VerticalLines =
            [
                new GridLinePayload
                {
                    StartPoint = new ChartPoint(0, 0),
                    EndPoint = new ChartPoint(0, 10)
                }
            ],
            Points = []
        };

        var context = new ChartThemeContext
        {
            ComputedValues = new ChartComputedValues
            {
                FinalGridColor = new Srgb8(10, 20, 30),
                FinalGridThickness = 2
            }
        };

        ChartGridSvgBuilder.Build(builder, payload, context);

        var markup = builder.Build();

        Assert.Contains("chart-grid", markup);
        Assert.Contains("path", markup, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ChartGridSvgBuilder_Build_RendersDots()
    {
        var builder = new SvgBuilder();
        var payload = new GridPayload
        {
            DisplayMode = GridDisplayMode.Dots,
            Opacity = 1,
            PointRadius = 2,
            HorizontalLines = [],
            VerticalLines = [],
            Points =
            [
                new GridPointPayload
                {
                    Position = new ChartPoint(5, 6)
                }
            ]
        };

        var context = new ChartThemeContext
        {
            ComputedValues = new ChartComputedValues
            {
                FinalGridColor = new Srgb8(10, 20, 30),
                FinalGridThickness = 2
            }
        };

        ChartGridSvgBuilder.Build(builder, payload, context);

        var markup = builder.Build();

        Assert.Contains("chart-grid", markup);
        Assert.Contains("circle", markup, StringComparison.OrdinalIgnoreCase);
    }
}
