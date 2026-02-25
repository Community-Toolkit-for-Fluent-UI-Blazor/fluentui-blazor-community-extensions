using System.Globalization;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to construct scheduler slots representing months within a single calendar year.
/// </summary>
/// <remarks>This class is intended for internal use by scheduling components that require year-based slot
/// segmentation. It generates slots for each month of the specified year, supporting culture-specific formatting for
/// month names.</remarks>
internal sealed class YearSlotBuilder : ISchedulerSlotBuilder
{
    /// <inheritdoc />
    public DateTime GetEndDate(DateTime reference, CultureInfo culture)
    {
        return GetStartDate(reference, culture).AddYears(1);
    }

    /// <inheritdoc />
    public IEnumerable<SchedulerSlot> GetSlots(CultureInfo culture, DateTime startDate, DateTime endDate)
    {
        for (var month = 0; month < 12; month++)
        {
            var date = startDate.AddMonths(month);

            yield return new SchedulerSlot(date.ToString("MMMM", culture), date, date.AddMonths(1));
        }
    }

    /// <inheritdoc />
    public DateTime GetStartDate(DateTime reference, CultureInfo culture)
    {
        return new DateTime(reference.Year, 1, 1);
    }
}
