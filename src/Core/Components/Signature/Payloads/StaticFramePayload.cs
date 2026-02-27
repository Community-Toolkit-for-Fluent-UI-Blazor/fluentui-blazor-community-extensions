namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the payload containing configuration and content data for a static signature frame, including view
/// settings, background, grid, axes, and watermark information.
/// </summary>
/// <remarks>This class aggregates multiple payloads that define the appearance and configuration of a static
/// signature frame. Each property corresponds to a specific aspect of the frame's layout or visual presentation. All
/// properties are optional except for the view payload, which is initialized by default.</remarks>
public sealed class StaticFramePayload
{
    /// <summary>
    /// Gets or sets the view payload that contains information
    ///  about the current view settings of the signature frame.
    /// </summary>
    public ViewPayload View { get; set; } = new();

    /// <summary>
    /// Gets or sets the background payload associated with the component.
    /// </summary>
    public BackgroundPayload? Background { get; set; }

    /// <summary>
    /// Gets or sets the grid payload associated with this instance.
    /// </summary>
    public GridPayload? Grid { get; set; }

    /// <summary>
    /// Gets or sets the axes configuration payload.
    /// </summary>
    public AxesPayload? Axes { get; set; }

    /// <summary>
    /// Gets or sets the watermark payload containing information about the watermark to be applied to the signature frame.
    /// </summary>
    public WatermarkPayload? Watermark { get; set; }

    /// <summary>
    /// Gets or sets the hover payload.
    /// </summary>
    public HoverPayload? Hover { get; set; }

    /// <summary>
    /// Gets the collection of flags indicating which properties have been modified since the last reset.
    /// </summary>
    public DirtyFlags Dirty { get; } = new();
}
