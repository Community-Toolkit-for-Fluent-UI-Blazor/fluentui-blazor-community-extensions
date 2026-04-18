using FluentUI.Blazor.Community.Components.Charts;
using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Series;
using Xunit;

namespace Components.Tests.Components.Charts.Engines;

public class ColumnLayoutEngineTests
{
    [Fact]
    public void ColumnLayoutEngine_Layout_ComputesColumns()
    {
        var context = new ChartContext { PlotArea = new ChartRect(0, 0, 100, 100) };
        var serie = new ColumnSerie { Name = "Serie" };
        serie.UpdateItems([
            new CategoryItem { Category = "A", Value = 10 },
            new CategoryItem { Category = "B", Value = 20 }
        ]);

        var columns = ColumnLayoutEngine.Layout(serie, new[] { serie }, 10, 20, 0, context);

        Assert.Equal(2, columns.Count);
        Assert.True(columns[0].Height >= 0);
    }
}
