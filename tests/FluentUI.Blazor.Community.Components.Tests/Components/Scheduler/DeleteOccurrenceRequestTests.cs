using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class DeleteOccurrenceRequestTests
{
    [Fact]
    public void Properties_Are_Assigned_From_Constructor()
    {
        var id = 123L;
        var date = new DateTime(2025, 11, 30);

        var req = new DeleteOccurrenceRequest(id, date);

        Assert.Equal(id, req.Id);
        Assert.Equal(date, req.OccurrenceDate);
    }

    [Fact]
    public void Two_Requests_With_Same_Values_Are_Equal()
    {
        var date = new DateTime(2025, 11, 30);
        var a = new DeleteOccurrenceRequest(1, date);
        var b = new DeleteOccurrenceRequest(1, date);

        Assert.Equal(a, b);
        Assert.True(a == b);
        Assert.True(a.Equals(b));
    }

    [Fact]
    public void HashCodes_Are_Equal_For_Equal_Requests()
    {
        var date = new DateTime(2025, 11, 30);
        var a = new DeleteOccurrenceRequest(7, date);
        var b = new DeleteOccurrenceRequest(7, date);

        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void Deconstruct_Works_As_Expected()
    {
        var id = 55L;
        var date = new DateTime(2025, 12, 1);
        var req = new DeleteOccurrenceRequest(id, date);

        var (deId, deDate) = req;

        Assert.Equal(id, deId);
        Assert.Equal(date, deDate);
    }

    [Fact]
    public void ToString_Contains_PropertyNames_And_Values()
    {
        var id = 9L;
        var date = new DateTime(2025, 1, 2, 3, 4, 5);
        var req = new DeleteOccurrenceRequest(id, date);

        var s = req.ToString();

        Assert.Contains("Id", s);
        Assert.Contains("OccurrenceDate", s);
        Assert.Contains(id.ToString(), s);
        Assert.Contains(date.ToString(), s);
    }
}
