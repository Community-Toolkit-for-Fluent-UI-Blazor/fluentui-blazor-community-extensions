using System.Drawing;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides mapping functionality for scheduling items onto a timeline view, calculating their visual positions and
/// layout based on time subdivisions and working hour settings.
/// </summary>
/// <remarks>Use this class to determine the placement and sizing of items within a timeline-based scheduler UI.
/// It supports configurable time subdivisions, working hour visibility, and per-day item layouts. Thread safety is not
/// guaranteed; concurrent access should be externally synchronized if required.</remarks>
/// <typeparam name="TItem">The type of the data item associated with each scheduled entry in the timeline.</typeparam>
public class TimelinePositionMapper<TItem> : IPositionMapper<TItem>
{
    /// <summary>
    /// Indicates the number of subdivisions per hour in the timeline.
    /// </summary>
    private readonly int _subdivisionCount;

    /// <summary>
    /// Indicates the start time of the timeline view.
    /// </summary>
    private readonly TimeSpan _timelineStart;

    /// <summary>
    /// Indicates the end time of the timeline view.
    /// </summary>
    private readonly TimeSpan _timelineEnd;

    /// <summary>
    /// Indicates the vertical gap between items in the timeline.
    /// </summary>
    private readonly int _gap;

    /// <summary>
    /// Indicates the fixed height allocated for each item in the timeline.
    /// </summary>
    private const int HeightPerItem = 40;

    /// <summary>
    /// Indicates a function to retrieve scheduled items for a specific day.
    /// </summary>
    private readonly Dictionary<DateTime, List<SchedulerItem<TItem>>> _getItemsByDay;

    /// <summary>
    /// Indicates a mapping of dates to item row indices for layout purposes.
    /// </summary>
    private readonly Dictionary<DateTime, Dictionary<long, int>> _rowIndexByDate = [];

    /// <summary>
    /// Indicates a mapping of dates to the count of rows required for layout.
    /// </summary>
    private readonly Dictionary<DateTime, int> _rowsCountByDate = [];

    /// <summary>
    /// Indicates whether non-working hours should be displayed in the timeline.
    /// </summary>
    private readonly bool _showNonWorkingHours;

    /// <summary>
    /// Indicates the number of minutes represented by each subdivision in the timeline.
    /// </summary>
    private readonly float _minutesPerSubdivision;

    /// <summary>
    /// Initializes a new instance of the TimelineMapper class with the specified timeline parameters and item mapping.
    /// </summary>
    /// <param name="subdivisionCount">The number of subdivisions per hour in the timeline. Must be a positive integer.</param>
    /// <param name="showNonWorkingHours">A value indicating whether non-working hours should be displayed in the timeline. Set to <see langword="true"/>
    /// to include non-working hours; otherwise, <see langword="false"/>.</param>
    /// <param name="timelineStart">The start time of the timeline, represented as a <see cref="TimeSpan"/>. Must be less than <paramref
    /// name="timelineEnd"/>.</param>
    /// <param name="timelineEnd">The end time of the timeline, represented as a <see cref="TimeSpan"/>. Must be greater than <paramref
    /// name="timelineStart"/>.</param>
    /// <param name="getItemsByDay">A dictionary mapping each date to a list of scheduler items for that day. Cannot be <see langword="null"/>.</param>
    /// <param name="gap">The gap, in pixels, between timeline subdivisions. The default value is 2.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="getItemsByDay"/> is <see langword="null"/>.</exception>
    public TimelinePositionMapper(
        int subdivisionCount,
        bool showNonWorkingHours,
        TimeSpan timelineStart,
        TimeSpan timelineEnd,
        Dictionary<DateTime, List<SchedulerItem<TItem>>> getItemsByDay,
        int gap = 2)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(subdivisionCount);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(timelineEnd, timelineStart);

