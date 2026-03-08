using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a recurrence pattern for scheduling events that repeat on a daily basis.
/// </summary>
/// <remarks>Use this class to define rules for events that occur every day, such as reminders or scheduled tasks.
/// The recurrence pattern can be customized to specify intervals, start dates, and end conditions. This type is
/// typically used in calendar or scheduling applications to automate repeated event creation.</remarks>
public partial class DailyRecurrence
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
    /// Stores the value representing the condition after a specified number of occurrences.
    /// </summary>
    private string? _afterXOccurrences;

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
        _interval = Recurrence?.Interval.ToString(Culture);
        _afterXOccurrences = Recurrence?.Count?.ToString(Culture) ?? "1";
    }

    /// <summary>
    /// Sets the current state to represent a recurrence interval.
    /// </summary>
    private void SetToRecurrenceInterval()
    {
        if (int.TryParse(_interval, NumberStyles.Integer, Culture, out var intervalValue) && intervalValue > 0)
        {
            Recurrence?.Interval = intervalValue;
        }
    }

    /// <summary>
    /// Sets the current state to represent a recurrence interval.
    /// </summary>
    private void SetAfterXOccurrences()
    {
        Recurrence?.Count = null;

        if (int.TryParse(_afterXOccurrences, NumberStyles.Integer, Culture, out var intervalValue) && intervalValue > 0)
        {
            Recurrence?.Count = intervalValue;
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
}
