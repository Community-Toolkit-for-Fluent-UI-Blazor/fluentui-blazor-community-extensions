using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Layers;

/// <summary>
/// Represents a chart layer that renders hierarchical data, such as tree maps or sunburst charts.
/// </summary>
/// <param name="payload">The payload containing the hierarchical data and chart type.</param>
internal sealed class HierarchyLayer(HierarchyPayload payload) : IChartLayer, ILayer<HierarchyPayload>
{
    /// <inheritdoc />
    public string Key => "hierarchy";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Content;

    /// <inheritdoc />
    public int Priority => (int)payload.Type;

    /// <inheritdoc />
    public ILayerPayload LayerPayload => payload;
}
