namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the payload for a canvas frame, including view information and associated layers.
/// </summary>
/// <remarks>This class is typically used to encapsulate the state of a canvas at a specific point in time, such
/// as for serialization, rendering, or communication between components. The payload includes both the overall view
/// settings and a collection of individual layer payloads, each identified by a unique string key.</remarks>
public class CanvasFramePayload
{
    /// <inheritdoc />
    public ViewPayload View { get; init; } = new();

    /// <inheritdoc />
    public IReadOnlyDictionary<string, object> Layers { get; set; } = new Dictionary<string, object>();
}
