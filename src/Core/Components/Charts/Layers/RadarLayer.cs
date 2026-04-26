using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Layers;

internal sealed class RadarLayer(RadarPayloadCollection payload) : IChartLayer, ILayer<RadarPayloadCollection>
{
    /// <inheritdoc />
    public string Key => "radar";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Content;

    /// <inheritdoc />
    public int Priority => (int)ChartType.Radar;

    /// <inheritdoc />
    public ILayerPayload LayerPayload => payload;
}
