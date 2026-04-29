namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Represents the different types of line charts that can be used in a charting component.
/// </summary>
public enum LineChartType
{
    /// <summary>
    /// Represents a standard line chart where data points are connected by straight lines.
    /// </summary>
    Line,

    /// <summary>
    /// Represents an area chart where the area below the line is filled with color.
    /// </summary>
    Area,

    /// <summary>
    /// Represents a stacked area chart where multiple series are stacked on top of each other.
    /// </summary>
    StackedArea,

    /// <summary>
    /// Represents a 100% stacked area chart where multiple series are stacked and normalized to represent percentages.
    /// </summary>
    Stacked100Area,

    /// <summary>
    /// Represents a step line chart where data points are connected by horizontal and vertical lines, creating a step-like appearance.
    /// </summary>
    Step,

    /// <summary>
    /// Represents a stacked step line chart where multiple series are stacked and connected by horizontal and vertical lines, creating a step-like appearance.
    /// </summary>
    StackedStep,

    /// <summary>
    /// Represents a 100% stacked step chart where values are normalized to percentages and displayed with step
    /// interpolation.
    /// </summary>
    Stacked100Step,
}
