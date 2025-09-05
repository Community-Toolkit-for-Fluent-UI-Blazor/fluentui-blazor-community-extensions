using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;
public class SignatureExportResultTests : TestBase
{

    public SignatureExportResultTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddScoped<SignatureState>();
        Services.AddFluentUIComponents();
    }

    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var fileName = "signature.png";
        var data = new byte[] { 1, 2, 3 };
        var contentType = "image/png";
        var format = SignatureExportFormat.Png;

        // Act
        var result = new SignatureExportResult(fileName, data, contentType, format);

        // Assert
        Assert.Equal(fileName, result.FileName);
        Assert.Equal(data, result.Data);
        Assert.Equal(contentType, result.ContentType);
        Assert.Equal(format, result.Format);
    }

    [Fact]
    public void Records_WithSameValues_AreEqual()
    {
        byte[] data = [1];

        // Arrange
        var a = new SignatureExportResult("a", data, "type", SignatureExportFormat.Png);
        var b = new SignatureExportResult("a", data, "type", SignatureExportFormat.Png);

        // Act & Assert
        Assert.Equal(a, b);
        Assert.True(a == b);
    }

    [Fact]
    public void Records_WithDifferentValues_AreNotEqual()
    {
        var a = new SignatureExportResult("a", new byte[] { 1 }, "type", SignatureExportFormat.Png);
        var b = new SignatureExportResult("b", new byte[] { 2 }, "type2", SignatureExportFormat.Jpeg);

        Assert.NotEqual(a, b);
        Assert.False(a == b);
    }

    [Fact]
    public void Deconstruct_WorksCorrectly()
    {
        var result = new SignatureExportResult("file", new byte[] { 1 }, "type", SignatureExportFormat.Pdf);

        var (fileName, data, contentType, format) = result;

        Assert.Equal("file", fileName);
        Assert.Equal(new byte[] { 1 }, data);
        Assert.Equal("type", contentType);
        Assert.Equal(SignatureExportFormat.Pdf, format);
    }

    [Fact]
    public void ToString_ReturnsExpectedFormat()
    {
        var result = new SignatureExportResult("file", new byte[] { 1, 2 }, "type", SignatureExportFormat.Svg);

        var str = result.ToString();

        Assert.Contains("file", str);
        Assert.Contains("type", str);
        Assert.Contains("Svg", str);
    }
}
