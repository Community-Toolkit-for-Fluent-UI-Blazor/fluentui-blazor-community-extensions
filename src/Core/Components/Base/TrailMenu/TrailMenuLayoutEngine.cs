namespace FluentUI.Blazor.Community.Components.TrailMenu;

internal static class TrailMenuLayoutEngine
{
    private const int OverflowButtonWidth = 32;

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

        var total = cache.TotalSize;
        var overflowNeeded = total + OverflowButtonWidth > containerWidth;

        var used = 0.0;
        var reserve = overflowNeeded ? OverflowButtonWidth : 0;

        for (var i = allItems.Count - 1; i >= 0; i--)
        {
            var item = allItems[i];
            var size = cache.TryGet(item.Id!, out var s) ? s : 0;

            if (used + size + reserve <= containerWidth)
            {
                visible.Add(item);
                used += size;
            }
            else
            {
                overflow.Add(item);
            }
        }

        visible.Reverse();
        overflow.Reverse();
    }
}
