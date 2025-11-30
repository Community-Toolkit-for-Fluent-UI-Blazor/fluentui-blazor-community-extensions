using System.Drawing;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a position mapping strategy for scheduler items that aligns their visual representation to vertical time
/// slots, supporting multi-column layouts and strict slot boundary snapping.
/// </summary>
/// <remarks>This mapper is designed for scenarios such as day or week views in scheduling applications, where
/// items are visually aligned to discrete time slots and columns (e.g., days of the week). It supports strict snapping
/// to slot boundaries, ensuring that item positions and durations correspond exactly to slot edges. The number of
/// columns and slot height can be configured to match the desired layout. Thread safety is not guaranteed; concurrent
/// access should be externally synchronized if required.</remarks>
/// <typeparam name="TItem">The type of the scheduler item to be mapped. Typically represents the domain model associated with each slot.</typeparam>
public class VerticalSlotAlignedPositionMapper<TItem> : IPositionMapper<TItem>
{
    /// <summary>
    /// Represents the height, in pixels, of each time slot.
    /// </summary>
    private readonly float _slotHeightPx;

    /// <summary>
    /// Represents the mapping of item IDs to their corresponding column index and column count within the layout.
    /// </summary>
    private Dictionary<long, (int columnIndex, int columnCount)> _layoutMap = [];

    /// <summary>
    /// Represents whether non-working hours should be included in the mapping.
    /// </summary>
    private readonly bool _showNonWorkingHours;

    /// <summary>
    /// Represents the start time of the working day.
    /// </summary>
    private readonly TimeSpan _workDayStart;

    /// <summary>
    /// Represents the end time of the working day.
    /// </summary>
    private readonly TimeSpan _workDayEnd;

    /// <summary>
    /// Represents the number of subdivisions per hour for slot calculations.
    /// </summary>
    private readonly int _subdivisionCount;

    /// <summary>
    /// Represents the margin applied to item positioning within slots.
    /// </summary>
    private readonly int _margin = 8;

    /// <summary>
    /// Initializes a new instance of the VerticalSlotAlignedPositionMapper class with the specified working hours, slot
    /// height, subdivisions, and column count.
    /// </summary>
    /// <remarks>If an argument is less than its minimum allowed value, it will be set to the minimum
    /// internally. This ensures valid configuration for slot height and column count.</remarks>
    /// <param name="showNonWorkingHours">true to include non-working hours in the mapping; otherwise, false.</param>
    /// <param name="workDayStart">The start time of the working day. Must be less than <paramref name="workDayEnd"/>.</param>
    /// <param name="workDayEnd">The end time of the working day. Must be greater than <paramref name="workDayStart"/>.</param>
    /// <param name="slotHeightPx">The height, in pixels, of each time slot. Must be greater than 0. Defaults to 60.</param>
    /// <param name="subdivisionCount">The number of subdivisions per slot. Must be greater than 0. Determines the granularity of time mapping within
    /// each slot.</param>
    public VerticalSlotAlignedPositionMapper(
        bool showNonWorkingHours,
        TimeSpan workDayStart,
        TimeSpan workDayEnd,
        float slotHeightPx = 60,
        int subdivisionCount = 4)
    {
        _slotHeightPx = Math.Max(1, slotHeightPx);
        _showNonWorkingHours = showNonWorkingHours;
        _workDayStart = workDayStart;
        _workDayEnd = workDayEnd;
        _subdivisionCount = subdivisionCount;
    }

    /// <summary>
    /// Sets the current slot layout using the specified collection of layout results.
    /// </summary>
    /// <remarks>This method replaces any existing layout mapping with the provided configuration. The layout
    /// is used to determine the column placement and span for each item.</remarks>
    /// <param name="layout">A list of <see cref="SlotLayoutResult{TItem}"/> objects representing the layout configuration for each item.
    /// Cannot be null.</param>
    public void SetLayout(List<SlotLayoutResult<TItem>> layout)
    {
        _layoutMap = layout.ToDictionary(r => r.Item.Id, r => (r.ColumnIndex, r.ColumnCount));
    }

    /// <inheritdoc/>
    public IEnumerable<MappedItemRect> Map(
        SchedulerSlot[] slots,
        SchedulerItem<TItem> item,
        ElementDimensions container,
        DateTime date)
    {
        if (slots == null || slots.Length == 0)
        {
            return [];
        }

        if (date.Date < item.Start.Date || date.Date > item.End.Date)
        {
            return [];
        }

        // Filtrage des slots selon horaires de bureau
        var filteredSlots = _showNonWorkingHours
            ? slots
            : [.. slots.Where(s =>
        {
            var slotStart = s.Start.TimeOfDay;
            var slotEnd = s.End.TimeOfDay;

            // Clip aux bornes de la journée de travail
            var effectiveStart = TimeSpan.FromTicks(Math.Max(slotStart.Ticks, _workDayStart.Ticks));
            var effectiveEnd   = TimeSpan.FromTicks(Math.Min(slotEnd.Ticks, _workDayEnd.Ticks));

            return effectiveEnd > effectiveStart;
        })];

        if (filteredSlots.Length == 0)
        {
            return [];
        }

        TimeSpan startTime, endTime;

        var dayStart = _showNonWorkingHours ? TimeSpan.Zero : _workDayStart;
        var dayEnd = _showNonWorkingHours ? TimeSpan.FromHours(24) : _workDayEnd;

        if (date.Date == item.Start.Date && date.Date == item.End.Date)
        {
            startTime = item.Start.TimeOfDay;
            endTime = item.End.TimeOfDay;
        }
        else if (date.Date == item.Start.Date)
        {
            startTime = item.Start.TimeOfDay;
            endTime = dayEnd;
        }
        else if (date.Date == item.End.Date)
        {
            startTime = dayStart;
            endTime = item.End.TimeOfDay;
        }
        else
        {
            startTime = dayStart;
            endTime = dayEnd;
        }

        if (!_showNonWorkingHours)
        {
            startTime = TimeSpan.FromTicks(Math.Max(startTime.Ticks, _workDayStart.Ticks));
            endTime = TimeSpan.FromTicks(Math.Min(endTime.Ticks, _workDayEnd.Ticks));
        }

        if (endTime <= startTime)
        {
            return [];
        }

        var pixelsPerHour = _slotHeightPx * _subdivisionCount;
        var baseOffset = dayStart;
        var relativeStart = startTime - baseOffset;
        var relativeEnd = endTime - baseOffset;
        var top = (float)relativeStart.TotalHours * pixelsPerHour;
        var height = (float)(relativeEnd - relativeStart).TotalHours * pixelsPerHour;

        var (colIndex, colCount) = _layoutMap.TryGetValue(item.Id, out var val) ? val : (0, 1);
        var columnWidth = (float)container.Width / colCount;
        var left = colIndex * columnWidth + _margin;
        var width = columnWidth - 2 * _margin;

        return
            [
                new()
                {
                    Rect = new RectangleF(left, top, width, height),
                    ShowBottomAnchor = true,
                    ShowTopAnchor = true
                }
            ];
    }
}
