using System.Collections;
using System.Text.Json.Serialization;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a base for a grid layout.
/// </summary>
[JsonDerivedType(typeof(TileGridLayout))]
public abstract class GridLayoutBase
    : IEnumerable<GridLayoutBaseItem>
{
    /// <summary>
    /// Represents the items inside the layout.
    /// </summary>
    private readonly List<GridLayoutBaseItem> _items = [];

    /// <summary>
    /// Gets the items inside the layout.
    /// </summary>
    public IEnumerable<GridLayoutBaseItem> Items => _items;

    /// <summary>
    /// Event to invoke when a save is requested.
    /// </summary>
    internal event EventHandler? SaveRequested;

    /// <summary>
    /// Gets or sets a value indicating is dirty or not.
    /// </summary>
    internal bool IsDirty { get; set; }

    /// <summary>
    /// Gets the item from its key.
    /// </summary>
    /// <typeparam name="T">Type of the item.</typeparam>
    /// <param name="key">Key to find.</param>
    /// <returns>Returns the item if found, <see langword="false" /> otherwise.</returns>
    protected T? Get<T>(string? key) where T : GridLayoutBaseItem
    {
        return _items.Find(x => string.Equals(x.Key, key, StringComparison.OrdinalIgnoreCase)) as T;
    }

    /// <summary>
    /// Gets the item from its index.
    /// </summary>
    /// <typeparam name="T">Type of the item.</typeparam>
    /// <param name="index">Index of the item.</param>
    /// <returns>Returns the index of the item.</returns>
    protected T? Get<T>(int index) where T : GridLayoutBaseItem
    {
        return _items.Find(x => x.Index == index) as T;
    }

    /// <summary>
    /// Adds a range of <paramref name="items"/> into the layout.
    /// </summary>
    /// <param name="items">Items to add.</param>
    internal void AddRange(IEnumerable<GridLayoutBaseItem> items)
    {
        if (items is not null &&
            items.Any() &&
            _items.Count > 0)
        {
            _items.Clear();
            _items.AddRange(items);
        }
    }

    /// <summary>
    /// Adds an item into the layout.
    /// </summary>
    /// <typeparam name="T">Type of the item.</typeparam>
    /// <param name="key">Key of the item.</param>
    /// <param name="index">Index of the item.</param>
    internal void Add<T>(string key, int index) where T : GridLayoutBaseItem, new()
    {
        _items.Add(new T()
        {
            Index = index,
            Key = key
        });
    }

    /// <summary>
    /// Removes an item from its index.
    /// </summary>
    /// <param name="index">Index to remove.</param>
    internal void Remove(int index)
    {
        var item = _items.Find(x => x.Index == index);

        if (item is not null)
        {
            _items.Remove(item);
        }
    }

    /// <inheritdoc />
    public IEnumerator<GridLayoutBaseItem> GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Request a save.
    /// </summary>
    internal void RequestSave()
    {
        SaveRequested?.Invoke(this, EventArgs.Empty);
        IsDirty = false;
    }

    /// <summary>
    /// Updates the items.
    /// </summary>
    /// <typeparam name="TItem">Type of the items.</typeparam>
    /// <param name="keyFunc">Function to extract a key from an item.</param>
    /// <param name="items">Items to rearrange.</param>
    /// <exception cref="InvalidOperationException">Occurs when <paramref name="keyFunc"/> is
    ///  <see langword="null" />.</exception>
    internal void Update<TItem>(Func<TItem, string>? keyFunc, IList<TItem> items)
    {
        if (keyFunc is null)
        {
            throw new InvalidOperationException("The keyFunc cannot be null.");
        }

        for (var i = 0; i < items.Count; ++i)
        {
            var layoutItem = _items.Find(x => string.Equals(x.Key, keyFunc(items[i]), StringComparison.OrdinalIgnoreCase));

            layoutItem?.Index = i;
        }
    }
}
