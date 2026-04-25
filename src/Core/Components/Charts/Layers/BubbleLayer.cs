using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Layers;

/// <summary>
/// Represents a chart layer that renders bubble series using the specified payload.
/// </summary>
/// <param name="payload">The payload containing the bubble points to be rendered.</param>
internal class BubbleLayer(BubblePayload payload) : IChartLayer, ILayer<BubblePayload>
{
    /// <inheritdoc />
    public string Key => "bubble";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Content;

    /// <inheritdoc />
    public int Priority => (int)ChartType.Bubble;

    /// <inheritdoc />
    public ILayerPayload LayerPayload => payload;
}
