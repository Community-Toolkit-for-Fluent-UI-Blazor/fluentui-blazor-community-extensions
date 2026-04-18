using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines the contract for a composable layer within a rendering or processing pipeline.
/// </summary>
/// <remarks>Implementations of this interface represent individual layers that can be ordered, prioritized, and
/// composed using a frame builder. Layers are typically used to encapsulate distinct processing or rendering logic and
/// can be managed collectively within a layered architecture.</remarks>
public interface ILayer
{
    /// <summary>
    /// Gets the unique identifier associated with the current instance.
    /// </summary>
    string Key { get; }

    /// <summary>
    /// Gets the order in which the layer is rendered relative to other layers.
    /// </summary>
    LayerOrder Order { get; }

    /// <summary>
    /// Gets the priority level assigned to the layer.
    /// </summary>
    int Priority { get; }

    /// <summary>
    /// Gets the payload associated with the layer.
    /// </summary>
    /// <remarks>Use this property to access the underlying payload when the specific type is not known at
    /// compile time. The returned object implements the ILayerPayload interface, allowing for flexible handling of
    /// different payload types.</remarks>
    ILayerPayload LayerPayload { get; }
}
