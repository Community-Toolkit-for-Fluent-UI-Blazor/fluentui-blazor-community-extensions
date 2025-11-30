namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides algorithms for computing slot-based layouts for scheduler items, assigning each item to a column such that
/// overlapping items are placed in separate columns.
/// </summary>
/// <remarks>This class is intended for use in scenarios where visual or logical arrangement of time-based items
/// is required, such as calendar or scheduling applications. It ensures that items which overlap in time are assigned
/// to different columns, allowing for clear, non-overlapping presentation. All methods are static and
/// thread-safe.</remarks>
public static class SlotLayoutEngine
{
    /// <summary>
    /// Calculates the column layout for a collection of scheduler items, assigning each item to a column such that
    /// overlapping items are placed in separate columns.
    /// </summary>
    /// <remarks>Items that overlap in time are assigned to different columns. The returned list preserves the
    /// mapping between each item and its layout information, which can be used for rendering or further processing. The
    /// method sorts the input items by start time before layout calculation.</remarks>
    /// <typeparam name="TItem">The type of the data contained within each scheduler item.</typeparam>
    /// <param name="items">The list of scheduler items to be arranged into columns. Items must specify their start times and durations for
    /// overlap calculation.</param>
    /// <returns>A list of layout results, where each result contains the original item, its assigned column index, and the total
    /// number of columns in its overlapping group.</returns>
    public static List<SlotLayoutResult<TItem>> ComputeLayout<TItem>(List<SchedulerItem<TItem>> items)
    {
        items.Sort((a, b) => a.Start.CompareTo(b.Start));
        var results = new List<SlotLayoutResult<TItem>>();

        foreach (var group in GroupOverlapping(items))
        {
            var activeColumns = new List<List<SchedulerItem<TItem>>>();

            foreach (var item in group)
            {
                var placed = false;

                for (var col = 0; col < activeColumns.Count; col++)
                {
                    var column = activeColumns[col];
                    if (!column.Any(existing => Overlaps(existing, item)))
                    {
                        column.Add(item);
                        results.Add(new SlotLayoutResult<TItem>
                        {
                            Item = item,
                            ColumnIndex = col,
                            ColumnCount = 0 // temporaire
                        });
                        placed = true;
                        break;
                    }
                }

                if (!placed)
                {
                    activeColumns.Add(new List<SchedulerItem<TItem>> { item });
                    results.Add(new SlotLayoutResult<TItem>
                    {
                        Item = item,
                        ColumnIndex = activeColumns.Count - 1,
                        ColumnCount = 0 // temporaire
                    });
                }
            }

            // Post-process : tous les items du groupe ont le même ColumnCount
            var groupResults = results.Where(r => group.Contains(r.Item)).ToList();
            var colCount = activeColumns.Count;
            foreach (var r in groupResults)
            {
                r.ColumnCount = colCount;
            }
        }

        return results;
    }

    /// <summary>
    /// Determines whether two scheduler items have overlapping time intervals.
    /// </summary>
    /// <remarks>The comparison is based on the Start and End properties of each scheduler item. Overlap is
    /// detected if the intervals share any portion of time.</remarks>
    /// <typeparam name="TItem">The type of the value contained within each scheduler item.</typeparam>
    /// <param name="a">The first scheduler item to compare.</param>
    /// <param name="b">The second scheduler item to compare.</param>
    /// <returns>true if the time interval of <paramref name="a"/> overlaps with that of <paramref name="b"/>; otherwise, false.</returns>
    private static bool Overlaps<TItem>(SchedulerItem<TItem> a, SchedulerItem<TItem> b)
    {
        return a.Start < b.End && b.Start < a.End;
    }

    /// <summary>
    /// Groups a list of scheduler items into collections where each group contains items that overlap with at least one
    /// other item in the same group.
    /// </summary>
    /// <remarks>Items are considered overlapping if they satisfy the criteria defined by the Overlaps method.
    /// The grouping is not necessarily minimal; items are assigned to the first overlapping group found. The method
    /// does not modify the input list.</remarks>
    /// <typeparam name="TItem">The type of the value contained within each scheduler item.</typeparam>
    /// <param name="items">The list of scheduler items to be grouped based on overlapping criteria. Cannot be null.</param>
    /// <returns>A list of groups, where each group is a list of scheduler items that overlap with each other. Each item appears
    /// in exactly one group.</returns>
    public static List<List<SchedulerItem<TItem>>> GroupOverlapping<TItem>(List<SchedulerItem<TItem>> items)
    {
        var groups = new List<List<SchedulerItem<TItem>>>();
        var visited = new HashSet<SchedulerItem<TItem>>();

        foreach (var item in items)
        {
            if (visited.Contains(item))
                continue;

            var group = new List<SchedulerItem<TItem>>();
            var stack = new Stack<SchedulerItem<TItem>>();
            stack.Push(item);

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                if (!visited.Add(current))
                {
                    continue;
                }

                group.Add(current);

                foreach (var other in items)
                {
                    if (!visited.Contains(other) && Overlaps(current, other))
                    {
                        stack.Push(other);
                    }
                }
            }

            groups.Add(group);
        }

        return groups;
    }
}
