using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a layer that applies eraser strokes to a drawing surface using the specified payload.
/// </summary>
/// <remarks>The EraserLayer is typically used to process or render eraser actions within a layered drawing or
/// annotation system. It implements the ILayer interface, allowing it to be managed alongside other content layers. The
/// layer uses the provided EraserPayload to determine the eraser's behavior and appearance.</remarks>
public sealed class EraserLayer : ILayer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EraserLayer" /> class with the specified grid payload.
    /// </summary>
    /// <param name="payload">The stroke payload containing the necessary information for rendering or processing the stroke layer.</param>
    public EraserLayer(EraserPayload payload)
    {
        LayerPayload = payload;
    }

    /// <inheritdoc />
    public string Key => "eraser";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Overlay;

    /// <inheritdoc />
    public LayerPriority Priority => LayerPriority.Lowest;

    /// <inheritdoc />
    public ILayerPayload LayerPayload { get; }
}
