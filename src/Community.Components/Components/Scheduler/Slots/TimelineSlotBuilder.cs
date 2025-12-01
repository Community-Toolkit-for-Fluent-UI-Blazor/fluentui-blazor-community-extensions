using System.Globalization;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to build scheduler slots for a timeline, generating time intervals and calculating start and
/// end dates based on a reference point.
/// </summary>
/// <remarks>This class is intended for internal use within scheduling components and implements the
/// ISchedulerSlotBuilder interface. It generates slots representing hourly intervals and calculates timeline boundaries
/// relative to a specified reference date. Thread safety is not guaranteed; concurrent access should be managed
/// externally if required.</remarks>
internal sealed class TimelineSlotBuilder : ISchedulerSlotBuilder
{
    /// <summary>
    /// Represents the number of subdivisions per hour.
    /// </summary>
    private readonly int _subdivisionCount;

    /// <summary>
    /// Initializes a new instance of the TimelineSlotBuilder class with the specified number of subdivisions per day slot.
    /// </summary>
    /// <param name="subdivisionCount">The number of subdivisions to create within each day slot. Must be greater than or equal to 1. If a value less
    /// than 1 is provided, 1 subdivision will be used.</param>
    public TimelineSlotBuilder(int subdivisionCount = 1)
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
                    string.Empty,
                    slotStart,
                    slotEnd,
                    false,
                    0,
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
