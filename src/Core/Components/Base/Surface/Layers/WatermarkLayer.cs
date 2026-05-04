using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a rendering layer responsible for displaying a watermark using the specified payload configuration.
/// </summary>
/// <remarks>This layer is typically used to overlay watermark content, such as text or images, on top of other
/// visual elements. It is positioned at the front of the rendering order with the lowest priority, ensuring that the
/// watermark appears above other layers but does not interfere with their rendering logic.</remarks>
public sealed class WatermarkLayer : ILayer<WatermarkPayload>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WatermarkLayer" /> class with the specified axes payload.
    /// </summary>
    /// <param name="payload">The watermark payload containing the necessary data and configuration for rendering the watermark.</param>
    public WatermarkLayer(WatermarkPayload payload)
    {
        ArgumentNullException.ThrowIfNull(payload, nameof(payload));

        LayerPayload = payload;
    }

    /// <inheritdoc />
    public string Key => "watermark";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Front;

    /// <inheritdoc />
    public int Priority => (int)LayerPriority.Lowest;

    /// <inheritdoc />
    public ILayerPayload LayerPayload { get; }
}
