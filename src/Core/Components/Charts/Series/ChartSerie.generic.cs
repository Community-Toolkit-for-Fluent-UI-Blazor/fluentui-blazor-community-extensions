using FluentUI.Blazor.Community.Components.Charts.Options;
namespace FluentUI.Blazor.Community.Components.Charts.Series;

/// <summary>
/// Base class for all chart series.
/// Contains only identity, visibility, styling, animation and interaction.
/// Data and options belong to derived types.
/// </summary>
public abstract class ChartSerie<TItem, TOptions> : ChartSerie
    where TItem : ChartItem
    where TOptions : IChartSerieOptions
{
    private readonly List<TItem> _items = [];

    /// <summary>
    /// Gets the collection of items to be displayed.
    /// </summary>
    public IReadOnlyList<TItem> Items => _items;

    /// <summary>
    /// Gets the configuration options for the chart series.
    /// </summary>
    public TOptions? Options { get; set; }

    /// <inheritdoc />
    protected internal sealed override int ItemsCount => Items.Count;

    /// <summary>
    /// Gets the raw collection of items to be displayed.
    /// </summary>
    internal override IReadOnlyList<ChartItem> RawItems => Items;

    internal void AddItem(TItem item) => _items.Add(item);

    internal void RemoveItem(TItem item) => _items.Remove(item);

    internal void UpdateItems(IEnumerable<TItem> items)
    {
        _items.Clear();
        _items.AddRange(items);
    }
}
