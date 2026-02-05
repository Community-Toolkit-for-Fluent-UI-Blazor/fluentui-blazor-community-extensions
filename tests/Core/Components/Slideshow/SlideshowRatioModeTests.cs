using System;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Slideshow;

public class SlideshowRatioModeTests
{
    [Fact]
    public void SlideshowRatioMode_DefinesExpectedValues()
    {
        Assert.Equal(0, (int)SlideshowRatioMode.Image);
        Assert.Equal(1, (int)SlideshowRatioMode.Container);
    }

    [Fact]
    public void SlideshowRatioMode_ContainsExpectedMembers()
    {
        var values = Enum.GetValues<SlideshowRatioMode>();

        Assert.Equal(2, values.Length);
        Assert.Contains(SlideshowRatioMode.Image, values);
        Assert.Contains(SlideshowRatioMode.Container, values);
    }
}
