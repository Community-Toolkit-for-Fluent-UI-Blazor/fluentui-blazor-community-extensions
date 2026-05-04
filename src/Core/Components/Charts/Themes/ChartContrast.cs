using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Represents the contrast settings for a chart theme.
/// </summary>
public sealed record ChartContrast
{
    /// <summary>
    /// Gets the contrast mode used for rendering the chart.
    /// </summary>
    public ChartContrastMode Mode { get; init; } = ChartContrastMode.Normal;
}
