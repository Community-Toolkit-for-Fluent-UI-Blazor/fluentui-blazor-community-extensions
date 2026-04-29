using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Layers;

/// <summary>
/// Represents a chart layer that renders bubble series using the specified payload.
/// </summary>
/// <param name="payload"></param>
internal sealed class XYBubbleLayer(XYBubblePayloadCollection payload) : IChartLayer, ILayer<XYBubblePayload>
{
    /// <inheritdoc />
    public string Key => "xy-bubble";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Content;

    /// <inheritdoc />
    public int Priority => (int)ChartType.Bubble;

    /// <inheritdoc />
    public ILayerPayload LayerPayload => payload;
}
