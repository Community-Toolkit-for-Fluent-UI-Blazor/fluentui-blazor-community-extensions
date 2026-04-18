using Components.Tests.Components.Charts.TestHelpers;
using FluentUI.Blazor.Community.Components;
using ChartOptions = FluentUI.Blazor.Community.Components.Charts.Options.ChartOptions;
using Xunit;

namespace Components.Tests.Components.Charts.Composers;

public class ChartComposerTests
{
    private sealed class TestComposer : ISurfaceComposer<ChartOptions>
    {
        public int ComposeCount { get; private set; }

        public void Compose(ISurfaceRenderTarget target, ChartOptions options)
        {
            ComposeCount++;
        }

        public ValueTask ComposeAsync(ISurfaceRenderTarget target, ChartOptions options)
        {
            ComposeCount++;
            return ValueTask.CompletedTask;
        }
    }

    [Fact]
    public void ChartComposer_Compose_InvokesComposers()
    {
        var composer = new ChartComposer();
        var first = new TestComposer();
        var second = new TestComposer();
        var target = new ChartTestRenderTarget();

        composer.AddRange(first, second);
        composer.Compose(target, new ChartOptions());

        Assert.Equal(1, first.ComposeCount);
        Assert.Equal(1, second.ComposeCount);
    }

    [Fact]
    public async Task ChartComposer_ComposeAsync_InvokesComposers()
    {
        var composer = new ChartComposer();
        var first = new TestComposer();
        var second = new TestComposer();
        var target = new ChartTestRenderTarget();

        composer.AddRange(first, second);
        await composer.ComposeAsync(target, new ChartOptions());

        Assert.Equal(1, first.ComposeCount);
        Assert.Equal(1, second.ComposeCount);
    }
}
