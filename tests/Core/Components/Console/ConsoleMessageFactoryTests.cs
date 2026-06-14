using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Console;

public class ConsoleMessageFactoryTests
{
    [Fact]
    public void Create_FullOverload_AssignsValues()
    {
        var exception = new InvalidOperationException("failure");
        var properties = new Dictionary<string, object?> { ["Key"] = 1 };
        var tags = new[] { "tag" };
        var payload = new { Name = "payload" };

        var message = ConsoleMessageFactory.Create(
            ConsoleLevel.Warning,
            "message",
            "source",
            "category",
            exception,
            properties,
            "activity",
            "correlation",
            "formatted",
            "machine",
            42,
            tags,
            payload);

        Assert.Equal(ConsoleLevel.Warning, message.Level);
        Assert.Equal("message", message.Message);
        Assert.Equal("source", message.Source);
        Assert.Equal("category", message.Category);
        Assert.Equal("activity", message.ActivityId);
        Assert.Equal("correlation", message.CorrelationId);
        Assert.Equal("formatted", message.FormattedMessage);
        Assert.Equal("machine", message.MachineName);
        Assert.Equal(42, message.ThreadId);
        Assert.Same(exception, message.Exception);
        Assert.Same(properties, message.Properties);
        Assert.Same(tags, message.Tags);
        Assert.Same(payload, message.Payload);
    }

    [Fact]
    public void Create_SimpleOverload_AssignsMessageAndException()
    {
        var exception = new InvalidOperationException("failure");

        var message = ConsoleMessageFactory.Create(ConsoleLevel.Error, "message", exception);

        Assert.Equal(ConsoleLevel.Error, message.Level);
        Assert.Equal("message", message.Message);
        Assert.Same(exception, message.Exception);
    }
}
