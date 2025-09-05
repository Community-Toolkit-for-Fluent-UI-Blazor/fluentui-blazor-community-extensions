using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;
public class SignatureExportFormatTests
{
    [Fact]
    public void Enum_Should_Contain_All_Expected_Values()
    {
        Assert.Contains(SignatureExportFormat.Png, Enum.GetValues<SignatureExportFormat>());
        Assert.Contains(SignatureExportFormat.Jpeg, Enum.GetValues<SignatureExportFormat>());
        Assert.Contains(SignatureExportFormat.Webp, Enum.GetValues<SignatureExportFormat>());
        Assert.Contains(SignatureExportFormat.Svg, Enum.GetValues<SignatureExportFormat>());
        Assert.Contains(SignatureExportFormat.Pdf, Enum.GetValues<SignatureExportFormat>());
    }

    [Theory]
    [InlineData("Png", SignatureExportFormat.Png)]
    [InlineData("Jpeg", SignatureExportFormat.Jpeg)]
    [InlineData("Webp", SignatureExportFormat.Webp)]
    [InlineData("Svg", SignatureExportFormat.Svg)]
    [InlineData("Pdf", SignatureExportFormat.Pdf)]
    public void Parse_Should_Return_Correct_Enum(string value, SignatureExportFormat expected)
    {
        var result = Enum.Parse<SignatureExportFormat>(value);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(SignatureExportFormat.Png, "Png")]
    [InlineData(SignatureExportFormat.Jpeg, "Jpeg")]
    [InlineData(SignatureExportFormat.Webp, "Webp")]
    [InlineData(SignatureExportFormat.Svg, "Svg")]
    [InlineData(SignatureExportFormat.Pdf, "Pdf")]
    public void ToString_Should_Return_Correct_String(SignatureExportFormat format, string expected)
    {
        Assert.Equal(expected, format.ToString());
    }
}
