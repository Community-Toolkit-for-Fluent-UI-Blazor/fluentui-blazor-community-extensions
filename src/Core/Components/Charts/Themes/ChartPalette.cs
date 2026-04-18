using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;

namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Represents a color palette for chart components.
/// </summary>
public sealed record ChartPalette
{
    /// <summary>
    /// Gets the collection of sRGB color values that define the series.
    /// </summary>
    public IReadOnlyList<Srgb8> Series { get; init; } = [];

    /// <summary>
    /// Gets the collection of stroke colors used for rendering each data series.
    /// </summary>
    /// <remarks>The colors in this collection are applied in order to the corresponding data series. If there
    /// are more series than colors, the colors may be reused or default values may be applied, depending on the
    /// component's implementation.</remarks>
    public IReadOnlyList<Srgb8> StrokeSeries { get; init; } = [];

    /// <summary>
    /// Gets the background color used for rendering, represented as an sRGB value.
    /// </summary>
    public Srgb8 Background { get; init; } = new(255, 255, 255);

    /// <summary>
    /// Gets the foreground color used for rendering content.
    /// </summary>
    public Srgb8 Foreground { get; init; } = new(0, 0, 0);

    /// <summary>
    /// Gets the grid color represented in the sRGB color space.
    /// </summary>
    public Srgb8 Grid { get; init; } = new(200, 200, 200);

    /// <summary>
    /// Gets the axis color used for rendering or calculations.
    /// </summary>
    /// <remarks>The axis color is represented as an instance of the Srgb8 structure. The default value is
    /// (50, 50, 50).</remarks>
    public Srgb8 Axis { get; init; } = new(50, 50, 50);
}
