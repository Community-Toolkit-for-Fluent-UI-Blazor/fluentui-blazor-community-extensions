using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Charts.Engines;

public class ColumnAxisFactoryTests
{
    [Fact]
    public void ColumnAxisFactory_CreateAxes_UsesCategoriesAndValues()
    {
        var plot = new ChartRect(0, 0, 100, 50);
        var serie = new ColumnSerie { Name = "Serie" };
        serie.UpdateItems([
            new CategoryItem { Category = "B", Value = -2 },
            new CategoryItem { Category = "A", Value = 5 }
        ]);

        var factory = new ColumnAxisFactory();
        var (xAxis, yAxis) = factory.CreateAxes(true, new[] { serie }, plot);

        Assert.Equal(ChartAxisType.Category, xAxis.AxisType);
        Assert.Equal(ChartAxisType.Numeric, yAxis.AxisType);
        Assert.Equal(-2, yAxis.Minimum);
        Assert.Equal(5, yAxis.Maximum);
        Assert.Equal("A", xAxis.Labels![0]);
    }

    [Fact]
    public void ColumnAxisFactory_CreateAxes_EmptySeriesReturnsDefaults()
    {
        var plot = new ChartRect(0, 0, 100, 50);
        var factory = new ColumnAxisFactory();

        var (xAxis, yAxis) = factory.CreateAxes(false, Array.Empty<ChartSerie>(), plot);

        Assert.Equal(0, yAxis.Minimum);
        Assert.Equal(1, yAxis.Maximum);
        Assert.Empty(xAxis.Labels!);
    }
}
