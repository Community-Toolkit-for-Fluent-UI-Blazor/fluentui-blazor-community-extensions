namespace FluentUI.Blazor.Community.Components.Charts.Series;

/// <summary>
/// Represents a slice in a radial chart (pie, donut).
/// </summary>
public sealed class RadialSlice : ChartItem
{
    /// <summary>
    /// Gets the numeric value represented by this slice.
    /// </summary>
    public required double Value { get; init; }
}
