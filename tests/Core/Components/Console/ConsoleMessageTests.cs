using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Console;

public class ConsoleMessageTests
{
    [Fact]
    public void ConsoleMessage_Defaults_AreInitialized()
    {
        var message = new ConsoleMessage { Level = ConsoleLevel.Information };

        Assert.NotEqual(Guid.Empty, message.Id);
        Assert.NotEqual(default, message.Timestamp);
        Assert.Equal(ConsoleLevel.Information, message.Level);
    }

    [Fact]
    public void ConsoleMessage_InitProperties_AreAssigned()
    {
        var exception = new InvalidOperationException("boom");
        var properties = new Dictionary<string, object?> { ["Key"] = "Value" };
        var tags = new[] { "tag" };

        var message = new ConsoleMessage
        {
            Level = ConsoleLevel.Error,
            Message = "message",
            FormattedMessage = "formatted",
            Source = "source",
            Category = "category",
            Exception = exception,
            Properties = properties,
            CorrelationId = "corr",
            ActivityId = "activity",
            ThreadId = 1,
            MachineName = "machine",
            Tags = tags,
            Payload = new { Name = "payload" }
        };

        Assert.Equal(ConsoleLevel.Error, message.Level);
        Assert.Equal("message", message.Message);
        Assert.Equal("formatted", message.FormattedMessage);
        Assert.Equal("source", message.Source);
        Assert.Equal("category", message.Category);
        Assert.Same(exception, message.Exception);
        Assert.Same(properties, message.Properties);
        Assert.Equal("corr", message.CorrelationId);
        Assert.Equal("activity", message.ActivityId);
        Assert.Equal(1, message.ThreadId);
        Assert.Equal("machine", message.MachineName);
        Assert.Same(tags, message.Tags);
        Assert.NotNull(message.Payload);
    }
}
