using FluentUI.Blazor.Community.Components.Charts;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Charts.Engines;

public class CategoryValueEngineTests
{
    [Fact]
    public void CategoryValueEngine_ComputeValueRange_HandlesEmpty()
    {
        var (min, max) = CategoryValueEngine.ComputeValueRange([]);

        Assert.Equal(0, min);
        Assert.Equal(0, max);
    }

    [Fact]
    public void CategoryValueEngine_ComputeValueRange_ExpandsWhenEqual()
    {
        var items = new[] { new CategoryItem { Category = "A", Value = 10 } };

        var (min, max) = CategoryValueEngine.ComputeValueRange(items);

        Assert.True(min < 10);
        Assert.True(max > 10);
    }

    [Fact]
    public void CategoryValueEngine_MapValue_UsesAxisMap()
    {
        var axis = new ChartAxis { AxisType = ChartAxisType.Numeric, Minimum = 0, Maximum = 1, Map = v => v * 2 };

        Assert.Equal(6, CategoryValueEngine.MapValue(3, axis));
    }
}
