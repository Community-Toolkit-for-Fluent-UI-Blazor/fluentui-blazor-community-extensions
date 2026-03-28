using Microsoft.Extensions.Logging;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a composite signature exporter that delegates export operations to multiple underlying exporters.
/// </summary>
/// <remarks>This class enables combining several signature exporters, allowing export operations to be performed
/// by each registered exporter in sequence. Use the Add method to register one or more ISignatureExporter instances
/// before calling ExportAllAsync. The order in which exporters are added determines the order in which they are
/// invoked.</remarks>
public sealed class CompositeSurfaceExporter<TPayload>(ILogger logger)
{
    /// <summary>
    /// Contains the collection of signature exporters used to process or export signatures.
    /// </summary>
    private readonly List<ISurfaceExporter<TPayload>> _exporters = [];

    /// <summary>
    /// Adds the specified signature exporter to the composite exporter.
    /// </summary>
    /// <param name="exporter">The signature exporter to add to the composite. Cannot be null.</param>
    /// <returns>The current instance of the composite signature exporter, enabling method chaining.</returns>
    public CompositeSurfaceExporter<TPayload> Add(ISurfaceExporter<TPayload> exporter)
    {
        _exporters.Add(exporter);

        return this;
    }

    /// <summary>
    /// Exports all signature strokes using the available exporters and returns the results asynchronously.
    /// </summary>
    /// <remarks>The method invokes all registered exporters and aggregates their results. The order of
    /// results corresponds to the order of the exporters.</remarks>
    /// <param name="fileName">The optional file name to use for the export. Can be null or empty if not applicable.</param>
    /// <param name="payload">The payload to export.</param>
    /// <param name="options">The options that configure the export process. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of export results,
    /// one for each exporter.</returns>
    public async ValueTask<IReadOnlyList<ExportResult>> ExportAllAsync(
        string? fileName,
        SurfacePayload<TPayload> payload,
        SurfaceExportOptions options)
    {
        var results = new List<ExportResult>();

        foreach (var exporter in _exporters)
        {
            try
            {
                var result = await exporter.ExportAsync(fileName, payload, options);
                results.Add(result);
            }
            catch (NotSupportedException nsex)
            {
                logger.LogError(nsex, "Exporter {ExporterType} does not support the requested export format or options.", exporter.GetType().FullName);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while exporting with {ExporterType}.", exporter.GetType().FullName);
            }
        }

        return results;
    }
}

