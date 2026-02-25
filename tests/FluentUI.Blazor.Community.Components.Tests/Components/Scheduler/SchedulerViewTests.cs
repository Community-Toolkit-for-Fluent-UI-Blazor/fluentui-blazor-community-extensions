using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class SchedulerViewTests
{
    [Fact]
    public void Enum_HasExpectedNumberOfValues()
    {
        var count = Enum.GetValues(typeof(SchedulerView)).Length;
        Assert.Equal(6, count);
    }

    [Fact]
    public void Enum_UnderlyingValues_AreStable()
    {
        Assert.Equal(0, (int)SchedulerView.Day);
        Assert.Equal(1, (int)SchedulerView.Week);
        Assert.Equal(2, (int)SchedulerView.Month);
        Assert.Equal(3, (int)SchedulerView.Year);
        Assert.Equal(4, (int)SchedulerView.Timeline);
        Assert.Equal(5, (int)SchedulerView.Agenda);
    }

    [Theory]
    [InlineData("Day", SchedulerView.Day)]
    [InlineData("Week", SchedulerView.Week)]
    [InlineData("Month", SchedulerView.Month)]
    [InlineData("Year", SchedulerView.Year)]
    [InlineData("Timeline", SchedulerView.Timeline)]
    [InlineData("Agenda", SchedulerView.Agenda)]
    public void Can_Parse_String_To_Enum(string name, SchedulerView expected)
    {
        var parsed = (SchedulerView)Enum.Parse(typeof(SchedulerView), name);
        Assert.Equal(expected, parsed);
    }

    [Theory]
    [InlineData(SchedulerView.Day, "Day")]
    [InlineData(SchedulerView.Week, "Week")]
    [InlineData(SchedulerView.Month, "Month")]
    [InlineData(SchedulerView.Year, "Year")]
    [InlineData(SchedulerView.Timeline, "Timeline")]
    [InlineData(SchedulerView.Agenda, "Agenda")]
    public void ToString_Returns_MemberName(SchedulerView value, string expectedName)
    {
        Assert.Equal(expectedName, value.ToString());
    }
}
