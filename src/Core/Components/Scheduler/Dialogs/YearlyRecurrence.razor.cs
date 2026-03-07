using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides configuration and management for yearly recurrence patterns, including rules, exceptions, and display
/// labels for scheduling events.
/// </summary>
/// <remarks>Use this class to define how events repeat on a yearly basis, specify exception dates to exclude
/// certain occurrences, and customize scheduler labels. The class is typically used in scheduling or calendar
/// components to support complex recurrence scenarios.</remarks>
public partial class YearlyRecurrence
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
    /// Represents the selected interval for the recurrence pattern, such as the number of weeks between occurrences.
    /// </summary>
    private string? _interval;

    /// <summary>
    /// Represents the selected day of the month for the recurrence pattern, such as the specific date on which events should occur.
    /// </summary>
    private string? _dayOfMonth;

    /// <summary>
    /// Represents the selected count of occurrences for the recurrence pattern, indicating how many times the event should repeat before ending.
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

    /// <summary>
    /// Gets or sets the culture information used for formatting and localization within the component.
    /// </summary>
    /// <remarks>If not set, the property defaults to the current culture of the executing environment.
    /// Changing this property affects how dates, numbers, and other culture-sensitive data are displayed.</remarks>
    [Parameter]
    public CultureInfo Culture { get; set; } = CultureInfo.CurrentCulture;

    /// <summary>
    /// Parses the interval value from the input string and assigns it to the recurrence interval if the parsing
    /// succeeds.
    /// </summary>
    /// <remarks>This method updates the recurrence interval only if the input string can be successfully
    /// parsed as an integer using the specified culture. No changes are made if parsing fails.</remarks>
    private void SetToRecurrenceInterval()
    {
        Recurrence?.Interval = 1;

        if (int.TryParse(_interval, NumberStyles.Integer, Culture, out var interval))
        {
            Recurrence?.Interval = interval;
        }
    }

    /// <summary>
    /// Sets the recurrence pattern to occur on the first day of the month and updates the interval based on the current
    /// input value.
    /// </summary>
    /// <remarks>This method assigns the recurrence to the first day of the month and attempts to parse the
    /// interval from the current input using the specified culture. If parsing succeeds, the recurrence interval is
    /// updated accordingly.</remarks>
    private void SetToRecurrenceDayOfMonth()
    {
        Recurrence?.DayOfMonth = 1;

        if (int.TryParse(_dayOfMonth, NumberStyles.Integer, Culture, out var interval) &&
            interval >= 1 && interval <= 31)
        {
            Recurrence?.DayOfMonth = interval;
        }
    }

    /// <summary>
    /// Configures the recurrence settings to use a fixed number of occurrences.
    /// </summary>
    /// <remarks>Call this method to switch the recurrence pattern to be based on a specific count rather than
    /// a date or other criteria. This typically resets or updates related recurrence properties to reflect the
    /// count-based mode.</remarks>
    private void SetToRecurrenceCount()
    {
        Recurrence?.Count = null;

        if (int.TryParse(_recurrenceCount, NumberStyles.Integer, Culture, out var interval) &&
            interval >= 1)
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
    /// <returns>An <see cref="ButtonAppearance"/> value representing the accent style if <paramref name="primary"/> is <see
    /// langword="true"/>; otherwise, the neutral style.</returns>
    private static ButtonAppearance GetAppearance(bool primary)
    {
        return primary ? ButtonAppearance.Primary : ButtonAppearance.Default;
    }

    /// <summary>
    /// Toggles the inclusion of the specified month in the recurrence pattern. If the month is already included, it is
    /// removed; otherwise, it is added.
    /// </summary>
    /// <param name="month">The month to toggle in the recurrence pattern. Must be an integer between 1 and 12, where 1 represents January
    /// and 12 represents December.</param>
    private void ToggleMonth(int month)
    {
        if (!Recurrence!.Months.Remove(month))
        {
            Recurrence.Months.Add(month);
        }
    }
}
