using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Layers;

/// <summary>
/// Represents the layer responsible for rendering a semi-donut chart.
/// </summary>
/// <param name="payload">The payload containing the data and configuration for the semi-donut chart.</param>
internal sealed class SemiDonutLayer(DonutPayloadCollection payload) : IChartLayer, ILayer<DonutPayloadCollection>
{
    /// <inheritdoc />
    public string Key => "semi-donut";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Content;

    /// <inheritdoc />
    public int Priority => (int)ChartType.SemiDonut;

    /// <inheritdoc />
    public ILayerPayload LayerPayload => payload;
}
