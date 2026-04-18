namespace FluentUI.Blazor.Community.Components.Charts.Styles;

/// <summary>
/// Represents the default line styles for marker lines in a chart.
/// </summary>
internal sealed class ChartMarkerLineStyle
{
    /// <summary>
    /// Gets the normal visual state.
    /// </summary>
    public ChartVisualStateStyle Normal { get; init; } = new()
    {
        Stroke = "#FFFFFF",
        StrokeWidth = 1,
        Opacity = 1,
        Cursor = "default",
        Fill = "#4a90e2",
        Filter = null,
        Shadow = null
    };

    /// <summary>
    /// Gets the hover visual state.
    /// </summary>
    public ChartVisualStateStyle Hover { get; init; } = new();

    /// <summary>
    /// Gets the pressed visual state.
    /// </summary>
    public ChartVisualStateStyle Pressed { get; init; } = new();

    /// <summary>
    /// Gets the selected visual state.
    /// </summary>
    public ChartVisualStateStyle Selected { get; init; } = new();
}
