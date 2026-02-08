using FluentUI.Blazor.Community.Components;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Console;

public class ConsoleStateTests
{
    [Fact]
    public async Task AddAsync_AddsMessagesAndNotifiesObservers()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        var state = new ConsoleState(new ConsoleOptions { EnableThreadSafety = false });
        var observer = new TestObserver();
        state.RegisterObserver(observer);

        await state.AddAsync(new ConsoleMessage { Level = ConsoleLevel.Information, Message = "Hello" });

        Assert.Single(state.Messages);
        Assert.Single(state.FilteredMessages);
        Assert.Single(observer.AddedMessages);
        Assert.Equal("Hello", observer.AddedMessages[0].Message);
        Assert.Equal(ConsoleChangeKind.MessageAdded, observer.LastChange);

        await state.ClearAsync();

        Assert.Empty(state.Messages);
        Assert.Empty(state.FilteredMessages);
        Assert.Equal(ConsoleChangeKind.Cleared, observer.LastChange);
        cancellationToken.ThrowIfCancellationRequested();
    }

    [Fact]
    public async Task SetFilterAsync_UpdatesFilteredMessages()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        var state = new ConsoleState(new ConsoleOptions { EnableThreadSafety = false });

        await state.AddRangeAsync([
            new ConsoleMessage { Level = ConsoleLevel.Information, Message = "Alpha", Category = "A" },
            new ConsoleMessage { Level = ConsoleLevel.Warning, Message = "Beta", Category = "B" }
        ]);

        await state.SetFilterAsync(new ConsoleFilter
        {
            Levels = [ConsoleLevel.Warning],
            Category = "B"
        });

        Assert.Single(state.FilteredMessages);
        Assert.Equal("Beta", state.FilteredMessages[0].Message);
        cancellationToken.ThrowIfCancellationRequested();
    }

    [Fact]
    public async Task SuspendNotifications_RaisesBatchCompletedOnDispose()
    {
        var change = ConsoleChangeKind.Cleared;
        var state = new ConsoleState(new ConsoleOptions { EnableThreadSafety = false });
        state.Changed += State_Changed;

        using (state.SuspendNotifications())
        {
            await state.AddAsync(new ConsoleMessage { Level = ConsoleLevel.Information, Message = "Hello" });
        }

        Assert.Equal(ConsoleChangeKind.BatchCompleted, change);

        void State_Changed(object? sender, ConsoleStateChangedEventArgs e)
        {
            change = e.Kind;
        }
    }

    private sealed class TestObserver : IConsoleStateObserver
    {
        public List<ConsoleMessage> AddedMessages { get; } = [];

        public ConsoleChangeKind? LastChange { get; private set; }

        public void OnCleared() => LastChange = ConsoleChangeKind.Cleared;

        public void OnMessagesAdded(IReadOnlyList<ConsoleMessage> messages)
        {
            AddedMessages.AddRange(messages);
            LastChange = ConsoleChangeKind.MessageAdded;
        }
    }
}
