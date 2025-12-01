using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class WeekSlotBuilderTests
{
    [Theory]
    [InlineData("en-US")] // first day = Sunday
    [InlineData("fr-FR")] // first day = Monday
    public void GetStartDate_Respects_CultureFirstDayOfWeek(string cultureName)
    {
        var culture = CultureInfo.GetCultureInfo(cultureName);
        var reference = new DateTime(2025, 11, 26); // Wednesday
        var builder = new WeekSlotBuilder();

        var start = builder.GetStartDate(reference, culture);

        var expectedFirstDay = culture.DateTimeFormat.FirstDayOfWeek;
        var diff = ((int)reference.DayOfWeek - (int)expectedFirstDay + 7) % 7;
        var expected = reference.Date.AddDays(-diff);

        Assert.Equal(expected, start);
    }

    [Fact]
    public void GetEndDate_Returns_StartPlus7Days()
    {
        var culture = CultureInfo.InvariantCulture;
        var reference = new DateTime(2024, 5, 10);
        var builder = new WeekSlotBuilder();

        var start = builder.GetStartDate(reference, culture);
        var end = builder.GetEndDate(reference, culture);

        Assert.Equal(start.AddDays(7), end);
    }

    [Fact]
    public void GetSlots_Produces_Expected_Count_And_Indices_For_Subdivisions()
    {
        const int subdivisionCount = 4;
        var culture = CultureInfo.InvariantCulture;
        var reference = new DateTime(2025, 11, 26);
        var builder = new WeekSlotBuilder(subdivisionCount);

        var startDate = builder.GetStartDate(reference, culture);
        var endDate = builder.GetEndDate(reference, culture);

        var slots = builder.GetSlots(culture, startDate, endDate).ToList();

        var expectedCount = 7 * 24 * subdivisionCount;
        Assert.Equal(expectedCount, slots.Count);

        // premier créneau = day 0, hour 0, sub 0
        var first = slots.First();
        Assert.Equal(startDate, first.Start.Date);
        Assert.Equal(startDate.AddHours(0), first.Start);
        Assert.Equal(startDate.AddMinutes(60.0 / subdivisionCount), first.End);
        Assert.Equal(0, first.Row);    // hour * subdivision + sub => 0
        Assert.Equal(0, first.Column); // day index

        // dernier créneau = day 6, hour 23, sub subdivisionCount-1
        var last = slots.Last();
        var expectedLastStart = startDate.AddDays(6).AddHours(23).AddMinutes((subdivisionCount - 1) * (60.0 / subdivisionCount));
        Assert.Equal(expectedLastStart, last.Start);
        Assert.Equal(expectedLastStart.AddMinutes(60.0 / subdivisionCount), last.End);
        Assert.Equal(24 * subdivisionCount - 1, last.Row);
        Assert.Equal(6, last.Column);
    }

    [Fact]
    public void GetSlots_Throws_When_Range_IsNot_7Days()
    {
        var culture = CultureInfo.InvariantCulture;
        var builder = new WeekSlotBuilder();
        var start = new DateTime(2025, 11, 1);
        var end = start.AddDays(6); // not 7

        Assert.Throws<InvalidOperationException>(() => builder.GetSlots(culture, start, end).ToList());
    }

    [Fact]
    public void Constructor_Clamps_SubdivisionCount_To_AtLeastOne()
    {
        var culture = CultureInfo.InvariantCulture;
        var reference = new DateTime(2025, 11, 26);

        var builderZero = new WeekSlotBuilder(0);
        var start = builderZero.GetStartDate(reference, culture);
        var end = builderZero.GetEndDate(reference, culture);
        var slotsZero = builderZero.GetSlots(culture, start, end).ToList();

        // subdivisionCount 0 is clamped to 1 => count == 7*24*1
        Assert.Equal(7 * 24 * 1, slotsZero.Count);

        var builderNegative = new WeekSlotBuilder(-5);
        var slotsNeg = builderNegative.GetSlots(culture, start, end).ToList();
        Assert.Equal(7 * 24 * 1, slotsNeg.Count);
    }
}
