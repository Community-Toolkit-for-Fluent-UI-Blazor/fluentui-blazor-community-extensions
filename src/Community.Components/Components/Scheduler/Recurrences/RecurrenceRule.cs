namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a recurrence rule that defines how an event repeats over time.
/// </summary>
/// <remarks>A recurrence rule specifies the pattern for repeating events, such as meetings or reminders. It
/// includes the frequency of recurrence, the interval between occurrences, and optional end conditions. This class is
/// commonly used in calendar and scheduling applications to model repeating events.</remarks>
public class RecurrenceRule
{
    /// <summary>
    /// Gets or sets the recurrence frequency for the schedule.
    /// </summary>
    /// <remarks>Use this property to specify how often the event recurs, such as daily, weekly, or monthly.
    /// The value determines the interval at which the recurrence pattern is applied.</remarks>
    public RecurrenceFrequency Frequency { get; set; }

    /// <summary>
    /// Gets or sets the interval value used for periodic operations.
    /// </summary>
    public int Interval { get; set; } = 1;

    /// <summary>
    /// Gets or sets the date and time until which the item remains valid or active.
    /// </summary>
    public DateTime? Until { get; set; }

    /// <summary>
    /// Gets or sets the number of items in the collection, or <see langword="null"/> if the count is unspecified.
    /// </summary>
    public int? Count { get; set; }

    /// <summary>
    /// Gets or sets the collection of days of the week to which the operation applies.
    /// </summary>
    /// <remarks>The list may contain any combination of <see cref="DayOfWeek"/> values. Modifying this
    /// collection affects which days are considered by related operations.</remarks>
    public List<DayOfWeek> DaysOfWeek { get; set; } = [];

    /// <summary>
    /// Gets or sets the day of the month represented by this instance.
    /// </summary>
    /// <remarks>This is useful for month view</remarks>
    public int? DayOfMonth { get; set; }

    /// <summary>
    /// Gets or sets the collection of months represented as integers.
    /// </summary>
    /// <remarks>This is useful for year view</remarks>
    public List<int> Months { get; set; } = [];
}
