using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a scheduler view that displays appointments and slots organized by month.
/// </summary>
/// <remarks>Use this view to present scheduling data in a monthly calendar format, where each slot corresponds to
/// a single day. This type is typically used in calendar or scheduling applications to provide users with an overview
/// of events for an entire month.</remarks>
public partial class SchedulerMonthView
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
    [Parameter]
    public IEnumerable<SchedulerSlot> Slots { get; set; } = [];

    /// <summary>
    /// Gets or sets the culture information used for formatting and parsing content within the component.
    /// </summary>
    /// <remarks>If not set, the current culture of the application is used by default. Setting this property
    /// allows customization of localization and formatting behaviors, such as date, number, and string
    /// representations.</remarks>
    [Parameter]
    public CultureInfo? Culture { get; set; } = CultureInfo.CurrentCulture;

    /// <summary>
    /// Gets or sets the date value for the component.
    /// </summary>
    [Parameter]
    public DateTime Date { get; set; } = DateTime.Today;

    /// <summary>
    /// Gets or sets the height, in pixels, of each slot in the component.
    /// </summary>
    [Parameter]
    public int SlotHeight { get; set; } = 80;

    /// <summary>
    /// Gets or sets a mapping of dates to the number of overflow events that occurred on each day.
    /// </summary>
    [Parameter]
    public Dictionary<DateTime, int> OverflowByDay { get; set; } = [];

    /// <summary>
    /// Gets or sets the callback that is invoked when a day is selected.
    /// </summary>
    /// <remarks>The callback receives the selected date as a <see cref="DateTime"/> parameter. Assign this
    /// property to handle day selection events, such as updating application state or triggering additional logic when
    /// the user selects a date.</remarks>
    [Parameter]
    public EventCallback<DateTime> OnDaySelected { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a scheduler slot is double-clicked.
    /// </summary>
    /// <remarks>Use this property to handle double-click events on individual slots within the scheduler
    /// component. The callback receives the associated <see cref="SchedulerSlot"/> as its argument, allowing you to
    /// access slot-specific information in your event handler.</remarks>
    [Parameter]
    public EventCallback<SchedulerSlot> OnSlotDoubleClick { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether cells are disabled and cannot be interacted with.
    /// </summary>
    [Parameter]
    public bool DisabledCells { get; set; }

    /// <summary>
    /// Gets the computed CSS style string used internally to render the component.
    /// </summary>
    private string? InternalStyle => new StyleBuilder(Style)
        .AddStyle("--slot-height", $"{SlotHeight}px", SlotHeight > 0)
        .Build();

    /// <summary>
    /// Invokes the day selection callback asynchronously when a scheduler slot is selected.
    /// </summary>
    /// <param name="slot">The scheduler slot representing the selected day. Must not be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnDaySelectedAsync(SchedulerSlot slot)
    {
        if (OnDaySelected.HasDelegate)
        {
            await OnDaySelected.InvokeAsync(slot.Start);
        }
    }

    /// <summary>
    /// Invokes the slot double-click event handler asynchronously for the specified scheduler slot.
    /// </summary>
    /// <param name="slot">The scheduler slot that was double-clicked. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnSlotDoubleClickAsync(SchedulerSlot? slot)
    {
        if (slot is null || slot.Disabled)
        {
            return;
        }

        if (OnSlotDoubleClick.HasDelegate)
        {
            await OnSlotDoubleClick.InvokeAsync(slot);
        }
    }
}
