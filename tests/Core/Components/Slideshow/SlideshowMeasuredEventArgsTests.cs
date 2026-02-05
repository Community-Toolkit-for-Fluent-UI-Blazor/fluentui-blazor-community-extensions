using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Slideshow;

public class SlideshowMeasuredEventArgsTests
{
    [Fact]
    public void SlideshowMeasuredEventArgs_DefaultValues()
    {
        var args = new SlideshowMeasuredEventArgs();

        Assert.Null(args.Id);
        Assert.Equal(0, args.Width);
        Assert.Equal(0, args.Height);
    }

    [Fact]
    public void SlideshowMeasuredEventArgs_AssignsProperties()
    {
        var args = new SlideshowMeasuredEventArgs
        {
            Id = "slide-1",
            Width = 320,
            Height = 240
        };

        Assert.Equal("slide-1", args.Id);
        Assert.Equal(320, args.Width);
        Assert.Equal(240, args.Height);
    }
}
