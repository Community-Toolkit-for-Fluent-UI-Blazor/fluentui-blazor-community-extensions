namespace FluentUI.Blazor.Community.Components.TrailMenu;

internal static class TrailMenuLayoutEngine
{
    private const int OverflowButtonWidth = 40;

    /// <summary>
    /// Calculates which menu items should be displayed directly and which should be placed in an overflow menu, based
    /// on the available container width and the cached sizes of the items.
    /// </summary>
    /// <remarks>This method modifies the contents of the provided 'visible' and 'overflow' lists. It does not
    /// return a value, but updates these lists to reflect the current layout based on the container width and item
    /// sizes. The method assumes that the cache contains valid size information for all items.</remarks>
    /// <param name="allItems">The complete list of menu items to evaluate for visibility and overflow placement.</param>
    /// <param name="cache">A cache containing the measured sizes of each menu item, used to determine how many items can fit within the
    /// container.</param>
    /// <param name="containerWidth">The total width, in pixels, available for displaying menu items in the container.</param>
    /// <param name="visible">A list that will be populated with the menu items that fit within the container width and are visible to the
    /// user. The list is cleared before being populated.</param>
    /// <param name="overflow">A list that will be populated with the menu items that do not fit within the container width and are placed in
    /// the overflow menu. The list is cleared before being populated.</param>
    public static void Compute(
    IReadOnlyList<ITrailMenuItem> allItems,
    TrailMenuCache cache,
    double containerWidth,
    List<ITrailMenuItem> visible,
    List<ITrailMenuItem> overflow)
    {
        visible.Clear();
        overflow.Clear();

        if (allItems.Count == 0)
        {
            return;
        }

        var last = allItems[^1];
        cache.TryGet(last.Id!, out var lastSize);

        var remaining = containerWidth - lastSize - OverflowButtonWidth;

        double used = 0;

        for (var i = 0; i < allItems.Count - 1; i++)
        {
            var item = allItems[i];
            cache.TryGet(item.Id!, out var size);

            if (used + size > remaining)
            {
                overflow.AddRange(allItems.Take(allItems.Count - 1));
                visible.Add(last);

                return;
            }

            used += size;
        }

        visible.AddRange(allItems);
    }
}
