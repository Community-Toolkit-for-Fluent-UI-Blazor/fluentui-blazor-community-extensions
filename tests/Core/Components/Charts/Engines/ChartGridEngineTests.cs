using FluentUI.Blazor.Community.Components.Charts;
using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Charts.Engines;

public class ChartGridEngineTests
{
    [Fact]
    public void ChartGridEngine_Build_ReturnsNullWhenHidden()
    {
        var engine = new ChartGridEngine();
        var context = new ChartContext();
        var options = new ChartGridOptions { ShowHorizontal = false, ShowVertical = false };

        var payload = engine.Build(context, options);

        Assert.Null(payload);
    }

    [Fact]
    public void ChartGridEngine_Build_LinesWithAxes()
    {
        var engine = new ChartGridEngine();
        var context = new ChartContext
        {
            PlotArea = new ChartRect(0, 0, 100, 100),
            XAxis = new ChartAxis { AxisType = ChartAxisType.Category, Minimum = 0, Maximum = 1, Map = i => i * 50 },
            YAxis = new ChartAxis { AxisType = ChartAxisType.Numeric, Minimum = 0, Maximum = 10, Map = v => 100 - v * 10 }
        };
        var options = new ChartGridOptions { ShowHorizontal = true, ShowVertical = true };

        var payload = engine.Build(context, options);

        Assert.NotNull(payload);
        Assert.NotEmpty(payload!.HorizontalLines);
        Assert.NotEmpty(payload.VerticalLines);
    }

    [Fact]
    public void ChartGridEngine_Build_DotsWhenNoAxes()
    {
        var engine = new ChartGridEngine();
        var context = new ChartContext { PlotArea = new ChartRect(0, 0, 100, 100) };
        var options = new ChartGridOptions { DisplayMode = FluentUI.Blazor.Community.Components.GridDisplayMode.Dots, ShowHorizontal = true, ShowVertical = true, CellSize = 10 };

        var payload = engine.Build(context, options);

        Assert.NotNull(payload);
        Assert.Empty(payload!.HorizontalLines);
        Assert.Empty(payload.VerticalLines);
        Assert.NotEmpty(payload.Points);
    }
}
