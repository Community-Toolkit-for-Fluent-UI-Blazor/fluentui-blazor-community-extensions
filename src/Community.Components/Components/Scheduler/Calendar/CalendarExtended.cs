using System.Globalization;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides extension methods for retrieving localized calendar data, such as day names, month names, week dates, and
/// year ranges, based on cultural conventions.
/// </summary>
/// <remarks>The methods in this class facilitate the display and manipulation of calendar-related information in
/// applications that require localization or custom calendar layouts. All members are static and thread-safe.</remarks>
public static class CalendarExtended
{
    /// <summary>
    /// Returns a read-only list of tuples containing the full and abbreviated names of the days of the week, ordered
    /// according to the first day of the specified culture.
    /// </summary>
    /// <remarks>The returned list reflects the day name conventions and ordering of the specified culture.
    /// This is useful for displaying localized day names in calendars or date pickers.</remarks>
    /// <param name="culture">An optional <see cref="CultureInfo"/> that determines the language and ordering of day names. If <see
    /// langword="null"/>, the current culture is used.</param>
    /// <returns>A read-only list of seven tuples, each containing the full name and abbreviated name of a day of the week,
    /// ordered starting from the culture's first day of the week.</returns>
    public static IReadOnlyList<(string Name, string Shorted)> GetDayNames(CultureInfo? culture = null)
    {
        culture ??= CultureInfo.CurrentCulture;
        var dayNames = culture.DateTimeFormat.AbbreviatedDayNames;
        var fullNames = culture.DateTimeFormat.DayNames;

        // Reorder starting from the culture's first day of week
        var firstDay = (int)culture.DateTimeFormat.FirstDayOfWeek;
        return [.. Enumerable.Range(0, 7).Select(i => (Name: fullNames[(i + firstDay) % 7], Shorted: dayNames[(i + firstDay) % 7]))];
    }

    /// <summary>
    /// Returns a list of dates representing the days in the specified week of a calendar month, using the provided
    /// culture's week definition.
    /// </summary>
    /// <remarks>The method calculates weeks as displayed in typical calendar grids, which may include days
    /// from the previous or next month to fill out the week. The first week (weekIndex = 0) starts with the first
    /// visible day according to the culture's first day of the week.</remarks>
    /// <param name="weekIndex">The zero-based index of the week to retrieve, where 0 corresponds to the first visible week in the calendar grid
    /// for the month.</param>
    /// <param name="referenceMonth">An optional reference date indicating the month for which to calculate the week. If null, the current date is
    /// used.</param>
    /// <param name="culture">An optional culture that determines the first day of the week and calendar rules. If null, the current culture
    /// is used.</param>
    /// <returns>A read-only list of seven DateTime values, each representing a day in the specified week. The list may include
    /// dates from adjacent months if the week overlaps month boundaries.</returns>
    public static IReadOnlyList<DateTime> GetDaysOfWeek(int weekIndex, DateTime? referenceMonth = null, CultureInfo? culture = null)
    {
        culture ??= CultureInfo.CurrentCulture;
        referenceMonth ??= DateTime.Today;

        var firstOfMonth = new DateTime(referenceMonth.Value.Year, referenceMonth.Value.Month, 1);
        var firstDayOfWeek = culture.DateTimeFormat.FirstDayOfWeek;

        // Find the first visible day in the calendar grid
        var diff = ((int)firstOfMonth.DayOfWeek - (int)firstDayOfWeek + 7) % 7;
        var firstVisibleDay = firstOfMonth.AddDays(-diff);

        // Return the 7 days of the requested week
        return [.. Enumerable.Range(0, 7).Select(i => firstVisibleDay.AddDays(weekIndex * 7 + i))];
    }
}
