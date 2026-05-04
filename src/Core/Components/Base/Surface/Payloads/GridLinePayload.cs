using FluentUI.Blazor.Community.Components.Charts.Drawing;

namespace FluentUI.Blazor.Community.Components.Surface.Payloads;

/// <summary>
/// Represents the payload data for a grid.
/// </summary>
public sealed class GridLinePayload
{
    /// <summary>
    /// Gets the starting point of line grid.
    /// </summary>
    public required ChartPoint StartPoint { get; init; }

    /// <summary>
    /// Gets the ending point of line grid.
    /// </summary>
    public required ChartPoint EndPoint { get; init; }

    /// <summary>
    /// Gets a value indicating whether the content is displayed in bold style.
    /// </summary>
    public bool IsBold { get; init; }
}
