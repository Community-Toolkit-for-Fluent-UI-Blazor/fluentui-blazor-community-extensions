using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Layers;

/// <summary>
/// Represents a chart layer that renders line series using the specified payload.
/// </summary>
/// <param name="payload">The payload containing the data and configuration for the line layer. Cannot be null.</param>
internal sealed class LineLayer(LinePayload payload) : IChartLayer, ILayer<LinePayload>
{
    /// <inheritdoc />
    public string Key => "line";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Content;

    /// <inheritdoc />
    public int Priority => (int)ChartType.Line;

    /// <inheritdoc />
    public ILayerPayload LayerPayload => payload;
}
