using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a layer that manages selection overlays within a layered UI system.
/// </summary>
/// <remarks>This layer is typically used to display selection highlights or handles above other content layers.
/// It is assigned a high priority and is rendered in the front order to ensure selection visuals are visible above
/// other elements. The associated payload provides the data required to render the selection state.</remarks>
public sealed class SelectionLayer : ILayer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SelectionLayer" /> class with the specified selection payload.
    /// </summary>
    /// <param name="payload">The selection payload containing the necessary information for rendering or processing the selection layer</param>
    public SelectionLayer(SelectionPayload payload)
    {
        LayerPayload = payload;
    }

    /// <inheritdoc />
    public string Key => "selection";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Front;

    /// <inheritdoc />
    public LayerPriority Priority => LayerPriority.High;

    /// <inheritdoc />
    public ILayerPayload LayerPayload { get; }
}
