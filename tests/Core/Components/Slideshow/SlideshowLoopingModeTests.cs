using System;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Slideshow;

public class SlideshowLoopingModeTests
{
    [Fact]
    public void SlideshowLoopingMode_DefinesExpectedValues()
    {
        Assert.Equal(0, (int)SlideshowLoopingMode.None);
        Assert.Equal(1, (int)SlideshowLoopingMode.Rewind);
        Assert.Equal(2, (int)SlideshowLoopingMode.Infinite);
    }

    [Fact]
    public void SlideshowLoopingMode_ContainsExpectedMembers()
    {
        var values = Enum.GetValues<SlideshowLoopingMode>();

        Assert.Equal(3, values.Length);
        Assert.Contains(SlideshowLoopingMode.None, values);
        Assert.Contains(SlideshowLoopingMode.Rewind, values);
        Assert.Contains(SlideshowLoopingMode.Infinite, values);
    }
}
