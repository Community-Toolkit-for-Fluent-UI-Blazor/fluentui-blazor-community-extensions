namespace FluentUI.Blazor.Community.Components.Charts.Options;

/// <summary>
/// Represents the grid options for the chart.
/// </summary>
internal sealed class ChartGridOptions : SurfaceGridOptions
{
    /// <summary>
    /// Gets a value indicating whether the horizontal grid lines should be displayed.
    /// </summary>
    public bool ShowHorizontal { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether the vertical grid lines should be displayed.
    /// </summary>
    public bool ShowVertical { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether the grid lines should snap to the axis ticks.
    /// </summary>
    public bool SnapToTicks { get; init; } = true;
}
