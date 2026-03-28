using System.IO.Compression;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to export multiple signature formats into a single ZIP archive using registered signature
/// exporters.
/// </summary>
/// <typeparam name="TPaylad">The type of the payload containing the data to be exported. Must be a non-nullable type.</typeparam>
/// <remarks>This class allows chaining of multiple signature exporters, each responsible for exporting a
/// signature in a specific format. When exporting, all registered exporters are invoked and their results are packaged
/// together into a ZIP file. This is useful for scenarios where signatures need to be provided in several formats
/// simultaneously. The class is not thread-safe; concurrent modifications or exports should be externally
/// synchronized.</remarks>
public class ZipExporter<TPaylad>(CompositeSurfaceExporter<TPaylad> composite)
{
    /// <inheritdoc />
    public async ValueTask<ExportResult> ExportAsync(
        string? fileName,
        SurfacePayload<TPaylad> payload,
        SurfaceExportOptions options)
    {
        var results = await composite.ExportAllAsync(fileName, payload, options);

        using var ms = new MemoryStream();
        using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, true))
        {
            foreach (var result in results)
            {
                var entry = zip.CreateEntry(result.FileName, CompressionLevel.Optimal);
                using var entryStream = entry.Open();
                await entryStream.WriteAsync(result.Data.AsMemory());
            }
        }

        return new ExportResult(
            "application/zip",
            !string.IsNullOrEmpty(fileName) ? Path.ChangeExtension(fileName, ".zip") : "signature.zip",
            ms.ToArray());
    }
}

/// <summary>
/// Provides functionality to export a collection of results as a ZIP archive asynchronously.
/// </summary>
/// <remarks>This static class offers methods for exporting data to ZIP files, enabling batch export scenarios
/// where multiple results need to be packaged together. All members are thread-safe and can be used concurrently from
/// multiple threads.</remarks>
public static class ZipExporter
{
    /// <summary>
    /// Asynchronously exports the specified results to a file with the given name.
    /// </summary>
    /// <param name="fileName">The name of the file to which the results will be exported. Can be null to use a default file name.</param>
    /// <param name="results">A read-only list of results to export. Cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous export operation. The task result contains an ExportResult indicating
    /// the outcome of the export.</returns>
    public static async ValueTask<ExportResult> ExportAsync(
        string? fileName,
        IReadOnlyList<ExportResult> results)
    {
        using var ms = new MemoryStream();

        using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, true))
        {
            foreach (var result in results)
            {
                var entry = zip.CreateEntry(result.FileName, CompressionLevel.Optimal);
                using var entryStream = entry.Open();
                await entryStream.WriteAsync(result.Data.AsMemory());
            }
        }

        return new ExportResult(
            "application/zip",
            !string.IsNullOrEmpty(fileName) ? Path.ChangeExtension(fileName, ".zip") : "signature.zip",
            ms.ToArray());
    }
}
