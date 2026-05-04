using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Layers;

internal sealed class BarLayer(BarPayloadCollection payload) : IChartLayer, ILayer<BarPayloadCollection>
{
    /// <inheritdoc />
    public string Key => "bar";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Content;

    /// <inheritdoc />
    public int Priority => (int)ChartType.Bar;

    /// <inheritdoc />
    public ILayerPayload LayerPayload => payload;
}
