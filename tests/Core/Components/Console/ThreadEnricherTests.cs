using FluentUI.Blazor.Community.Components;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Console;

public class ThreadEnricherTests
{
    [Fact]
    public void Enrich_AddsThreadProperties()
    {
        var bag = new ConsoleEnrichmentBag();
        var enricher = new ThreadEnricher();

        enricher.Enrich(bag);

        Assert.Equal(Environment.CurrentManagedThreadId, bag.Properties["Thread.Id"]);
        Assert.NotNull(bag.Properties["Thread.State"]);
        Assert.NotNull(bag.Properties["Thread.Priority"]);
    }
}
