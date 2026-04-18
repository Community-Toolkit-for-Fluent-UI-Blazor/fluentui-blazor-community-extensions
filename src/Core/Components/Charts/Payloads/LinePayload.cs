namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload containing the path and data points for rendering a line in a chart component.
/// </summary>
public sealed record LinePayload : ChartItemPayloadBase
{
    /// <summary>
    /// Gets the payload that defines the line path for the component.
    /// </summary>
    public required LinePathPayload Path { get; init; }

    /// <summary>
    /// Gets the collection of data points to be displayed in the chart.
    /// </summary>
    public required IReadOnlyList<LinePointPayload> Points { get; set; } = [];
}
