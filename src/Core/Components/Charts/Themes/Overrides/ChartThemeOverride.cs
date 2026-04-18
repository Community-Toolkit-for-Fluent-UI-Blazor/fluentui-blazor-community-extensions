namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Represents a set of theme overrides that can be applied to a chart.
/// </summary>
public sealed record ChartThemeOverride
{
    /// <summary>
    /// Gets the color palette override to use for rendering the chart.
    /// </summary>
    public ChartPaletteOverride? Palette { get; init; }

    /// <summary>
    /// Gets the typography override to use for rendering text elements in the chart.
    /// </summary>
    public ChartTypographyOverride? Typography { get; init; }

    /// <summary>
    /// Gets the layout override to apply to the chart, if any.
    /// </summary>
    public ChartLayoutOverride? Layout { get; init; }

    /// <summary>
    /// Gets the chart strategies override to apply to the chart configuration.
    /// </summary>
    public ChartStrategiesOverride? Strategies { get; init; }
}

