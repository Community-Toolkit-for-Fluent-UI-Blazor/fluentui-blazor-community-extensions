using FluentUI.Blazor.Community.Components;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Console;

public class ConsoleExportFileOptionsTests
{
    [Fact]
    public void SelectedFormats_ReturnsAllEnabledFormats()
    {
        var options = new ConsoleExportFileOptions();

        Assert.Contains("json", options.SelectedFormats);
        Assert.Contains("csv", options.SelectedFormats);
        Assert.Contains("txt", options.SelectedFormats);
        Assert.Contains("xml", options.SelectedFormats);
        Assert.Contains("markdown", options.SelectedFormats);
        Assert.True(options.IsMultiExportSelected);
    }

    [Fact]
    public void SelectedFormat_ReturnsSingleFormatWhenOnlyOneEnabled()
    {
        var options = new ConsoleExportFileOptions
        {
            ExportJson = true,
            ExportCsv = false,
            ExportTxt = false,
            ExportXml = false,
            ExportMarkdown = false
        };

        Assert.Equal("json", options.SelectedFormat);
        Assert.False(options.IsMultiExportSelected);
    }
}
