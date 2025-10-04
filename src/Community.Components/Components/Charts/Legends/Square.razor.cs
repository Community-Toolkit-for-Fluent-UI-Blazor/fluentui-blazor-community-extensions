using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

public partial class Square
{
    /// <summary>
    /// Gets or sets the size of the circle in pixels.
    /// </summary>
    /// <remarks>Default is 24.</remarks>
    [Parameter]
    public int Size { get; set; } = 24;

    /// <summary>
    /// Gets or sets the fill color for the component, specified as a CSS color string.
    /// </summary>
    /// <remarks>The default value is "#000000" (black). Accepts any valid CSS color format, such as
    /// hexadecimal, RGB, RGBA, or named colors. This property is typically used to control the visual appearance of SVG
    /// or HTML elements rendered by the component.</remarks>
    [Parameter]
    public string? Fill { get; set; } = "#000000";

    /// <summary>
    /// Gets or sets the color used to draw the outline of the shape.
    /// </summary>
    /// <remarks>The value should be a valid CSS color string, such as a hexadecimal color code (e.g.,
    /// "#000000"), an RGB value, or a named color. If not specified, the default outline color is black.</remarks>
    [Parameter]
    public string? Stroke { get; set; } = "#000000";

    /// <summary>
    /// Gets or sets the width of the stroke used to draw shapes, in pixels.
    /// </summary>
    /// <remarks>Default is 1.</remarks>
    [Parameter]
    public int StrokeWidth { get; set; } = 1;

    /// <summary>
    /// Gets or sets the radius of the circle.
    /// </summary>
    [Parameter]
    public double Radius { get; set; } = 10;

    /// <summary>
    /// Gets or sets the opacity level of the component, where 1.0 is fully opaque and 0.0 is fully transparent.
    /// </summary>
    /// <remarks>Valid values range from 0.0 to 1.0. Values outside this range may result in undefined
    /// behavior or be clamped depending on the rendering implementation.</remarks>
    [Parameter]
    public double Opacity { get; set; } = 1.0;
}
