using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Layers;

/// <summary>
/// Represents a layer for rendering polar bar charts in a chart component.
/// </summary>
/// <param name="payload"></param>
internal sealed class PolarBarLayer(PolarBarPayloadCollection payload) : IChartLayer, ILayer<PolarBarPayloadCollection>
{
    /// <inheritdoc />
    public string Key => "polar-bar";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Content;

    /// <inheritdoc />
    public int Priority => (int)ChartType.Radar;

    /// <inheritdoc />
    public ILayerPayload LayerPayload => payload;
}
