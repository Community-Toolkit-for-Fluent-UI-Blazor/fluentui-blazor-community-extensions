using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Charts.Series;

public class ChartSerieTests
{
    private sealed class TestSerie : ChartSerie<CategoryItem>
    {
        public override ChartType ChartType => ChartType.Bar;

        protected internal override IEnumerable<double> Values => Items.Select(i => i.Value);

        public void Add(CategoryItem item) => AddItem(item);

        public void Remove(CategoryItem item) => RemoveItem(item);

        public void Replace(IEnumerable<CategoryItem> items) => UpdateItems(items);
    }

    [Fact]
    public void ChartSerie_ManagesItems()
    {
        var serie = new TestSerie { Name = "Serie" };
        var item = new CategoryItem { Name = "A", Value = 1 };

        serie.Add(item);

        Assert.Single(serie.Items);
        Assert.Equal(1, serie.ItemsCount);
        Assert.Equal([1], serie.Values.ToArray());

        serie.Remove(item);
        Assert.Empty(serie.Items);

        serie.Replace([new CategoryItem { Name = "B", Value = 2 }]);
        Assert.Single(serie.Items);
        Assert.Equal(1, serie.ItemsCount);
    }
}
