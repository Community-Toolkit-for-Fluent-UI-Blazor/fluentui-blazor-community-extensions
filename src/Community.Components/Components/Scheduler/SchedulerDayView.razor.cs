using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a day-based view for the scheduler component, displaying time slots for a single day and enabling
/// customization of slot rendering.
/// </summary>
/// <remarks>Use this view to present and interact with scheduler data organized by individual days. The view
/// provides hourly slots for the selected date and supports custom templates for slot appearance. This class implements
/// <see cref="ISchedulerSlotBuilder"/> to integrate with scheduler navigation and slot generation.</remarks>
public partial class SchedulerDayView
{
    /// <summary>
    /// Gets or sets the template used to render each slot in the scheduler component.
    /// </summary>
    /// <remarks>The template receives a <see cref="SchedulerSlot"/> instance representing the slot to be
    /// rendered. Use this property to customize the appearance and content of individual slots within the
    /// scheduler.</remarks>
    [Parameter]
    public RenderFragment<SchedulerSlot>? SlotTemplate { get; set; }

    /// <summary>
    /// Gets or sets the collection of scheduler slots to be displayed or managed by the component.
    /// </summary>
    /// <remarks>Each slot in the collection represents a time interval or resource allocation within the
    /// scheduler. The property must be set to a non-null collection; an empty collection indicates that no slots are
    /// available or scheduled.</remarks>
    [Parameter]
    public IEnumerable<SchedulerSlot> Slots { get; set; } = [];

    /// <summary>
    /// Gets or sets the height, in pixels, of each slot in the component.
    /// </summary>
    [Parameter]
    public int SlotHeight { get; set; } = 60;

    /// <summary>
    /// Gets or sets the start time of the workday.
    /// </summary>
    /// <remarks>The default value is 8:00 AM. Adjust this property to define when the workday begins for
    /// scheduling or time-based calculations.</remarks>
    [Parameter]
    public TimeSpan WorkDayStart { get; set; } = TimeSpan.FromHours(8);

    /// <summary>
    /// Gets or sets the time of day when the workday ends.
    /// </summary>
    [Parameter]
    public TimeSpan WorkDayEnd { get; set; } = TimeSpan.FromHours(17);

    /// <summary>
    /// Gets or sets a value indicating whether hour labels are displayed on the time picker.
    /// </summary>
    [Parameter]
    public bool ShowHourLabels { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether non-working hours are displayed in the schedule view.
    /// </summary>
    /// <remarks>Set this property to <see langword="true"/> to show time slots outside of standard working
    /// hours. When <see langword="false"/>, only working hours are visible. This can be useful for focusing the view on
    /// typical business hours or for displaying a full-day schedule.</remarks>
    [Parameter]
    public bool ShowNonWorkingHours { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether hour content is displayed in the component.
    /// </summary>
    [Parameter]
    public bool ShowHourContent { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether weekends are shown.
    /// </summary>
    /// <remarks>When set to <see langword="true"/>, the week ends days are rendered in gray. Set to
    /// <see langword="false"/> to show them in white.</remarks>
    [Parameter]
    public bool ShowWeekEnd { get; set; } = true;

    /// <summary>
    /// Gets or sets the current date value for the view.
    /// </summary>
    [Parameter]
    public DateTime CurrentDate { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a scheduler slot is double-clicked.
    /// </summary>
    /// <remarks>Use this property to handle double-click events on individual slots within the scheduler
    /// component. The callback receives the associated <see cref="SchedulerSlot"/> as its argument, allowing you to
    /// access slot-specific information in your event handler.</remarks>
    [Parameter]
    public EventCallback<SchedulerSlot> OnSlotDoubleClick { get; set; }

    /// <summary>
    /// Gets or sets the number of subdivisions per hour in the scheduler view.
    /// </summary>
    [Parameter]
    public int SubdivisionCount { get; set; } = 1;

    /// <summary>
    /// Gets the starting hour for the displayed time range, depending on whether non-working hours are shown.
    /// </summary>
    /// <remarks>If non-working hours are included, the starting hour is set to 0. Otherwise, it reflects the
    /// beginning of the workday as specified by the <see cref="WorkDayStart"/> property.</remarks>
    private int StartHour => ShowNonWorkingHours ? 0 : WorkDayStart.Hours;

    /// <summary>
    /// Gets the hour at which the displayed time range ends, based on whether non-working hours are shown.
    /// </summary>
    /// <remarks>If non-working hours are displayed, the end hour is set to 24. Otherwise, it reflects the end
    /// of the working day as specified by the <see cref="WorkDayEnd"/> property.</remarks>
    private int EndHour => ShowNonWorkingHours ? 24 : WorkDayEnd.Hours;

    /// <summary>
    /// Gets or sets a value indicating whether cells are disabled and cannot be interacted with.
    /// </summary>
    [Parameter]
    public bool DisabledCells { get; set; }

    /// <summary>
    /// Gets the computed style string used internally for rendering slot elements.
    /// </summary>
    private string? InternalStyle => new StyleBuilder(Style)
        .AddStyle("--slot-height", $"{SlotHeight}px", SlotHeight > 0)
        .AddStyle("--scheduler-hour-info-width", "250px", ShowHourLabels && ShowHourContent)
        .AddStyle("--scheduler-hour-info-width", "90px", ShowHourLabels && !ShowHourContent)
        .AddStyle("--scheduler-hour-info-width", "130px", !ShowHourLabels && ShowHourContent)
        .AddStyle("--scheduler-hour-info-width", "0px", !ShowHourContent && !ShowHourLabels)
        .Build();

    /// <summary>
    /// Determines whether the specified scheduler slot starts outside of defined working hours.
    /// </summary>
    /// <remarks>This method uses the start time of the slot to assess whether it falls outside the configured
    /// working hours. It does not consider the slot's end time.</remarks>
    /// <param name="slot">The scheduler slot to evaluate. The slot's start time is compared against the working hours.</param>
    /// <returns>true if the slot starts before the beginning of working hours or at or after the end of working hours;
    /// otherwise, false.</returns>
    private bool IsOutsideWorkingHours(SchedulerSlot slot)
    {
        var start = slot.Start.TimeOfDay;

        return start < WorkDayStart || start >= WorkDayEnd;
    }

    /// <summary>
    /// Determines whether the specified scheduler slot falls on a weekend day.
    /// </summary>
    /// <param name="slot">The scheduler slot to evaluate. The slot's start date is used to determine the day of the week.</param>
    /// <returns>true if the slot's start date is a Saturday or Sunday; otherwise, false.</returns>
    private static bool IsWeekend(SchedulerSlot slot)
    {
        var day = slot.Start.DayOfWeek;

        return day == DayOfWeek.Saturday || day == DayOfWeek.Sunday;
    }

    /// <summary>
    /// Invokes the slot double-click event handler asynchronously for the specified scheduler slot.
    /// </summary>
    /// <param name="slot">The scheduler slot that was double-clicked. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnSlotDoubleClickAsync(SchedulerSlot slot)
    {
        if (OnSlotDoubleClick.HasDelegate)
        {
            await OnSlotDoubleClick.InvokeAsync(slot);
        }
    }
}
