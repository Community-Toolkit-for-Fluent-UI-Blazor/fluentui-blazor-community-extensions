using System.Drawing;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides mapping logic to calculate the position and size of scheduler items within a month view overlay, organizing
/// items by day and arranging them into a grid layout.
/// </summary>
/// <remarks>This class is typically used in calendar or scheduling applications to visually position items within
/// a month grid, accounting for gaps, label heights, and cell dimensions. The mapping ensures that items for the same
/// day are distributed vertically within their respective cells. Thread safety is not guaranteed; concurrent access
/// should be managed externally if required.</remarks>
/// <typeparam name="TItem">The type of the data associated with each scheduler item to be mapped.</typeparam>
internal class MonthPositionMapper<TItem> : IPositionMapper<TItem>
{
    /// <summary>
    /// Represents the number of columns in the month view grid.
    /// </summary>
    private readonly int _columns;

    /// <summary>
    /// Represents the scheduler items grouped by their respective days.
    /// </summary>
    private readonly Dictionary<DateTime, List<SchedulerItem<TItem>>> _itemsByDay;

    /// <summary>
    /// Represents the gap between rows and columns in the month overlay layout.
    /// </summary>
    private readonly float _gap;

    /// <summary>
    /// Represents the height allocated for the day label in each cell.
    /// </summary>
    private readonly float _labelHeight;

    /// <summary>
    /// Represents the height of each day cell in the month overlay.
    /// </summary>
    private readonly float _cellHeight;

    /// <summary>
    /// Represents the total vertical space available for rendering the month overlay.
    /// </summary>
    private readonly float _usableHeight;

    /// <summary>
    /// Represents the left padding within each cell.
    /// </summary>
    private readonly float _paddingLeft;

    /// <summary>
    /// Represents the right padding within each cell.
    /// </summary>
    private readonly float _paddingRight;

    /// <summary>
    /// Represents the vertical spacing between items within the same day cell.
    /// </summary>
    private const int _itemSpacing = 2;

    /// <summary>
    /// Represents the maximum number of items allowed in each cell.
    /// </summary>
    private readonly int _maxItemsCount;

    /// <summary>
    /// Initializes a new instance of the MonthPositionMapper class with the specified column count, scheduler items by
    /// day, and cell layout settings.
    /// </summary>
    /// <param name="columns">The number of columns to display in the month overlay. Must be greater than zero; values less than one are
    /// treated as one.</param>
    /// <param name="itemsByDay">A dictionary mapping each day to a list of scheduler items to be displayed for that day. Cannot be null.</param>
    /// <param name="layout">The layout settings for each month cell, including gap size, label height, cell height, usable height, and
    /// padding values. Cannot be null.</param>
    /// <param name="maxItemsCount">The maximum number of items allowed in each cell.</param>
    public MonthPositionMapper(
        int columns,
        Dictionary<DateTime, List<SchedulerItem<TItem>>> itemsByDay,
        MeasureLayout layout,
        int maxItemsCount)
    {
        _columns = Math.Max(1, columns);
        _itemsByDay = (itemsByDay ?? [])
            .ToDictionary(
                kvp => kvp.Key.Date,
                kvp => (kvp.Value ?? [])
                    .OrderBy(it => it.Start)
                    .ThenBy(it => it.End)
                    .ThenBy(it => it.Id)
                    .ToList());

        _gap = layout.Gap;
        _labelHeight = layout.LabelSize.Height;
        _cellHeight = layout.CellSize.Height;
        _usableHeight = layout.UsableHeight;
        _paddingLeft = layout.Padding.Left;
        _paddingRight = layout.Padding.Right;
        _maxItemsCount = maxItemsCount;
    }

