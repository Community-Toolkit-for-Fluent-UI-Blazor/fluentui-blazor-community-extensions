using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.FileManager;

public class AcceptFileProviderTests
    : TestBase
{
    [Theory]
    [InlineData(AcceptFile.None, "")]
    [InlineData(AcceptFile.Audio, "audio/*")]
    [InlineData(AcceptFile.Video, "video/*")]
    [InlineData(AcceptFile.Image, "image/*")]
    [InlineData(AcceptFile.Excel, ".xls, .xlsx")]
    [InlineData(AcceptFile.Pdf, ".pdf")]
    [InlineData(AcceptFile.Powerpoint, ".ppt, .pptx")]
    [InlineData(AcceptFile.Word, ".doc, .docx")]
    [InlineData(AcceptFile.Audio | AcceptFile.Image, "audio/*, image/*")]
    [InlineData(AcceptFile.Document, ".pdf, .xls, .xlsx, .doc, .docx, .ppt, .pptx")]
    [InlineData(AcceptFile.Audio | AcceptFile.Document, "audio/*, .pdf, .xls, .xlsx, .doc, .docx, .ppt, .pptx")]
    [InlineData(AcceptFile.Video | AcceptFile.Document, "video/*, .pdf, .xls, .xlsx, .doc, .docx, .ppt, .pptx")]
    public void AcceptFileProvider_Get_ReturnsExpected(AcceptFile value, string expected)
    {
        var result = AcceptFileProvider.Get(value);
        Assert.Equal(expected, result);
    }
}
