namespace FluentUI.Blazor.Community.Components.Charts.Series;

/// <summary>
/// Represents a single item in a categorical chart.
/// </summary>
public sealed class CategoryItem : ChartItem
{
    /// <summary>
    /// Gets the category key associated with this item.
    /// </summary>
    public required string Category { get; init; }

    /// <summary>
    /// Gets the numeric value associated with this item.
    /// </summary>
    public required double Value { get; init; }
}
