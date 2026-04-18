using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Charts.Engines;

public class BarAxisFactoryTests
{
    [Fact]
    public void BarAxisFactory_CreateAxes_UsesCategoriesAndValues()
    {
        var plot = new ChartRect(0, 0, 100, 50);
        var serie = new BarSerie { Name = "Serie" };
        serie.UpdateItems([
            new CategoryItem { Category = "B", Value = -2 },
            new CategoryItem { Category = "A", Value = 5 }
        ]);

        var factory = new BarAxisFactory();
        var (xAxis, yAxis) = factory.CreateAxes(true, new[] { serie }, plot);

        Assert.Equal(ChartAxisType.Numeric, xAxis.AxisType);
        Assert.Equal(ChartAxisType.Category, yAxis.AxisType);
        Assert.Equal(-2, xAxis.Minimum);
        Assert.Equal(5, xAxis.Maximum);
        Assert.Equal("A", yAxis.Labels![0]);
    }

    [Fact]
    public void BarAxisFactory_CreateAxes_EmptySeriesReturnsDefaults()
    {
        var plot = new ChartRect(0, 0, 100, 50);
        var factory = new BarAxisFactory();

        var (xAxis, yAxis) = factory.CreateAxes(false, Array.Empty<ChartSerie>(), plot);

        Assert.Equal(0, xAxis.Minimum);
        Assert.Equal(1, xAxis.Maximum);
        Assert.Empty(yAxis.Labels!);
    }
}
