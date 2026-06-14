using System.IO.Compression;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Console;

public class ConsoleExportServiceTests
{
    [Fact]
    public async Task ExportAsync_UsesSelectedExporter()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        var state = new ConsoleState(new ConsoleOptions { EnableThreadSafety = false });
        await state.AddAsync(new ConsoleMessage { Level = ConsoleLevel.Information, Message = "Hello" });

        var exporter = new TestExporter("json");
        var service = new ConsoleExportService(state, new[] { exporter });

        var result = await service.ExportAsync("json", new ConsoleExportOptions(), cancellationToken);

        Assert.Equal("json.txt", result.FileName);
        Assert.Single(exporter.CapturedMessages);
    }

    [Fact]
    public async Task ExportAsync_MultipleFormats_ReturnsZip()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        var state = new ConsoleState(new ConsoleOptions { EnableThreadSafety = false });
        await state.AddAsync(new ConsoleMessage { Level = ConsoleLevel.Information, Message = "Hello" });

        var jsonExporter = new TestExporter("json");
        var txtExporter = new TestExporter("txt");
        var service = new ConsoleExportService(state, new[] { jsonExporter, txtExporter });

        var result = await service.ExportAsync(["json", "txt"], new ConsoleExportOptions(), cancellationToken);

        Assert.Equal("application/zip", result.ContentType);

        using var stream = new MemoryStream(result.Content);
        using var archive = new ZipArchive(stream, ZipArchiveMode.Read);

        Assert.NotNull(archive.GetEntry("json.txt"));
        Assert.NotNull(archive.GetEntry("txt.txt"));
    }

    private sealed class TestExporter(string format) : IConsoleExporter
    {
        public string Format { get; } = format;

        public string MimeType => "text/plain";

        public string DefaultFileName => "";

        public List<ConsoleMessage> CapturedMessages { get; } = [];

        public ValueTask<ConsoleExportResult> ExportAsync(
            IReadOnlyCollection<ConsoleMessage> messages,
            ConsoleExportOptions options,
            CancellationToken cancellationToken = default)
        {
            CapturedMessages.AddRange(messages);

            return ValueTask.FromResult(new ConsoleExportResult
            {
                FileName = $"{Format}.txt",
                ContentType = "text/plain",
                Content = []
            });
        }
    }
}
