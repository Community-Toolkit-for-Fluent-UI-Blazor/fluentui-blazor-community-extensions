namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the payload containing dynamic data and configuration for a signature frame, including view settings,
/// stroke information, selection state, and optional debug details.
/// </summary>
/// <remarks>This record aggregates multiple payloads relevant to the state and rendering of a dynamic signature
/// frame. It is typically used to transfer or persist the complete state of the frame, including user interactions and
/// visual configuration. All properties are mutable to support dynamic updates during the frame's lifecycle.</remarks>
public sealed record DynamicFramePayload
{
    /// <summary>
    /// Gets or sets the view payload that contains information
    ///  about the current view settings of the signature frame.
    /// </summary>
    public ViewPayload View { get; set; } = new();

    /// <summary>
    /// Gets or sets the payload representing the stroke layer configuration.
    /// </summary>
    public StrokeLayerPayload? StrokeLayer { get; set;  }

    /// <summary>
    /// Gets or sets the dynamic stroke payload.
    /// </summary>
    public DynamicStrokePayload? DynamicStroke { get; set; }

    /// <summary>
    /// Gets or sets the current selection payload.
    /// </summary>
    public SelectionPayload? Selection { get; set; }

    /// <summary>
    /// Gets or sets the debug information.
    /// </summary>
    public DebugPayload? Debug { get; set; }

    /// <summary>
    /// Gets the collection of flags indicating which properties have been modified since the last reset.
    /// </summary>
    public DirtyFlags Dirty { get; } = new();
}
