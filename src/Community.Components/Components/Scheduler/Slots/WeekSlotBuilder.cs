using System.Globalization;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to generate scheduler slots representing a week, starting from the first day of the week as
/// determined by the specified reference date.
/// </summary>
/// <remarks>This class implements the ISchedulerSlotBuilder interface to create weekly time slots for scheduling
/// scenarios. It calculates the start and end dates of a week based on a reference date and produces individual day
/// slots within that week. The week is considered to start on the day corresponding to DayOfWeek.Sunday, following the
/// standard .NET convention. This class is intended for internal use within scheduling components and is not
/// thread-safe.</remarks>
internal sealed class WeekSlotBuilder : ISchedulerSlotBuilder
{
    /// <summary>
    /// Represents the number of subdivisions per hour.
    /// </summary>
    private readonly int _subdivisionCount;

    /// <summary>
    /// Initializes a new instance of the WeekSlotBuilder class with the specified number of subdivisions per week slot.
    /// </summary>
    /// <param name="subdivisionCount">The number of subdivisions to create within each week slot. Must be greater than zero.</param>
    public WeekSlotBuilder(int subdivisionCount = 1)
    {
        _subdivisionCount = Math.Max(1, subdivisionCount);
    }

    /// <inheritdoc />
    public DateTime GetEndDate(DateTime reference, CultureInfo culture)
    {
        return GetStartDate(reference, culture).AddDays(7);
    }

    /// <inheritdoc />
    public IEnumerable<SchedulerSlot> GetSlots(CultureInfo culture, DateTime startDate, DateTime endDate)
    {
        var totalDays = (endDate.Date - startDate.Date).Days;

        if (totalDays != 7)
        {
            throw new InvalidOperationException("The week slot builder requires a 7-day range.");
        }

        for (var day = 0; day < 7; day++)
        {
            var currentDay = startDate.AddDays(day);

            var minutesPerSlot = 60.0 / _subdivisionCount;
            var slotSpan = TimeSpan.FromMinutes(minutesPerSlot);
            var totalHours = 24;

            for (var hour = 0; hour < totalHours; hour++)
            {
                for (var sub = 0; sub < _subdivisionCount; sub++)
                {
                    var slotStart = currentDay.AddHours(hour).AddMinutes(sub * minutesPerSlot);
                    var slotEnd = slotStart + slotSpan;

                    yield return new SchedulerSlot(
                        string.Empty,
                        slotStart,
                        slotEnd,
                        false,
                        hour * _subdivisionCount + sub,
                        day
                    );
                }
            }
        }
    }

    /// <inheritdoc />
    public DateTime GetStartDate(DateTime reference, CultureInfo culture)
    {
        var firstDayOfWeek = culture.DateTimeFormat.FirstDayOfWeek;
        var diff = ((int)reference.DayOfWeek - (int)firstDayOfWeek + 7) % 7;

        return reference.Date.AddDays(-diff);
    }
}
