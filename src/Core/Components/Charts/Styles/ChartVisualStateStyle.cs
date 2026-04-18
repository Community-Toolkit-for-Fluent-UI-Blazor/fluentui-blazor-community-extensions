namespace FluentUI.Blazor.Community.Components.Charts.Styles;

/// <summary>
/// Defines visual styling for a single interaction state.
/// </summary>
public sealed class ChartVisualStateStyle
{
    /// <summary>
    /// Gets the fill color to render the item.
    /// </summary>
    public string? Fill { get; init; }

    /// <summary>
    /// Gets the stroke color to render the item.
    /// </summary>
    public string? Stroke { get; init; }

    /// <summary>
    /// Gets the width of the stroke to render the item.
    /// </summary>
    public double? StrokeWidth { get; init; }

    /// <summary>
    /// Gets the opacity of the item.
    /// </summary>
    public double? Opacity { get; init; }

    /// <summary>
    /// Gets the cursor to display when hovering over the item.
    /// </summary>
    public string? Cursor { get; init; }

    /// <summary>
    /// Gets the filter to apply to the item.
    /// </summary>
    public string? Filter { get; init; }

    /// <summary>
    /// Gets the shadow to apply to the item.
    /// </summary>
    public string? Shadow { get; init; }

    /// <summary>
    /// Gets the scale factor to apply to the item.
    /// </summary>
    public double? Scale { get; init; }

    /// <summary>
    /// Gets the brightness level as a nullable value.
    /// </summary>
    public double? Brightness { get; init; }

    /// <summary>
    /// Gets the saturation value as a nullable value.
    /// </summary>
    public double? Saturation { get; init; }

    /// <summary>
    /// Gets the duration of the transition animation.
    /// </summary>
    public TimeSpan? TransitionDuration { get; init; }
}
