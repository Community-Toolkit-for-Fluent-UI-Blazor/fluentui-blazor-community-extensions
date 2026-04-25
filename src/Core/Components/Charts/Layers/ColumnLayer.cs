using FluentUI.Blazor.Community.Components.Charts.Payloads;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Layers;

/// <summary>
/// Represents a column layer in a chart.
/// </summary>
/// <param name="payload">The payload collection containing the data for the column layer.</param>
internal sealed class ColumnLayer(ColumnPayloadCollection payload) : IChartLayer, ILayer<ColumnPayloadCollection>
{
    /// <inheritdoc />
    public string Key => "column";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Content;

    /// <inheritdoc />
    public int Priority => (int)ChartType.Column;

    /// <inheritdoc />
    public ILayerPayload LayerPayload => payload;
}
