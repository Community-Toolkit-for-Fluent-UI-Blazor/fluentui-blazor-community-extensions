using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;

namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Represents a set of computed values used for rendering chart elements, including dimensions and colors for bars,
/// bubbles, lines, grids, axes, and text.
/// </summary>
/// <remarks>This record encapsulates the finalized visual parameters for a chart, typically determined after
/// applying user settings, theme defaults, or data-driven calculations. The values are intended to be used directly
/// when rendering chart components to ensure consistent appearance.</remarks>
public sealed record ChartComputedValues
{
    /// <summary>
    /// Gets the final corner radius applied to the bar element.
    /// </summary>
    public double FinalBarRadius { get; init; }

    /// <summary>
    /// Gets the minimum value for the final bubble size.
    /// </summary>
    public double FinalBubbleMin { get; init; }

    /// <summary>
    /// Gets the maximum value used for the final bubble size.
    /// </summary>
    public double FinalBubbleMax { get; init; }

    /// <summary>
    /// Gets the smoothing factor applied to the final line rendering.
    /// </summary>
    public double FinalLineSmoothing { get; init; }

    /// <summary>
    /// Gets the final thickness value applied to the grid after all calculations are complete.
    /// </summary>
    public double FinalGridThickness { get; init; }

    /// <summary>
    /// Gets the final thickness of the axis after all calculations and adjustments are applied.
    /// </summary>
    public double FinalAxisThickness { get; init; }

    /// <summary>
    /// Gets the final stroke thickness to be applied.
    /// </summary>
    public double FinalStrokeThickness { get; init; }

    /// <summary>
    /// Gets the final text color to be used for rendering content.
    /// </summary>
    public Srgb8 FinalTextColor { get; init; } = new(0, 0, 0);

    /// <summary>
    /// Gets the final color used for rendering the grid.
    /// </summary>
    public Srgb8 FinalGridColor { get; init; } = new(0, 0, 0);

    /// <summary>
    /// Gets the final color used for the axis.
    /// </summary>
    public Srgb8 FinalAxisColor { get; init; } = new(0, 0, 0);
}

