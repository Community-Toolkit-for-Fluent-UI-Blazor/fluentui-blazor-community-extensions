using System.Drawing;
using System.Globalization;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides mapping logic to calculate the visual position and size of scheduler items within a week-based grid layout.
/// </summary>
/// <remarks>This class is intended for internal use in week-based scheduler views, where items are positioned
/// according to their start and end times, and distributed across days and time slots. It supports layouts with or
/// without non-working hours and accounts for overlapping items by dividing available space. Thread safety is not
/// guaranteed.</remarks>
/// <typeparam name="TItem">The type of the data associated with each scheduler item to be mapped.</typeparam>
internal class WeekGridPositionMapper<TItem> : IPositionMapper<TItem>
{
    /// <summary>
    /// Represents the number of columns in a week view, corresponding to the seven days of the week.
    /// </summary>
    private const int Columns = 7;

    /// <summary>
    /// Represents the number of rows in the week view, determined by the hours displayed per day.
    /// </summary>
    private readonly int _rows;

    /// <summary>
    /// Represents a mapping of scheduler items organized by their starting date and hour.
    /// </summary>
    private readonly Dictionary<(DateTime, int), List<SchedulerItem<TItem>>> _itemsByWeek;

    /// <summary>
    /// Represents the layout measurements used for positioning calculations.
    /// </summary>
    private readonly MeasureLayout _layout;

    /// <summary>
    /// Represents the maximum number of items that can be displayed in a single time slot.
    /// </summary>
    private readonly int _maxItemsCount;

    /// <summary>
    /// Represents the starting hour of the working day for layouts that exclude non-working hours.
    /// </summary>
    private readonly int _startHour;

    /// <summary>
    /// Represents a flag indicating whether non-working hours should be displayed in the layout.
    /// </summary>
    private readonly bool _showNonWorkingHours;

    /// <summary>
    /// Represents the height of each time slot in the week view grid.
    /// </summary>
    private readonly int _weekSlotHeight;

    /// <summary>
    /// Represents the margin applied to item positioning within the grid.
    /// </summary>
    private const int _margin = 2;

    /// <summary>
    /// Represents the number of subdivisions per hour for slot calculations.
    /// </summary>
    private readonly int _subdivisionCount;

    /// <summary>
    /// Represents the culture information used for date and time formatting.
    /// </summary>
    private readonly CultureInfo _culture;

