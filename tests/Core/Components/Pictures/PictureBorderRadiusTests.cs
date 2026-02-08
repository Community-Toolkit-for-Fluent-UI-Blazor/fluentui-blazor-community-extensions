using FluentUI.Blazor.Community.Components;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Pictures;

public class PictureBorderRadiusTests
{
    [Theory]
    [InlineData(PictureBorderRadius.None, 0)]
    [InlineData(PictureBorderRadius.Circle, 1)]
    [InlineData(PictureBorderRadius.Square, 2)]
    [InlineData(PictureBorderRadius.RoundSquare, 3)]
    [InlineData(PictureBorderRadius.Custom, 4)]
    public void PictureBorderRadius_HasExpectedValue(PictureBorderRadius value, int expected)
    {
        Assert.Equal(expected, (int)value);
    }
}
