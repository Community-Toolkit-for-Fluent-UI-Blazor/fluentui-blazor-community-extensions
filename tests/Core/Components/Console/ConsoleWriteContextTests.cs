using FluentUI.Blazor.Community.Components;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Console;

public class ConsoleWriteContextTests
{
    [Fact]
    public void ConsoleWriteContext_InitializesProperties()
    {
        var properties = new Dictionary<string, object?> { ["Key"] = "Value" };
        var tags = new[] { "tag" };

        var context = new ConsoleWriteContext
        {
            Source = "source",
            Category = "category",
            CorrelationId = "corr",
            ActivityId = "activity",
            Properties = properties,
            Tags = tags
        };

        Assert.Equal("source", context.Source);
        Assert.Equal("category", context.Category);
        Assert.Equal("corr", context.CorrelationId);
        Assert.Equal("activity", context.ActivityId);
        Assert.Same(properties, context.Properties);
        Assert.Same(tags, context.Tags);
    }
}
