namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a surface renderer for one-dimensional barcodes that adds the barcode payload as a rendering layer to the
/// specified surface target.
/// </summary>
/// <remarks>This renderer does not perform any visual drawing itself. Instead, it attaches the barcode payload to
/// the rendering target as a layer, allowing further processing or rendering by other components. This class is
/// typically used in scenarios where barcode data needs to be represented as a layer within a rendering pipeline rather
/// than being directly drawn.</remarks>
/// <param name="barcodeData">The raw data to be encoded into the barcode.</param>
/// <param name="barcodeOptions">The options used to configure the barcode encoding process.</param>
/// <param name="encoder">The encoder responsible for converting the raw barcode data into a format suitable for rendering.</param>
/// <param name="composer">The composer responsible for creating the barcode payload based on the encoded data and rendering options.</param>
internal sealed class Barcode1DStackedComposer<TBarcodeOptions>(
    IBarcodeEncoder<Barcode1DStackedPayload, TBarcodeOptions> encoder,
    Func<string> barcodeData,
    TBarcodeOptions barcodeOptions,
    Barcode1DStackedLayerComposer<TBarcodeOptions>? composer = null
    ) : ISurfaceComposer<BarcodeRenderingOptions> where TBarcodeOptions : IBarcode1DStackedOptions
{
    /// <summary>
    /// Gets the options used to configure the barcode generation or recognition process.
    /// </summary>
    public TBarcodeOptions Options => barcodeOptions;

    /// <inheritdoc />
    public bool Compose(
        ISurfaceRenderTarget target,
        BarcodeRenderingOptions options)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(options);

        var realComposer = composer ?? new Barcode1DStackedLayerComposer<TBarcodeOptions>();
        var encodedData = encoder.Encode(barcodeData(), barcodeOptions);
        var payload = realComposer.Compose(encodedData, options, barcodeOptions);

        target.AddLayer(new BarcodeLayer<Barcode1DStackedPayload>(payload));

        return true;
    }

    /// <inheritdoc />
    public ValueTask<bool> ComposeAsync(
        ISurfaceRenderTarget target,
        BarcodeRenderingOptions options)
    {
        return ValueTask.FromResult(Compose(target, options));
    }
}

