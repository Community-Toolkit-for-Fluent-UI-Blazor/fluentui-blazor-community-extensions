using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a year view for the scheduler component, providing configuration for slot templates and the collection of
/// slots to display.
/// </summary>
/// <remarks>Use this class to configure how scheduler slots are rendered and managed in a yearly layout. The view
/// allows customization of slot appearance through templates and supports binding to a collection of slots representing
/// scheduled items or time periods.</remarks>
public partial class SchedulerYearView<TItem>
{
    /// <summary>
    /// Gets or sets the culture information used for formatting and localization within the component.
    /// </summary>
    /// <remarks>If not set, the property defaults to the current culture of the application. Changing this
    /// property affects how dates, numbers, and other culture-sensitive data are displayed.</remarks>
    [Parameter]
    public CultureInfo Culture { get; set; } = CultureInfo.CurrentCulture;

    /// <summary>
    /// Gets or sets the function used to determine whether a specific date should be disabled in the component.
    /// </summary>
    /// <remarks>The function receives a <see cref="DateTime"/> value and should return <see langword="true"/>
    /// to disable the date, or <see langword="false"/> to enable it. This can be used to restrict selectable dates
    /// based on custom logic, such as disabling weekends or holidays.</remarks>
    [Parameter]
    public Func<DateTime, bool>? DisabledDateFunc { get; set; }

    /// <summary>
    /// Gets or sets the collection of scheduler items to be displayed and managed by the component.
    /// </summary>
    /// <remarks>Each item in the collection represents a scheduled entry of type <typeparamref
    /// name="TItem"/>. Modifying this collection will update the items shown in the scheduler UI. The property must be
    /// set to a non-null list; an empty list indicates that no items are scheduled.</remarks>
    [Parameter]
    public List<SchedulerItem<TItem>> Items { get; set; } = [];

    /// <summary>
    /// Gets or sets the callback that is invoked when a date is clicked.
    /// </summary>
    /// <remarks>The callback receives the clicked date as a <see cref="DateTime"/> parameter. Assign this
    /// property to handle user interactions with individual dates, such as selecting or highlighting a date in the
    /// UI.</remarks>
    [Parameter]
    public EventCallback<DateTime> OnDateClick { get; set; }

    /// <summary>
    /// Gets or sets the template used to render each slot in the scheduler component.
    /// </summary>
    /// <remarks>The template receives a <see cref="SchedulerSlot"/> instance representing the slot to be
    /// rendered. Use this property to customize the appearance and content of individual slots within the
    /// scheduler.</remarks>
    [Parameter]
    public RenderFragment<SchedulerSlot>? SlotTemplate { get; set; }

    /// <summary>
    /// Gets or sets the current date value used by the component.
    /// </summary>
    [Parameter]
    public DateTime CurrentDate { get; set; } = DateTime.Now;

    /// <summary>
    /// Returns a DateTime representing the first day of the specified month in the current year.
    /// </summary>
    /// <param name="index">The zero-based index of the month to retrieve, where 0 corresponds to January and 11 to December. Must be in the
    /// range 0 to 11.</param>
    /// <returns>A DateTime value set to the first day of the specified month in the current year.</returns>
    private DateTime GetMonthDate(int index)
    {
        return new DateTime(CurrentDate.Year, index + 1, 1);
    }

    /// <summary>
    /// Retrieves all scheduler slots that include the specified date.
    /// </summary>
    /// <param name="date">The date to search for within scheduler slots. If <paramref name="date"/> is <see langword="null"/>, an empty
    /// collection is returned.</param>
    /// <returns>An enumerable collection of <see cref="SchedulerSlot"/> objects that span the specified date. Returns an empty
    /// collection if no slots match or if <paramref name="date"/> is <see langword="null"/>.</returns>
    private IEnumerable<SchedulerSlot> GetSlotsForDate(DateTime? date)
    {
        if (!date.HasValue)
        {
            return [];
        }

        return Items.Where(x => x.Start.Date <= date.Value.Date && x.End.Date >= date.Value.Date)
                    .Select(x => new SchedulerSlot(x.Title, x.Start, x.End));
    }

    /// <summary>
    /// Determines whether there is an available slot on the specified date, considering item recurrences and
    /// exceptions.
    /// </summary>
    /// <remarks>This method evaluates both single and recurring items, excluding any dates listed as
    /// exceptions. It checks for slot availability by comparing the specified date against item ranges and recurrence
    /// patterns.</remarks>
    /// <param name="date">The date to check for slot availability. Only the date component is considered; the time component is ignored.</param>
    /// <returns>true if a slot is available on the specified date; otherwise, false.</returns>
    private bool HasSlot(DateTime date)
    {
        foreach (var item in Items)
        {
            if (item.Exceptions.Any(ex => ex.Date == date.Date))
            {
                continue;
            }

            if (item.Recurrence == null)
            {
                if (date.Date >= item.Start.Date && date.Date <= item.End.Date)
                {
                    return true;
                }
            }
            else
            {
                var from = date.Date.AddDays(-1);
                var to = date.Date.AddDays(1);

                var occurrences = RecurrenceEngine.GetOccurrences(
                    item.Recurrence,
                    item.Start.Date,
                    from,
                    to
                );

                foreach (var occurrence in occurrences)
                {
                    var duration = (item.End.Date - item.Start.Date).Days;
                    var occurrenceEnd = occurrence.AddDays(duration);

                    if (date.Date >= occurrence.Date && date.Date <= occurrenceEnd.Date)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}
