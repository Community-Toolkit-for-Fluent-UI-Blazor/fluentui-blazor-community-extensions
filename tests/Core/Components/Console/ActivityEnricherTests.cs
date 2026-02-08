using System.Diagnostics;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Console;

public class ActivityEnricherTests
{
    [Fact]
    public void Enrich_NoActivity_DoesNotModifyBag()
    {
        var bag = new ConsoleEnrichmentBag();
        var enricher = new ActivityEnricher();

        enricher.Enrich(bag);

        Assert.Null(bag.ActivityId);
        Assert.Null(bag.CorrelationId);
        Assert.Empty(bag.Properties);
    }

    [Fact]
    public void Enrich_WithActivity_SetsProperties()
    {
        using var activity = new Activity("test-activity");
        activity.Start();

        var bag = new ConsoleEnrichmentBag();
        var enricher = new ActivityEnricher();

        enricher.Enrich(bag);

        Assert.Equal(activity.Id, bag.ActivityId);
        Assert.Equal(activity.TraceId.ToString(), bag.CorrelationId);
        Assert.Equal(activity.DisplayName, bag.Properties["Activity.Name"]);

        activity.Stop();
    }
}
