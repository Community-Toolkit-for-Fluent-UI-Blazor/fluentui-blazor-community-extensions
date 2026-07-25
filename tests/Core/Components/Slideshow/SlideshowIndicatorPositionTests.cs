using System;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Slideshow;

public class SlideshowIndicatorPositionTests
{
    [Fact]
    public void SlideshowIndicatorPosition_DefinesExpectedValues()
    {
        Assert.Equal(0, (int)SlideshowIndicatorPosition.Top);
        Assert.Equal(1, (int)SlideshowIndicatorPosition.Bottom);
        Assert.Equal(2, (int)SlideshowIndicatorPosition.Left);
        Assert.Equal(3, (int)SlideshowIndicatorPosition.Right);
    }

    [Fact]
    public void SlideshowIndicatorPosition_ContainsExpectedMembers()
    {
        var values = Enum.GetValues<SlideshowIndicatorPosition>();

        Assert.Equal(4, values.Length);
        Assert.Contains(SlideshowIndicatorPosition.Top, values);
        Assert.Contains(SlideshowIndicatorPosition.Bottom, values);
        Assert.Contains(SlideshowIndicatorPosition.Left, values);
        Assert.Contains(SlideshowIndicatorPosition.Right, values);
    }
}
