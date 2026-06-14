using System.Text;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Console;

public class TextConsoleExporterTests
{
    [Fact]
    public async Task ExportAsync_WritesTextContent()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        var message = new ConsoleMessage
        {
            Level = ConsoleLevel.Information,
            Message = "Hello",
            Category = "Test",
            Timestamp = new DateTimeOffset(2024, 1, 1, 12, 0, 0, TimeSpan.Zero)
        };

        var exporter = new TextConsoleExporter();
        var result = await exporter.ExportAsync([message], new ConsoleExportOptions(), cancellationToken);
        var content = Encoding.UTF8.GetString(result.Content);

        Assert.Contains("Hello", content);
        Assert.Contains("Test", content);
        Assert.EndsWith(".txt", result.FileName);
    }
}
