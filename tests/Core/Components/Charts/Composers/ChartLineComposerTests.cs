using Components.Tests.Components.Charts.TestHelpers;
using FluentUI.Blazor.Community.Components.Charts;
using FluentUI.Blazor.Community.Components.Charts.Composers;
using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;
using FluentUI.Blazor.Community.Components.Charts.Options;
using Xunit;

namespace Components.Tests.Components.Charts.Composers;

public class ChartLineComposerTests
{
    [Fact]
    public void ChartLineComposer_Compose_AddsLineLayer()
    {
        var context = new ChartContext
        {
            PlotArea = new ChartRect(0, 0, 100, 100),
            XAxis = new ChartAxis { AxisType = ChartAxisType.Category, Minimum = 0, Maximum = 0, Map = _ => 50 },
            YAxis = new ChartAxis { AxisType = ChartAxisType.Numeric, Minimum = 0, Maximum = 10, Map = v => 100 - v }
        };

        var serie = new CategoryLineSerie { Name = "Serie" };
        serie.UpdateItems([new CategoryItem { Category = "A", Value = 5 }]);

        var composer = new ChartLineComposer("chart", () => context, () => new[] { serie });
        var target = new ChartTestRenderTarget();

        composer.Compose(target, new ChartOptions());

        Assert.Single(target.Layers);
        Assert.Equal("line", target.Layers[0].Key);
    }

    [Fact]
    public void ChartLineComposer_Compose_IgnoresInvisibleSeries()
    {
        var context = new ChartContext
        {
            PlotArea = new ChartRect(0, 0, 100, 100),
            XAxis = new ChartAxis { AxisType = ChartAxisType.Category, Minimum = 0, Maximum = 0, Map = _ => 50 },
            YAxis = new ChartAxis { AxisType = ChartAxisType.Numeric, Minimum = 0, Maximum = 10, Map = v => 100 - v }
        };

        var serie = new CategoryLineSerie { Name = "Serie", IsVisible = false };

        var composer = new ChartLineComposer("chart", () => context, () => new[] { serie });
        var target = new ChartTestRenderTarget();

        composer.Compose(target, new ChartOptions());

        Assert.Empty(target.Layers);
    }
}
