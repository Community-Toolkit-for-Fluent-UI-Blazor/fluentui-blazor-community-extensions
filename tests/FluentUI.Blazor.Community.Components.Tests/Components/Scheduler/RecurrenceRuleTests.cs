using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class RecurrenceRuleTests
{
    [Fact]
    public void DefaultConstructor_SetsExpectedDefaults()
    {
        var rule = new RecurrenceRule();

        // Frequency default (enum) should be Daily (first enum value -> 0)
        Assert.Equal(RecurrenceFrequency.Daily, rule.Frequency);

        // Defaults
        Assert.Equal(1, rule.Interval);
        Assert.Null(rule.Until);
        Assert.Null(rule.Count);
        Assert.Null(rule.DayOfMonth);

        // Collections must be non-null and empty by default
        Assert.NotNull(rule.DaysOfWeek);
        Assert.Empty(rule.DaysOfWeek);
        Assert.NotNull(rule.Months);
        Assert.Empty(rule.Months);
    }

    [Fact]
    public void Properties_CanBeSet_AndPersistValues()
    {
        var until = new DateTime(2026, 1, 1);
        var days = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Wednesday };
        var months = new List<int> { 2, 12 };

        var rule = new RecurrenceRule
        {
            Frequency = RecurrenceFrequency.Weekly,
            Interval = 2,
            Until = until,
            Count = 10,
            DaysOfWeek = days,
            DayOfMonth = 15,
            Months = months
        };

        Assert.Equal(RecurrenceFrequency.Weekly, rule.Frequency);
        Assert.Equal(2, rule.Interval);
        Assert.Equal(until, rule.Until);
        Assert.Equal(10, rule.Count);
        Assert.Same(days, rule.DaysOfWeek);
        Assert.Equal(15, rule.DayOfMonth);
        Assert.Same(months, rule.Months);
    }
}
