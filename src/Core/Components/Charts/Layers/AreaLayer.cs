using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Layers;

/// <summary>
/// Represents a chart layer that renders area series using the specified payload.
/// </summary>
/// <param name="payload">The payload containing the data and configuration for the area layer. Cannot be null.</param>
internal sealed class AreaLayer(AreaPayload payload) : ILayer<AreaPayload>
{
    /// <inheritdoc />
    public string Key => "area";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Content;

    /// <inheritdoc />
    public int Priority => (int)ChartType.CategoryLine;

    /// <inheritdoc />
    public ILayerPayload LayerPayload => payload;
}
