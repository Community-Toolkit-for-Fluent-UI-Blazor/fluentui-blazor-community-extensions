using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Charts.Series;

public class ChartItemTests
{
    private sealed class TestItem : ChartItem
    {
        public TestItem()
        {
            Id = "item";
        }
    }

    [Fact]
    public void ChartItem_Defaults()
    {
        var item = new TestItem();

        Assert.NotNull(item.Id);
        Assert.Equal(ChartInteractionState.Normal, item.InteractionState);
        Assert.Equal(ChartAnimationEffect.Fade, item.Effect);
        Assert.True(item.IsVisible);
    }
}
