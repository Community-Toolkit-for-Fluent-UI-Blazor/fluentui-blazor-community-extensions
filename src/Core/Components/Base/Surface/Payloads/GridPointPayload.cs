using FluentUI.Blazor.Community.Components.Charts.Drawing;

namespace FluentUI.Blazor.Community.Components.Surface.Payloads;

/// <summary>
/// Represents the payload for a grid point, including its position and whether it is marked as bold.
/// </summary>
public sealed class GridPointPayload
{
    /// <summary>
    /// Gets the position of the dot in the grid, represented as a <see cref="ChartPoint"/> with X and Y coordinates.
    /// </summary>
    public required ChartPoint Position { get; init; }

    /// <summary>
    /// Gets a value indicating whether this grid point should be rendered with a bold style.
    /// </summary>
    public bool IsBold { get; init; }
}
