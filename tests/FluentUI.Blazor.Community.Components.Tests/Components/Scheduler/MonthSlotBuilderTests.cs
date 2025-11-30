using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class MonthSlotBuilderTests
{
    [Theory]
    [InlineData("en-US")] // FirstDayOfWeek = Sunday
    [InlineData("fr-FR")] // FirstDayOfWeek = Monday
    public void GetStartDate_Respects_CurrentCultureFirstDayOfWeek(string cultureName)
    {
        var culture = CultureInfo.GetCultureInfo(cultureName);
        var original = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = culture;

            var builder = new MonthSlotBuilder();
            var reference = new DateTime(2025, 11, 15); // november 2025
            var start = builder.GetStartDate(reference, culture);

            var firstDayOfMonth = new DateTime(reference.Year, reference.Month, 1);
            var firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            var diff = ((int)firstDayOfMonth.DayOfWeek - (int)firstDayOfWeek + 7) % 7;
            var expected = firstDayOfMonth.AddDays(-diff);

            Assert.Equal(expected, start);
        }
        finally
        {
            CultureInfo.CurrentCulture = original;
        }
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("fr-FR")]
    public void GetEndDate_Produces_ExclusiveEndAlignedToWeek(string cultureName)
    {
        var culture = CultureInfo.GetCultureInfo(cultureName);
        var original = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = culture;

            var builder = new MonthSlotBuilder();
            var reference = new DateTime(2023, 2, 10); // February 2023 (non-leap)
            var end = builder.GetEndDate(reference, culture);

            var firstDayOfMonth = new DateTime(reference.Year, reference.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            var diff = ((int)lastDayOfMonth.DayOfWeek - (int)firstDayOfWeek + 7) % 7;
            var expected = lastDayOfMonth.AddDays(7 - diff).AddDays(1);

            Assert.Equal(expected, end);
        }
        finally
        {
            CultureInfo.CurrentCulture = original;
        }
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("fr-FR")]
    public void GetSlots_Generates_CompleteCalendarGrid_WithCorrectLabelsAndIndices(string cultureName)
    {
        var culture = CultureInfo.GetCultureInfo(cultureName);
        var original = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = culture;

            var builder = new MonthSlotBuilder();
            var reference = new DateTime(2025, 4, 10); // April 2025
            var start = builder.GetStartDate(reference, culture);
            var end = builder.GetEndDate(reference, culture);

            var slots = builder.GetSlots(culture, start, end).ToList();
            var totalDays = (end - start).Days;

            Assert.Equal(totalDays, slots.Count);

            var middle = start.AddDays(totalDays / 2);
            var displayedYear = middle.Year;
            var displayedMonth = middle.Month;

            for (var i = 0; i < totalDays; i++)
            {
                var expectedDate = start.AddDays(i);
                var slot = slots[i];

                Assert.Equal(expectedDate, slot.Start);
                Assert.Equal(expectedDate.AddDays(1), slot.End);
                Assert.Equal(expectedDate.ToString("dd", culture), slot.Label);

                var expectedCol = i % 7;
                var expectedRow = i / 7;
                Assert.Equal(expectedCol, slot.Column);
                Assert.Equal(expectedRow, slot.Row);

                var isOutside = expectedDate.Year != displayedYear || expectedDate.Month != displayedMonth;
                Assert.Equal(isOutside, slot.Disabled);
            }
        }
        finally
        {
            CultureInfo.CurrentCulture = original;
        }
    }

    [Fact]
    public void GetSlots_WeeksAndRows_AreConsistent_For_February_OnLeapYearAndNonLeap()
    {
        var culture = CultureInfo.GetCultureInfo("en-US");
        var original = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = culture;
            var builder = new MonthSlotBuilder();

            // Non-leap February 2025 (28 days)
            var refNonLeap = new DateTime(2025, 2, 10);
            var startNonLeap = builder.GetStartDate(refNonLeap, culture);
            var endNonLeap = builder.GetEndDate(refNonLeap, culture);
            var slotsNonLeap = builder.GetSlots(culture, startNonLeap, endNonLeap).ToList();
            Assert.Equal((endNonLeap - startNonLeap).Days, slotsNonLeap.Count);

            // Leap February 2024 (29 days)
            var refLeap = new DateTime(2024, 2, 10);
            var startLeap = builder.GetStartDate(refLeap, culture);
            var endLeap = builder.GetEndDate(refLeap, culture);
            var slotsLeap = builder.GetSlots(culture, startLeap, endLeap).ToList();
            Assert.Equal((endLeap - startLeap).Days, slotsLeap.Count);
        }
        finally
        {
            CultureInfo.CurrentCulture = original;
        }
    }
}
