using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Charts.Series;

namespace FluentUI.Blazor.Community.Components.Charts;

/// <summary>
/// Represents the engine responsible for processing category axis data in a chart.
/// </summary>
internal static class CategoryAxisEngine
{
    /// <summary>
    /// Returns a sorted list of category items based on their category names, or the original list if sorting is
    /// disabled in the options.
    /// </summary>
    /// <remarks>If the options parameter is null or its Sort property is false, the method returns the input
    /// list without modification. Otherwise, a new list is returned with items sorted by their Category
    /// property.</remarks>
    /// <param name="items">The collection of category items to sort.</param>
    /// <param name="options">The options that determine whether sorting is applied. If null or if sorting is disabled, the original order is
    /// preserved.</param>
    /// <returns>A read-only list of category items sorted by category name, or the original list if sorting is not enabled.</returns>
    public static IReadOnlyList<CategoryItem> Sort(
        IReadOnlyList<CategoryItem> items,
        CategorySerieOptions? options)
    {
        if (options?.Sort ?? false)
        {
            return items;
        }

        var sortedItems = new List<CategoryItem>(items);
        sortedItems.Sort((a, b) =>
        {
            return a.Category.CompareTo(b.Category);
        });

        return sortedItems!;
    }

    /// <summary>
    /// Calculates the center positions for each category along the specified chart axis.
    /// </summary>
    /// <remarks>The method uses the provided axis to map each category index to its corresponding
    /// center position. The mapping depends on the implementation of the axis's Map method.</remarks>
    /// <param name="categoryCount">The number of categories to compute centers for. Must be greater than zero.</param>
    /// <param name="axis">The chart axis used to map category indices to axis positions. Cannot be null.</param>
    /// <returns>An array of doubles representing the center positions of each category on the axis. The array length equals
    /// the specified category count.</returns>
    public static double[] ComputeCategoryCenters(
        int categoryCount,
        ChartAxis axis)
    {
        var centers = new double[categoryCount];

        var step = (axis.DataMaximum - axis.DataMinimum) / categoryCount;

        for (var i = 0; i < categoryCount; i++)
        {
            centers[i] = axis.Map(i);
        }

        return centers;
    }

    /// <summary>
    /// Calculates the width allocated to each category within the specified plot area.
    /// </summary>
    /// <param name="categoryCount">The number of categories to be displayed. Must be greater than zero to allocate width.</param>
    /// <param name="plotArea">The plot area in which the categories are rendered. The width of this area determines the total available
    /// space.</param>
    /// <returns>The width assigned to each category. Returns 0 if the category count is less than or equal to zero.</returns>
    public static double ComputeCategoryWidth(
        int categoryCount,
        ChartRect plotArea)
    {
        if (categoryCount <= 0)
        {
            return 0;
        }

        return plotArea.Width / categoryCount;
    }
}
