using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class TimelineSlotBuilderTests
{
    [Fact]
    public void GetStartDate_Returns_DateComponent()
    {
        var builder = new TimelineSlotBuilder();
        var reference = new DateTime(2025, 11, 26, 13, 45, 0);
        var start = builder.GetStartDate(reference, CultureInfo.InvariantCulture);

        Assert.Equal(reference.Date, start);
    }

    [Fact]
    public void GetEndDate_Returns_NextDayDate()
    {
        var builder = new TimelineSlotBuilder();
        var reference = new DateTime(2025, 6, 5, 8, 0, 0);
        var end = builder.GetEndDate(reference, CultureInfo.InvariantCulture);

        Assert.Equal(reference.Date.AddDays(1), end);
    }

    [Fact]
    public void GetSlots_Produces_24HoursTimeslots_With_Subdivisions()
    {
        const int subdivisionCount = 4;
        var builder = new TimelineSlotBuilder(subdivisionCount);
        var culture = CultureInfo.InvariantCulture;
        var reference = new DateTime(2025, 11, 24);
        var startDate = builder.GetStartDate(reference, culture);
        var endDate = builder.GetEndDate(reference, culture);

        var slots = builder.GetSlots(culture, startDate, endDate).ToList();

        var expectedCount = 24 * subdivisionCount;
        Assert.Equal(expectedCount, slots.Count);

        // Vérifie premier créneau
        var first = slots.First();
        Assert.Equal(startDate, first.Start);
        var minutesPerSlot = 60.0 / subdivisionCount;
        Assert.Equal(startDate.AddMinutes(minutesPerSlot), first.End);
        Assert.Equal(0, first.Row);
        Assert.Equal(0, first.Column);

        // Vérifie dernier créneau
        var last = slots.Last();
        var expectedLastStart = startDate.AddHours(23).AddMinutes((subdivisionCount - 1) * minutesPerSlot);
        Assert.Equal(expectedLastStart, last.Start);
        Assert.Equal(expectedLastStart.AddMinutes(minutesPerSlot), last.End);
        Assert.Equal(0, last.Row);
        Assert.Equal(24 * subdivisionCount - 1, last.Column);

        // Vérifie couverture complète : dernier End == endDate
        Assert.Equal(endDate, last.End);
    }

    [Fact]
    public void Constructor_Clamps_SubdivisionCount_To_AtLeastOne()
    {
        var culture = CultureInfo.InvariantCulture;
        var reference = new DateTime(2025, 11, 24);

        var builderZero = new TimelineSlotBuilder(0);
        var slotsZero = builderZero.GetSlots(culture, builderZero.GetStartDate(reference, culture), builderZero.GetEndDate(reference, culture)).ToList();
        Assert.Equal(24 * 1, slotsZero.Count);

        var builderNegative = new TimelineSlotBuilder(-3);
        var slotsNeg = builderNegative.GetSlots(culture, builderNegative.GetStartDate(reference, culture), builderNegative.GetEndDate(reference, culture)).ToList();
        Assert.Equal(24 * 1, slotsNeg.Count);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(6)]
    public void Slot_Duration_Is_Equal_To_MinutesPerSlot(int subdivisionCount)
    {
        var builder = new TimelineSlotBuilder(subdivisionCount);
        var culture = CultureInfo.InvariantCulture;
        var reference = new DateTime(2025, 11, 24);
        var start = builder.GetStartDate(reference, culture);
        var end = builder.GetEndDate(reference, culture);

        var slots = builder.GetSlots(culture, start, end).ToList();
        var minutesPerSlot = 60.0 / subdivisionCount;

        foreach (var slot in slots)
        {
            var durationMinutes = (slot.End - slot.Start).TotalMinutes;
            Assert.Equal(minutesPerSlot, durationMinutes, 6);
        }
    }
}
