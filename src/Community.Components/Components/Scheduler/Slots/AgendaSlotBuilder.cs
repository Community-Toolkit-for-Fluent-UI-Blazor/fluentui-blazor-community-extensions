using System.Globalization;

namespace FluentUI.Blazor.Community.Components;

internal sealed class AgendaSlotBuilder : ISchedulerSlotBuilder
{
    private readonly int _numberOfDays;

    public AgendaSlotBuilder(int numberOfDays)
    {
        _numberOfDays = numberOfDays;
    }

    public DateTime GetEndDate(DateTime reference, CultureInfo culture)
    {
        return GetStartDate(reference, culture).AddDays(_numberOfDays);
    }

    public IEnumerable<SchedulerSlot> GetSlots(CultureInfo culture, DateTime startDate, DateTime endDate)
    {
        return [];
    }

    public DateTime GetStartDate(DateTime reference, CultureInfo culture)
    {
        return reference;
    }
}
