using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Slideshow;

public class SlideshowIndicatorPositionTests
{
    [Fact]
    public void Enum_Should_Have_All_Expected_Values()
    {
        var values = Enum.GetValues<SlideshowIndicatorPosition>();
        Assert.Contains(SlideshowIndicatorPosition.Top, values);
        Assert.Contains(SlideshowIndicatorPosition.Bottom, values);
        Assert.Contains(SlideshowIndicatorPosition.Left, values);
        Assert.Contains(SlideshowIndicatorPosition.Right, values);
        Assert.Equal(4, values.Length);
    }

    [Theory]
    [InlineData("Top", SlideshowIndicatorPosition.Top)]
    [InlineData("Bottom", SlideshowIndicatorPosition.Bottom)]
    [InlineData("Left", SlideshowIndicatorPosition.Left)]
    [InlineData("Right", SlideshowIndicatorPosition.Right)]
    public void Enum_Parse_Should_Return_Correct_Value(string name, SlideshowIndicatorPosition expected)
    {
        var result = Enum.Parse<SlideshowIndicatorPosition>(name);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(SlideshowIndicatorPosition.Top, "Top")]
    [InlineData(SlideshowIndicatorPosition.Bottom, "Bottom")]
    [InlineData(SlideshowIndicatorPosition.Left, "Left")]
    [InlineData(SlideshowIndicatorPosition.Right, "Right")]
    public void Enum_ToString_Should_Return_Correct_Name(SlideshowIndicatorPosition value, string expected)
    {
        Assert.Equal(expected, value.ToString());
    }
}
