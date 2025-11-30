using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class CalendarExtendedTests
{
    [Theory]
    [InlineData("en-US")]
    [InlineData("fr-FR")]
    public void GetDayNames_ReturnsSevenNames_InCultureOrder(string cultureName)
    {
        var culture = new CultureInfo(cultureName);

        var names = CalendarExtended.GetDayNames(culture);

        Assert.Equal(7, names.Count);

        for (var i = 0; i < 7; i++)
        {
            var expectedIndex = ((int)culture.DateTimeFormat.FirstDayOfWeek + i) % 7;
            Assert.Equal(culture.DateTimeFormat.DayNames[expectedIndex], names[i].Name);
            Assert.Equal(culture.DateTimeFormat.AbbreviatedDayNames[expectedIndex], names[i].Shorted);
        }
    }

    [Fact]
    public void GetDaysOfWeek_FirstDayMatchesCultureAndAreConsecutive()
    {
        var culture = new CultureInfo("fr-FR");
        var reference = new DateTime(2025, 11, 15);

        var week = CalendarExtended.GetDaysOfWeek(0, reference, culture);

        Assert.Equal(7, week.Count);
        Assert.Equal(culture.DateTimeFormat.FirstDayOfWeek, week[0].DayOfWeek);

        for (var i = 1; i < 7; i++)
        {
            Assert.Equal(week[i - 1].AddDays(1), week[i]);
        }
    }

    [Fact]
    public void GetDaysOfWeek_WeekIndexShiftsBySevenDays()
    {
        var culture = CultureInfo.InvariantCulture;
        var reference = new DateTime(2025, 11, 15);

        var week0 = CalendarExtended.GetDaysOfWeek(0, reference, culture);
        var week1 = CalendarExtended.GetDaysOfWeek(1, reference, culture);

        Assert.Equal(7, week0.Count);
        Assert.Equal(7, week1.Count);
        Assert.Equal(week0[0].AddDays(7), week1[0]);
        Assert.Equal(week0[6].AddDays(7), week1[6]);
    }

    [Fact]
    public void GetDaysOfWeek_WithNullArguments_ReturnsSevenConsecutiveDays()
    {
        var days = CalendarExtended.GetDaysOfWeek(0);

        Assert.Equal(7, days.Count);

        for (var i = 1; i < 7; i++)
        {
            Assert.Equal(days[i - 1].AddDays(1), days[i]);
        }
    }
}
