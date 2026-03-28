using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a layer that contains stroke data for rendering or processing within a layered graphics system.
/// </summary>
/// <remarks>The StrokeLayer class is used to encapsulate stroke-related information as a distinct layer, enabling
/// separation of content types in applications that support multiple rendering layers. It implements the ILayer
/// interface to ensure compatibility with systems that manage ordered and prioritized layers.</remarks>
public sealed class StrokeLayer : ILayer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StrokeLayer" /> class with the specified grid payload.
    /// </summary>
    /// <param name="payload">The stroke payload containing the necessary information for rendering or processing the stroke layer.</param>
    public StrokeLayer(StrokeLayerPayload payload)
    {
        LayerPayload = payload;
    }

    /// <inheritdoc />
    public string Key => "strokes";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Content;

    /// <inheritdoc />
    public LayerPriority Priority => LayerPriority.Normal;

    /// <inheritdoc />
    public ILayerPayload LayerPayload { get; }
}
