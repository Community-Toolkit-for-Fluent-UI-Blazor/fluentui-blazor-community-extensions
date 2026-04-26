using FluentUI.Blazor.Community.Components.Charts;
using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Charts.Engines;

public class CategoryAxisEngineTests
{
    [Fact]
    public void CategoryAxisEngine_Sort_PreservesOrderWhenSortEnabled()
    {
        var items = new List<CategoryItem>
        {
            new() { Name = "B", Value = 1 },
            new() { Name = "A", Value = 2 }
        };

        var result = CategoryAxisEngine.Sort(items, new TestCategoryOptions { Sort = true });

        Assert.Equal("B", result[0].Name);
    }

    [Fact]
    public void CategoryAxisEngine_Sort_SortsWhenDisabled()
    {
        var items = new List<CategoryItem>
        {
            new() { Name = "B", Value = 1 },
            new() { Name = "A", Value = 2 }
        };

        var result = CategoryAxisEngine.Sort(items, new TestCategoryOptions { Sort = false });

        Assert.Equal("A", result[0].Name);
    }

    [Fact]
    public void CategoryAxisEngine_ComputeCategoryWidth_HandlesZeroCount()
    {
        var plot = new ChartRect(0, 0, 100, 50);

        Assert.Equal(0, CategoryAxisEngine.ComputeCategoryWidth(0, plot));
    }

    [Fact]
    public void CategoryAxisEngine_ComputeCategoryCenters_MapsAxis()
    {
        var axis = new ChartAxis { AxisType = ChartAxisType.Category, Minimum = 0, Maximum = 1, Map = i => i * 10 };

        var centers = CategoryAxisEngine.ComputeCategoryCenters(2, axis);

        Assert.Equal([0.0, 10.0], centers);
    }

    private sealed class TestCategoryOptions : CategorySerieOptions
    {
    }
}
