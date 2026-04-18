using FluentUI.Blazor.Community.Components.Charts;
using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Charts.Themes;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Charts.Engines;

public class ChartAxisResolverTests
{
    [Fact]
    public void Resolve_ThrowsForMixedCategories()
    {
        var options = new ChartOptions();
        var context = new ChartContext { RemainingSpaceArea = new ChartRect(0, 0, 100, 100) };
        var theme = new ChartThemeContext { Theme = new ChartTheme() };

        var series = new ChartSerie[]
        {
            new BarSerie { Name = "Bar" },
            new PieSerie { Name = "Pie" }
        };

        Assert.Throws<NotSupportedException>(() => ChartAxisResolver.Resolve(options, context, theme, series));
    }

    [Fact]
    public void Resolve_SetsAxesForCategorySeries()
    {
        var options = new ChartOptions();
        var context = new ChartContext { RemainingSpaceArea = new ChartRect(0, 0, 200, 100) };
        var theme = new ChartThemeContext { Theme = new ChartTheme() };

        var serie = new BarSerie { Name = "Bar" };
        serie.UpdateItems([new CategoryItem { Category = "A", Value = 1 }]);

        ChartAxisResolver.Resolve(options, context, theme, new ChartSerie[] { serie });

        Assert.NotNull(context.XAxis);
        Assert.NotNull(context.YAxis);
        Assert.True(context.PlotArea.Width > 0);
        Assert.True(context.PlotArea.Height > 0);
    }
}
