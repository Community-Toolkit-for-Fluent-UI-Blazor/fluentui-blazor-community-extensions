namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a scheduled item with associated data, time range, recurrence rules, and exception dates.
/// </summary>
/// <remarks>Use this class to model events or tasks that occur within a specific time frame and may repeat
/// according to a recurrence rule. Exception dates can be specified to exclude particular occurrences from the
/// schedule.</remarks>
/// <typeparam name="T">The type of data associated with the scheduled item.</typeparam>
public class SchedulerItem<T>
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the title associated with the object.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the data associated with the response.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Gets or sets the start date and time for the event or time period.
    /// </summary>
    public DateTime Start { get; set; }

    /// <summary>
    /// Gets or sets the end date and time for the range or event.
    /// </summary>
    public DateTime End { get; set; }

    /// <summary>
    /// Gets or sets the recurrence rule that defines how and when the event repeats.
    /// </summary>
    public RecurrenceRule? Recurrence { get; set; }

    /// <summary>
    /// Gets or sets the collection of dates that are considered exceptions to the standard schedule.
    /// </summary>
    /// <remarks>Use this property to specify dates that should be excluded from regular processing, such as
    /// holidays or special events. The list may be empty if no exceptions are defined.</remarks>
    public List<DateTime> Exceptions { get; set; } = [];

    /// <summary>
    /// Gets or sets a description or additional information about the scheduled item.
    /// </summary>
    public string? Description { get; set; }
}
