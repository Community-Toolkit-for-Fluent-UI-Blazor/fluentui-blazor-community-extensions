using System.Threading.Tasks;
using FluentUI.Blazor.Community.Components.TrailMenu;
using Xunit;

namespace Components.Tests.Components.TrailMenu;

public class TrailMenuMutationHandlerTests
{
    [Fact]
    public async Task HandleResizeAsync_InvokesRefresh()
    {
        var handler = new TrailMenuMutationHandler();
        var called = false;

        await handler.HandleResizeAsync(() =>
        {
            called = true;
            return Task.CompletedTask;
        });

        Assert.True(called);
    }

    [Fact]
    public async Task HandleResizeAsync_CancelsPreviousRefresh()
    {
        var handler = new TrailMenuMutationHandler();
        var refreshCalls = 0;
        var completion = new TaskCompletionSource<bool>();

        var first = handler.HandleResizeAsync(() =>
        {
            refreshCalls++;
            return Task.CompletedTask;
        });

        var second = handler.HandleResizeAsync(() =>
        {
            refreshCalls++;
            completion.SetResult(true);
            return Task.CompletedTask;
        });

        await Task.WhenAll(first, second);
        await Task.WhenAny(completion.Task, Task.Delay(200, TestContext.Current.CancellationToken));

        Assert.True(completion.Task.IsCompleted);
        Assert.Equal(1, refreshCalls);
    }
}
