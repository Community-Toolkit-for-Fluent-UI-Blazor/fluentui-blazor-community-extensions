using FluentUI.Blazor.Community.Components.Charts;
using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Charts.Engines;

public class CategoryLineLayoutEngineTests
{
    [Fact]
    public void CategoryLineLayoutEngine_Layout_MapsPoints()
    {
        var context = new ChartContext
        {
            XAxis = new ChartAxis { AxisType = ChartAxisType.Category, Minimum = 0, Maximum = 1, Map = i => i * 10 },
            YAxis = new ChartAxis { AxisType = ChartAxisType.Numeric, Minimum = 0, Maximum = 10, Map = v => v * 2 }
        };

        var serie = new CategoryLineSerie { Name = "Serie" };
        serie.UpdateItems([
            new CategoryItem { Category = "A", Value = 2 },
            new CategoryItem { Category = "B", Value = 3 }
        ]);

        var (path, points) = CategoryLineLayoutEngine.Layout(serie, context);

        Assert.Equal(2, path.Points.Count);
        Assert.Equal(0, points[0].X);
        Assert.Equal(4, points[0].Y);
    }
}
