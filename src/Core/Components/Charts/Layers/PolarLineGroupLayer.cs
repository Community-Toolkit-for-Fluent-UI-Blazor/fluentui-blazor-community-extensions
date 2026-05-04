using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Layers;

internal sealed class PolarLineGroupLayer(PolarLinePayloadCollection payload) : IChartLayer, ILayer<PolarLinePayloadCollection>
{
    /// <inheritdoc />
    public string Key => "polar-line";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Content;

    /// <inheritdoc />
    public int Priority => (int)ChartType.PolarLine;

    /// <inheritdoc />
    public ILayerPayload LayerPayload => payload;
}
