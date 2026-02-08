using FluentUI.Blazor.Community.Components;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Pictures;

public class PictureBorderStyleTests
{
    [Theory]
    [InlineData(PictureBorderStyle.None, 0)]
    [InlineData(PictureBorderStyle.Solid, 1)]
    [InlineData(PictureBorderStyle.Dashed, 2)]
    [InlineData(PictureBorderStyle.Dotted, 3)]
    [InlineData(PictureBorderStyle.Double, 4)]
    public void PictureBorderStyle_HasExpectedValue(PictureBorderStyle value, int expected)
    {
        Assert.Equal(expected, (int)value);
    }
}
