using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.States;
using Xunit;

namespace Components.Tests.Components.Slideshow;

public class SlideshowVideoJSTests
{
    [Fact]
    public void OnVideoMeasured_UpdatesStateAndRaisesEvent()
    {
        var state = new SlideshowState();
        var jsModule = new SlideshowVideoJS<string>(null!, state);
        var raisedId = string.Empty;

        state.SizeChanged += (_, id) => raisedId = id;

        jsModule.OnVideoMeasured(new SlideshowMeasuredEventArgs
        {
            Id = "video-1",
            Width = 1920,
            Height = 1080
        });

        var (width, height) = state.GetSize("video-1");

        Assert.Equal(1920, width);
        Assert.Equal(1080, height);
        Assert.Equal("video-1", raisedId);
    }
}
