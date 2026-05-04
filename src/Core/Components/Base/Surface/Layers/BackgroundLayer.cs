using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a layer responsible for handling background rendering logic within a layered composition system.
/// </summary>
/// <remarks>This class implements the ILayer interface for background-specific payloads. It is typically used to
/// manage and render background elements in a compositional framework, ensuring that background visuals are handled
/// separately from other layers such as content or overlays. Instances of this class are usually managed by a layer
/// manager or composition engine.</remarks>
public sealed class BackgroundLayer : ILayer<BackgroundPayload>
{
    /// <summary>
    /// Initializes a new instance of the BackgroundLayer class with the specified frame builder and background payload.
    /// </summary>
    /// <param name="payload">The background payload containing the necessary data and configuration for rendering the background.</param>
    public BackgroundLayer(BackgroundPayload payload)
    {
        ArgumentNullException.ThrowIfNull(payload, nameof(payload));

        LayerPayload = payload;
    }

    /// <inheritdoc />
    public string Key => "background";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Background;

    /// <inheritdoc />
    public int Priority => (int)LayerPriority.Lowest;

    /// <inheritdoc />
    public ILayerPayload LayerPayload { get; private set; }
}
