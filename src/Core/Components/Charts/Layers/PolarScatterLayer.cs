using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Layers;

internal sealed class PolarScatterLayer(PolarScatterPayloadCollection payload) : IChartLayer, ILayer<PolarScatterPayloadCollection>
{
    /// <inheritdoc />
    public string Key => "polar-scatter";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Content;

    /// <inheritdoc />
    public int Priority => (int)ChartType.PolarScatter;

    /// <inheritdoc />
    public ILayerPayload LayerPayload => payload;
}
