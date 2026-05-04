using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Layers;

/// <summary>
/// Represents a chart layer that renders line series using the specified payload.
/// </summary>
/// <param name="payload"></param>
internal sealed class XYLineLayer(XYLinePayloadCollection payload) : IChartLayer, ILayer<XYLinePayloadCollection>
{
    /// <inheritdoc />
    public string Key => "xy-line";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Content;

    /// <inheritdoc />
    public int Priority => (int)ChartType.XYLine;

    /// <inheritdoc />
    public ILayerPayload LayerPayload => payload;
}
