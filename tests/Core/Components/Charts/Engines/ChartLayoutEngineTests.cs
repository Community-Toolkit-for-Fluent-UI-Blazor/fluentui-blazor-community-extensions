using FluentUI.Blazor.Community.Components.Charts;
using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Themes;
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Charts.Engines;

public class ChartLayoutEngineTests
{
    [Fact]
    public void ChartLayoutEngine_ComputeLayout_SetsAreas()
    {
        var context = new ChartContext { Width = 200, Height = 100 };
        var theme = new ChartThemeContext
        {
            Theme = new ChartTheme
            {
                Layout = new ChartLayout(),
                Typography = new ChartTypography
                {
                    Title = new ChartTextStyle { FontSize = 12, Color = new Srgb8(1, 1, 1) },
                    Subtitle = new ChartTextStyle { FontSize = 10, Color = new Srgb8(2, 2, 2) },
                    Legend = new ChartTextStyle { FontSize = 8, Color = new Srgb8(3, 3, 3) }
                }
            }
        };

        ChartLayoutEngine.ComputeLayout(context, theme, "Title", ChartTitlePosition.Top, "Subtitle", ChartTitlePosition.Bottom, 1, ChartLegendPosition.Bottom);

        Assert.NotEqual(ChartRect.Empty, context.TitleArea);
        Assert.NotEqual(ChartRect.Empty, context.SubtitleArea);
        Assert.NotEqual(ChartRect.Empty, context.LegendArea);
        Assert.NotEqual(ChartRect.Empty, context.RemainingSpaceArea);
    }

    [Fact]
    public void ChartLayoutEngine_ComputeLayout_ThrowsForInvalidPositions()
    {
        var context = new ChartContext { Width = 200, Height = 100 };
        var theme = new ChartThemeContext
        {
            Theme = new ChartTheme
            {
                Layout = new ChartLayout(),
                Typography = new ChartTypography
                {
                    Title = new ChartTextStyle { FontSize = 12, Color = new Srgb8(1, 1, 1) },
                    Subtitle = new ChartTextStyle { FontSize = 10, Color = new Srgb8(2, 2, 2) },
                    Legend = new ChartTextStyle { FontSize = 8, Color = new Srgb8(3, 3, 3) }
                }
            }
        };

        Assert.Throws<InvalidOperationException>(() =>
            ChartLayoutEngine.ComputeLayout(context, theme, "Title", ChartTitlePosition.Bottom, "Subtitle", ChartTitlePosition.Top, 0, ChartLegendPosition.Top));
    }
}
