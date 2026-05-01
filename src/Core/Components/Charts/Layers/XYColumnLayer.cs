using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Layers;

internal sealed class XYColumnLayer(PayloadCollection<XYColumnPayloadCollection> payload) : IChartLayer, ILayer<PayloadCollection<XYColumnPayloadCollection>>
{
    /// <inheritdoc />
    public string Key => "xy-column";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Content;

    /// <inheritdoc />
    public int Priority => (int)ChartType.XYColumn;

    /// <inheritdoc />
    public ILayerPayload LayerPayload => payload;
}
