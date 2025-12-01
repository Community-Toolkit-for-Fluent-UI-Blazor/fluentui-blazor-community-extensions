namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a scheduled time slot with an optional label and associated grid position.
/// </summary>
/// <remarks>Use this record to model individual time slots in a scheduling system, such as for calendar or
/// resource allocation scenarios. The grid position specified by <paramref name="Row"/> and <paramref name="Column"/>
/// can be used to organize slots visually or logically.</remarks>
/// <param name="Label">The label or description for the slot. Can be null if no label is required.</param>
/// <param name="Start">The start date and time of the slot.</param>
/// <param name="End">The end date and time of the slot. Must be greater than or equal to <paramref name="Start"/>.</param>
/// <param name="Disabled">Indicates whether the slot is disabled or inactive. Defaults to false.</param>
/// <param name="Row">The row index in the scheduling grid. Defaults to 0.</param>
/// <param name="Column">The column index in the scheduling grid. Defaults to 0.</param>
public record SchedulerSlot(string Label, DateTime Start, DateTime End, bool Disabled = false, int Row = 0, int Column = 0)
{
}
