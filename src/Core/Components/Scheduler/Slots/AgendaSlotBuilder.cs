using System.Globalization;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to build agenda-style scheduler slots spanning a specified number of days.
/// </summary>
/// <remarks>This class implements the ISchedulerSlotBuilder interface to generate scheduler slots for agenda
/// views. It is intended for internal use within scheduling components and is not thread-safe.</remarks>
internal sealed class AgendaSlotBuilder : ISchedulerSlotBuilder
{
    /// <summary>
    /// Represents the number of days that each agenda slot should span.
    /// </summary>
    private readonly int _numberOfDays;

    /// <summary>
    /// Initializes a new instance of the <see cref="AgendaSlotBuilder"/> class with the specified number of days for each slot.
    /// </summary>
    /// <param name="numberOfDays">Number of days that each agenda slot should span.</param>
    public AgendaSlotBuilder(int numberOfDays)
    {
        _numberOfDays = numberOfDays;
    }

    /// <inheritdoc />
    public DateTime GetEndDate(DateTime reference, CultureInfo culture)
    {
        return GetStartDate(reference, culture).AddDays(_numberOfDays);
    }

    /// <inheritdoc />
    public IEnumerable<SchedulerSlot> GetSlots(
        IFluentLocalizer localizer,
        CultureInfo culture,
        DateTime startDate,
        DateTime endDate)
    {
        return [];
    }

    /// <inheritdoc />
    public DateTime GetStartDate(DateTime reference, CultureInfo culture)
    {
        return reference;
    }
}
