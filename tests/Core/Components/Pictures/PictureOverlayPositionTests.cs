using FluentUI.Blazor.Community.Components;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Pictures;

public class PictureOverlayPositionTests
{
    [Theory]
    [InlineData(PictureOverlayPosition.TopLeft, 0)]
    [InlineData(PictureOverlayPosition.TopCenter, 1)]
    [InlineData(PictureOverlayPosition.TopRight, 2)]
    [InlineData(PictureOverlayPosition.MiddleLeft, 3)]
    [InlineData(PictureOverlayPosition.MiddleCenter, 4)]
    [InlineData(PictureOverlayPosition.MiddleRight, 5)]
    [InlineData(PictureOverlayPosition.BottomLeft, 6)]
    [InlineData(PictureOverlayPosition.BottomCenter, 7)]
    [InlineData(PictureOverlayPosition.BottomRight, 8)]
    public void PictureOverlayPosition_HasExpectedValue(PictureOverlayPosition value, int expected)
    {
        Assert.Equal(expected, (int)value);
    }
}
