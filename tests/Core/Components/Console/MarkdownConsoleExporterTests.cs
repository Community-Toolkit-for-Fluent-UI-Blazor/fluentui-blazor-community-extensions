using System.Text;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Console;

public class MarkdownConsoleExporterTests
{
    [Fact]
    public async Task ExportAsync_WritesMarkdownContent()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        var message = new ConsoleMessage
        {
            Level = ConsoleLevel.Information,
            Message = "Hello",
            Category = "Test",
            Timestamp = new DateTimeOffset(2024, 1, 1, 12, 0, 0, TimeSpan.Zero)
        };

        var exporter = new MarkdownConsoleExporter();
        var result = await exporter.ExportAsync([message], new ConsoleExportOptions(), cancellationToken);
        var content = Encoding.UTF8.GetString(result.Content);

        Assert.Contains("# CONSOLE MESSAGES", content);
        Assert.Contains("Hello", content);
        Assert.EndsWith(".md", result.FileName);
    }
}
