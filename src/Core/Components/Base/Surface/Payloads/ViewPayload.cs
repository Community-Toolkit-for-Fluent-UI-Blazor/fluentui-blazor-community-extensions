namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the layout and rendering parameters for a visual view, including canvas dimensions, render area, and
/// scaling settings.
/// </summary>
/// <remarks>This class encapsulates both the physical canvas size and the logical render area, along with global
/// display parameters such as DPI and scaling. It is typically used to convey view configuration data between
/// components that perform rendering or layout calculations.</remarks>
public sealed class ViewPayload
{
    /// <summary>
    /// Gets or sets the width of the surface area in device-independent units.
    /// </summary>
    public double Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the surface in device-independent units.
    /// </summary>
    public double Height { get; set; }

    /// <summary>
    /// Gets or sets the horizontal position, in pixels, at which the component is rendered from the left edge of its
    /// container.
    /// </summary>
    public double OffsetX { get; set; }

    /// <summary>
    /// Gets or sets the vertical offset, in device-independent units (DIPs), at which the content is rendered from the
    /// top edge.
    /// </summary>
    public double OffsetY { get; set; }

    /// <summary>
    /// Gets or sets the width of the render area.
    /// </summary>
    public double RenderWidth { get; set; }

    /// <summary>
    /// Gets or sets the height of the render area.
    /// </summary>
    public double RenderHeight { get; set; }

    /// <summary>
    /// Gets or sets the dots per inch (DPI) value used for rendering or measurement operations.
    /// </summary>
    public double Dpi { get; set; }

    /// <summary>
    /// Gets or sets the scale factor applied to the component.
    /// </summary>
    public double Scale { get; set; }
}

