using FluentUI.Blazor.Community.Components.Charts;
using FluentUI.Blazor.Community.Components.Charts.Builders;
using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Charts.Builders;

public class ChartAxesBuilderTests
{
    private sealed class TestSerie : ChartSerie<CategoryItem, CategorySerieOptions>
    {
        public override ChartType ChartType => ChartType.Bar;

        protected internal override IEnumerable<double> Values => Items.Select(i => i.Value);
    }

    [Fact]
    public void ChartAxesBuilder_BuildCategoryAxes_SetsAxisRangesAndMap()
    {
        var context = new ChartContext
        {
            PlotArea = new ChartRect(10, 5, 100, 50)
        };

        var serie1 = new TestSerie { Name = "s1" };
        var serie2 = new TestSerie { Name = "s2" };

        serie1.UpdateItems([
            new CategoryItem { Category = "A", Value = 5 },
            new CategoryItem { Category = "B", Value = -2 }
        ]);
        serie2.UpdateItems([
            new CategoryItem { Category = "A", Value = 10 },
            new CategoryItem { Category = "B", Value = 3 }
        ]);

        ChartAxesBuilder.BuildCategoryAxes(context, new[] { serie1, serie2 });

        Assert.NotNull(context.XAxis);
        Assert.NotNull(context.YAxis);
        Assert.Equal(ChartAxisType.Category, context.XAxis!.AxisType);
        Assert.Equal(0, context.XAxis.Minimum);
        Assert.Equal(1, context.XAxis.Maximum);
        Assert.Equal(ChartAxisType.Numeric, context.YAxis!.AxisType);
        Assert.Equal(-2, context.YAxis.Minimum);
        Assert.Equal(10, context.YAxis.Maximum);

        var xMapped = context.XAxis.Map(1);
        var yMapped = context.YAxis.Map(0);

        Assert.Equal(110, xMapped, 6);
        Assert.Equal(46.666667, yMapped, 6);
    }
}
