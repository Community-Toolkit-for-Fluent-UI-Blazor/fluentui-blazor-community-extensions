using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;

public class SignatureExportOptionsTests
{
    [Fact]
    public void Default_Values_Are_Correct()
    {
        var options = new SignatureExportOptions();
        Assert.Equal(SignatureImageFormat.Png, options.Format);
        Assert.Equal(100, options.Quality);
    }

    [Theory]
    [InlineData(SignatureImageFormat.Bmp)]
    [InlineData(SignatureImageFormat.Jpeg)]
    [InlineData(SignatureImageFormat.Webp)]
    [InlineData(SignatureImageFormat.Gif)]
    [InlineData(SignatureImageFormat.Svg)]
    [InlineData(SignatureImageFormat.Pdf)]
    public void Can_Set_And_Get_Format(SignatureImageFormat format)
    {
        var options = new SignatureExportOptions { Format = format };
        Assert.Equal(format, options.Format);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(50)]
    [InlineData(100)]
    public void Can_Set_And_Get_Quality(int quality)
    {
        var options = new SignatureExportOptions { Quality = quality };
        Assert.Equal(quality, options.Quality);
    }

    [Fact]
    public void Reset_Sets_Default_Values()
    {
        var options = new SignatureExportOptions
        {
            Format = SignatureImageFormat.Jpeg,
            Quality = 42
        };

        options.Reset();

        Assert.Equal(SignatureImageFormat.Png, options.Format);
        Assert.Equal(100, options.Quality);
    }
}
