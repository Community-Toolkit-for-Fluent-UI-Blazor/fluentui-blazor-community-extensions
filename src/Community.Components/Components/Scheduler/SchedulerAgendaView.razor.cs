using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an agenda-style view for a scheduler component, displaying scheduled items grouped by day over a
/// specified date range.
/// </summary>
/// <remarks>Use this view to present scheduler items in a daily agenda format, allowing customization of the
/// number of days displayed, cell height, and whether to hide days with no scheduled items. The view is suitable for
/// scenarios where users need to see a chronological list of events or appointments over multiple days.</remarks>
/// <typeparam name="TItem">The type of the data item associated with each scheduled entry in the agenda view.</typeparam>
public partial class SchedulerAgendaView<TItem>
{
    /// <summary>
    /// Represents the text used for midnight time display.
    /// </summary>
    private const string MidnightTimeText = "00:00";

    /// <summary>
    /// Represents the text used for end-of-day time display.
    /// </summary>
    private const string EndOfDayTimeText = "23:59";

    /// <summary>
    /// Gets or sets the number of days to include in the operation.
    /// </summary>
    /// <remarks>The default value is 7. Set this property to specify the time span, in days, that the
    /// component should consider when performing its logic.</remarks>
    [Parameter]
    public int NumberOfDays { get; set; } = 7;

    /// <summary>
    /// Gets or sets a value indicating whether days with no events or data should be hidden from display.
    /// </summary>
    [Parameter]
    public bool HideEmptyDays { get; set; } = false;

    /// <summary>
    /// Gets or sets the height, in pixels, of each cell in the component.
    /// </summary>
    [Parameter]
    public int CellHeight { get; set; } = 40;

    /// <summary>
    /// Gets or sets the collection of scheduler items to be displayed and managed by the component.
    /// </summary>
    /// <remarks>Each item in the collection represents a scheduled entry of type <typeparamref
    /// name="TItem"/>. Modifying this collection will update the items shown in the scheduler UI.</remarks>
    [Parameter]
    public List<SchedulerItem<TItem>> Items { get; set; } = [];

    /// <summary>
    /// Gets or sets the current date value used by the component.
    /// </summary>
    [Parameter]
    public DateTime CurrentDate { get; set; } = DateTime.Today;

    /// <summary>
    /// Gets or sets the culture information used for formatting and parsing content in the component.
    /// </summary>
    /// <remarks>If not set, the property defaults to the current culture of the application. Changing this
    /// property affects how dates, numbers, and other culture-sensitive data are displayed and interpreted within the
    /// component.</remarks>
    [Parameter]
    public CultureInfo Culture { get; set; } = CultureInfo.CurrentCulture;

    /// <summary>
    /// Retrieves a collection of agenda days and their associated scheduler items within the current date range.
    /// </summary>
    /// <remarks>The returned dictionary is sorted by date in ascending order. The date range starts from <see
    /// langword="CurrentDate"/> and includes the next <see langword="NumberOfDays"/> days. This method does not modify
    /// the underlying items collection.</remarks>
    /// <returns>A sorted dictionary mapping each agenda day to a list of scheduler items scheduled for that day. If <see
    /// langword="HideEmptyDays"/> is <see langword="true"/>, only days with items are included; otherwise, all days in
    /// the range are present, with empty lists for days without items.</returns>
    private SortedDictionary<DateTime, List<SchedulerItem<TItem>>> GetAgendaDays()
    {
        var startDate = CurrentDate.Date;
        var endDate = startDate.AddDays(NumberOfDays);

        var grouped = new Dictionary<DateTime, List<SchedulerItem<TItem>>>();

        foreach (var item in Items)
        {
            if (item.Start.Date < endDate && item.End.Date >= startDate)
            {
                for (var d = item.Start.Date; d <= item.End.Date; d = d.AddDays(1))
                {
                    if (d < startDate || d >= endDate)
                    {
                        continue;
                    }

                    if (!grouped.TryGetValue(d, out var value))
                    {
                        value = [];
                        grouped[d] = value;
                    }

                    value.Add(item);
                }
            }
        }

        if (!HideEmptyDays)
        {
            for (var d = startDate; d < endDate; d = d.AddDays(1))
            {
                if (!grouped.ContainsKey(d))
                {
                    grouped[d] = [];
                }
            }
        }

        foreach (var kvp in grouped.ToList())
        {
            var day = kvp.Key;
            var list = kvp.Value;

            list.Sort((a, b) =>
            {
                var (sa, ea) = GetDailySegment(a, day);
                var (sb, eb) = GetDailySegment(b, day);

                var cmpStart = sa.CompareTo(sb);
                if (cmpStart != 0)
                {
                    return cmpStart;
                }

                var cmpEnd = ea.CompareTo(eb);

                if (cmpEnd != 0)
                {
                    return cmpEnd;
                }

                return a.Id.CompareTo(b.Id);
            });
        }

        return new SortedDictionary<DateTime, List<SchedulerItem<TItem>>>(grouped);
    }

