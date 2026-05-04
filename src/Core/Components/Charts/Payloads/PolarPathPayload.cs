using FluentUI.Blazor.Community.Components.Charts.Drawing;

namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents a payload containing a path or polygon defined by polar coordinate points.
/// </summary>
public sealed class PolarPathPayload
{
    /// <summary>
    /// Gets the identifier of the payload.
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Gets the list of points that define the path or polygon in polar coordinates.
    /// </summary>
    public required IReadOnlyList<PolarPoint> Points { get; init; }

    /// <summary>
    /// Gets a value indicating whether the path is closed (forming a polygon) or open (forming a line).
    /// </summary>
    public bool Closed { get; init; } = true;
}
