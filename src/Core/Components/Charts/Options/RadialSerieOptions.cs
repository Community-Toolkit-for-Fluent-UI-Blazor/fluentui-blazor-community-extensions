namespace FluentUI.Blazor.Community.Components.Charts.Options;

/// <summary>
/// Provides configuration options for a radial chart series, such as inner and outer radii and label display settings.
/// </summary>
/// <remarks>Use this class to customize the appearance and labeling behavior of a radial chart series. The
/// options control aspects such as the thickness of the series and whether labels or percentage values are shown on the
/// chart.</remarks>
public sealed class RadialSerieOptions : IChartSerieOptions
{
    /// <summary>
    /// Gets the inner radius value for the radial chart serie.
    /// </summary>
    public double InnerRadius { get; set; } = 0.5;

    /// <summary>
    /// Gets the outer radius value for the radial chart serie.
    /// </summary>
    public double OuterRadius { get; set; } = 1.0;

    /// <summary>
    /// Gets a value indicating whether labels are displayed alongside the radial chart.
    /// </summary>
    public bool ShowLabels { get; set; } = true;

    /// <summary>
    /// Gets a value indicating whether percentage values are displayed alongside the radial chart.
    /// </summary>
    public bool ShowPercentages { get; set; }

    /// <summary>
    /// Gets the starting angle for the radial chart serie, in degrees.
    /// </summary>
    public double StartAngle { get; set; }

    /// <summary>
    /// Gets the end angle, in degrees, for the arc or segment.
    /// </summary>
    public double EndAngle { get; set; } = 360.0;

    /// <inheritdoc />
    public ChartAnimationOptions? Animation { get; set; }
}
