namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the payload for configuring dynamic stroke behavior in the signature component, allowing for real-time updates to stroke properties based on user input or other dynamic factors.
/// </summary>
public sealed record DynamicStrokePayload : ILayerPayload
{
    /// <summary>
    /// Gets the list of stroke payloads.
    /// </summary>
    public List<StrokePayload> Strokes { get; init; } = [];

    /// <summary>
    /// Gets the collection of selected stroke payloads.
    /// </summary>
    public List<string> Selected { get; init; } = [];

    /// <summary>
    /// Gets the pen payload that defines the properties and behavior of the pen tool used for drawing strokes in the signature component. This includes attributes such as pen color, thickness, and style, which can be dynamically updated to provide a responsive and customizable drawing experience for users when creating their signatures.
    /// </summary>
    public PenPayload? Pen { get; init; }

    /// <summary>
    /// Gets the rectangular region that is currently selected, or null if no selection is active.
    /// </summary>
    public RectPayload? SelectionRect { get; init; }
}
