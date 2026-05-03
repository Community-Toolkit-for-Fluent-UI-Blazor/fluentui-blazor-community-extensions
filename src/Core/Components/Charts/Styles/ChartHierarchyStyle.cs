using FluentUI.Blazor.Community.Components.Surface.Utils;

namespace FluentUI.Blazor.Community.Components.Charts.Styles;

internal sealed class ChartHierarchyStyle
{
    private const string BaseFill = "#4A90E2";

    /// <summary>
    /// Gets the normal visual state style for hierarchy elements in a chart.
    /// </summary>
    public ChartVisualStateStyle Normal { get; init; } = new()
    {
        Fill = BaseFill,
        Stroke = SurfaceColorUtils.Darken(BaseFill, 0.15),
        StrokeWidth = 2,
        Opacity = 1
    };

    /// <summary>
    /// Gets the hover visual state style for hierarchy elements in a chart.
    /// </summary>
    public ChartVisualStateStyle Hover { get; init; } = new()
    {
        Fill = SurfaceColorUtils.Lighten(BaseFill, 0.15),
        Stroke = SurfaceColorUtils.Darken(BaseFill, 0.05),
        StrokeWidth = 2,
        Opacity = 1
    };

    /// <summary>
    /// Gets the pressed visual state style for hierarchy elements in a chart.
    /// </summary>
    public ChartVisualStateStyle Pressed { get; init; } = new()
    {
        Fill = SurfaceColorUtils.Darken(BaseFill, 0.25),
        Stroke = SurfaceColorUtils.Darken(BaseFill, 0.35),
        StrokeWidth = 2.5,
        Opacity = 0.9
    };

    /// <summary>
    /// Gets the selected visual state style for hierarchy elements in a chart.
    /// </summary>
    public ChartVisualStateStyle Selected { get; init; } = new()
    {
        Fill = SurfaceColorUtils.Darken(BaseFill, 0.35),
        Stroke = "#FFD700",
        StrokeWidth = 3,
        Opacity = 1
    };

    /// <summary>
    /// Gets the selected visual state style for hierarchy elements in a chart.
    /// </summary>
    public ChartVisualStateStyle Disabled { get; init; } = new()
    {
        Fill = SurfaceColorUtils.Lighten(BaseFill, 0.5),
        Stroke = SurfaceColorUtils.Darken(BaseFill, 0.5),
        StrokeWidth = 1,
        Opacity = 0.5
    };
}
