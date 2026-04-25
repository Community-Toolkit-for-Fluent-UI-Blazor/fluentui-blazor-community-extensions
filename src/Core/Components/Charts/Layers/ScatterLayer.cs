using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Layers;

/// <summary>
/// Represents a chart layer that renders scatter series using the specified payload.
/// </summary>
/// <param name="payload">The payload containing the scatter points to be rendered.</param>
internal class ScatterLayer(ScatterPayload payload) : IChartLayer, ILayer<ScatterPayload>
{
    /// <inheritdoc />
    public string Key => "scatter";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Content;

    /// <inheritdoc />
    public int Priority => (int)ChartType.Scatter;

    /// <inheritdoc />
    public ILayerPayload LayerPayload => payload;
}
