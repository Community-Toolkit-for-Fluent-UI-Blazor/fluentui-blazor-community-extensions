using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents configuration options for rendering axes in a signature input component.
/// </summary>
/// <remarks>Use this class to customize the appearance and behavior of axes displayed within a signature canvas,
/// such as visibility, color, opacity, stroke style, margin, scaling, and rendering layer. These options allow for
/// fine-grained control over how axes are presented to users when capturing or displaying signatures.</remarks>
public class SurfaceAxesOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether the content is visible.
    /// </summary>
    public bool Show { get; set; } = true;

    /// <summary>
    /// Gets or sets the color value in hexadecimal format.
    /// </summary>
    public string Color { get; set; } = "#000000";

    /// <summary>
    /// Gets or sets the opacity level of the component.
    /// </summary>
    /// <remarks>The opacity value determines the transparency of the component, where 1.0 is fully opaque and
    /// 0.0 is fully transparent. Values outside the range of 0.0 to 1.0 may not be supported and could result in
    /// undefined behavior.</remarks>
    public double Opacity { get; set; } = 1.0;

    /// <summary>
    /// Gets or sets the width of the stroke used to render the component.
    /// </summary>
    public double StrokeWidth { get; set; } = 2.0;

    /// <summary>
    /// Gets or sets the dash pattern used to render the outline of a shape or path.
    /// </summary>
    /// <remarks>The dash pattern is specified as a string of comma-separated numbers, where each number
    /// represents the length of dashes and gaps in the pattern. For example, "5,2" creates a pattern of a 5-unit dash
    /// followed by a 2-unit gap. If the value is null or empty, a solid line is rendered.</remarks>
    public string? DashArray { get; set; }

    /// <summary>
    /// Gets or sets the grid layer on which the component is rendered.
    /// </summary>
    /// <remarks>Use this property to control whether the component appears above or below other grid
    /// elements, such as strokes or content layers. The default value is <see
    /// cref="AxesLayerOrder.Background"/>.</remarks>
    public AxesLayerOrder Layer { get; set; } = AxesLayerOrder.Background;

    /// <summary>
    /// Resets all properties of the object to their default values.
    /// </summary>
    /// <remarks>Use this method to restore the object's state to its initial configuration. This is useful
    /// when reusing the object or discarding any customizations that have been applied.</remarks>
    internal void Reset()
    {
        Show = true;
        Color = "#000000";
        Opacity = 1.0;
        StrokeWidth = 2.0;
        DashArray = null;
        Layer = AxesLayerOrder.Background;
    }
}
