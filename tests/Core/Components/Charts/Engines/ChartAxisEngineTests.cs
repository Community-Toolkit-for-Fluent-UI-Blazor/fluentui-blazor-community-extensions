using FluentUI.Blazor.Community.Components.Charts;
using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Charts.Engines;

public class ChartAxisEngineTests
{
    [Fact]
    public void Build_ReturnsNullWhenHiddenOrMissingAxes()
    {
        var engine = new ChartAxisEngine();
        var context = new ChartContext { PlotArea = new ChartRect(0, 0, 100, 100) };
        var options = new ChartAxisOptions { Show = false };

        var result = engine.Build(context, options);

        Assert.Null(result);
    }

    [Fact]
    public void Build_BuildsCategoryAxes()
    {
        var engine = new ChartAxisEngine();
        var context = new ChartContext
        {
            PlotArea = new ChartRect(0, 0, 100, 100),
            XAxis = new ChartAxis { AxisType = ChartAxisType.Category, Minimum = 0, Maximum = 1, Map = i => i * 10, Labels = ["A", "B"] },
            YAxis = new ChartAxis { AxisType = ChartAxisType.Numeric, Minimum = 0, Maximum = 10, Map = v => v }
        };

        var options = new ChartAxisOptions { Show = true };

        var result = engine.Build(context, options);

        Assert.NotNull(result);
        Assert.NotNull(result!.XAxis);
        Assert.NotNull(result.YAxis);
        Assert.NotEmpty(result.XAxis!.Ticks);
        Assert.NotEmpty(result.YAxis!.Ticks);
        Assert.NotEmpty(result.XAxis.Labels);
    }
}
