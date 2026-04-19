using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Layers;

/// <summary>
/// Represents a layer implementation for stacked area charts.
/// </summary>
/// <param name="payload">The payload for the stacked area layer.</param>
internal sealed class StackedAreaLayer(StackedAreaPayload payload) : ILayer<StackedAreaPayload>
{
    /// <inheritdoc />
    public string Key => "stacked-area";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Content;

    /// <inheritdoc />
    public int Priority => (int)ChartType.StackedArea;

    /// <inheritdoc />
    public ILayerPayload LayerPayload => payload;
}
