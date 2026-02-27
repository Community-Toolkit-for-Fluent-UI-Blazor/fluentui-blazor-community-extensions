namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines methods for configuring and rendering static surface elements such as background, grid, axes, and watermark,
/// and for flushing any buffered rendering data asynchronously.
/// </summary>
/// <remarks>Implementations of this interface allow consumers to programmatically control the visual
/// configuration of a static surface, such as a chart or canvas, by setting various rendering payloads. The interface
/// also provides a method to ensure that any pending rendering operations are completed asynchronously. Thread safety
/// and specific rendering behaviors depend on the concrete implementation.</remarks>
public interface IStaticSurfaceRender
{
    /// <summary>
    /// Gets the static frame payload associated with this instance.
    /// </summary>
    StaticFramePayload Frame { get; }

    /// <summary>
    /// Sets the background configuration for the component using the specified payload.
    /// </summary>
    /// <param name="background">The background payload to apply.</param>
    void SetBackground(BackgroundPayload? background);

    /// <summary>
    /// Sets the grid configuration using the specified payload.
    /// </summary>
    /// <param name="grid">The grid configuration to apply. If null, the grid will be reset to its default state.</param>
    void SetGrid(GridPayload? grid);

    /// <summary>
    /// Sets the axes configuration for the current context.
    /// </summary>
    /// <param name="axes">The axes payload to apply.</param>
    void SetAxes(AxesPayload? axes);

    /// <summary>
    /// Sets the watermark for the current context using the specified payload.
    /// </summary>
    /// <param name="watermark">The payload containing watermark information to apply. If null, removes any existing watermark.</param>
    void SetWatermark(WatermarkPayload? watermark);

    /// <summary>
    /// Sets the hover state using the specified payload.
    /// </summary>
    /// <param name="payload">The payload containing hover information to apply. If null, the hover state will be cleared.</param>
    void SetHover(HoverPayload? payload);
}
