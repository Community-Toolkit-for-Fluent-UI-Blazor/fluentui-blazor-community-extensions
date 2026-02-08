using FluentUI.Blazor.Community.Components;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Console;

public class ConsoleEnrichmentBagTests
{
    [Fact]
    public void MergeProperties_MergesAndOverrides()
    {
        var bag = new ConsoleEnrichmentBag();
        bag.Properties["Key"] = "Original";

        bag.MergeProperties(new Dictionary<string, object?> { ["Key"] = "New", ["Other"] = 1 });

        Assert.Equal("New", bag.Properties["Key"]);
        Assert.Equal(1, bag.Properties["Other"]);
    }

    [Fact]
    public void MergeTags_AddsNonEmptyTags()
    {
        var bag = new ConsoleEnrichmentBag();

        bag.MergeTags(["tag", " ", "other"]);

        Assert.Contains("tag", bag.Tags);
        Assert.Contains("other", bag.Tags);
        Assert.DoesNotContain(" ", bag.Tags);
    }
}
