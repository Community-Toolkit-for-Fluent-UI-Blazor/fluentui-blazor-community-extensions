using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class SchedularCalendarEventArgsTests
{
    [Fact]
    public void Ctor_WithValues_SetsProperties()
    {
        var id = "event-1";
        var date = new DateTime(2025, 11, 29, 12, 0, 0, DateTimeKind.Utc);

        var args = new SchedularCalendarEventArgs(id, date);

        Assert.Equal(id, args.Id);
        Assert.Equal(date, args.Date);
    }

    [Fact]
    public void Equality_WithSameValues_AreEqual()
    {
        var id = "event-2";
        var date = DateTime.UtcNow;

        var a = new SchedularCalendarEventArgs(id, date);
        var b = new SchedularCalendarEventArgs(id, date);

        Assert.Equal(a, b);
        Assert.True(a == b);
    }

    [Fact]
    public void Deconstruct_ProducesExpectedValues()
    {
        var id = "event-3";
        var date = DateTime.UtcNow;

        var args = new SchedularCalendarEventArgs(id, date);

        var (deconstructedId, deconstructedDate) = args;

        Assert.Equal(id, deconstructedId);
        Assert.Equal(date, deconstructedDate);
    }
}
