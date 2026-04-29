using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Layers;

internal sealed class XYAreaLayer(XYAreaPayloadCollection payload) : IChartLayer, ILayer<XYAreaPayloadCollection>
{
    /// <inheritdoc />
    public string Key => "xy-area";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Content;

    /// <inheritdoc />
    public int Priority => (int)ChartType.XYArea;

    /// <inheritdoc />
    public ILayerPayload LayerPayload => payload;
}
