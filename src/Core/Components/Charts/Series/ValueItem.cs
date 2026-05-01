namespace FluentUI.Blazor.Community.Components.Charts.Series;

/// <summary>
/// Represents a value item for an histogram chart.
/// </summary>
public sealed class ValueItem : ChartItem
{
    /// <summary>
    /// Gets the value of the item, which represents the data point for the chart series.
    /// </summary>
    public double Value { get; set; }
}
