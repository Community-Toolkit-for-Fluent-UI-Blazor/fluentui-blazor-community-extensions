namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a builder for the <see cref="IPathBarItem"/>.
/// </summary>
internal class PathBarItemBuilder
{
    /// <summary>
    /// Represents the prefix used for the identifiers.
    /// </summary>
    internal const string Prefix = "path-bar-item-";

    /// <summary>
    /// Removes the item with the specified <paramref name="id"/> from the <paramref name="root"/>.
    /// </summary>
    /// <param name="root">Root to use for searching the value to remove.</param>
    /// <param name="id">Identifier to remove.</param>
    private static void Remove(IPathBarItem root, string id)
    {
        var item = Find(root.Items, id, true);

        if (item is not null &&
            item.Parent is not null)
        {
            var parent = item.Parent;
            var items = parent.Items.ToList();
            items.Remove(item);
            parent.Items = items;
        }
    }

    /// <summary>
    /// Removes all the items with the specified <paramref name="idCollection"/> from the <paramref name="root"/>.
    /// </summary>
    /// <param name="root">Root to use for searching the values to remove.</param>
    /// <param name="idCollection">Collection of identifiers to remove.</param>
    public static void Remove(
        IPathBarItem? root,
        IEnumerable<string> idCollection)
    {
        if (root is null || !idCollection.Any())
        {
            return;
        }

        foreach (var id in idCollection)
        {
            Remove(root, id);
        }
    }

    /// <summary>
    /// Builds a collection of <see cref="IPathBarItem"/> from the specified <paramref name="values"/>.
    /// </summary>
    /// <typeparam name="TItem">Type of the items.</typeparam>
    /// <param name="values">Values to use to create the <see cref="IPathBarItem"/>.</param>
    /// <returns>Returns the <see cref="IPathBarItem"/> items.</returns>
    public static IEnumerable<IPathBarItem> From<TItem>(IEnumerable<FileManagerEntry<TItem>> values) where TItem : class, new()
    {
        List<IPathBarItem> items = [];

        foreach (var value in values)
        {
            var item = new PathBarItem
            {
                Id = value.Id,
                Label = value.Name,
                Items = From(value.GetDirectories())
            };

            items.Add(item);
        }

        return items;
    }

    /// <summary>
    /// Merge the specified <paramref name="items"/> into the <paramref name="root"/>.
    /// </summary>
    /// <param name="root">Root to use to merge items.</param>
    /// <param name="items">Items to merge.</param>
    public static void Merge(IPathBarItem root, IEnumerable<IPathBarItem> items)
    {
        var l = root.Items.ToHashSet();

        foreach (var item in items)
        {
            l.Add(item);
        }

        root.Items = [.. l];
    }

    /// <summary>
    /// Finds the item inside <paramref name="items"/> by its <paramref name="id"/>.
    /// </summary>
    /// <param name="items">Items to use for search.</param>
    /// <param name="id">Id to find.</param>
    /// <param name="removePrefix">Value indicating if the prefix must be removed.</param>
    /// <returns>Returns the item when found, <see langword="null" /> otherwise.</returns>
    public static IPathBarItem? Find(IEnumerable<IPathBarItem>? items, string id, bool removePrefix = false)
    {
        if (items is null || !items.Any() || string.IsNullOrWhiteSpace(id))
        {
            return null;
        }

        foreach (var item in items)
        {
            var itemId = item.Id;

            if (removePrefix && (item.Id?.StartsWith(Prefix) ?? false))
            {
                itemId = item.Id[Prefix.Length..];
            }

            if (string.Equals(itemId, id, StringComparison.OrdinalIgnoreCase))
            {
                return item;
            }

            var node = Find(item.Items, id, removePrefix);

            if (node is not null)
            {
                return node;
            }
        }

        return null;
    }

    /// <summary>
    /// Gets all the parts from the specified <paramref name="root"/> matching the specified <paramref name="segments"/>.
    /// </summary>
    /// <param name="root">Root to use to build the path.</param>
    /// <param name="segments">Segments which represent the path.</param>
    /// <returns>Returns the <see cref="IPathBarItem"/> which represents the parts of the path.</returns>
    public static IEnumerable<IPathBarItem> GetAllParts(IPathBarItem? root, string[] segments)
    {
        if (segments.Length == 0)
        {
            return [];
        }

        var items = root?.Items;

        if (items is null || !items.Any())
        {
            return [];
        }

        List<IPathBarItem> result = [];

        foreach (var segment in segments[1..])
        {
            var item = Find(items, segment);

            if (item is not null)
            {
                result.Add(item);
                items = item.Items;
            }
        }

        return result;

        static IPathBarItem? Find(IEnumerable<IPathBarItem> items, string segment)
        {
            foreach (var item in items)
            {
                if (string.Equals(item.Label, segment, StringComparison.OrdinalIgnoreCase))
                {
                    return item;
                }
            }

            return null;
        }
    }

    /// <summary>
    /// Gets the identifier of the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">Identifier of the item.</param>
    /// <returns>Returns the real identifier of the item.</returns>
    public static string? GetIdentifier(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        if (value.StartsWith(Prefix))
        {
            return value;
        }

        return $"{Prefix}{value}";
    }

    /// <summary>
    /// Updates the label of the item with the specified <paramref name="id"/>.
    /// </summary>
    /// <param name="rootPath">Item where to begin the search.</param>
    /// <param name="id">Identifier of the id.</param>
    /// <param name="newLabel"></param>
    public static void UpdateLabel(IPathBarItem? rootPath, string id, string newLabel)
    {
        if (rootPath is null)
        {
            return;
        }

        var item = Find(rootPath.Items, id, true);

        item?.Label = newLabel;
    }

    /// <summary>
    /// Gets the full path of the specified <paramref name="value"/>.
    /// </summary>
    /// <param name="value">Last item of the bar.</param>
    /// <returns>Returns the full path of the specified <paramref name="value" />.</returns>
    internal static string? GetPath(IPathBarItem? value)
    {
        if (value is null)
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
