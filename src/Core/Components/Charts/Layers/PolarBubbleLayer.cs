using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Layers;

internal sealed class PolarBubbleLayer(PolarBubblePayloadCollection payload) : IChartLayer, ILayer<PolarBubblePayloadCollection>
{
    /// <inheritdoc />
    public string Key => "polar-bubble";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Content;

    /// <inheritdoc />
    public int Priority => (int)ChartType.PolarBubble;

    /// <inheritdoc />
    public ILayerPayload LayerPayload => payload;
}
