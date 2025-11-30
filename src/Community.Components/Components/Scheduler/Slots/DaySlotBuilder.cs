using System.Globalization;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to generate scheduler slots representing hourly intervals within a single day.
/// </summary>
/// <remarks>This class implements the <see cref="ISchedulerSlotBuilder"/> interface to create day-based slot
/// sequences, where each slot corresponds to one hour. It is intended for use in scheduling scenarios that require
/// division of a day into discrete hourly segments. This class is not intended to be used directly; use through the
/// interface for extensibility.</remarks>
internal sealed class DaySlotBuilder : ISchedulerSlotBuilder
{
    /// <summary>
    /// Represents the number of subdivisions per hour.
    /// </summary>
    private readonly int _subdivisionCount;

    /// <summary>
    /// Initializes a new instance of the DaySlotBuilder class with the specified number of subdivisions per day slot.
    /// </summary>
    /// <param name="subdivisionCount">The number of subdivisions to create within each day slot. Must be greater than or equal to 1. If a value less
    /// than 1 is provided, 1 subdivision will be used.</param>
    public DaySlotBuilder(int subdivisionCount = 1)
    {
        _subdivisionCount = Math.Max(1, subdivisionCount);
    }

    /// <inheritdoc />
    public DateTime GetEndDate(DateTime reference, CultureInfo culture)
    {
        return reference.Date.AddDays(1);
    }

    /// <inheritdoc />
    public IEnumerable<SchedulerSlot> GetSlots(CultureInfo culture, DateTime startDate, DateTime endDate)
    {
        var minutesPerSlot = 60.0 / _subdivisionCount;
        var slotSpan = TimeSpan.FromMinutes(minutesPerSlot);
        var totalHours = (endDate - startDate).TotalHours;

        for (var hour = 0; hour < totalHours; hour++)
        {
            for (var sub = 0; sub < _subdivisionCount; sub++)
            {
                var slotStart = startDate.AddHours(hour).AddMinutes(sub * minutesPerSlot);
                var slotEnd = slotStart + slotSpan;

                yield return new SchedulerSlot(
                    slotStart.ToString("HH:mm", culture),
                    slotStart,
                    slotEnd,
                    false,
                    hour * _subdivisionCount + sub
                );
            }
        }
    }

    /// <inheritdoc />
    public DateTime GetStartDate(DateTime reference, CultureInfo culture)
    {
        return reference.Date;
    }
}
