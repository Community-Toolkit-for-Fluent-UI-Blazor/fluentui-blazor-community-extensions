using System.Globalization;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a calendar component for scheduling that displays a month view and supports date selection, slot
/// indication, and custom date disabling logic.
/// </summary>
/// <remarks>Use the <see cref="Month"/> property to specify which month is displayed. The <see
/// cref="HasSlotFunc"/> delegate can be provided to indicate which dates have available slots, and <see
/// cref="DisabledDateFunc"/> can be used to disable specific dates from selection. The <see cref="OnDateClick"/> event
/// is triggered when a date is selected or navigated to via keyboard interaction. The <see cref="Culture"/> property
/// determines the locale used for date formatting and display.</remarks>
public partial class SchedulerCalendar
{
    /// <summary>
    /// Represents a mapping of dates to their opened (true) or closed (false) status within the calendar.
    /// </summary>
    private readonly Dictionary<DateTime, bool> _openedDay = [];

    /// <summary>
    /// Gets or sets the month value used for the component or operation.
    /// </summary>
    /// <remarks>The value typically represents the first day of the desired month. Changing this property may
    /// trigger updates or re-rendering in components that depend on the selected month.</remarks>
    [Parameter]
    public DateTime Month { get; set; }

    /// <summary>
    /// Gets or sets the culture information used for formatting and parsing operations within the component.
    /// </summary>
    /// <remarks>If not set, the property defaults to the current culture of the executing environment.
    /// Changing this property affects how dates, numbers, and other culture-sensitive data are displayed and
    /// interpreted.</remarks>
    [Parameter]
    public CultureInfo Culture { get; set; } = CultureInfo.CurrentCulture;

    /// <summary>
    /// Gets or sets the delegate used to determine whether a slot is available for a given date and time.
    /// </summary>
    /// <remarks>Assign a function that accepts a <see cref="DateTime"/> and returns <see langword="true"/> if
    /// a slot is available for that date and time; otherwise, <see langword="false"/>. If <see langword="null"/>, no
    /// slot availability check will be performed.</remarks>
    [Parameter]
    public Func<DateTime, bool>? HasSlotFunc { get; set; }

    /// <summary>
    /// Gets or sets the delegate used to determine whether a specific date should be disabled in the date picker.
    /// </summary>
    /// <remarks>The delegate receives a <see cref="DateTime"/> value and should return <see langword="true"/>
    /// to disable the date, or <see langword="false"/> to enable it. If <see langword="null"/>, all dates are enabled
    /// by default.</remarks>
    [Parameter]
    public Func<DateTime, bool>? DisabledDateFunc { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a date is selected.
    /// </summary>
    /// <remarks>The callback receives the selected date as a <see cref="DateTime"/> parameter. Assign this
    /// property to handle date selection events in parent components.</remarks>
    [Parameter]
    public EventCallback<DateTime> OnDateSelected { get; set; }

    /// <summary>
    /// Gets or sets the template used to render the content for each day in the calendar.
    /// </summary>
    /// <remarks>The template receives the date to be rendered as a parameter. Use this property to customize
    /// the appearance or behavior of individual days within the calendar component.</remarks>
    [Parameter]
    public RenderFragment<DateTime> DayTemplate { get; set; } = default!;

    /// <summary>
    /// Gets or sets the currently selected date value.
    /// </summary>
    [Parameter]
    public DateTime? SelectedDate { get; set; }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        InitializeOpenPopoverArray();
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        if (parameters.HasValueChanged(nameof(Month), Month))
        {
            InitializeOpenPopoverArray();
        }

        return base.SetParametersAsync(parameters);
    }

    /// <summary>
    /// Initializes the internal array that tracks the open state of popovers for each day in the displayed calendar
    /// month.
    /// </summary>
    /// <remarks>Resets the open state for all days in the calendar, ensuring that no popovers are marked as
    /// open at initialization. This method should be called before rendering or updating the calendar to prevent stale
    /// state from previous interactions.</remarks>
    private void InitializeOpenPopoverArray()
    {
        for (var week = 0; week < 6; week++)
        {
            foreach (var day in CalendarExtended.GetDaysOfWeek(week, Month, Culture))
            {
                _openedDay[day] = false;
            }
        }
    }

    /// <summary>
    /// Determines whether the specified date falls outside the current month.
    /// </summary>
    /// <param name="day">The date to evaluate against the current month.</param>
    /// <returns>true if the month of the specified date does not match the current month; otherwise, false.</returns>
    private bool IsInactive(DateTime day)
    {
        return day.Month != Month.Month;
    }

    /// <summary>
    /// Determines whether the specified date is considered disabled based on the configured criteria.
    /// </summary>
    /// <param name="day">The date to evaluate for disabled status.</param>
    /// <returns>true if the specified date is disabled; otherwise, false.</returns>
    private bool IsDisabled(DateTime day)
    {
        return DisabledDateFunc?.Invoke(day) ?? false;
    }

    /// <summary>
    /// Determines whether any items are available for the specified day.
    /// </summary>
    /// <param name="day">The date to check for available items.</param>
    /// <returns>true if one or more items are available for the specified day; otherwise, false.</returns>
    private bool HasItems(DateTime day)
    {
        return HasSlotFunc?.Invoke(day) ?? false;
    }

    /// <summary>
    /// Generates an accessible label for the specified date using the current culture's long date format.
    /// </summary>
    /// <param name="day">The date for which to generate the ARIA label.</param>
    /// <returns>A string containing the long date representation of <paramref name="day"/> formatted according to the current
    /// culture.</returns>
    private string GetAriaLabel(DateTime day)
    {
        return day.ToString("D", Culture);
    }

    /// <summary>
    /// Handles the selection of a date by updating the selected date and invoking the selection callback if applicable.
    /// </summary>
    /// <remarks>If the specified date is disabled or inactive, the method does not update the selection or
    /// invoke the callback.</remarks>
    /// <param name="date">The date that was selected. Must not be disabled or inactive.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnSelectedCellAsync(DateTime date)
    {
        if (IsDisabled(date) || IsInactive(date))
        {
            return;
        }

        if (HasSlotFunc?.Invoke(date) ?? false)
        {
            _openedDay[date] = true;
            return;
        }
    }

    /// <summary>
    /// Handles the selection of a date by updating the selected date and triggering the associated event
    /// asynchronously.
    /// </summary>
    /// <param name="date">The date that was selected. The method ignores the selection if the date is disabled or inactive.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnDateSelectedAsync(DateTime date)
    {
        if (IsDisabled(date) || IsInactive(date))
        {
            return;
        }

        _openedDay[date] = false;
        SelectedDate = date;
        await OnDateSelected.InvokeAsync(date);
    }
}
