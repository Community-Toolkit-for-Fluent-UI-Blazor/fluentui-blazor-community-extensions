using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Series;

/// <summary>
/// Defines tooltip configuration for a chart item.
/// </summary>
public sealed record ChartItemTooltip
{
    /// <summary>
    /// Gets the title displayed in the tooltip.
    /// </summary>
    public string? Title { get; init; }

    /// <summary>
    /// Gets the main text displayed in the tooltip.
    /// </summary>
    public string? Text { get; init; }

    /// <summary>
    /// Gets the optional custom payload for templated tooltips.
    /// </summary>
    public object? Payload { get; init; }

    /// <summary>
    /// Gets the position of the tooltip in chart coordinates.
    /// </summary>
    public ChartPoint Position { get; internal set; }

    /// <summary>
    /// Gets the placement of the tooltip.
    /// </summary>
    /// <remarks>
    /// <see cref="Position"/> is not used if <see cref="Placement"/> is set to a value other than <see cref="ChartTooltipPlacement.Pointer"/>.
    /// </remarks>
    public ChartTooltipPlacement Placement { get; internal set; }

    /// <summary>
    /// Gets a value indicating whether the element is visible.
    /// </summary>
    public bool Visible { get; internal set; }
}
