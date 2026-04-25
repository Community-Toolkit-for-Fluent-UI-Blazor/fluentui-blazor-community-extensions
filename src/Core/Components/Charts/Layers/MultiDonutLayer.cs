using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Layers;

/// <summary>
/// Represents the layer responsible for rendering a multi donut chart.
/// </summary>
/// <param name="payload">The payload containing the data and configuration for the donut chart.</param>
internal sealed class MultiDonutLayer(MultiDonutPayloadCollection payload) : IChartLayer, ILayer<MultiDonutPayloadCollection>
{
    /// <inheritdoc />
    public string Key => "multi-donut";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Content;

    /// <inheritdoc />
    public int Priority => (int)ChartType.MultiDonut;

    /// <inheritdoc />
    public ILayerPayload LayerPayload => payload;
}
