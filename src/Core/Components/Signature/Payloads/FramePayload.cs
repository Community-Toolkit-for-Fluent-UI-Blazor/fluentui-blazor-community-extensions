namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the payload containing static and dynamic frame data for a component or operation.
/// </summary>
/// <remarks>This class encapsulates both static and dynamic frame payloads, allowing consumers to access or
/// modify the front, back, and dynamic frame data as needed. It is typically used to organize and transport
/// frame-related information within the application.</remarks>
public sealed class FramePayload
{
    /// <summary>
    /// Gets or sets the static frame payload used for the back frame.
    /// </summary>
    public StaticFramePayload StaticBack { get; set; } = new();

    /// <summary>
    /// Gets or sets the static front frame payload for the component.
    /// </summary>
    public StaticFramePayload StaticFront { get; set; } = new();

    /// <summary>
    /// Gets or sets the dynamic frame payload associated with this instance.
    /// </summary>
    public DynamicFramePayload Dynamic { get; set; } = new();

    /// <summary>
    /// Gets or sets the debug payload for the dynamic frame.
    /// </summary>
    public DynamicFramePayload Debug { get; set; } = new();

    /// <summary>
    /// Gets or sets the watermark payload.
    /// </summary>
    public StaticFramePayload Watermark { get; set; } = new();

    /// <summary>
    /// Gets or sets the hover payload.
    /// </summary>
    public StaticFramePayload Hover { get; set; } = new();
}
