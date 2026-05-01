using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Layers;

/// <summary>
/// Represents a chart layer that renders histogram series using the specified payload.
/// </summary>
/// <param name="payload">The collection of histogram payloads for the layer.</param>
internal sealed class HistogramLayer(HistogramPayloadCollection payload) : IChartLayer, ILayer<HistogramPayloadCollection>
{
    /// <inheritdoc />
    public string Key => "histogram";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Content;

    /// <inheritdoc />
    public int Priority => (int)ChartType.Histogram;

    /// <inheritdoc />
    public ILayerPayload LayerPayload => payload;
}
