namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload for a scatter chart, containing the points to be plotted.
/// </summary>
public sealed record ScatterPayload : ChartItemPayloadBase
{
    /// <summary>
    /// Gets the read-only list of points to be plotted in the scatter chart.
    /// </summary>
    public required IReadOnlyList<LinePointPayload> Points { get; init; }

    /// <summary>
    /// Gets or sets the radius of the points in the scatter chart.
    /// </summary>
    public double Radius { get; init; } = 3;
}

