using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Layers;

/// <summary>
/// Represents a chart layout layer that contains the layout information for the chart, including title, subtitle, and legend payloads.
/// </summary>
/// <param name="payload">The payload containing the layout information for the chart.</param>
internal sealed record ChartLayoutLayer(ChartLayoutPayload payload)
    : IChartLayer, ILayer<ChartLayoutPayload>
{
    /// <inheritdoc />
    public string Key => "chart-layout";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Front;

    /// <inheritdoc />
    public int Priority => 0;

    /// <inheritdoc />
    public ILayerPayload LayerPayload => payload;
}
