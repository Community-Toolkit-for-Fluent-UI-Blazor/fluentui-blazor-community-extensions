using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class AgendaSlotBuilderTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(30)]
    public void GetEndDate_Adds_NumberOfDays_To_Start(int numberOfDays)
    {
        var reference = new DateTime(2025, 11, 24, 9, 30, 0);
        var builder = new AgendaSlotBuilder(numberOfDays);

        var start = builder.GetStartDate(reference, CultureInfo.InvariantCulture);
        var end = builder.GetEndDate(reference, CultureInfo.InvariantCulture);

        Assert.Equal(reference, start);
        Assert.Equal(start.AddDays(numberOfDays), end);
    }

    [Fact]
    public void GetStartDate_Returns_Reference_AsIs()
    {
        var reference = new DateTime(2023, 6, 5, 14, 15, 0);
        var builder = new AgendaSlotBuilder(7);

        var start = builder.GetStartDate(reference, CultureInfo.InvariantCulture);

        Assert.Equal(reference, start);
    }

    [Fact]
    public void GetSlots_CurrentImplementation_Returns_Empty()
    {
        var numberOfDays = 3;
        var reference = new DateTime(2025, 11, 24);
        var builder = new AgendaSlotBuilder(numberOfDays);

        var start = builder.GetStartDate(reference, CultureInfo.InvariantCulture);
        var end = builder.GetEndDate(reference, CultureInfo.InvariantCulture);

        var slots = builder.GetSlots(CultureInfo.InvariantCulture, start, end).ToList();

        Assert.Empty(slots);
    }
}
