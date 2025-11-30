using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a timeline view component for displaying scheduler slots over a specified time range.
/// </summary>
/// <remarks>Use <see cref="SchedulerTimelineView"/> to visualize and interact with scheduled items across a
/// timeline, with customizable slot templates, time range, and subdivisions. The component supports user interaction
/// such as double-clicking slots and can be configured for various scheduling scenarios. All parameters must be set
/// appropriately to ensure correct rendering and behavior.</remarks>
public partial class SchedulerTimelineView : FluentComponentBase
{
    /// <summary>
    /// Gets or sets the template used to render each slot in the timeline.
    /// </summary>
    [Parameter]
    public RenderFragment<SchedulerSlot>? SlotTemplate { get; set; }

    /// <summary>
    /// Gets or sets the collection of scheduler slots to be displayed in the timeline.
    /// </summary>
    [Parameter]
    public List<SchedulerSlot> Slots { get; set; } = [];

    /// <summary>
    /// Gets or sets the start time of the timeline (default 0:00).
    /// </summary>
    [Parameter]
    public TimeSpan TimelineStart { get; set; } = TimeSpan.Zero;

    /// <summary>
    /// Gets or sets the end time of the timeline (default 24:00).
    /// </summary>
    [Parameter]
    public TimeSpan TimelineEnd { get; set; } = TimeSpan.FromHours(24);

    /// <summary>
    /// Gets or sets a value indicating whether non-working hours are displayed in the component.
    /// </summary>
    /// <remarks>Set this property to <see langword="true"/> to show time periods outside of standard working
    /// hours. This can be useful for scenarios where scheduling or viewing events during evenings, weekends, or other
    /// non-standard times is required.</remarks>
    [Parameter]
    public bool ShowNonWorkingHours { get; set; } = true;

    /// <summary>
    /// Gets or sets the number of subdivisions per hour in the timeline.
    /// </summary>
    [Parameter]
    public int SubdivisionCount { get; set; } = 1;

    /// <summary>
    /// Gets or sets the current date for which the timeline is displayed.
    /// </summary>
    [Parameter]
    public DateTime CurrentDate { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when a slot is double-clicked.
    /// </summary>
    [Parameter]
    public EventCallback<SchedulerSlot> OnSlotDoubleClick { get; set; }

    /// <summary>
    /// Gets or sets the width, in pixels, of each cell in the grid.
    /// </summary>
    [Parameter]
    public int CellWidth { get; set; } = 80;

    /// <summary>
    /// Gets or sets the height, in pixels, of each cell in the component.
    /// </summary>
    [Parameter]
    public int CellHeight { get; set; } = 105;

    /// <summary>
    /// Gets or sets a value indicating whether cells are disabled and cannot be interacted with.
    /// </summary>
    [Parameter]
    public bool DisabledCells { get; set; }

    /// <summary>
    /// Gets the total number of timeline subdivisions based on the current view settings.
    /// </summary>
    /// <remarks>The value reflects the number of subdivisions within the visible timeline range, which may
    /// vary depending on whether non-working hours are shown. This property is read-only.</remarks>
    private int TotalSubdivisions
    {
        get
        {
            var effectiveStart = ShowNonWorkingHours ? TimeSpan.Zero : TimelineStart;
            var effectiveEnd = ShowNonWorkingHours ? TimeSpan.FromHours(24) : TimelineEnd;
            return (int)Math.Round((effectiveEnd - effectiveStart).TotalMinutes / (60.0 / SubdivisionCount));
        }
    }

    /// <summary>
    /// Gets the total width of the grid, calculated as the product of the number of subdivisions and the width of each
    /// cell.
    /// </summary>
    private int GridWidth => TotalSubdivisions * CellWidth;

    /// <summary>
    /// Gets the starting hour for the displayed time range, depending on whether non-working hours are shown.
    /// </summary>
    /// <remarks>If non-working hours are included, the starting hour is set to 0. Otherwise, it reflects the
    /// beginning of the workday as specified by the <see cref="TimelineStart"/> property.</remarks>
    private int StartHour => ShowNonWorkingHours ? 0 : TimelineStart.Hours;

    /// <summary>
    /// Gets the hour at which the displayed time range ends, based on whether non-working hours are shown.
    /// </summary>
    /// <remarks>If non-working hours are displayed, the end hour is set to 24. Otherwise, it reflects the end
    /// of the working day as specified by the <see cref="TimelineEnd"/> property.</remarks>
    private int EndHour => ShowNonWorkingHours ? 24 : TimelineEnd.Hours;

    private string? InternalStyle => new StyleBuilder(Style)
        .AddStyle("--timeline-cell-width", $"{CellWidth}px", CellWidth > 0)
        .AddStyle("--timeline-cell-height", $"{CellHeight}px", CellHeight > 0)
        .Build();

    /// <summary>
    /// Invokes the slot double-click event asynchronously for the specified scheduler slot.
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
