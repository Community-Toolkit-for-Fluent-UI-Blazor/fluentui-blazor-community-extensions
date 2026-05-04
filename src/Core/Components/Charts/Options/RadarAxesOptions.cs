using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Options;

/// <summary>
/// Represents the configuration options for the axes of a radar chart.
/// </summary>
public class RadarAxesOptions
{
    /// <summary>
    /// Gets or sets the type of grid to render (circle or polygon).
    /// </summary>
    public RadarGridType Grid { get; set; } = RadarGridType.Circle;

    /// <summary>
    /// Gets or sets the number of concentric grid levels.
    /// </summary>
    public int? GridLevels { get; set; }
}
