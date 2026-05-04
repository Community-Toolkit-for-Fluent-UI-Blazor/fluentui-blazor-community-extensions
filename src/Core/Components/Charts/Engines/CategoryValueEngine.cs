using FluentUI.Blazor.Community.Components.Charts.Series;

namespace FluentUI.Blazor.Community.Components.Charts;

/// <summary>
/// Represents a utility class responsible for computing value ranges and mapping values to chart axes for category-based charts.
/// </summary>
internal static class CategoryValueEngine
{
    /// <summary>
    /// Calculates the minimum and maximum values from a collection of category items.
    /// </summary>
    /// <remarks>If all items have the same value, the method expands the range slightly to ensure the
    /// minimum and maximum are distinct. This can be useful for scenarios such as chart axis scaling.</remarks>
    /// <param name="items">The collection of category items to evaluate. Cannot be null.</param>
    /// <returns>A tuple containing the minimum and maximum values found in the collection. Returns (0, 0) if the collection
    /// is empty.</returns>
    public static (double Minimum, double Maximum) ComputeValueRange(IReadOnlyList<CategoryItem> items)
    {
        if (items.Count == 0)
        {
            return (0, 0);
        }

        var min = items.Min(i => i.Value);
        var max = items.Max(i => i.Value);

        if (min == max)
        {
            var delta = Math.Abs(min) > 0 ? Math.Abs(min) * 0.1 : 1.0;
            min -= delta;
            max += delta;
        }

        return (min, max);
    }

    /// <summary>
    /// Maps a numeric value to its corresponding position on the specified chart axis.
    /// </summary>
    /// <remarks>This method delegates the mapping operation to the specified axis. The result depends
    /// on the configuration and scaling of the provided axis.</remarks>
    /// <param name="value">The numeric value to map to the axis scale.</param>
    /// <param name="axis">The chart axis used to perform the mapping. Cannot be null.</param>
    /// <returns>The mapped position of the value on the axis scale as a double.</returns>
    public static double MapValue(double value, ChartAxis axis)
    {
        return axis.Map(value);
    }
}
