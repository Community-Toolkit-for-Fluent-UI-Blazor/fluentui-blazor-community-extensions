namespace FluentUI.Blazor.Community.Components.Charts.Series;

/// <summary>
/// Represents a single item in a categorical chart.
/// </summary>
public sealed class CategoryItem : ChartItem
{
    /// <summary>
    /// Gets the numeric value associated with this item.
    /// </summary>
    public required double Value { get; init; }
}
