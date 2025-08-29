using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Slideshow;
public class SlideshowResizeTests
{
    [Fact]
    public void DefaultConstructor_ShouldSetAllPropertiesToDefault()
    {
        var resize = new SlideshowResize();

        Assert.False(resize.FixedWidth);
        Assert.False(resize.FixedHeight);
        Assert.Equal(0, resize.Width);
        Assert.Equal(0, resize.Height);
    }

    [Fact]
    public void Properties_ShouldBeSettableAndGettable()
    {
        var resize = new SlideshowResize
        {
            FixedWidth = true,
            FixedHeight = true,
            Width = 800,
            Height = 600
        };

        Assert.True(resize.FixedWidth);
        Assert.True(resize.FixedHeight);
        Assert.Equal(800, resize.Width);
        Assert.Equal(600, resize.Height);
    }

    [Fact]
    public void CanAssignAndCompareStructs()
    {
        var resize1 = new SlideshowResize
        {
            FixedWidth = true,
            FixedHeight = false,
            Width = 1024,
            Height = 768
        };

        var resize2 = resize1;

        Assert.Equal(resize1.FixedWidth, resize2.FixedWidth);
        Assert.Equal(resize1.FixedHeight, resize2.FixedHeight);
        Assert.Equal(resize1.Width, resize2.Width);
        Assert.Equal(resize1.Height, resize2.Height);
    }
}