    /// <summary>
    /// Initializes a new instance of the WeekGridPositionMapper class with the specified configuration for mapping
    /// scheduler items to week grid positions.
    /// </summary>
    /// <remarks>Use this constructor to customize the week grid's time range, slot sizing, and item placement
    /// behavior. The parameters allow for flexible configuration of both working and non-working hour visibility, as
    /// well as control over item density and layout.</remarks>
    /// <param name="culture">The culture information used for date and time formatting within the scheduler.</param>
    /// <param name="showNonWorkingHour">Indicates whether non-working hours should be included in the grid layout. Set to <see langword="true"/> to
    /// display all 24 hours; otherwise, only working hours are shown.</param>
    /// <param name="startHour">The starting hour of the working period, represented as a <see cref="TimeSpan"/>. Determines the first hour
    /// displayed when non-working hours are excluded.</param>
    /// <param name="endHour">The ending hour of the working period, represented as a <see cref="TimeSpan"/>. Determines the last hour
    /// displayed when non-working hours are excluded.</param>
    /// <param name="weekSlotHeight">The height, in pixels, of each time slot row in the week grid.</param>
    /// <param name="itemsByWeek">A dictionary mapping each week and day index to a list of scheduler items to be positioned in the grid.</param>
    /// <param name="layout">The layout measurement configuration used to calculate item positions within the grid.</param>
    /// <param name="maxItemsCount">The maximum number of scheduler items allowed per grid cell.</param>
    /// <param name="subdivisionCount">The number of subdivisions per hour for slot calculations. Defaults to 4, allowing for 15-minute intervals.</param>
    public WeekGridPositionMapper(
        CultureInfo culture,
        bool showNonWorkingHour,
        TimeSpan startHour,
        TimeSpan endHour,
        int weekSlotHeight,
        Dictionary<(DateTime, int), List<SchedulerItem<TItem>>> itemsByWeek,
        MeasureLayout layout,
        int maxItemsCount,
        int subdivisionCount = 4)
    {
        _culture = culture;
        _showNonWorkingHours = showNonWorkingHour;
        _startHour = startHour.Hours;
        _rows = showNonWorkingHour ? 24 : (int)(endHour - startHour).TotalHours;
        _itemsByWeek = itemsByWeek;
        _layout = layout;
        _maxItemsCount = maxItemsCount;
        _weekSlotHeight = weekSlotHeight;
        _subdivisionCount = subdivisionCount;
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

        var weekEnd = date.AddDays(Columns);
        var minutesPerSubdivision = 60.0 / _subdivisionCount;
        var subdivisionsPerHour = _subdivisionCount;
        var visibleStartHour = _showNonWorkingHours ? 0 : _startHour;
        var visibleEndHour = _showNonWorkingHours ? 24 : _startHour + _rows;
        var visibleHours = visibleEndHour - visibleStartHour;
        var visibleSubdivisions = (int)(visibleHours * subdivisionsPerHour);

        int ToSubdivisionIndex(TimeSpan t)
        {
            var totalMinutes = t.TotalMinutes;
            var startOffsetMinutes = visibleStartHour * 60.0;
            var minutesFromVisibleStart = totalMinutes - startOffsetMinutes;
            var index = (int)Math.Floor(minutesFromVisibleStart / minutesPerSubdivision);

            return index;
        }

        var daySpan = (item.End.Date - item.Start.Date).Days + 1;

        for (var offset = 0; offset < daySpan; offset++)
        {
            var currentDate = item.Start.Date.AddDays(offset);

            if (currentDate < date || currentDate >= weekEnd)
            {
                continue;
            }

            TimeSpan startTime, endTime;

            if (currentDate == item.Start.Date && currentDate == item.End.Date)
            {
                startTime = item.Start.TimeOfDay;
                endTime = item.End.TimeOfDay;
            }
            else if (currentDate == item.Start.Date)
            {
                startTime = item.Start.TimeOfDay;
                endTime = TimeSpan.FromHours(24);
            }
            else if (currentDate == item.End.Date)
            {
                startTime = TimeSpan.Zero;
                endTime = item.End.TimeOfDay;
            }
            else
            {
                startTime = TimeSpan.Zero;
                endTime = TimeSpan.FromHours(24);
            }

            var rowStartSub = ToSubdivisionIndex(startTime);
            var rowEndSub = ToSubdivisionIndex(endTime);

            if (rowStartSub < 0)
            {
                rowStartSub = 0;
            }

            if (rowEndSub > visibleSubdivisions)
            {
                rowEndSub = visibleSubdivisions;
            }

            if (rowStartSub >= visibleSubdivisions || rowEndSub <= 0)
            {
                continue;
            }

            var dayIndex = (currentDate - date).Days;
            var cellWidth = (container.Width - (Columns - 1)) / Columns;
            var left = dayIndex * cellWidth;
            var width = cellWidth;
            var top = rowStartSub * _weekSlotHeight;
            var heightSubCount = Math.Max(1, rowEndSub - rowStartSub);
            var height = Math.Max(_weekSlotHeight * heightSubCount, _weekSlotHeight / 2.0);

            var itemsInDay = _itemsByWeek
                .Where(kvp => kvp.Key.Item1 == currentDate)
                .SelectMany(kvp => kvp.Value)
                .ToList();

            var overlappingItems = itemsInDay
                .Where(i => i.Start < item.End && i.End > item.Start)
                .ToList();

            var index = overlappingItems.FindIndex(i => i.Id == item.Id);

            if (index < 0)
            {
                index = 0;
            }

            var count = Math.Max(1, Math.Min(overlappingItems.Count, _maxItemsCount));
            var availableWidth = width - _layout.Padding.Left - _layout.Padding.Right;
            var stride = (float)(availableWidth / count);
            var margin = _margin;
            var itemWidth = Math.Max(0f, stride - 2f * margin);
            var itemLeft = left + _layout.Padding.Left + index * stride + margin;
            var itemTop = top + _layout.Padding.Top;
            var itemHeight = (float)Math.Max(0, height - _layout.Padding.Top - _layout.Padding.Bottom);
            var cal = CultureInfo.CurrentCulture.Calendar;
            var startWeek = cal.GetWeekOfYear(item.Start.Date, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            var endWeek = cal.GetWeekOfYear(item.End.Date, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            var currentWeek = cal.GetWeekOfYear(currentDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

            results.Add(new MappedItemRect
            {
                Rect = new RectangleF(itemLeft, itemTop, itemWidth, itemHeight),
                ShowLeftAnchor = currentDate == item.Start.Date,
                ShowRightAnchor = currentDate == item.End.Date,
                ShowTopAnchor = currentWeek == startWeek,
                ShowBottomAnchor = currentWeek == endWeek
            });
        }

        return results;
    }
}
