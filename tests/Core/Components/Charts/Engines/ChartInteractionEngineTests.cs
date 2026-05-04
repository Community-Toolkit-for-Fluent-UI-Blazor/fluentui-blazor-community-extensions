using FluentUI.Blazor.Community.Components.Charts.Engines;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Charts.Engines;

public class ChartInteractionEngineTests
{
    [Fact]
    public void ChartInteractionEngine_Hover_SetsHoverState()
    {
        var serie = new BarSerie { Name = "Serie" };
        serie.UpdateItems([new CategoryItem { Name = "A", Value = 1 }]);
        var item = serie.Items[0];

        var engine = new ChartInteractionEngine(() => new[] { serie });

        var changed = engine.Hover(serie.Id, item.Id!);

        Assert.True(changed);
        Assert.Equal(ChartInteractionState.Hover, item.InteractionState);
        Assert.Equal(ChartAnimationTrigger.HoverIn, item.Trigger);
    }

    [Fact]
    public void ChartInteractionEngine_Select_TogglesSelection()
    {
        var serie = new BarSerie { Name = "Serie" };
        serie.UpdateItems([new CategoryItem { Name = "A", Value = 1 }]);
        var item = serie.Items[0];

        var engine = new ChartInteractionEngine(() => new[] { serie });

        Assert.True(engine.Select(serie.Id, item.Id!));
        Assert.Equal(ChartInteractionState.Selected, item.InteractionState);

        Assert.True(engine.Select(serie.Id, item.Id!));
        Assert.Equal(ChartInteractionState.Normal, item.InteractionState);
    }

    [Fact]
    public void ChartInteractionEngine_PointerLeave_ClearsHover()
    {
        var serie = new BarSerie { Name = "Serie" };
        serie.UpdateItems([new CategoryItem { Name = "A", Value = 1 }]);
        var item = serie.Items[0];

        var engine = new ChartInteractionEngine(() => new[] { serie });
        engine.Hover(serie.Id, item.Id!);

        var changed = engine.PointerLeave();

        Assert.True(changed);
        Assert.Equal(ChartInteractionState.Normal, item.InteractionState);
        Assert.Equal(ChartAnimationTrigger.HoverOut, item.Trigger);
    }

    [Fact]
    public void ChartInteractionEngine_PressAndRelease_UpdatesState()
    {
        var serie = new BarSerie { Name = "Serie" };
        serie.UpdateItems([new CategoryItem { Name = "A", Value = 1 }]);
        var item = serie.Items[0];

        var engine = new ChartInteractionEngine(() => new[] { serie });

        Assert.True(engine.PointerDown(serie.Id, item.Id!));
        Assert.Equal(ChartInteractionState.Pressed, item.InteractionState);

        Assert.True(engine.PointerUp(serie.Id, item.Id!));
        Assert.Equal(ChartInteractionState.Normal, item.InteractionState);
    }
}
