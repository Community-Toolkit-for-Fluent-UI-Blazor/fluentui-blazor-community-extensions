using FluentUI.Blazor.Community.Components;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Pictures;

public class PictureObjectFitTests
{
    [Theory]
    [InlineData(PictureObjectFit.None, 0)]
    [InlineData(PictureObjectFit.Fill, 1)]
    [InlineData(PictureObjectFit.Cover, 2)]
    [InlineData(PictureObjectFit.Contain, 3)]
    [InlineData(PictureObjectFit.ScaleDown, 4)]
    public void PictureObjectFit_HasExpectedValue(PictureObjectFit value, int expected)
    {
        Assert.Equal(expected, (int)value);
    }
}
