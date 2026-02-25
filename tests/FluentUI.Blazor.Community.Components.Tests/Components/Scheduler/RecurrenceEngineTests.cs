using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public static class RecurrenceEngineTests
{
    private static DateTime U(int year, int month, int day, int hour = 0, int minute = 0)
        => DateTime.SpecifyKind(new DateTime(year, month, day, hour, minute, 0), DateTimeKind.Unspecified);

    [Fact]
    public static void Daily_Generates_Sequence_InRange()
    {
        var rule = new RecurrenceRule { Frequency = RecurrenceFrequency.Daily, Interval = 1 };
        var start = U(2025, 11, 1, 9, 0);
        var from = U(2025, 11, 1);
        var to = U(2025, 11, 5).AddDays(0);
        var occurrences = RecurrenceEngine.GetOccurrences(rule, start, from, to).ToList();

        var expected = new[]
        {
            U(2025,11,1,9,0),
            U(2025,11,2,9,0),
            U(2025,11,3,9,0),
            U(2025,11,4,9,0)
        };

        Assert.Equal(expected.Length, occurrences.Count);
        Assert.Equal(expected, occurrences);
    }

    [Fact]
    public static void Daily_Respects_Count_Limit()
    {
        var rule = new RecurrenceRule { Frequency = RecurrenceFrequency.Daily, Interval = 1, Count = 2 };
        var start = U(2025, 11, 1, 8, 30);
        var from = U(2025, 11, 1);
        var to = U(2025, 11, 10);
        var occurrences = RecurrenceEngine.GetOccurrences(rule, start, from, to).ToList();

        Assert.Equal(2, occurrences.Count);
        Assert.Equal(U(2025, 11, 1, 8, 30), occurrences[0]);
        Assert.Equal(U(2025, 11, 2, 8, 30), occurrences[1]);
    }

    [Fact]
    public static void Daily_Excludes_Exceptions()
    {
        var rule = new RecurrenceRule { Frequency = RecurrenceFrequency.Daily, Interval = 1 };
        var start = U(2025, 11, 1, 7, 0);
        var from = U(2025, 11, 1);
        var to = U(2025, 11, 3);
        var exceptions = new[] { U(2025, 11, 2) };

        var occurrences = RecurrenceEngine.GetOccurrences(rule, start, from, to, exceptions).ToList();

        var expected = new[] { U(2025, 11, 1, 7, 0)};
        Assert.Equal(expected, occurrences);
    }

    [Fact]
    public static void WeeklySingleDay_Generates_On_Specified_Weekday()
    {
        // start on Monday 2025-11-24 09:00
        var start = U(2025, 11, 24, 9, 0);
        var rule = new RecurrenceRule { Frequency = RecurrenceFrequency.Weekly, Interval = 1 };
        var from = U(2025, 11, 24);
        var to = U(2025, 12, 8);

        var occurrences = RecurrenceEngine.GetOccurrences(rule, start, from, to).ToList();

        var expected = new[] { U(2025, 11, 24, 9, 0), U(2025, 12, 1, 9, 0)};

        Assert.Equal(expected, occurrences);
    }

    [Fact]
    public static void WeeklyMultipleDays_Generates_For_Each_Day()
    {
        // start provides the time component
        var start = U(2025, 11, 24, 10, 30); // Monday 10:30
        var rule = new RecurrenceRule
        {
            Frequency = RecurrenceFrequency.Weekly,
            Interval = 1,
            DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Wednesday }
        };

        var from = U(2025, 11, 24);
        var to = U(2025, 12, 3);

        var occurrences = RecurrenceEngine.GetOccurrences(rule, start, from, to).ToList();

        // Expect occurrences on Mon 24, Wed 26, Mon 1 Dec, Wed 3 Dec at 10:30
        var expected = new[]
        {
            U(2025,11,24,10,30),
            U(2025,11,26,10,30),
            U(2025,12,1,10,30)
        };

        Assert.Equal(expected, occurrences);
    }

    [Fact]
    public static void Monthly_Adjusts_Day_That_Doesnt_Exist_To_LastDay()
    {
        // Start on Jan 31, occurrences should fall to Feb 28 (non-leap 2025) then Mar 31
        var start = U(2025, 1, 31, 14, 0);
        var rule = new RecurrenceRule { Frequency = RecurrenceFrequency.Monthly, Interval = 1, DayOfMonth = 31 };

        var from = U(2025, 1, 1);
        var to = U(2025, 3, 31);

        var occurrences = RecurrenceEngine.GetOccurrences(rule, start, from, to).ToList();

        var expected = new[]
        {
            U(2025,1,31,14,0),
            U(2025,2,28,14,0)
        };

        Assert.Equal(expected, occurrences);
    }

    [Fact]
    public static void Yearly_Uses_Months_List()
    {
        var start = U(2020, 3, 15, 8, 0);
        var rule = new RecurrenceRule
        {
            Frequency = RecurrenceFrequency.Yearly,
            Interval = 1,
            Months = new List<int> { 2, 5 } // Feb and May
        };

        var from = U(2021, 1, 1);
        var to = U(2023, 12, 31);

        var occurrences = RecurrenceEngine.GetOccurrences(rule, start, from, to).ToList();

        // Expect for years 2021..2023 Feb 15 and May 15 (if valid day)
        var expected = new List<DateTime>
        {
            U(2021,2,15,8,0),
            U(2021,5,15,8,0),
            U(2022,2,15,8,0),
            U(2022,5,15,8,0),
            U(2023,2,15,8,0),
            U(2023,5,15,8,0)
        };

        Assert.Equal(expected, occurrences);
    }

    [Fact]
    public static void Until_Limits_Generated_Occurrences()
    {
        var rule = new RecurrenceRule { Frequency = RecurrenceFrequency.Daily, Interval = 1, Until = U(2025, 11, 3) };
        var start = U(2025, 11, 1, 12, 0);
        var from = U(2025, 11, 1);
        var to = U(2025, 11, 10);

        var occurrences = RecurrenceEngine.GetOccurrences(rule, start, from, to).ToList();

        var expected = new[] { U(2025, 11, 1, 12, 0), U(2025, 11, 2, 12, 0) };
        Assert.Equal(expected, occurrences);
    }
}
