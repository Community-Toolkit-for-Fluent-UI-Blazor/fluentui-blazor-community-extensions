using System.Text;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Console;

public class CsvConsoleExporterTests
{
    [Fact]
    public async Task ExportAsync_WritesCsvContent()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        var exception = CreateException();
        var message = new ConsoleMessage
        {
            Level = ConsoleLevel.Error,
            Message = "Failure",
            Category = "Test",
            Timestamp = new DateTimeOffset(2024, 1, 1, 12, 0, 0, TimeSpan.Zero),
            Exception = exception,
            Properties = new Dictionary<string, object?> { ["Key"] = "Value" }
        };

        var options = new ConsoleExportOptions
        {
            FileNamePrefix = "custom"
        };

        var exporter = new CsvConsoleExporter();
        var result = await exporter.ExportAsync([message], options, cancellationToken);
        var content = Encoding.UTF8.GetString(result.Content);

        Assert.Contains("Timestamp,Level,Category,Message,Properties,Exception", content);
        Assert.Contains("Failure", content);
        Assert.Contains("InvalidOperationException", content);
        Assert.StartsWith("custom_", result.FileName);
        Assert.EndsWith(".csv", result.FileName);
    }

    private static Exception CreateException()
    {
        try
        {
            throw new InvalidOperationException("boom");
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}
