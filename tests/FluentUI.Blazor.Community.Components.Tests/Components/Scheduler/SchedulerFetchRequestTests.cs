using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class SchedulerFetchRequestTests
{
    [Fact]
    public void Constructor_Assigns_Properties_With_Default_CancellationToken()
    {
        var start = new DateTime(2025, 11, 1);
        var end = new DateTime(2025, 11, 30);

        var req = new SchedulerFetchRequest(SchedulerView.Month, start, end);

        Assert.Equal(SchedulerView.Month, req.View);
        Assert.Equal(start, req.StartDate);
        Assert.Equal(end, req.EndDate);
        Assert.Equal(default(CancellationToken), req.cancellationToken);
    }

    [Fact]
    public void Constructor_Assigns_Provided_CancellationToken_And_Records_Are_Equal_When_Same_Token()
    {
        var start = new DateTime(2025, 6, 1);
        var end = new DateTime(2025, 6, 30);
        using var cts = new CancellationTokenSource();
        var token = cts.Token;

        var a = new SchedulerFetchRequest(SchedulerView.Week, start, end, token);
        var b = new SchedulerFetchRequest(SchedulerView.Week, start, end, token);

        Assert.Equal(a, b);
        Assert.True(a == b);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void Requests_With_Different_CancellationTokens_Are_NotEqual()
    {
        var start = new DateTime(2025, 7, 1);
        var end = new DateTime(2025, 7, 31);
        using var cts1 = new CancellationTokenSource();
        using var cts2 = new CancellationTokenSource();

        var a = new SchedulerFetchRequest(SchedulerView.Day, start, end, cts1.Token);
        var b = new SchedulerFetchRequest(SchedulerView.Day, start, end, cts2.Token);

        Assert.NotEqual(a, b);
        Assert.False(a == b);
    }

    [Fact]
    public void Deconstruct_Provides_All_Values()
    {
        var start = new DateTime(2025, 8, 1);
        var end = new DateTime(2025, 8, 31);
        using var cts = new CancellationTokenSource();
        var token = cts.Token;

        var req = new SchedulerFetchRequest(SchedulerView.Agenda, start, end, token);

        var (view, s, e, ct) = req;

        Assert.Equal(SchedulerView.Agenda, view);
        Assert.Equal(start, s);
        Assert.Equal(end, e);
        Assert.Equal(token, ct);
    }

    [Fact]
    public void ToString_Includes_PropertyNames_And_Values()
    {
        var start = new DateTime(2025, 9, 1, 10, 0, 0);
        var end = new DateTime(2025, 9, 1, 18, 0, 0);
        var req = new SchedulerFetchRequest(SchedulerView.Timeline, start, end);

        var s = req.ToString();

        Assert.Contains("View", s);
        Assert.Contains("StartDate", s);
        Assert.Contains("EndDate", s);
        Assert.Contains(SchedulerView.Timeline.ToString(), s);
        Assert.Contains(start.ToString(), s);
        Assert.Contains(end.ToString(), s);
    }
}
