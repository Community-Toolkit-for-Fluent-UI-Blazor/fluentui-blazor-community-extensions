using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Layers;

/// <summary>
/// Represents a chart layer that renders scatter series using the specified payload.
/// </summary>
/// <param name="payload"></param>
internal sealed class XYScatterLayer(XYScatterPayloadCollection payload) : IChartLayer, ILayer<XYScatterPayloadCollection>
{
    /// <inheritdoc />
    public string Key => "xy-scatter";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Content;

    /// <inheritdoc />
    public int Priority => (int)ChartType.Scatter;

    /// <inheritdoc />
    public ILayerPayload LayerPayload => payload;
}
