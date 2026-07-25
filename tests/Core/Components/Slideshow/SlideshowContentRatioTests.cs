using System;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Slideshow;

public class SlideshowContentRatioTests
{
    [Fact]
    public void SlideshowContentRatio_DefinesExpectedValues()
    {
        Assert.Equal(0, (int)SlideshowContentRatio.Original);
        Assert.Equal(1, (int)SlideshowContentRatio.Square);
        Assert.Equal(2, (int)SlideshowContentRatio.Landscape);
        Assert.Equal(3, (int)SlideshowContentRatio.Widescreen);
        Assert.Equal(4, (int)SlideshowContentRatio.UltraWidescreen);
        Assert.Equal(5, (int)SlideshowContentRatio.Portrait);
        Assert.Equal(6, (int)SlideshowContentRatio.Vertical);
        Assert.Equal(7, (int)SlideshowContentRatio.PortraitSocial);
        Assert.Equal(8, (int)SlideshowContentRatio.Story);
        Assert.Equal(9, (int)SlideshowContentRatio.CoverPhoto);
        Assert.Equal(10, (int)SlideshowContentRatio.GoldenRatio);
        Assert.Equal(11, (int)SlideshowContentRatio.A4Portrait);
        Assert.Equal(12, (int)SlideshowContentRatio.A4Landscape);
        Assert.Equal(13, (int)SlideshowContentRatio.FullContainer);
    }

    [Fact]
    public void SlideshowContentRatio_ContainsExpectedMembers()
    {
        var values = Enum.GetValues<SlideshowContentRatio>();

        Assert.Equal(14, values.Length);
        Assert.Contains(SlideshowContentRatio.Original, values);
        Assert.Contains(SlideshowContentRatio.Square, values);
        Assert.Contains(SlideshowContentRatio.Landscape, values);
        Assert.Contains(SlideshowContentRatio.Widescreen, values);
        Assert.Contains(SlideshowContentRatio.UltraWidescreen, values);
        Assert.Contains(SlideshowContentRatio.Portrait, values);
        Assert.Contains(SlideshowContentRatio.Vertical, values);
        Assert.Contains(SlideshowContentRatio.PortraitSocial, values);
        Assert.Contains(SlideshowContentRatio.Story, values);
        Assert.Contains(SlideshowContentRatio.CoverPhoto, values);
        Assert.Contains(SlideshowContentRatio.GoldenRatio, values);
        Assert.Contains(SlideshowContentRatio.A4Portrait, values);
        Assert.Contains(SlideshowContentRatio.A4Landscape, values);
        Assert.Contains(SlideshowContentRatio.FullContainer, values);
    }
}
