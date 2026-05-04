namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Specifies the available categories of charts supported by the library.
/// </summary>
/// <remarks>Use this enumeration to indicate the general type of chart to render or process. Each category groups
/// related chart types, such as bar and column charts under Category, or pie and donut charts under Polar. Selecting
/// the appropriate category can help determine suitable visualizations and behaviors for data presentation.</remarks>
public enum ChartFamily
{
    /// <summary>
    /// No specific chart category is assigned.
    /// </summary>
    None,

    /// <summary>
    /// Specifies a chart category that represents discrete data points grouped into categories, such as bar or column charts.
    /// </summary>
    Category,

    /// <summary>
    /// Specifies a chart category that represents continuous data points plotted along two axes, such as line or scatter charts.
    /// </summary>
    XY,

    /// <summary>
    /// Specifies a chart category that represents data visualized in a polar coordinate system, such as radar or polar area charts.
    /// </summary>
    Polar,

    /// <summary>
    /// Provides financial-related functionality and utilities.
    /// </summary>
    Financial,

    /// <summary>
    /// Specifies the hierarchy visualization type used for displaying data relationships.
    /// </summary>
    /// <remarks>Use this enumeration to select the desired hierarchical chart style, such as Treemap,
    /// Sunburst, or Icicle, when visualizing nested or parent-child data structures.</remarks>
    Hierarchy,   // Treemap, Sunburst, Icicle

    /// <summary>
    /// Specifies the circular visualization type used for displaying data in a circular layout.
    /// </summary>
    Circular,

    /// <summary>
    /// Specifies the histogram visualization type used for displaying data distributions in a bar chart format.
    /// </summary>
    Histogram
}

