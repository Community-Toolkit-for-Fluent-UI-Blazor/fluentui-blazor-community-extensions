using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class YearSlotBuilderTests
{
    [Fact]
    public void GetStartDate_Returns_Jan1_Of_ReferenceYear()
    {
        var builder = new YearSlotBuilder();
        var reference = new DateTime(2025, 11, 24);
        var start = builder.GetStartDate(reference, CultureInfo.InvariantCulture);

        Assert.Equal(new DateTime(2025, 1, 1), start);
    }

    [Fact]
    public void GetEndDate_Returns_Jan1_Of_NextYear()
    {
        var builder = new YearSlotBuilder();
        var reference = new DateTime(2023, 6, 5);
        var end = builder.GetEndDate(reference, CultureInfo.InvariantCulture);

        Assert.Equal(new DateTime(2024, 1, 1), end);
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("fr-FR")]
    public void GetSlots_Produces_12_Months_With_CorrectLabels_And_Ranges(string cultureName)
    {
        var culture = CultureInfo.GetCultureInfo(cultureName);
        var builder = new YearSlotBuilder();
        var reference = new DateTime(2022, 8, 15);

        var startDate = builder.GetStartDate(reference, culture);
        var endDate = builder.GetEndDate(reference, culture);

        var slots = builder.GetSlots(culture, startDate, endDate).ToList();

        Assert.Equal(12, slots.Count);

        for (var i = 0; i < 12; i++)
        {
            var expectedMonth = i + 1;
            var expectedStart = new DateTime(reference.Year, expectedMonth, 1);
            var expectedEnd = expectedStart.AddMonths(1);
            var expectedLabel = expectedStart.ToString("MMMM", culture);

            var slot = slots[i];

            Assert.Equal(expectedLabel, slot.Label);
            Assert.Equal(expectedStart, slot.Start);
            Assert.Equal(expectedEnd, slot.End);
        }
    }
}
