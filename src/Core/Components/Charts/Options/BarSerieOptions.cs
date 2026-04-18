namespace FluentUI.Blazor.Community.Components.Charts.Options;

/// <summary>
/// Represents configuration options for a bar series in a chart.
/// </summary>
public sealed class BarSerieOptions : CategorySerieOptions
{
    /// <summary>
    /// Gets the height of each bar in relative units (0–1).
    /// </summary>
    public double BarHeight { get; set; } = 0.8;
}

