using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload data for a chart legend layer.
/// </summary>
/// <remarks>This record is used to encapsulate information required by a chart legend within a layered charting
/// component. It implements the ILayer interface to support integration with chart rendering infrastructure.</remarks>
public sealed record ChartLegendPayload : ILayerPayload
{
    /// <summary>
    /// Gets or sets the number of items in the legend.
    /// </summary>
    public int ItemCount{ get; set; }

    /// <summary>
    /// Gets or sets the area of the legend as a rectangle.
    /// </summary>
    public ChartRect Area { get; set; } = ChartRect.Empty;

    /// <summary>
    /// Gets or sets the shape used to represent the legend item in the chart.
    /// </summary>
    public ChartLegendItemShape Shape { get; set; }

    /// <summary>
    /// Gets the collection of series indices corresponding to the legend items.
    /// </summary>
    public IReadOnlyList<LegendItem> Items { get; init; } = [];
}
