using FluentUI.Blazor.Community.Components.Enums;
using FluentUI.Blazor.Community.Components.Surface.Payloads;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a rendering layer for displaying a grid background or overlay within a visual component. This layer
/// determines its position and rendering order based on the provided grid payload.
/// </summary>
/// <remarks>The GridLayer is typically used to render grid lines or backgrounds in charting or visualization
/// components. Its rendering order is determined by the layer type specified in the payload, allowing it to appear
/// either behind or in front of other visual elements. This class is sealed and cannot be inherited.</remarks>
public sealed class GridLayer: ILayer<GridPayload>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GridLayer" /> class with the specified grid payload.
    /// </summary>
    /// <param name="payload">The grid payload containing the necessary information for rendering the grid layer.</param>
    public GridLayer(GridPayload payload)
    {
        ArgumentNullException.ThrowIfNull(payload, nameof(payload));

        LayerPayload = payload;
    }

    /// <inheritdoc />
    public string Key => "grid";

    /// <inheritdoc />
    public LayerOrder Order => ((GridPayload)LayerPayload).Layer == GridLayerOrder.Background ? LayerOrder.Background : LayerOrder.Front;

    /// <inheritdoc />
    public int Priority => (int)LayerPriority.Lowest;

    /// <inheritdoc />
    public ILayerPayload LayerPayload { get; private set; }
}
