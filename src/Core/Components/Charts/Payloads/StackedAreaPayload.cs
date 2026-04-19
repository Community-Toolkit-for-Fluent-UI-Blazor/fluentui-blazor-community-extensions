namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload for a stacked area chart, containing the top and bottom line paths and the data points for rendering the stacked area.
/// </summary>
public sealed record StackedAreaPayload : ChartItemPayloadBase
{
    /// <summary>
    /// Gets the payload that defines the top line path for the stacked area.
    /// </summary>
    public LinePathPayload TopPath { get; init; } = default!;

    /// <summary>
    /// Gets the payload that defined the bottom line path for the stacked area.
    /// </summary>
    public LinePathPayload BottomPath { get; init; } = default!;

    /// <summary>
    /// Gets the collection of data points used to draw intersection circles.
    /// </summary>
    public IReadOnlyList<LinePointPayload> Points { get; init; } = [];
}