        _subdivisionCount = subdivisionCount;
        _timelineStart = timelineStart;
        _timelineEnd = timelineEnd;
        _gap = gap;
        _getItemsByDay = getItemsByDay ?? throw new ArgumentNullException(nameof(getItemsByDay));
        _showNonWorkingHours = showNonWorkingHours;
        _minutesPerSubdivision = 60.0f / _subdivisionCount;
    }

    /// <summary>
    /// Calculates the total vertical height required to display all items for the specified date.
    /// </summary>
    /// <param name="date">The date for which to calculate the required display height. Only the date component is considered; the time
    /// component is ignored.</param>
    /// <returns>The total height, in pixels, needed to display all items for the given date. Returns 0 if there are no items for
    /// the specified date.</returns>
    public int GetRequiredHeight(DateTime date)
    {
        EnsureRowLayout(date);
        var rows = _rowsCountByDate.TryGetValue(date.Date, out var rc) ? rc : 0;

        return rows <= 0 ? 0 : rows * HeightPerItem + (rows - 1) * _gap;
    }

    /// <summary>
    /// Maps a scheduler item to its visual rectangle(s) within the specified container for a given date, based on the
    /// provided time slots.
    /// </summary>
    /// <remarks>If the item's time range does not intersect with the specified date or falls outside the
    /// visible timeline, no rectangles are returned. The mapping accounts for non-working hours and timeline
    /// subdivisions as configured.</remarks>
    /// <param name="slots">An array of scheduler slots representing the time subdivisions for the specified date. Used to determine the
    /// item's placement and sizing.</param>
    /// <param name="item">The scheduler item to be mapped. Contains the item's time range and identifying information.</param>
    /// <param name="container">The dimensions of the container in which the item will be visually rendered. Determines the scaling and
    /// positioning of the mapped rectangle.</param>
    /// <param name="date">The date for which the mapping is performed. Only the portion of the item that falls within this date will be
    /// mapped.</param>
    /// <returns>An enumerable collection of MappedItemRect objects representing the visual rectangles for the item within the
    /// container on the specified date. The collection will be empty if the item does not overlap with the date.</returns>
    public IEnumerable<MappedItemRect> Map(
        SchedulerSlot[] slots,
        SchedulerItem<TItem> item,
        ElementDimensions container,
        DateTime date)
    {
        var results = new List<MappedItemRect>();
        var dayStart = _showNonWorkingHours ? TimeSpan.Zero : _timelineStart;
        var dayEnd = _showNonWorkingHours ? TimeSpan.FromHours(24) : _timelineEnd;
        var totalMinutes = (dayEnd - dayStart).TotalMinutes;
        var totalSubdivisions = (int)Math.Round(totalMinutes / _minutesPerSubdivision);
        var cellWidth = (float)(container.Width / totalSubdivisions);

        var startMinutes = (item.Start - date.Date).TotalMinutes - dayStart.TotalMinutes;
        var endMinutes = (item.End - date.Date).TotalMinutes - dayStart.TotalMinutes;
        startMinutes = Math.Max(0, startMinutes);
        endMinutes = Math.Min(totalMinutes, endMinutes);

        if (endMinutes <= startMinutes)
        {
            return results;
        }

        var startSubdivision = (int)Math.Floor(startMinutes / _minutesPerSubdivision);
        var endSubdivision = (int)Math.Ceiling(endMinutes / _minutesPerSubdivision);
        var left = startSubdivision * cellWidth;
        var width = Math.Max(0f, (endSubdivision - startSubdivision) * cellWidth);

        EnsureRowLayout(date);

        var rowsMap = _rowIndexByDate.TryGetValue(date.Date, out var map) ? map : null;
        var rowIndex = 0;

        if (rowsMap != null && rowsMap.TryGetValue(item.Id, out var idx))
        {
            rowIndex = idx;
        }

        var top = rowIndex * (HeightPerItem + _gap);
        var height = HeightPerItem;

        results.Add(new MappedItemRect
        {
            Rect = new RectangleF(left, top, width, height),
            ShowLeftAnchor = item.Start.Date == date.Date,
            ShowRightAnchor = item.End.Date == date.Date,
            ShowTopAnchor = false,
            ShowBottomAnchor = false
        });

        return results;
    }

    /// <summary>
    /// Ensures that the row layout for the specified date is initialized and up to date.
    /// </summary>
    /// <remarks>This method prepares internal mappings for item rows on the given date, allowing efficient
    /// access to row indices and counts. If no items exist for the specified date, the layout is initialized with zero
    /// rows.</remarks>
    /// <param name="date">The date for which to ensure the row layout. Only the date component is considered; the time portion is ignored.</param>
    private void EnsureRowLayout(DateTime date)
    {
        var d = date.Date;

        if (_rowIndexByDate.ContainsKey(d))
        {
            return;
        }

        if (!_getItemsByDay.TryGetValue(d, out var items) || items == null || items.Count == 0)
        {
            _rowIndexByDate[d] = new Dictionary<long, int>();
            _rowsCountByDate[d] = 0;
            return;
        }

        items.Sort((a, b) =>
        {
            var cmp = a.Start.CompareTo(b.Start);

            return cmp != 0 ? cmp : a.End.CompareTo(b.End);
        });

        var rowIndexMap = new Dictionary<long, int>();
        var rowLastEnd = new List<DateTime>();

        var dayStart = _showNonWorkingHours ? TimeSpan.Zero : _timelineStart;
        var dayEnd = _showNonWorkingHours ? TimeSpan.FromHours(24) : _timelineEnd;

        foreach (var it in items)
        {
            var windowStart = d + dayStart;
            var windowEnd = d + dayEnd;
            var clippedStart = it.Start < windowStart ? windowStart : it.Start;
            var clippedEnd = it.End > windowEnd ? windowEnd : it.End;

            if (clippedEnd <= clippedStart)
            {
                continue;
            }

            var placed = false;

            for (var r = 0; r < rowLastEnd.Count; r++)
            {
                if (rowLastEnd[r] <= clippedStart)
                {
                    rowIndexMap[it.Id] = r;
                    rowLastEnd[r] = clippedEnd;
                    placed = true;
                    break;
                }
            }

            if (!placed)
            {
                var newRow = rowLastEnd.Count;
                rowIndexMap[it.Id] = newRow;
                rowLastEnd.Add(clippedEnd);
            }
        }

        _rowIndexByDate[d] = rowIndexMap;
        _rowsCountByDate[d] = rowLastEnd.Count;
    }

    /// <summary>
    /// Invalidates any cached layout information associated with the specified date, forcing it to be recalculated when
    /// next accessed.
    /// </summary>
    /// <remarks>Call this method after making changes that affect the layout for a particular date to ensure
    /// that subsequent operations use up-to-date layout data.</remarks>
    /// <param name="date">The date for which layout information should be invalidated. Only the date component is considered; the time
    /// component is ignored.</param>
    public void InvalidateDateLayout(DateTime date)
    {
        var d = date.Date;
        _rowIndexByDate.Remove(d);
        _rowsCountByDate.Remove(d);
    }

    /// <summary>
    /// Invalidates all cached layout data, forcing subsequent layout calculations to recompute from scratch.
    /// </summary>
    /// <remarks>Call this method when the underlying data changes in a way that affects layout, to ensure
    /// that all layout-related caches are cleared and updated appropriately. This operation does not trigger any layout
    /// recalculation automatically; it only clears the internal caches.</remarks>
    public void InvalidateAllLayouts()
    {
        _rowIndexByDate.Clear();
        _rowsCountByDate.Clear();
    }
}
