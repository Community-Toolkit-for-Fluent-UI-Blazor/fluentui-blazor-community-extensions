namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides data for calendar event notifications in the scheduler, including the event identifier and scheduled date.
/// </summary>
/// <param name="Id">The unique identifier for the calendar event. Cannot be null or empty.</param>
/// <param name="Date">The date and time of the scheduled calendar event.</param>
public record class SchedularCalendarEventArgs(string Id, DateTime Date);
