namespace FluentUI.Blazor.Community.Components.Charts.Options;

/// <summary>
/// Represents the options for a scatter chart serie.
/// </summary>
public sealed class ScatterSerieOptions : CategorySerieOptions
{
    /// <summary>
    /// Gets the radius of the points in the scatter chart.
    /// </summary>
    public double Radius { get; init; } = 3;
}
