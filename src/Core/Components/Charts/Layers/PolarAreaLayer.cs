using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Layers;

internal sealed class PolarAreaLayer(PolarAreaPayloadCollection payload) : IChartLayer, ILayer<PolarAreaPayloadCollection>
{
    /// <inheritdoc />
    public string Key => "polar-area";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Content;

    /// <inheritdoc />
    public int Priority => (int)ChartType.PolarArea;

    /// <inheritdoc />
    public ILayerPayload LayerPayload => payload;
}