    /// <summary>
    /// Advances the current date by the specified number of days and updates the component state.
    /// </summary>
    /// <remarks>Call this method to increment the date and trigger a UI refresh. This method is intended for
    /// internal use within the component and is not thread-safe.</remarks>
    internal void MoveNext()
    {
        CurrentDate = CurrentDate.AddDays(NumberOfDays);
        StateHasChanged();
    }

    /// <summary>
    /// Moves the current date backward by the configured number of days.
    /// </summary>
    /// <remarks>This method updates the current date to an earlier value and triggers a state change
    /// notification. Use this method to navigate to previous dates in scenarios where date-based navigation is
    /// supported.</remarks>
    internal void MovePrevious()
    {
        CurrentDate = CurrentDate.AddDays(-NumberOfDays);
        StateHasChanged();
    }

    /// <summary>
    /// Returns the formatted start and end times for a scheduled item on a specified day, based on its multi-day or
    /// single-day span.
    /// </summary>
    /// <remarks>The returned times reflect the segment boundaries for the given day, accounting for
    /// single-day, first-day, last-day, and intermediate-day scenarios in multi-day events.</remarks>
    /// <param name="item">The scheduled item for which to retrieve the segment times. The item's start and end dates determine how times
    /// are formatted for the given day.</param>
    /// <param name="day">The day for which to obtain the segment start and end times. Only the date component is considered.</param>
    /// <returns>A tuple containing the start and end times as strings in "HH:mm" format for the specified day. If the day does
    /// not correspond to any segment, both values may be empty strings.</returns>
    private static (string StartText, string EndText) GetDailySegmentTexts(CultureInfo culture, SchedulerItem<TItem> item, DateTime day)
    {
        var dayDate = day.Date;
        var startDate = item.Start.Date;
        var endDate = item.End.Date;

        static string fmt(TimeSpan t, CultureInfo culture) => t.ToString(@"hh\:mm", culture);

        if (startDate == endDate &&
            dayDate == startDate)
        {
            return (fmt(item.Start.TimeOfDay, culture), fmt(item.End.TimeOfDay, culture));
        }

        if (dayDate == startDate &&
            dayDate != endDate)
        {
            return (fmt(item.Start.TimeOfDay, culture), EndOfDayTimeText);
        }

        if (dayDate != startDate &&
            dayDate != endDate)
        {
            return (MidnightTimeText, EndOfDayTimeText);
        }

        if (dayDate == endDate && dayDate != startDate)
        {
            return (MidnightTimeText, fmt(item.End.TimeOfDay, culture));
        }

        return ("", "");
    }

    private static (TimeSpan Start, TimeSpan End) GetDailySegment(
        SchedulerItem<TItem> item,
        DateTime day,
        bool inclusiveEnd = true)
    {
        var dayDate = day.Date;
        var startDate = item.Start.Date;
        var endDate = item.End.Date;

        var dayStart = TimeSpan.Zero;
        var dayEnd = inclusiveEnd ? TimeSpan.FromHours(24) : TimeSpan.FromHours(23) + TimeSpan.FromMinutes(59);

        if (startDate == endDate && dayDate == startDate)
        {
            return (item.Start.TimeOfDay, item.End.TimeOfDay);
        }

        if (dayDate == startDate && dayDate != endDate)
        {
            return (item.Start.TimeOfDay, dayEnd);
        }

        if (dayDate != startDate && dayDate != endDate)
        {
            return (dayStart, dayEnd);
        }

        if (dayDate == endDate && dayDate != startDate)
        {
            return (dayStart, item.End.TimeOfDay);
        }

        return (TimeSpan.Zero, TimeSpan.Zero);
    }
}
