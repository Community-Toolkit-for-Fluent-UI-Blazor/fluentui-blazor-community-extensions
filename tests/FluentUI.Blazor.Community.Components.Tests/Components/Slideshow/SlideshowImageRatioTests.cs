namespace FluentUI.Blazor.Community.Components.Tests.Components.Slideshow;

public class SlideshowImageRatioTests
{
    [Fact]
    public void Enum_Should_Have_Auto_And_Fill_Members()
    {
        Assert.Equal(0, (int)SlideshowImageRatio.None);
        Assert.Equal(1, (int)SlideshowImageRatio.Auto);
        Assert.Equal(2, (int)SlideshowImageRatio.Fill);
        Assert.Equal(3, (int)SlideshowImageRatio.Container);
    }

    [Theory]
    [InlineData(SlideshowImageRatio.None, "None")]
    [InlineData(SlideshowImageRatio.Auto, "Auto")]
    [InlineData(SlideshowImageRatio.Fill, "Fill")]
    [InlineData(SlideshowImageRatio.Container, "Container")]
    public void ToString_Should_Return_Correct_Name(SlideshowImageRatio ratio, string expected)
    {
        Assert.Equal(expected, ratio.ToString());
    }

    [Theory]
    [InlineData("None", SlideshowImageRatio.None)]
    [InlineData("Auto", SlideshowImageRatio.Auto)]
    [InlineData("Fill", SlideshowImageRatio.Fill)]
    [InlineData("Container", SlideshowImageRatio.Container)]
    public void Parse_Should_Return_Correct_Enum(string value, SlideshowImageRatio expected)
    {
        var result = Enum.Parse<SlideshowImageRatio>(value);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetValues_Should_Contain_All_Members()
    {
        var values = System.Enum.GetValues<SlideshowImageRatio>();
        Assert.Contains(SlideshowImageRatio.None, values);
        Assert.Contains(SlideshowImageRatio.Auto, values);
        Assert.Contains(SlideshowImageRatio.Fill, values);
        Assert.Contains(SlideshowImageRatio.Container, values);
        Assert.Equal(4, values.Length);
    }
}
