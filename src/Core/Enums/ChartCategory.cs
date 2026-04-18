namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Specifies the available categories of charts supported by the library.
/// </summary>
/// <remarks>Use this enumeration to indicate the general type of chart to render or process. Each category groups
/// related chart types, such as bar and column charts under Category, or pie and donut charts under Polar. Selecting
/// the appropriate category can help determine suitable visualizations and behaviors for data presentation.</remarks>
public enum ChartCategory
{
    /// <summary>
    /// Represents a category or classification used to group related items or entities.
    /// </summary>
    Category,

    /// <summary>
    /// Represents a value or object related to the XY coordinate system or concept.
    /// </summary>
    XY,

    /// <summary>
    /// Represents a value or entity related to polar coordinates or polar systems.
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
    Hierarchy   // Treemap, Sunburst, Icicle
}

