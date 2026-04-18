using FluentUI.Blazor.Community.Components.Surface.Utils;

namespace FluentUI.Blazor.Community.Components.Charts.Styles;

/// <summary>
/// Represents the default radial styles for radial elements in a chart, such as pie or donut segments.
/// </summary>
internal sealed class ChartPieStyle
{
    private const string BaseFill = "#FF7F50";

    /// <summary>
    /// Gets the normal visual state.
    /// </summary>
    public ChartVisualStateStyle Normal { get; init; } = new()
    {
        Fill = BaseFill,
        Stroke = SurfaceColorUtils.Darken(BaseFill, 0.15),
        StrokeWidth = 1.5,
        Opacity = 1
    };

    /// <summary>
    /// Gets the hover visual state.
    /// </summary>
    public ChartVisualStateStyle Hover { get; init; } = new()
    {
        Fill = SurfaceColorUtils.Lighten(BaseFill, 0.15),
        Stroke = SurfaceColorUtils.Darken(BaseFill, 0.05),
        StrokeWidth = 2,
        Opacity = 1
    };

    /// <summary>
    /// Gets the pressed visual state.
    /// </summary>
    public ChartVisualStateStyle Pressed { get; init; } = new()
    {
        Fill = SurfaceColorUtils.Darken(BaseFill, 0.25),
        Stroke = SurfaceColorUtils.Darken(BaseFill, 0.35),
        StrokeWidth = 2.5,
        Opacity = 0.9
    };

    /// <summary>
    /// Gets the selected visual state.
    /// </summary>
    public ChartVisualStateStyle Selected { get; init; } = new()
    {
        Fill = SurfaceColorUtils.Darken(BaseFill, 0.35),
        Stroke = "#FFD700",
        StrokeWidth = 3,
        Opacity = 1
    };
}
