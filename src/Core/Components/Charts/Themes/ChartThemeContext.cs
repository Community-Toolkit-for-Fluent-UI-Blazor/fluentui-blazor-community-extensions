namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Provides contextual theme information for chart components, including the current chart theme settings.
/// </summary>
/// <remarks>This record is typically used to supply theme-related data to chart components, enabling consistent
/// styling and appearance throughout the charting system. The context may be extended to include additional theming or
/// configuration options as needed.</remarks>
public sealed record ChartThemeContext
{
    /// <summary>
    /// Gets the theme settings used to style the chart.
    /// </summary>
    /// <remarks>Use this property to customize the appearance of the chart, such as colors, fonts, and other
    /// visual elements, by providing a specific theme configuration.</remarks>
    public ChartTheme Theme { get; init; } = new();

    /// <summary>
    /// Gets the theme overrides that can be applied to the chart.
    /// </summary>
    public ChartThemeOverride? Overrides { get; init; }

    /// <summary>
    /// Gets the density settings that control the spacing and layout of chart elements.
    /// </summary>
    public ChartDensity Density { get; init; } = new();

    /// <summary>
    /// Gets the contrast settings used to adjust the visual appearance of the chart for accessibility or visual
    /// clarity.
    /// </summary>
    public ChartContrast Contrast { get; init; } = new();

    /// <summary>
    /// Gets the computed values for the chart, including calculated data points and related metrics.
    /// </summary>
    public ChartComputedValues ComputedValues { get; init; } = new();
}
