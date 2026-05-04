using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Charts.Builders;
using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Enums;
using FluentUI.Blazor.Community.Components.Charts.Themes;
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using Microsoft.FluentUI.AspNetCore.Components;
using Xunit;

namespace Components.Tests.Components.Charts.Builders;

public class ChartLayoutSvgBuilderTests
{
    [Fact]
    public void ChartLayoutSvgBuilder_Build_RendersTitleSubtitleAndLegend()
    {
        var builder = new SvgBuilder();
        var payload = new ChartLayoutPayload(
            new ChartTitlePayload { Text = "Title", Area = new ChartRect(0, 0, 100, 20) },
            new ChartSubtitlePayload { Text = "Subtitle", Area = new ChartRect(0, 20, 100, 20) },
            new ChartLegendPayload
            {
                Area = new ChartRect(0, 40, 200, 60),
                ItemCount = 1,
                Shape = ChartLegendItemShape.Circle,
                Items = [new LegendItem("Serie", 0)]
            });

        var themeContext = new ChartThemeContext
        {
            Theme = new ChartTheme
            {
                Palette = new ChartPalette
                {
                    Series = [new Srgb8(10, 20, 30)]
                },
                Typography = new ChartTypography
                {
                    Title = new ChartTextStyle { FontFamily = "Segoe", FontSize = 16, FontWeight = TextWeight.Semibold, Color = new Srgb8(1, 1, 1) },
                    Subtitle = new ChartTextStyle { FontFamily = "Segoe", FontSize = 12, FontWeight = TextWeight.Regular, Color = new Srgb8(2, 2, 2) },
                    Legend = new ChartTextStyle { FontFamily = "Segoe", FontSize = 10, FontWeight = TextWeight.Regular, Color = new Srgb8(3, 3, 3) }
                },
                Layout = new ChartLayout()
            }
        };

        ChartLayoutSvgBuilder.Build(builder, payload, themeContext);

        var markup = builder.Build();

        Assert.Contains("chart-layout", markup);
        Assert.Contains("Title", markup);
        Assert.Contains("Subtitle", markup);
        Assert.Contains("Serie", markup);
    }

    [Fact]
    public void ChartLayoutSvgBuilder_Build_SkipsWhenNoPayloads()
    {
        var builder = new SvgBuilder();
        var payload = new ChartLayoutPayload(null, null, null);
        var themeContext = new ChartThemeContext
        {
            Theme = new ChartTheme
            {
                Palette = new ChartPalette
                {
                    Series = [new Srgb8(10, 20, 30)]
                },
                Typography = new ChartTypography
                {
                    Title = new ChartTextStyle { FontFamily = "Segoe", FontSize = 16, FontWeight = TextWeight.Semibold, Color = new Srgb8(1, 1, 1) }
                },
                Layout = new ChartLayout()
            }
        };

        ChartLayoutSvgBuilder.Build(builder, payload, themeContext);

        var markup = builder.Build();

        Assert.DoesNotContain("chart-layout", markup);
    }
}
