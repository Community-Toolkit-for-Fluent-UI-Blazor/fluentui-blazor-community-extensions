using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an individual item in a legend, including its display text, color, shape, and size properties.
/// </summary>
/// <remarks>Use this class to define the appearance and label of a legend entry in a chart or visualization. Each
/// property controls a specific aspect of the legend item's visual representation, such as its text, fill color, border
/// color, border width, size, and shape.</remarks>
public partial class LegendItem
{
    /// <summary>
    /// Initializes a new instance of the LegendItem class with a unique identifier.
    /// </summary>
    public LegendItem()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the text content to be displayed or processed by the component.
    /// </summary>
    [Parameter]
    public string Text { get; set; } = "";

    /// <summary>
    /// Gets or sets the fill color used to render the component.
    /// </summary>
    /// <remarks>The fill color should be specified as a valid CSS color value, such as a hex code (e.g.,
    /// "#000"), RGB, RGBA, or named color. The default value is "#000" (black).</remarks>
    [Parameter]
    public string Fill { get; set; } = "#000";

    /// <summary>
    /// Gets or sets the color used to draw the outline of the component.
    /// </summary>
    /// <remarks>Specify a valid CSS color value, such as a hex code (e.g., "#fff"), color name, or RGB/RGBA
    /// value. The default is white ("#fff").</remarks>
    [Parameter]
    public string Stroke { get; set; } = "#fff";

    /// <summary>
    /// Gets or sets the width of the stroke used to render the component, in pixels.
    /// </summary>
    [Parameter]
    public int StrokeWidth { get; set; } = 2;

    /// <summary>
    /// Gets or sets the size of the component, in pixels.
    /// </summary>
    [Parameter]
    public int Size { get; set; } = 24;

    /// <summary>
    /// Gets or sets the shape used to display the legend item indicator.
    /// </summary>
    [Parameter]
    public LegendItemShape Shape { get; set; } = LegendItemShape.RoundSquare;
}
