using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class SchedulerItemTests
{
    [Fact]
    public void DefaultConstructor_Sets_Expected_Defaults()
    {
        var item = new SchedulerItem<object>();

        Assert.Equal(0, item.Id);
        Assert.Equal(string.Empty, item.Title);
        Assert.Null(item.Data);
        Assert.Equal(default(DateTime), item.Start);
        Assert.Equal(default(DateTime), item.End);
        Assert.Null(item.Recurrence);
        Assert.NotNull(item.Exceptions);
        Assert.Empty(item.Exceptions);
        Assert.Null(item.Description);
    }

    [Fact]
    public void Properties_CanBe_Set_And_Read_Back()
    {
        var recurrence = new RecurrenceRule { Frequency = RecurrenceFrequency.Daily, Interval = 2 };
        var exceptions = new List<DateTime> { new DateTime(2025, 12, 25) };

        var item = new SchedulerItem<string>
        {
            Id = 42,
            Title = "Meeting",
            Data = "payload",
            Start = new DateTime(2025, 11, 30, 9, 0, 0),
            End = new DateTime(2025, 11, 30, 10, 0, 0),
            Recurrence = recurrence,
            Exceptions = exceptions,
            Description = "Team sync"
        };

        Assert.Equal(42, item.Id);
        Assert.Equal("Meeting", item.Title);
        Assert.Equal("payload", item.Data);
        Assert.Equal(new DateTime(2025, 11, 30, 9, 0, 0), item.Start);
        Assert.Equal(new DateTime(2025, 11, 30, 10, 0, 0), item.End);
        Assert.Same(recurrence, item.Recurrence);
        Assert.Same(exceptions, item.Exceptions);
        Assert.Equal("Team sync", item.Description);
    }

    [Fact]
    public void Exceptions_List_Is_Mutable()
    {
        var item = new SchedulerItem<object>();
        var exDate = new DateTime(2026, 1, 1);

        item.Exceptions.Add(exDate);

        Assert.Single(item.Exceptions);
        Assert.Contains(exDate, item.Exceptions);
    }

    [Fact]
    public void Generic_Data_Type_Is_Preserved()
    {
        var item = new SchedulerItem<int>
        {
            Data = 123
        };

        Assert.Equal(123, item.Data);
    }

    [Fact]
    public void Start_And_End_Are_Independent_And_Persisted()
    {
        var item = new SchedulerItem<object>
        {
            Start = new DateTime(2025, 5, 1, 14, 0, 0),
            End = new DateTime(2025, 5, 1, 13, 0, 0) // end earlier than start is allowed by the model
        };

        Assert.Equal(new DateTime(2025, 5, 1, 14, 0, 0), item.Start);
        Assert.Equal(new DateTime(2025, 5, 1, 13, 0, 0), item.End);
    }
}
