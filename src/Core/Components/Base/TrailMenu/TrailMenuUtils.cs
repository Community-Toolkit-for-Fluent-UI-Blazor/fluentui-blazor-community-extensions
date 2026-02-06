namespace FluentUI.Blazor.Community.Components.TrailMenu;

/// <summary>
/// Provides utility methods for manipulating and querying trail menu items.
/// This class is fully independent from any factory or builder.
/// </summary>
internal static class TrailMenuUtils
{
    /// <summary>
    /// Prefix used for generated identifiers.
    /// </summary>
    internal const string Prefix = "trail-menu-item-";

    /// <summary>
    /// Removes the menu item with the specified identifier from the menu structure, starting from the given root item.
    /// </summary>
    /// <remarks>If the item with the specified identifier is not found or does not have a parent, no changes
    /// are made to the menu structure. The method updates the parent's item collection to reflect the
    /// removal.</remarks>
    /// <param name="root">The root menu item from which the search for the item to remove begins. Cannot be null.</param>
    /// <param name="id">The unique identifier of the menu item to remove. Cannot be null or empty.</param>
    private static void RemoveInternal(ITrailMenuItem root, string id)
    {
        var item = Find(root.Items, id, removePrefix: true);

        if (item is null || item.Parent is null)
        {
            return;
        }

        var parent = item.Parent;

        if (parent.Items is IList<ITrailMenuItem> list)
        {
            list.Remove(item);
            parent.Items = list;
        }
        else
        {
            parent.Items = parent.Items.Where(i => i != item).ToList();
        }
    }

    /// <summary>
    /// Removes the menu items with the specified identifiers from the given root menu item.
    /// </summary>
    /// <remarks>If the root menu item is null, the method performs no operation. Each identifier in the
    /// collection is processed individually.</remarks>
    /// <param name="root">The root menu item from which to remove the specified items. This parameter can be null, in which case no action
    /// is taken.</param>
    /// <param name="ids">A collection of string identifiers representing the menu items to remove from the root menu item.</param>
    public static void Remove(ITrailMenuItem? root, IEnumerable<string> ids)
    {
        if (root is null)
        {
            return;
        }

        foreach (var id in ids)
        {
            RemoveInternal(root, id);
        }
    }

