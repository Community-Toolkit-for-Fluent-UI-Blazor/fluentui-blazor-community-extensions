using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a layer that renders dynamic stroke selections using the provided selection payload.
/// </summary>
/// <remarks>This layer is typically used to display or process dynamic stroke-based selections within a rendering
/// context. It is configured with a payload that supplies the necessary data for the selection's appearance and
/// behavior. The layer is assigned a high priority and is ordered as content within the layer stack.</remarks>
public sealed class DynamicStrokeLayer : ILayer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DynamicStrokeLayer" /> class with the specified selection payload.
    /// </summary>
    /// <param name="payload">The selection payload containing the necessary information for rendering or processing the selection layer</param>
    public DynamicStrokeLayer(DynamicStrokePayload payload)
    {
        LayerPayload = payload;
    }

    /// <inheritdoc />
    public string Key => "dynamic-stroke";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Content;

    /// <inheritdoc />
    public int Priority => (int)LayerPriority.High;

    /// <inheritdoc />
    public ILayerPayload LayerPayload { get; }
}

