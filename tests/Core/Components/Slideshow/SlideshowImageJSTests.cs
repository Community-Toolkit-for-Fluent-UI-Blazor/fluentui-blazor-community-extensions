using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.States;
using Xunit;

namespace Components.Tests.Components.Slideshow;

public class SlideshowImageJSTests
{
    [Fact]
    public void OnImageMeasured_UpdatesStateAndRaisesEvent()
    {
        var state = new SlideshowState();
        var jsModule = new SlideshowImageJS<string>(null!, state);
        var raisedId = string.Empty;

        state.SizeChanged += (_, id) => raisedId = id;

        jsModule.OnImageMeasured(new SlideshowMeasuredEventArgs
        {
            Id = "image-1",
            Width = 120,
            Height = 80
        });

        var (width, height) = state.GetSize("image-1");

        Assert.Equal(120, width);
        Assert.Equal(80, height);
        Assert.Equal("image-1", raisedId);
    }
}
