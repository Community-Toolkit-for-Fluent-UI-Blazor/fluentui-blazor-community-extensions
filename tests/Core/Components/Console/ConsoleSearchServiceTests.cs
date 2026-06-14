using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Console;

public class ConsoleSearchServiceTests
{
    [Fact]
    public async Task SearchAsync_FindsMatchingMessages()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        var state = new ConsoleState(new ConsoleOptions { EnableThreadSafety = false });
        var service = new ConsoleSearchService(state);

        await state.AddAsync(new ConsoleMessage
        {
            Level = ConsoleLevel.Information,
            Message = "Hello world",
            Category = "Test"
        });

        var results = await service.SearchAsync("hello", cancellationToken: cancellationToken);

        Assert.Single(results);
        Assert.Equal("Hello world", results[0].Message);
    }

    [Fact]
    public async Task SearchAsync_RespectsExcludesAndPhrases()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        var state = new ConsoleState(new ConsoleOptions { EnableThreadSafety = false });
        var service = new ConsoleSearchService(state);

        await state.AddAsync(new ConsoleMessage
        {
            Level = ConsoleLevel.Information,
            Message = "Hello world"
        });

        var excluded = await service.SearchAsync("hello -world", cancellationToken: cancellationToken);
        var phrase = await service.SearchAsync("\"hello world\"", cancellationToken: cancellationToken);

        Assert.Empty(excluded);
        Assert.Single(phrase);
    }

    [Fact]
    public async Task SearchAsync_Top_LimitsResults()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        var state = new ConsoleState(new ConsoleOptions { EnableThreadSafety = false });
        var service = new ConsoleSearchService(state);

        await state.AddRangeAsync([
            new ConsoleMessage { Level = ConsoleLevel.Information, Message = "Alpha" },
            new ConsoleMessage { Level = ConsoleLevel.Information, Message = "Alpha Beta" }
        ]);

        var results = await service.SearchAsync("alpha", new ConsoleSearchOptions { Top = 1 }, cancellationToken);

        Assert.Single(results);
    }
}
