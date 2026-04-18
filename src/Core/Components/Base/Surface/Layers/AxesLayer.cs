using FluentUI.Blazor.Community.Components.Enums;
using FluentUI.Blazor.Community.Components.Surface.Payloads;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a layer responsible for rendering axes or background elements within a frame, using the specified frame
/// builder and background payload.
/// </summary>
/// <remarks>This class is typically used as part of a layered rendering system, where each layer contributes
/// specific visual elements to the final output. The AxesLayer is initialized with a frame builder and a background
/// payload, which provide the necessary configuration and data for rendering. Instances of this class are immutable
/// after construction.</remarks>
public sealed class AxesLayer : ILayer<AxisPayload>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AxesLayer" /> class with the specified frame builder and axes payload.
    /// </summary>
    /// <param name="payload">The axes payload containing the necessary data and configuration for rendering the axes.</param>
    public AxesLayer(
        AxisPayload payload)
    {
        ArgumentNullException.ThrowIfNull(payload, nameof(payload));

        LayerPayload = payload;
    }

    /// <inheritdoc />
    public string Key => "axes";

    /// <inheritdoc />
    public LayerOrder Order => ((AxisPayload)LayerPayload).Layer == AxesLayerOrder.Background ? LayerOrder.Background : LayerOrder.Front;

    /// <inheritdoc />
    public int Priority => (int)LayerPriority.Low;

    /// <inheritdoc />
    public ILayerPayload LayerPayload { get; private set; }
}
