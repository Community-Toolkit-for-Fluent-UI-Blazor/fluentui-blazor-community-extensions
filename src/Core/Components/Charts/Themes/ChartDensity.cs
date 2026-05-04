using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Represents the density settings for a chart.
/// </summary>
public sealed record ChartDensity
{
    /// <summary>
    /// Gets the chart density mode that determines how data points are displayed.
    /// </summary>
    public ChartDensityMode Mode { get; init; } = ChartDensityMode.Normal;
}
