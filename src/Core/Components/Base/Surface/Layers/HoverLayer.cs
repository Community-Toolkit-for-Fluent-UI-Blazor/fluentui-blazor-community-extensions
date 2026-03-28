using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a layer responsible for rendering hover.
/// </summary>
/// <remarks>The HoverLayer is typically used to display visual feedback, such as highlighting or tooltips, when a
/// user hovers over elements within a grid. It is positioned at the front of the layer stack and has high priority to
/// ensure that hover effects are rendered above other layers.</remarks>
public sealed class HoverLayer : ILayer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HoverLayer" /> class with the specified grid payload.
    /// </summary>
    /// <param name="payload">The hover payload containing the necessary information for rendering the hover layer.</param>
    public HoverLayer(HoverPayload payload)
    {
        LayerPayload = payload;
    }

    /// <inheritdoc />
    public string Key => "hover";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Front;

    /// <inheritdoc />
    public LayerPriority Priority => LayerPriority.High;

    /// <inheritdoc />
    public ILayerPayload LayerPayload { get; }
}
