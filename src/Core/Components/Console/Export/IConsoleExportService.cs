namespace FluentUI.Blazor.Community.Components;

internal interface IConsoleExportService
{
    Task<ConsoleExportResult> ExportAsync(IReadOnlyCollection<string> formats, ConsoleExportOptions options, CancellationToken cancellationToken = default);
    Task<ConsoleExportResult> ExportAsync(string format, ConsoleExportOptions options, CancellationToken cancellationToken = default);
}