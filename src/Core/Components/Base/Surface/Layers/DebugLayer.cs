using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a rendering layer used for displaying debug information within the visualization pipeline.
/// </summary>
/// <remarks>This layer is intended for diagnostic or development purposes and overlays debug-specific content on
/// top of other visualization layers. It uses a provided debug payload to determine what information to render. The
/// layer is assigned the highest priority and is always rendered as an overlay.</remarks>
public sealed class DebugLayer : ILayer<DebugPayload>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DebugLayer" /> class with the specified axes payload.
    /// </summary>
    /// <param name="payload">The debug payload containing the data and configuration for rendering the debug information.</param>
    public DebugLayer(DebugPayload payload)
    {
        ArgumentNullException.ThrowIfNull(payload, nameof(payload));

        LayerPayload = payload;
    }

    /// <inheritdoc />
    public string Key => "debug";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Overlay;

    /// <inheritdoc />
    public int Priority => (int)LayerPriority.Highest;

    /// <inheritdoc />
    public ILayerPayload LayerPayload { get; }
}

