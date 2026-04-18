using FluentUI.Blazor.Community.Components.Charts.Styles;

namespace FluentUI.Blazor.Community.Components.Charts.Series;

/// <summary>
/// Defines styling for a chart item across all interaction states.
/// </summary>
public sealed class ChartItemStyle
{
    /// <summary>
    /// Gets the style applied to the chart when it is in the normal visual state.
    /// </summary>
    public ChartVisualStateStyle? Normal { get; init; }

    /// <summary>
    /// Gets the style applied to the chart when it is in the hover visual state.
    /// </summary>
    public ChartVisualStateStyle? Hover { get; init; }

    /// <summary>
    /// Gets the style applied to the chart when it is in the pressed visual state.
    /// </summary>
    public ChartVisualStateStyle? Pressed { get; init; }

    /// <summary>
    /// Gets the style applied to the chart when it is in the hover visual state.
    /// </summary>
    public ChartVisualStateStyle? Selected { get; init; }
}
