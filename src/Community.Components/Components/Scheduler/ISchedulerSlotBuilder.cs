using System.Globalization;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines methods for constructing and retrieving scheduler slots and their associated date ranges.
/// </summary>
/// <remarks>Implementations of this interface provide logic for determining slot availability and formatting slot
/// data according to cultural settings. Methods allow callers to obtain start and end dates based on a reference date,
/// as well as to enumerate available slots within a specified range. This interface is typically used in scheduling
/// systems to support flexible slot generation and localization.</remarks>
public interface ISchedulerSlotBuilder
{
    /// <summary>
    /// Returns the start date associated with the specified reference date.
    /// </summary>
    /// <param name="reference">The reference date for which to retrieve the corresponding start date.</param>
    /// <param name="culture">The culture information used to determine date formatting and conventions. Cannot be null.</param>
    /// <returns>A <see cref="DateTime"/> value representing the start date related to <paramref name="reference"/>.</returns>
    DateTime GetStartDate(DateTime reference, CultureInfo culture);

    /// <summary>
    /// Calculates the end date based on the specified reference date.
    /// </summary>
    /// <param name="reference">The reference date from which to calculate the end date.</param>
    /// <returns>A <see cref="DateTime"/> value representing the calculated end date.</returns>
    DateTime GetEndDate(DateTime reference, CultureInfo culture);

    /// <summary>
    /// Retrieves a collection of available scheduler slots within the specified date range, formatted according to the
    /// provided culture.
    /// </summary>
    /// <param name="culture">The culture information used to format slot data, such as dates and times. Cannot be null.</param>
    /// <param name="startDate">The start date and time of the range for which to retrieve scheduler slots.</param>
    /// <param name="endDate">The end date and time of the range for which to retrieve scheduler slots. Must be greater than or equal to
    /// <paramref name="startDate"/>.</param>
    /// <returns>An enumerable collection of <see cref="SchedulerSlot"/> objects representing available slots within the
    /// specified range. The collection will be empty if no slots are available.</returns>
    IEnumerable<SchedulerSlot> GetSlots(CultureInfo culture, DateTime startDate, DateTime endDate);
}
