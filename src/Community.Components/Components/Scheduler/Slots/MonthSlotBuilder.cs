using System.Globalization;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to generate scheduler slots representing individual days within a specific month, based on a
/// reference date and culture settings.
/// </summary>
/// <remarks>MonthSlotBuilder is used to construct time slots for scheduling scenarios where each slot corresponds
/// to a single day in a month. It determines the start and end dates of the month and produces slots with localized
/// labels. This class is intended for internal use and is not thread-safe.</remarks>
internal sealed class MonthSlotBuilder : ISchedulerSlotBuilder
{
    /// <inheritdoc />
    public DateTime GetStartDate(DateTime reference, CultureInfo culture)
    {
        var firstDayOfMonth = new DateTime(reference.Year, reference.Month, 1);
        var firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;

        // Shift to the first day of week <= first day of month
        var diff = ((int)firstDayOfMonth.DayOfWeek - (int)firstDayOfWeek + 7) % 7;

        return firstDayOfMonth.AddDays(-diff);
    }

    /// <inheritdoc />
    public DateTime GetEndDate(DateTime reference, CultureInfo culture)
    {
        var firstDayOfMonth = new DateTime(reference.Year, reference.Month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
        var firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;

        // Shift to the last day of week >= last day of month
        var diff = ((int)lastDayOfMonth.DayOfWeek - (int)firstDayOfWeek + 7) % 7;

        return lastDayOfMonth.AddDays(7 - diff).AddDays(1);
    }

    /// <inheritdoc />
    public IEnumerable<SchedulerSlot> GetSlots(CultureInfo culture, DateTime startDate, DateTime endDate)
    {
        var col = 0;
        var row = 0;

        var middle = startDate.AddDays((endDate - startDate).Days / 2);
        var displayedYear = middle.Year;
        var displayedMonth = middle.Month;

        for (var date = startDate; date < endDate; date = date.AddDays(1))
        {
            var isOutsideDisplayedMonth = date.Year != displayedYear || date.Month != displayedMonth;

            yield return new SchedulerSlot(
                date.ToString("dd", culture),
                date,
                date.AddDays(1),
                isOutsideDisplayedMonth,
                row,
                col
            );

            col++;

            if (col == 7)
            {
                col = 0;
                row++;
            }
        }
    }
}
