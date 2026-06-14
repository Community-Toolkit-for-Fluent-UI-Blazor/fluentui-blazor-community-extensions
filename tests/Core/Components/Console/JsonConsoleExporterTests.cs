using System.Globalization;
using System.Text;
using System.Text.Json;
using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Console;

public class JsonConsoleExporterTests
{
    [Fact]
    public async Task ExportAsync_WritesJsonContent()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        var message = new ConsoleMessage
        {
            Level = ConsoleLevel.Warning,
            Message = "Warning",
            Category = "Test",
            Timestamp = new DateTimeOffset(2024, 1, 1, 12, 0, 0, TimeSpan.Zero),
            Properties = new Dictionary<string, object?> { ["Key"] = "Value" }
        };

        var exporter = new JsonConsoleExporter();
        var options = new ConsoleExportOptions { FileNamePrefix = "custom" };
        var result = await exporter.ExportAsync([message], options, cancellationToken);

        using var document = JsonDocument.Parse(Encoding.UTF8.GetString(result.Content));
        var first = document.RootElement[0];

        Assert.Equal("Warning", first.GetProperty("message").GetString());
        Assert.Equal("Test", first.GetProperty("category").GetString());
        Assert.Equal(ConsoleLevel.Warning, Enum.Parse<ConsoleLevel>(first.GetProperty("level").GetInt32().ToString(CultureInfo.InvariantCulture)!));
        Assert.StartsWith("custom_", result.FileName);
        Assert.EndsWith(".json", result.FileName);
    }
}
