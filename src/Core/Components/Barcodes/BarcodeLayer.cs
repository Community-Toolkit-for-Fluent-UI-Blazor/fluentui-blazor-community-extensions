using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a rendering layer for displaying barcode elements using the specified payload type.
/// </summary>
/// <remarks>This layer is intended for use within a chart or visualization system that supports multiple
/// rendering layers. It encapsulates the logic and configuration required to render barcodes as part of the overall
/// visualization. The layer uses the provided payload to determine the barcode's appearance and data.</remarks>
/// <typeparam name="TPayload">The type of the data contained in the barcode payload used for rendering.</typeparam>
internal sealed class BarcodeLayer<TPayload>
    : ILayer<BarcodePayload<TPayload>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BarcodeLayer{TPayload}" /> class with the specified frame builder and axes payload.
    /// </summary>
    /// <param name="payload">The barcode payload containing the necessary data and configuration for rendering the barcode.</param>
    public BarcodeLayer(BarcodePayload<TPayload> payload)
    {
        LayerPayload = payload;
    }

    /// <inheritdoc />
    public string Key => "barcode";

    /// <inheritdoc />
    public LayerOrder Order => LayerOrder.Content;

    /// <inheritdoc />
    public int Priority => (int)LayerPriority.Normal;

    /// <inheritdoc />
    public ILayerPayload LayerPayload { get; }
}
