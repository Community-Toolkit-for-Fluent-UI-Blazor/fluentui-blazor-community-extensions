using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class DaySlotBuilderTests
{
    [Fact]
    public void GetStartDate_Returns_DateComponent()
    {
        var builder = new DaySlotBuilder();
        var reference = new DateTime(2025, 11, 26, 13, 45, 0);
        var start = builder.GetStartDate(reference, CultureInfo.InvariantCulture);

        Assert.Equal(reference.Date, start);
    }

    [Fact]
    public void GetEndDate_Returns_NextDay()
    {
        var builder = new DaySlotBuilder();
        var reference = new DateTime(2025, 6, 5, 8, 0, 0);
        var end = builder.GetEndDate(reference, CultureInfo.InvariantCulture);

        Assert.Equal(reference.Date.AddDays(1), end);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(4)]
    public void GetSlots_Produces_Expected_Count_And_Durations(int subdivisionCount)
    {
        var culture = CultureInfo.InvariantCulture;
        var builder = new DaySlotBuilder(subdivisionCount);
        var reference = new DateTime(2025, 11, 24);
        var startDate = builder.GetStartDate(reference, culture);
        var endDate = builder.GetEndDate(reference, culture);

        var slots = builder.GetSlots(culture, startDate, endDate).ToList();

        var expectedCount = 24 * subdivisionCount;
        Assert.Equal(expectedCount, slots.Count);

        var minutesPerSlot = 60.0 / subdivisionCount;

        var first = slots.First();
        Assert.Equal(startDate, first.Start);
        Assert.Equal(startDate.AddMinutes(minutesPerSlot), first.End);
        Assert.Equal(startDate.ToString("HH:mm", culture), first.Label);
        Assert.Equal(0, first.Row);
        Assert.Equal(0, first.Column);

        var last = slots.Last();
        var expectedLastStart = startDate.AddHours(23).AddMinutes((subdivisionCount - 1) * minutesPerSlot);
        Assert.Equal(expectedLastStart, last.Start);
        Assert.Equal(endDate, last.End);
        Assert.Equal(expectedCount - 1, last.Row);
        Assert.Equal(0, last.Column);

        // Tous les créneaux ont la durée attendue
        foreach (var slot in slots)
        {
            var duration = (slot.End - slot.Start).TotalMinutes;
            Assert.Equal(minutesPerSlot, duration, 6);
        }
    }

    [Fact]
    public void Constructor_Clamps_SubdivisionCount_To_AtLeastOne()
    {
        var culture = CultureInfo.InvariantCulture;
        var reference = new DateTime(2025, 11, 24);

        var builderZero = new DaySlotBuilder(0);
        var slotsZero = builderZero.GetSlots(culture, builderZero.GetStartDate(reference, culture), builderZero.GetEndDate(reference, culture)).ToList();
        Assert.Equal(24 * 1, slotsZero.Count);

        var builderNegative = new DaySlotBuilder(-5);
        var slotsNeg = builderNegative.GetSlots(culture, builderNegative.GetStartDate(reference, culture), builderNegative.GetEndDate(reference, culture)).ToList();
        Assert.Equal(24 * 1, slotsNeg.Count);
    }
}
