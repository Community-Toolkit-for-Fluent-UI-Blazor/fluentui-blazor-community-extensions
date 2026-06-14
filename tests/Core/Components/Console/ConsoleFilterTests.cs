using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Console;

public class ConsoleFilterTests
{
    [Fact]
    public void Match_ReturnsFalse_WhenLevelNotIncluded()
    {
        var filter = new ConsoleFilter
        {
            Levels = [ConsoleLevel.Error]
        };

        var message = new ConsoleMessage { Level = ConsoleLevel.Information, Message = "Info" };

        Assert.False(filter.Match(message));
    }

    [Fact]
    public void Match_RespectsTextCategoryAndExceptionFilters()
    {
        var filter = new ConsoleFilter
        {
            TextContains = "Hello",
            Category = "Test",
            IncludeExceptionsOnly = true
        };

        var match = new ConsoleMessage
        {
            Level = ConsoleLevel.Warning,
            Message = "Hello world",
            Category = "Test",
            Exception = new InvalidOperationException("boom")
        };

        var noMatch = new ConsoleMessage
        {
            Level = ConsoleLevel.Warning,
            Message = "Hello world",
            Category = "Test"
        };

        Assert.True(filter.Match(match));
        Assert.False(filter.Match(noMatch));
    }
}
