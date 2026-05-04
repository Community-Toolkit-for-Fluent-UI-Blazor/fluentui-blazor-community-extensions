using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the payload tooltip data for a chart tooltip layer.
/// </summary>
public sealed record ChartTooltipPayload
{
    /// <summary>
    /// Gets the text of the tooltip.
    /// </summary>
    public string? Text { get; init; }

    /// <summary>
    /// Gets a value indicating whether the tooltip is visible.
    /// </summary>
    public bool IsVisible { get; init; }

    /// <summary>
    /// Gets the position of the tooltip relative to the chart.
    /// </summary>
    public ChartTooltipPlacement Position { get; internal set; }
}
