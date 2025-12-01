using System.ComponentModel.DataAnnotations;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a schedulable item with support for recurrence, exceptions, and associated data.
/// </summary>
/// <remarks>Use this type to model events or operations that may repeat over time and require exception handling
/// for specific dates. The generic parameter allows attaching domain-specific data to each scheduled item.</remarks>
/// <typeparam name="T">The type of data associated with the scheduled item.</typeparam>
internal class EditSchedulerItem<T>
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the title associated with the object.
    /// </summary>
    [Required]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the data associated with the response.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Gets or sets the start date for the event.
    /// </summary>
    [Required]
    public DateTime? Start { get; set; }

    /// <summary>
    /// Gets or sets the scheduled start time for the operation.
    /// </summary>
    [Required]
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// Gets or sets the end date for the range or event.
    /// </summary>
    [Required]
    public DateTime? End { get; set; }

    /// <summary>
    /// Gets or sets the end time of the event or operation.
    /// </summary>
    [Required]
    public DateTime? EndTime { get; set; }

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
