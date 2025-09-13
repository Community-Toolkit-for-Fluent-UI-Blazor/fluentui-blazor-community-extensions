using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;

public class SignatureImageFormatTests
{
    [Theory]
    [InlineData(SignatureImageFormat.Bmp, "Bmp")]
    [InlineData(SignatureImageFormat.Png, "Png")]
    [InlineData(SignatureImageFormat.Jpeg, "Jpeg")]
    [InlineData(SignatureImageFormat.Webp, "Webp")]
    [InlineData(SignatureImageFormat.Gif, "Gif")]
    [InlineData(SignatureImageFormat.Svg, "Svg")]
    [InlineData(SignatureImageFormat.Pdf, "Pdf")]
    public void Enum_Has_Expected_Names(SignatureImageFormat format, string expectedName)
    {
        Assert.Equal(expectedName, format.ToString());
    }

    [Fact]
    public void Enum_Has_Expected_Values()
    {
        var values = (SignatureImageFormat[])System.Enum.GetValues(typeof(SignatureImageFormat));
        Assert.Equal(7, values.Length);
        Assert.Contains(SignatureImageFormat.Bmp, values);
        Assert.Contains(SignatureImageFormat.Png, values);
        Assert.Contains(SignatureImageFormat.Jpeg, values);
        Assert.Contains(SignatureImageFormat.Webp, values);
        Assert.Contains(SignatureImageFormat.Gif, values);
        Assert.Contains(SignatureImageFormat.Svg, values);
        Assert.Contains(SignatureImageFormat.Pdf, values);
    }

    [Theory]
    [InlineData("Bmp", SignatureImageFormat.Bmp)]
    [InlineData("Png", SignatureImageFormat.Png)]
    [InlineData("Jpeg", SignatureImageFormat.Jpeg)]
    [InlineData("Webp", SignatureImageFormat.Webp)]
    [InlineData("Gif", SignatureImageFormat.Gif)]
    [InlineData("Svg", SignatureImageFormat.Svg)]
    [InlineData("Pdf", SignatureImageFormat.Pdf)]
    public void Parse_String_To_Enum(string name, SignatureImageFormat expected)
    {
        var parsed = (SignatureImageFormat)System.Enum.Parse(typeof(SignatureImageFormat), name);
        Assert.Equal(expected, parsed);
    }
}
