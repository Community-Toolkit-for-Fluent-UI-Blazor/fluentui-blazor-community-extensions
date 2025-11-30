namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies the frequency with which a recurring event occurs.
/// </summary>
/// <remarks>Use this enumeration to indicate how often an event repeats, such as daily, weekly, monthly, or
/// yearly intervals. This is commonly used in scheduling and calendar applications to define recurrence
/// patterns.</remarks>
public enum RecurrenceFrequency
{
    /// <summary>
    /// Represents a value that occurs or is applicable every day.
    /// </summary>
    Daily,

    /// <summary>
    /// Represents a value that occurs or is processed on a weekly basis.
    /// </summary>
    Weekly,

    /// <summary>
    /// Represents a value or option that is applicable on a monthly basis.
    /// </summary>
    Monthly,

    /// <summary>
    /// Represents a value or option that occurs once per year.
    /// </summary>
    Yearly
}
