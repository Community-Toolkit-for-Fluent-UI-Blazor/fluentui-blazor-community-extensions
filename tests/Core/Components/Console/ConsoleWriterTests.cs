using FluentUI.Blazor.Community.Components;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Console;

public class ConsoleWriterTests
{
    [Fact]
    public async Task WriteAsync_MergesContextAndEnrichers()
    {
        var state = new ConsoleState(new ConsoleOptions { EnableThreadSafety = false });
        var enricher = new TestEnricher();
        var writer = new ConsoleWriter(state, [enricher]);
        var context = new ConsoleWriteContext
        {
            Source = "source",
            Category = "category",
            Properties = new Dictionary<string, object?> { ["Key"] = "Value" },
            Tags = new[] { "tag" }
        };

        await writer.WriteAsync(ConsoleLevel.Information, "Message", null, context);

        var message = Assert.Single(state.Messages);
        Assert.Equal("source", message.Source);
        Assert.Equal("category", message.Category);
        Assert.Equal("Value", message.Properties?["Key"]);
        Assert.Equal("Enriched", message.Properties?["Enricher"]);
        Assert.Contains("tag", message.Tags ?? []);
        Assert.Contains("enriched-tag", message.Tags ?? []);
    }

    [Fact]
    public async Task BeginScope_MergesScopeContext()
    {
        var state = new ConsoleState(new ConsoleOptions { EnableThreadSafety = false });
        var writer = new ConsoleWriter(state, []);
        var scopeContext = new ConsoleWriteContext
        {
            Properties = new Dictionary<string, object?> { ["Scope"] = "Value" },
            Tags = new[] { "scope-tag" }
        };

        using (writer.BeginScope(scopeContext))
        {
            await writer.WriteAsync(ConsoleLevel.Information, "Message", null, new ConsoleWriteContext
            {
                Properties = new Dictionary<string, object?> { ["Call"] = "Value" },
                Tags = new[] { "call-tag" }
            });
        }

        var message = Assert.Single(state.Messages);
        Assert.Equal("Value", message.Properties?["Scope"]);
        Assert.Equal("Value", message.Properties?["Call"]);
        Assert.Contains("scope-tag", message.Tags ?? []);
        Assert.Contains("call-tag", message.Tags ?? []);
    }

    private sealed class TestEnricher : IConsoleEnricher
    {
        public void Enrich(ConsoleEnrichmentBag bag)
        {
            bag.Properties["Enricher"] = "Enriched";
            bag.Tags.Add("enriched-tag");
        }
    }
}
