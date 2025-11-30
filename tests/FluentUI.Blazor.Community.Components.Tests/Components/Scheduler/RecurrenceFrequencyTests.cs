using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class RecurrenceFrequencyTests
{
    [Fact]
    public void Enum_HasExpectedNumberOfValues()
    {
        var count = Enum.GetValues(typeof(RecurrenceFrequency)).Length;
        Assert.Equal(4, count);
    }

    [Fact]
    public void Enum_UnderlyingValues_AreStable()
    {
        Assert.Equal(0, (int)RecurrenceFrequency.Daily);
        Assert.Equal(1, (int)RecurrenceFrequency.Weekly);
        Assert.Equal(2, (int)RecurrenceFrequency.Monthly);
        Assert.Equal(3, (int)RecurrenceFrequency.Yearly);
    }

    [Theory]
    [InlineData("Daily", RecurrenceFrequency.Daily)]
    [InlineData("Weekly", RecurrenceFrequency.Weekly)]
    [InlineData("Monthly", RecurrenceFrequency.Monthly)]
    [InlineData("Yearly", RecurrenceFrequency.Yearly)]
    public void Can_Parse_String_To_Enum(string name, RecurrenceFrequency expected)
    {
        var parsed = (RecurrenceFrequency)Enum.Parse(typeof(RecurrenceFrequency), name);
        Assert.Equal(expected, parsed);
    }

    [Theory]
    [InlineData(RecurrenceFrequency.Daily, "Daily")]
    [InlineData(RecurrenceFrequency.Weekly, "Weekly")]
    [InlineData(RecurrenceFrequency.Monthly, "Monthly")]
    [InlineData(RecurrenceFrequency.Yearly, "Yearly")]
    public void ToString_Returns_MemberName(RecurrenceFrequency value, string expectedName)
    {
        Assert.Equal(expectedName, value.ToString());
    }
}
