using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

public partial class Chevron
{
    /// <summary>
    /// Gets or sets the size of the circle in pixels.
    /// </summary>
    /// <remarks>Default is 24.</remarks>
    [Parameter]
    public int Size { get; set; } = 24;

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
}
