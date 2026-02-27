namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides configuration options for controlling zoom and pan behavior in a signature view component.
/// </summary>
/// <remarks>Use this class to specify the minimum and maximum zoom levels, and to enable or disable panning and
/// zooming functionality. These options allow customization of the user interaction experience when viewing or editing
/// signatures.</remarks>
public sealed class SignatureViewOptions
{
    /// <summary>
    /// Gets or sets the horizontal position, in pixels, at which the component is rendered from the left edge of its
    /// container.
    /// </summary>
    public double RenderLeft { get; set; }

    /// <summary>
    /// Gets or sets the vertical offset, in device-independent units (DIPs), at which the content is rendered from the
    /// top edge.
    /// </summary>
    public double RenderTop { get; set; }

    /// <summary>
    /// Gets or sets the width, in device-independent units (DIPs), used for rendering the component.
    /// </summary>
    public double RenderWidth { get; set; }

    /// <summary>
    /// Gets or sets the height, in device-independent units (DIP), used to render the component.
    /// </summary>
    public double RenderHeight { get; set; }

    /// <summary>
    /// Gets or sets the dots per inch (DPI) value used for rendering or measurement operations.
    /// </summary>
    public double Dpi { get; set; }
}

