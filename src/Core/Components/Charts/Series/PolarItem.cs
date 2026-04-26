namespace FluentUI.Blazor.Community.Components.Charts.Series;

/// <summary>
/// Represents a single item in a polar chart, which is defined by an angular category and a radial value.
/// </summary>
public sealed class PolarItem : ChartItem
{
    /// <summary>
    /// Gets the numeric value (radial magnitude) associated with this item.
    /// </summary>
    public required double Value { get; init; }
}

