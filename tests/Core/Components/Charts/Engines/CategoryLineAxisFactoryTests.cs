using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Factories;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Charts.Engines;

public class CategoryLineAxisFactoryTests
{
    [Fact]
    public void CategoryLineAxisFactory_CreateAxes_UsesCategoriesAndValues()
    {
        var plot = new ChartRect(0, 0, 100, 50);
        var serie = new CategoryLineSerie { Name = "Serie" };
        serie.UpdateItems([
            new CategoryItem { Category = "B", Value = -2 },
            new CategoryItem { Category = "A", Value = 5 }
        ]);

        var factory = new CategoryLineAxisFactory();
        var (xAxis, yAxis) = factory.CreateAxes(false, new[] { serie }, plot);

        Assert.Equal(ChartAxisType.Category, xAxis.AxisType);
        Assert.Equal(ChartAxisType.Numeric, yAxis.AxisType);
        Assert.Equal(-2, yAxis.Minimum);
        Assert.Equal(5, yAxis.Maximum);
    }

    [Fact]
    public void CategoryLineAxisFactory_CreateAxes_EmptySeriesReturnsDefaults()
    {
        var plot = new ChartRect(0, 0, 100, 50);
        var factory = new CategoryLineAxisFactory();

        var (xAxis, yAxis) = factory.CreateAxes(false, Array.Empty<ChartSerie>(), plot);

        Assert.Equal(0, yAxis.Minimum);
        Assert.Equal(1, yAxis.Maximum);
        Assert.Empty(xAxis.Labels!);
    }
}