    /// <inheritdoc />
    public IEnumerable<MappedItemRect> Map(
        SchedulerSlot[] slots,
        SchedulerItem<TItem> item,
        ElementDimensions container,
        DateTime date)
    {
        var results = new List<MappedItemRect>();

        if (slots == null || slots.Length == 0)
        {
            return results;
        }

        var firstDay = slots.Min(s => s.Start.Date);
        var lastDay = slots.Max(s => s.Start.Date);

        var firstDayOfWeek = DayOfWeek.Monday;
        var diff = ((int)firstDay.DayOfWeek - (int)firstDayOfWeek + 7) % 7;
        var gridWeekStart = firstDay.AddDays(-diff);

        var weeks = new List<(DateTime Start, DateTime EndExclusive)>();
        for (var ws = gridWeekStart; ws <= lastDay; ws = ws.AddDays(_columns))
        {
            weeks.Add((ws, ws.AddDays(_columns)));
        }

        var cellWidth = (float)((container.Width - (_columns - 1) * _gap) / _columns);
        var rowHeight = _cellHeight;

        var lastSlotDay = lastDay;

        foreach (var (weekStart, weekEndExclusive) in weeks)
        {
            var itemStart = item.Start.Date;
            var itemEndInclusive = item.End.Date;

            var segStart = itemStart < weekStart ? weekStart : itemStart;
            var segEndInclusive = itemEndInclusive >= weekEndExclusive.AddDays(-1)
                ? weekEndExclusive.AddDays(-1)
                : itemEndInclusive;

            if (segEndInclusive > lastSlotDay)
            {
                segEndInclusive = lastSlotDay;
            }

            if (segStart > segEndInclusive)
            {
                continue;
            }

            var weekIndex = (weekStart - gridWeekStart).Days / _columns;
            var weekOffsetY = weekIndex * (rowHeight + _gap);

            for (var day = segStart; day <= segEndInclusive; day = day.AddDays(1))
            {
                var dayIndex = (day - weekStart).Days;
                var left = dayIndex * (cellWidth + _gap) + _paddingLeft;
                var width = cellWidth - _paddingLeft - _paddingRight;

                if (width < 0)
                {
                    width = 0;
                }

                var slotUsable = _usableHeight;
                var totalSpacing = _itemSpacing * (_maxItemsCount + 1);
                var stackedHeight = _maxItemsCount > 0
                    ? (slotUsable - totalSpacing) / (_maxItemsCount + 1)
                    : slotUsable;

                if (stackedHeight < 0)
                {
                    stackedHeight = 0;
                }

                var itemsInDay = GetItemsOverlappingDay(day);
                var assignedIndexById = new Dictionary<long, int>();

                for (var i = 0; i < itemsInDay.Count; i++)
                {
                    assignedIndexById[itemsInDay[i].Id] = i;
                }

                var itemIndex = assignedIndexById.TryGetValue(item.Id, out var idxFound) ? idxFound : 0;
                var top = weekOffsetY + _labelHeight + _itemSpacing + itemIndex * (stackedHeight + _itemSpacing);
                var height = stackedHeight;

                var gutter = _itemSpacing;
                var finalLeft = left + gutter;
                var finalWidth = Math.Max(0f, width - 2f * gutter);
                var finalTop = top + gutter;
                var finalHeight = Math.Max(0f, height - 2f * gutter);

                results.Add(new MappedItemRect
                {
                    Rect = new RectangleF(finalLeft, finalTop, finalWidth, finalHeight),
                    ShowLeftAnchor = day == item.Start.Date,
                    ShowRightAnchor = day == item.End.Date
                });
            }
        }

        return results;
    }

    /// <summary>
    /// Retrieves all scheduled items that overlap the specified day.
    /// </summary>
    /// <remarks>The number of returned items is limited by the maximum items count configured for the
    /// scheduler.</remarks>
    /// <param name="day">The day to check for overlapping scheduled items. Only items whose start or end dates include this day are
    /// returned.</param>
    /// <returns>A list of scheduled items that overlap the specified day. The list will be empty if no items overlap the day.</returns>
    private List<SchedulerItem<TItem>> GetItemsOverlappingDay(DateTime day)
    {
        var results = new List<SchedulerItem<TItem>>();

        if (_itemsByDay.TryGetValue(day.Date, out var list) && list is not null)
        {
            var count = 0;

            for (var i = 0; i < list.Count && count < _maxItemsCount; i++)
            {
                var it = list[i];

                if (it.Start.Date <= day && it.End.Date >= day)
                {
                    results.Add(it);
                    count++;
                }
            }
        }

        return results;
    }
}
