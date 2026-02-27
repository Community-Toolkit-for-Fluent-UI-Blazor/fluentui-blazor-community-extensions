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
        ExportOptions options)
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
