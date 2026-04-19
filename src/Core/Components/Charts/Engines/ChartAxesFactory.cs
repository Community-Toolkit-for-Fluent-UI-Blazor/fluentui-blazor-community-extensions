namespace FluentUI.Blazor.Community.Components.Charts.Engines;

/// <summary>
/// Represents a factory for creating chart axes based on the types of series present in the chart.
/// </summary>
internal static class ChartAxesFactory
{
    /// <summary>
    /// Gets the factory for creating and configuring bar axis components.
    /// </summary>
    public static BarAxisFactory Bar { get; } = new();

    /// <summary>
    /// Gets the factory instance for creating column axis components.
    /// </summary>
    public static ColumnAxisFactory Column { get; } = new();

    /// <summary>
    /// Gets the factory for creating category line axis components.
    /// </summary>
    /// <remarks>Use this property to access methods and configuration options for generating category line
    /// axes in charts or visualizations.</remarks>
    public static CategoryLineAxisFactory CategoryLine { get; } = new();

    /// <summary>
    /// Gets the factory for creating stacked bar axis components.
    /// </summary>
    public static StackedBarAxisFactory StackedBar { get; } = new();

    /// <summary>
    /// Gets the factory for creating stacked column axis components.
    /// </summary>
    public static StackedColumnAxisFactory StackedColumn { get; } = new();
}
