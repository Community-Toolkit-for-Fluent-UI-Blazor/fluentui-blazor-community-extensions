namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies the available views for displaying scheduled events in a calendar or scheduler interface.
/// </summary>
/// <remarks>Use this enumeration to select the visual layout for presenting scheduling data, such as daily,
/// weekly, monthly, yearly, agenda, or timeline formats. The chosen view type determines how events are organized and
/// displayed to users.</remarks>
public enum SchedulerView
{
    /// <summary>
    /// Represents a view for a single day in a scheduler.
    /// </summary>
    Day,

    /// <summary>
    /// Represents the view for a week in a scheduler.
    /// </summary>
    Week,

    /// <summary>
    /// Represents the view for a month in a scheduler.
    /// </summary>
    Month,

    /// <summary>
    /// Represents the view for a year in a scheduler.
    /// </summary>
    Year,

    /// <summary>
    /// Represents a timeline view in a scheduler.
    /// </summary>
    Timeline,

    /// <summary>
    /// Represents an agenda view in a scheduler.
    /// </summary>
    Agenda
}
