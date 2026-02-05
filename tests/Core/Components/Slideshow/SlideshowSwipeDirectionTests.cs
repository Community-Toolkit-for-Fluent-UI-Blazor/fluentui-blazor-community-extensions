using System;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Slideshow;

public class SlideshowSwipeDirectionTests
{
    [Fact]
    public void SlideshowSwipeDirection_DefinesExpectedValues()
    {
        Assert.Equal(0, (int)SlideshowSwipeDirection.Next);
        Assert.Equal(1, (int)SlideshowSwipeDirection.Previous);
    }

    [Fact]
    public void SlideshowSwipeDirection_ContainsExpectedMembers()
    {
        var values = Enum.GetValues<SlideshowSwipeDirection>();

        Assert.Equal(2, values.Length);
        Assert.Contains(SlideshowSwipeDirection.Next, values);
        Assert.Contains(SlideshowSwipeDirection.Previous, values);
    }
}
