using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a weekly recurrence pattern for scheduling events, including support for exceptions and customizable
/// labels.
/// </summary>
/// <remarks>Use this class to configure recurring events that follow a weekly schedule, with options to exclude
/// specific dates and customize display labels. The recurrence pattern and exceptions can be set to control how and
/// when events repeat. This type is typically used in scheduling or calendar components to manage event repetition and
/// exception handling.</remarks>
public partial class WeeklyRecurrence
{
    /// <summary>
    /// Represents the selected end mode for the recurrence settings.
    /// </summary>
    private int _endMode;

    /// <summary>
    /// Represents the date when the recurrence exceptions start taking effect.
    /// </summary>
    private DateTime? _exceptionDate;

    /// <summary>
    /// Represents the interval at which the event recurs, such as every 1 day, every 2 days, etc.
    /// </summary>
    private string? _interval;

    /// <summary>
    /// Represents the value representing the condition after a specified number of occurrences.
    /// </summary>
    private string? _recurrenceCount;

    /// <summary>
    /// Gets or sets the recurrence rule that defines how and when the event repeats.
    /// </summary>
    /// <remarks>Set this property to specify the pattern for event repetition, such as daily, weekly, or
    /// monthly intervals. If <see langword="null"/>, the event does not recur.</remarks>
    [Parameter]
    public RecurrenceRule? Recurrence { get; set; }

    /// <summary>
    /// Gets or sets the culture information used for formatting and localization within the component.
    /// </summary>
    /// <remarks>If not set, the property defaults to the current culture of the executing environment.
    /// Changing this property affects how dates, numbers, and other culture-sensitive data are displayed.</remarks>
    [Parameter]
    public CultureInfo Culture { get; set; } = CultureInfo.CurrentCulture;

    /// <summary>
    /// Gets or sets the list of dates that are excluded from the schedule or recurrence pattern.
    /// </summary>
    /// <remarks>Use this property to specify dates that should be treated as exceptions and not included in
    /// the regular schedule. Modifying this list affects which dates are considered valid occurrences.</remarks>
    [Parameter]
    public List<DateTime> Exceptions { get; set; } = [];

    /// <summary>
    /// Gets or sets the set of labels used for customizing the text displayed by the scheduler component.
    /// </summary>
    [Inject]
    private IFluentLocalizer Localizer { get; set; } = default!;

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();
        _interval = Recurrence?.Interval.ToString(Culture) ?? "1";
        _recurrenceCount = Recurrence?.Count?.ToString(Culture) ?? "1";
    }

    /// <summary>
    /// Parses the interval value from the input string and assigns it to the recurrence interval if the parsing
    /// succeeds.
    /// </summary>
    /// <remarks>This method updates the recurrence interval only if the input string can be successfully
    /// parsed as an integer using the specified culture. No changes are made if parsing fails.</remarks>
    private void SetRecurrenceInterval()
    {
        Recurrence?.Interval = 1;

        if (int.TryParse(_interval, NumberStyles.Integer, Culture, out var interval))
        {
            Recurrence?.Interval = interval;
        }
    }

    /// <summary>
    /// Parses the recurrence count from the input string and updates the recurrence object if the value is within the
    /// valid range.
    /// </summary>
    /// <remarks>This method sets the recurrence count to null if the input is not a valid integer between 1
    /// and 31, inclusive. If the input is valid, the recurrence count is updated accordingly. The method does not throw
    /// exceptions for invalid input; it simply resets the count.</remarks>
    private void SetRecurrenceCount()
    {
        Recurrence?.Count = null;

        if (int.TryParse(_recurrenceCount, NumberStyles.Integer, Culture, out var interval) &&
            interval >= 1 && interval <= 31)
        {
            Recurrence?.Count = interval;
        }
    }

    /// <summary>
    /// Adds the current exception date to the collection of exceptions if it is set and not already present.
    /// </summary>
    private void AddException()
    {
        if (_exceptionDate.HasValue && !Exceptions.Contains(_exceptionDate.Value))
        {
            Exceptions.Add(_exceptionDate.Value);
            _exceptionDate = null;
        }
    }

    /// <summary>
    /// Removes the exception entry for the specified date from the collection.
    /// </summary>
    /// <param name="date">The date of the exception to remove from the collection.</param>
    private void RemoveException(DateTime date)
    {
        Exceptions.Remove(date);
    }

    /// <summary>
    /// Returns the appearance style based on whether the primary style is requested.
    /// </summary>
    /// <param name="primary">Indicates whether to return the primary appearance style. Specify <see langword="true"/> to select the accent
    /// style; otherwise, the neutral style is returned.</param>
    /// <returns>An <see cref="Appearance"/> value representing the accent style if <paramref name="primary"/> is <see
    /// langword="true"/>; otherwise, the neutral style.</returns>
    private static ButtonAppearance GetAppearance(bool primary)
    {
        return primary ? ButtonAppearance.Primary : ButtonAppearance.Default;
    }

    /// <summary>
    /// Toggles the specified day in the recurrence pattern, adding it if it is not present or removing it if it is.
    /// </summary>
    /// <param name="day">The day of the week to add to or remove from the recurrence pattern.</param>
    private void ToggleDay(DayOfWeek day)
    {
        if (!Recurrence!.DaysOfWeek.Remove(day))
        {
            Recurrence.DaysOfWeek.Add(day);
        }
    }
}
