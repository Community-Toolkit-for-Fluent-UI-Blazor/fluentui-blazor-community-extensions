using FluentUI.Blazor.Community.Components;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Console;

public class EnvironmentEnricherTests
{
    [Fact]
    public void Enrich_AddsEnvironmentProperties()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Application:Name"] = "test-app"
            })
            .Build();

        var enricher = new EnvironmentEnricher(configuration);
        var bag = new ConsoleEnrichmentBag();

        enricher.Enrich(bag);

        Assert.Equal("test-app", bag.Properties["Process.Name"]);
        Assert.NotNull(bag.Properties["Application.Name"]);
        Assert.NotNull(bag.Properties["Application.Version"]);
        Assert.NotNull(bag.Properties["Environment.MachineName"]);
        Assert.NotNull(bag.Properties["Environment.Framework"]);
    }
}
