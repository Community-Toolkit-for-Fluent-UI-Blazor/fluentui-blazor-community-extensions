using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Charts.Builders;
using ChartAxisOptions = FluentUI.Blazor.Community.Components.Charts.Options.ChartAxisOptions;
using FluentUI.Blazor.Community.Components.Charts.Themes;
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using FluentUI.Blazor.Community.Components.Enums;
using FluentUI.Blazor.Community.Components.Surface.Payloads;
using Xunit;

namespace Components.Tests.Components.Charts.Builders;

public class ChartAxesSvgBuilderTests
{
    [Fact]
    public void ChartAxesSvgBuilder_Build_RendersAxesTicksAndLabels()
    {
        var svg = new SvgBuilder();
        var payload = new AxisPayload
        {
            XAxis = new AxisGeometryPayload
            {
                StartPoint = new(0, 10),
                EndPoint = new(100, 10),
                Ticks = [new(50, 10)],
                Labels =
                [
                    new AxisLabelPayload
                    {
                        Position = new(50, 12),
                        Text = "X",
                        Anchor = SvgTextAnchor.Middle,
                        Rotation = 0
                    }
                ]
            },
            YAxis = new AxisGeometryPayload
            {
                StartPoint = new(0, 0),
                EndPoint = new(0, 100),
                Ticks = [new(0, 50)],
                Labels =
                [
                    new AxisLabelPayload
                    {
                        Position = new(0, 50),
                        Text = "Y",
                        Anchor = SvgTextAnchor.Middle,
                        Rotation = 0
                    }
                ]
            }
        };

        var options = new ChartAxisOptions
        {
            Show = true,
            ShowLabels = true,
            ShowTicks = true,
            TickLength = 10
        };

        var theme = new ChartTheme
        {
            Palette = new ChartPalette
            {
                Axis = new Srgb8(10, 20, 30)
            },
            Typography = new ChartTypography
            {
                Label = new ChartTextStyle { FontFamily = "Segoe", FontSize = 12, Color = new Srgb8(5, 5, 5) }
            }
        };

        var themeContext = new ChartThemeContext
        {
            Theme = theme,
            ComputedValues = new ChartComputedValues { FinalAxisThickness = 2 }
        };

        ChartAxesSvgBuilder.Build(svg, payload, options, themeContext);

        var markup = svg.Build();

        Assert.Contains("chart-axis", markup);
        Assert.Contains("X", markup);
        Assert.Contains("Y", markup);
    }
}
