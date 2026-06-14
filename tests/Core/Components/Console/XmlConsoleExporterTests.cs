using System.Text;
using System.Xml.Linq;
using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Console;

public class XmlConsoleExporterTests
{
    [Fact]
    public async Task ExportAsync_WritesXmlContent()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        var message = new ConsoleMessage
        {
            Level = ConsoleLevel.Information,
            Message = "Hello",
            Category = "Test",
            Timestamp = new DateTimeOffset(2024, 1, 1, 12, 0, 0, TimeSpan.Zero)
        };

        var exporter = new XmlConsoleExporter();
        var result = await exporter.ExportAsync([message], new ConsoleExportOptions(), cancellationToken);
        var xml = Encoding.UTF8.GetString(result.Content);

        var document = XDocument.Parse(xml);
        var messageElement = document.Root?.Element("Message");

        Assert.NotNull(messageElement);
        Assert.Equal("Hello", messageElement?.Element("Message")?.Value);
        Assert.EndsWith(".xml", result.FileName);
    }
}
