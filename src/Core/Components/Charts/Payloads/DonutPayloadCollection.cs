using FluentUI.Blazor.Community.Components.Charts.Drawing;

namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the collection of donut payloads for a donut chart layer.
/// </summary>
public sealed record DonutPayloadCollection : ILayerPayload
{
    /// <summary>
    /// Gets the center point of the chart element.
    /// </summary>
    public required ChartPoint Center { get; init; }

    /// <summary>
    /// Gets the radius value for the shape or calculation.
    /// </summary>
    public required double Radius { get; init; }

    /// <summary>
    /// Gets the inner radius value used for rendering or calculations.
    /// </summary>
    public required double InnerRadius { get; init; }

    /// <summary>
    /// Gets the collection of data slices that define the segments of the pie chart.
    /// </summary>
    public required IReadOnlyList<PiePayload> Slices { get; init; }

    /// <summary>
    /// Gets a value indicating whether labels are displayed.
    /// </summary>
    public bool ShowLabels { get; init; }

    /// <summary>
    /// Gets a value indicating whether percentage values are displayed.
    /// </summary>
    public bool ShowPercentages { get; init; }

    /// <summary>
    /// Gets a unique identifier for the payload collection.
    /// </summary>
    public string Id { get; } = Guid.NewGuid().ToString();
}
