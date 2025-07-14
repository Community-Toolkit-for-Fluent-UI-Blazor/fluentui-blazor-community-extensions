using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an item for the <see cref="FluentCxPathBar"/> component.
/// </summary>
public sealed class PathBarItem
    : IPathBarItem
{
    /// <summary>
    /// Represents the children of the current instance.
    /// </summary>
    private IEnumerable<IPathBarItem> _items = [];

    /// <inheritdoc />
    public string? Label { get; set; }

    /// <inheritdoc />
    public string? Id { get; set; }

    /// <inheritdoc />
    public Icon? Icon { get; set; }

    /// <inheritdoc />
    public IEnumerable<IPathBarItem> Items
    {
        get => _items;
        set
        {
            _items = value;

            foreach (var item in _items.OfType<PathBarItem>())
            {
                item.Parent = this;
            }
        }
    }

    /// <inheritdoc />
    public IPathBarItem Parent { get; private set; } = default!;

    /// <summary>
    /// Finds the item inside <paramref name="items"/> by its <paramref name="id"/>.
    /// </summary>
    /// <param name="items">Items to use for search.</param>
    /// <param name="id">Id to find.</param>
    /// <returns>Returns the item when found, <see langword="null" /> otherwise.</returns>
    internal static IPathBarItem? Find(IEnumerable<IPathBarItem> items, string id)
    {
        foreach (var item in items)
        {
            if (string.Equals(item.Id, id, StringComparison.OrdinalIgnoreCase))
            {
                return item;
            }

            var node = Find(item.Items, id);

            if (node is not null)
            {
                return node;
            }
        }

        return null;
    }

    internal static string? GetPath(IPathBarItem? value)
    {
        if(value is null)
        {
            return null;
        }

        List<string?> paths = [];
        paths.Insert(0, value.Label);
        var currentItem = value;

        while (currentItem.Parent is not null)
        {
            currentItem = currentItem.Parent;
            paths.Insert(0, currentItem.Label);
        }

        return string.Join(Path.DirectorySeparatorChar, paths);
    }
}
