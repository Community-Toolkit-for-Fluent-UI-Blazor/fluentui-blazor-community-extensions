using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Layers;

/// <summary>
/// Represents the layer responsible for rendering a pie chart.
/// </summary>
/// <param name="payload">The payload containing the data and configuration for the pie chart.</param>
internal sealed class PieLayer(PiePayloadCollection payload) : ILayer<PiePayloadCollection>
{
    /// <inheritdoc />
    public string Key => "pie";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Content;

    /// <inheritdoc />
    public int Priority => (int)ChartType.Pie;

    /// <inheritdoc />
    public ILayerPayload LayerPayload => payload;
}