    /// <summary>
    /// Merges the specified collection of menu items into the root menu item, ensuring that duplicate items are not
    /// added.
    /// </summary>
    /// <remarks>If any of the items in the collection already exist in the root menu item's collection, they
    /// will not be added again. The resulting set of items replaces the existing items in the root menu item.</remarks>
    /// <param name="root">The root menu item to which the specified items will be merged. This parameter must not be null.</param>
    /// <param name="items">An enumerable collection of menu items to merge with the root item. The collection can be empty but must not be
    /// null.</param>
    public static void Merge(ITrailMenuItem root, IEnumerable<ITrailMenuItem> items)
    {
        var set = new HashSet<ITrailMenuItem>(root.Items);
        set.UnionWith(items);
        root.Items = [.. set];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool IsSame(string? left, string? right)
    {
        if (string.IsNullOrEmpty(left) || string.IsNullOrEmpty(right))
        {
            return string.Equals(left, right, StringComparison.OrdinalIgnoreCase);
        }

        if (left.StartsWith(Prefix))
        {
            left = left[Prefix.Length..];
        }

        if (right.StartsWith(Prefix))
        {
            right = right[Prefix.Length..];
        }

        return string.Equals(left, right, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Searches for a trail menu item with the specified identifier within a collection and its descendants.
    /// </summary>
    /// <remarks>The search includes all items in the provided collection as well as their nested child items.
    /// If removePrefix is true, the method removes a predefined prefix from both the search identifier and each item's
    /// identifier before performing the comparison.</remarks>
    /// <param name="items">An enumerable collection of trail menu items to search. Can be null.</param>
    /// <param name="id">The identifier of the trail menu item to locate. The comparison is case-insensitive.</param>
    /// <param name="removePrefix">true to remove a predefined prefix from both the search identifier and item identifiers before comparison;
    /// otherwise, false. The default is false.</param>
    /// <returns>The trail menu item that matches the specified identifier, or null if no matching item is found or if the items
    /// collection is null.</returns>
    public static ITrailMenuItem? Find(IEnumerable<ITrailMenuItem>? items, string id, bool removePrefix = false)
    {
        if (items is null)
        {
            return null;
        }

        var queue = new Queue<ITrailMenuItem>(items);

        if (removePrefix && id.StartsWith(Prefix))
        {
            id = id[Prefix.Length..];
        }

        while (queue.Count > 0)
        {
            var item = queue.Dequeue();
            var itemId = item.Id;

            if (removePrefix && itemId?.StartsWith(Prefix) == true)
            {
                itemId = itemId[Prefix.Length..];
            }

            if (string.Equals(itemId, id, StringComparison.OrdinalIgnoreCase))
            {
                return item;
            }

            foreach (var child in item.Items)
            {
                queue.Enqueue(child);
            }
        }

        return null;
    }

    /// <summary>
    /// Retrieves a sequence of trail menu items that correspond to the specified path segments, starting from the given
    /// root item.
    /// </summary>
    /// <remarks>The search stops at the first segment that does not match a child item, and only the items
    /// found up to that point are returned.</remarks>
    /// <param name="root">The root menu item from which to begin the search. Cannot be null.</param>
    /// <param name="segments">An array of strings representing the path segments used to navigate through the menu hierarchy. The first
    /// segment is ignored; subsequent segments are used to locate child items.</param>
    /// <returns>An enumerable collection of trail menu items that match the provided segments. Returns an empty collection if
    /// the root is null, the segments array is empty, or if no matching items are found.</returns>
    public static IEnumerable<ITrailMenuItem> GetAllParts(ITrailMenuItem? root, string[] segments)
    {
        if (root is null || segments.Length == 0)
        {
            return [];
        }

        var items = root.Items;
        var result = new List<ITrailMenuItem>();

        for (var i = 1; i < segments.Length; i++)
        {
            var segment = segments[i];
            var found = FindByLabel(items, segment);

            if (found is null)
            {
                break;
            }

            result.Add(found);
            items = found.Items;
        }

        return result;
    }

    /// <summary>
    /// Searches for the first item in the collection whose label matches the specified value, using a case-insensitive
    /// comparison.
    /// </summary>
    /// <remarks>The comparison is case-insensitive; for example, 'Label' and 'label' are considered equal.
    /// The method performs a linear search and returns the first match encountered.</remarks>
    /// <param name="items">The collection of items to search for a matching label. Cannot be null.</param>
    /// <param name="label">The label to match against the items in the collection. Cannot be null or empty.</param>
    /// <returns>The first item whose label matches the specified value, or null if no such item is found.</returns>
    private static ITrailMenuItem? FindByLabel(IEnumerable<ITrailMenuItem> items, string label)
    {
        foreach (var item in items)
        {
            if (string.Equals(item.Label, label, StringComparison.OrdinalIgnoreCase))
            {
                return item;
            }
        }

        return null;
    }

    /// <summary>
    /// Returns the specified value with a predefined prefix if it does not already start with the prefix.
    /// </summary>
    /// <remarks>Use this method to ensure that identifiers conform to a required prefix format.</remarks>
    /// <param name="value">The string to process. If null or empty, the value is returned unchanged.</param>
    /// <returns>The input string with the prefix prepended if it did not already start with the prefix; otherwise, the original
    /// value.</returns>
    public static string? GetIdentifier(string? value)
    {
        return string.IsNullOrEmpty(value) || value.StartsWith(Prefix)
            ? value
            : $"{Prefix}{value}";
    }

    /// <summary>
    /// Updates the label of a menu item identified by the specified ID within the given root menu item.
    /// </summary>
    /// <remarks>If the menu item with the specified ID is not found, no changes are made.</remarks>
    /// <param name="root">The root menu item containing the items to search. This parameter can be null, in which case the method does
    /// nothing.</param>
    /// <param name="id">The unique identifier of the menu item whose label is to be updated.</param>
    /// <param name="newLabel">The new label to assign to the menu item identified by the specified ID.</param>
    public static void UpdateLabel(ITrailMenuItem? root, string id, string newLabel)
    {
        if (root is null)
        {
            return;
        }

        var item = Find(root.Items, id, removePrefix: true);

        item?.Label = newLabel;
    }

    /// <summary>
    /// Gets the full path of the specified trail menu item as a string, with each label separated by the system
    /// directory separator character.
    /// </summary>
    /// <remarks>The path is constructed by traversing the parent chain of the specified item, collecting each
    /// label, and joining them using <see cref="System.IO.Path.DirectorySeparatorChar"/>. This can be used to display
    /// or process the hierarchical location of a menu item within a trail menu structure.</remarks>
    /// <param name="value">The trail menu item for which to retrieve the path. If <see langword="null"/>, the method returns <see
    /// langword="null"/>.</param>
    /// <returns>A string representing the path of the trail menu item, or <see langword="null"/> if <paramref name="value"/> is
    /// <see langword="null"/>.</returns>
    public static string? GetPath(ITrailMenuItem? value)
    {
        if (value is null)
        {
            return null;
        }

        var parts = new List<string?>();

        var current = value;

        while (current is not null)
        {
            parts.Add(current.Label);
            current = current.Parent;
        }

        parts.Reverse();

        return string.Join(Path.DirectorySeparatorChar, parts);
    }

    /// <summary>
    /// Parses the specified path into an array of segments, splitting by the directory separator character.
    /// </summary>
    /// <remarks>This method trims any leading or trailing whitespace from each segment and removes any empty
    /// entries from the result.</remarks>
    /// <param name="path">The path to be parsed into segments. Must not be null or whitespace; otherwise, an empty array is returned.</param>
    /// <returns>An array of strings representing the segments of the path. The array will be empty if the input path is null or
    /// whitespace.</returns>
    internal static string[] ParseSegments(string? path)
        => string.IsNullOrWhiteSpace(path)
            ? []
            : path.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
}
