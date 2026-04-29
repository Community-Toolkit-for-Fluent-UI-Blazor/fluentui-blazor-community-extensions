namespace FluentUI.Blazor.Community.Components.Charts.Factories;

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
    /// Gets the factory for creating stacked 100% bar chart axes.
    /// </summary>
    public static Stacked100BarAxisFactory Stacked100Bar { get; } = new();

    /// <summary>
    /// Gets the factory for creating stacked column axis components.
    /// </summary>
    public static StackedColumnAxisFactory StackedColumn { get; } = new();

    /// <summary>
    /// Gets the factory for creating stacked 100% column chart axes.
    /// </summary>
    public static Stacked100ColumnAxisFactory Stacked100Column { get; } = new();

    /// <summary>
    /// Gets the factory for creating stacked area axis components.
    /// </summary>
    public static StackedCategoryLineAxisFactory StackedArea { get; } = new();

    /// <summary>
    /// Gets the factory for creating stacked 100% area chart axes.
    /// </summary>
    public static Stacked100CategoryLineAxisFactory Stacked100Area { get; } = new();

    /// <summary>
    /// Gets the factory for creating step axis components.
    /// </summary>
    public static StepAxisFactory Step { get; } = new();

    /// <summary>
    /// Gets the factory for creating stacked step axis configurations.
    /// </summary>
    public static StackedStepAxisFactory StackedStep { get; } = new();

    /// <summary>
    /// Gets the factory for creating stacked 100% step axis configurations.
    /// </summary>
    public static Stacked100StepAxisFactory Stacked100Step { get; } = new();

    /// <summary>
    /// Gets the factory for creating scatter axis configuration.
    /// </summary>
    public static ScatterAxesFactory Scatter { get; } = new();

    /// <summary>
    /// Gets the factory for creating bubble axis configuration.
    /// </summary>
    public static BubbleAxesFactory Bubble { get; } = new();

    /// <summary>
    /// Gets the factory for creating polar axis configuration.
    /// </summary>
    public static PolarAxisFactory Polar { get; } = new();

    /// <summary>
    /// Gets the factory for creating xy axis configuration.
    /// </summary>
    public static XYAxesFactory XY { get; } = new();
}
