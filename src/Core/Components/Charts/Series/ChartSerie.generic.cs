namespace FluentUI.Blazor.Community.Components.Charts.Series;

/// <summary>
/// Base class for all chart series.
/// Contains only identity, visibility, styling, animation and interaction.
/// Data and options belong to derived types.
/// </summary>
public abstract class ChartSerie<TItem> : ChartSerie
    where TItem : ChartItem
{
    private readonly List<TItem> _items = [];

    /// <summary>
    /// Gets the collection of items to be displayed.
    /// </summary>
    public IReadOnlyList<TItem> Items => _items;

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

    internal void Sort(Func<TItem, TItem, int> comparison)
    {
        _items.Sort((a, b) =>
        {
            return comparison(a, b);
        });
    }
}
