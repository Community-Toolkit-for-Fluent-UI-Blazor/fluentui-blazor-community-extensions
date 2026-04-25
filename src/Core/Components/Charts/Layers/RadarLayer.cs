using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Layers;

internal class RadarLayer(RadarPayload payload) : IChartLayer, ILayer<RadarPayload>
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
