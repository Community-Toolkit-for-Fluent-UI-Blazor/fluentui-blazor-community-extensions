namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a payload for surface, including view,
///  background, grid, axes, and content data of a specified type.
/// </summary>
/// <typeparam name="TPayload">The type of the content payload stored in the container.</typeparam>
public sealed class SurfacePayload<TPayload>
{
    /// <summary>
    /// Gets or sets the view payload.
    /// </summary>
    public ViewPayload? View { get; set; }

    /// <summary>
    /// Gets or sets the background payload.
    /// </summary>
    public BackgroundPayload? Background { get; set; }

    /// <summary>
    /// Gets or sets the grid payload.
    /// </summary>
    public GridPayload? Grid { get; set; }

    /// <summary>
    /// Gets or sets the axes payload.
    /// </summary>
    public AxesPayload? Axes { get; set; }

    /// <summary>
    /// Gets or sets the watermark payload.
    /// </summary>
    public WatermarkPayload? Watermark { get; set; }

    /// <summary>
    /// Gets or sets the content payload of the specified type.
    /// </summary>
    public TPayload? Content { get; set; }
}
