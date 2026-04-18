namespace FluentUI.Blazor.Community.Components.Charts.Options;

/// <summary>
/// Gets the configuration options for a line series in a chart.
/// </summary>
public sealed class CategoryLineOptions : CategorySerieOptions
{
    /// <summary>
    /// Gets a value indicating whether smooth transitions are enabled.
    /// </summary>
    public bool Smooth { get; init; }

    /// <summary>
    /// Gets a value indicating whether markers are shown at data points.
    /// </summary>
    public bool ShowMarkers { get; init; } = true;

    /// <summary>
    /// Gets the size of the marker.
    /// </summary>
    public double MarkerSize { get; init; } = 4;

    /// <summary>
    /// Gets a value indicating whether the area should be displayed.
    /// </summary>
    public bool ShowArea { get; init; }
}
