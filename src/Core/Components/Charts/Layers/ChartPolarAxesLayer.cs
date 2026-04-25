using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Layers;

/// <summary>
/// Represents the polar axes layer in a radar chart, responsible for rendering the axes and grid lines.
/// </summary>
internal class ChartPolarAxesLayer(PolarAxesPayload payload) : IChartLayer, ILayer<PolarAxesPayload>
{
    /// <inheritdoc />
    public string Key => "polar-axes";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Back;

    /// <inheritdoc />
    public int Priority => 0;

    /// <inheritdoc />
    public ILayerPayload LayerPayload => payload;
}
