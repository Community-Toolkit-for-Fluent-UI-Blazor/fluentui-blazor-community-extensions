using Components.Tests.Components.Charts.TestHelpers;
using FluentUI.Blazor.Community.Components.Charts;
using FluentUI.Blazor.Community.Components.Charts.Composers;
using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Options;
using Xunit;

namespace Components.Tests.Components.Charts.Composers;

public class ChartClipPathComposerTests
{
    [Fact]
    public void ChartClipPathComposer_Compose_AddsClipPathLayer()
    {
        var context = new ChartContext { PlotArea = new ChartRect(1, 2, 30, 40) };
        var composer = new ChartClipPathComposer(() => context);
        var target = new ChartTestRenderTarget();

        composer.Compose(target, new ChartOptions());

        Assert.Single(target.Layers);
        Assert.Equal("clip-path", target.Layers[0].Key);
        Assert.False(string.IsNullOrWhiteSpace(context.ClipPathId));
    }
}
