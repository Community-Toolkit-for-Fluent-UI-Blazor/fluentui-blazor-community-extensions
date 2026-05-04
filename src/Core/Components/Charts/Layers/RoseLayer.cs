using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Layers;

internal sealed class RoseLayer(RosePayloadCollection payload) : IChartLayer, ILayer<RosePayloadCollection>
{
    /// <inheritdoc />
    public string Key => "rose";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Content;

    /// <inheritdoc />
    public int Priority => (int)ChartType.PolarRose;

    /// <inheritdoc />
    public ILayerPayload LayerPayload => payload;
}
