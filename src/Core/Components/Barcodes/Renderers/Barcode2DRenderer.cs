namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a renderer for 2D barcodes using a specified encoder, barcode data source, and options.
/// </summary>
/// <typeparam name="TBarcodeOptions">The type of options used to configure barcode generation or recognition.</typeparam>
/// <param name="encoder">The encoder responsible for converting barcode payloads into encoded data using the specified options.</param>
/// <param name="barcodeData">A function that returns the string data to be encoded in the barcode.</param>
/// <param name="barcodeOptions">The options used to configure the barcode generation or recognition process.</param>
/// <param name="composer">An optional composer that arranges the encoded barcode data for rendering. If not provided, a default composer is
/// used.</param>
internal sealed class Barcode2DRenderer<TBarcodeOptions>(
    IBarcodeEncoder<Barcode2DPayload, TBarcodeOptions> encoder,
    Func<string> barcodeData,
    TBarcodeOptions barcodeOptions,
    Barcode2DComposer<TBarcodeOptions>? composer = null
    ) : ISurfaceRenderer<BarcodeRenderingOptions> where TBarcodeOptions : IBarcode2DOptions
{
    /// <summary>
    /// Gets the options used to configure the barcode generation or recognition process.
    /// </summary>
    public TBarcodeOptions Options => barcodeOptions;

    /// <inheritdoc />
    public void Render(
        ISurfaceRenderTarget target,
        BarcodeRenderingOptions options)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(options);

        var realComposer = composer ?? new Barcode2DComposer<TBarcodeOptions>();
        var encodedData = encoder.Encode(barcodeData(), barcodeOptions);
        var payload = realComposer.Compose(encodedData, options, barcodeOptions);

        target.AddLayer(new BarcodeLayer<Barcode2DPayload>(payload));
    }

    /// <inheritdoc />
    public ValueTask RenderAsync(
        ISurfaceRenderTarget target,
        BarcodeRenderingOptions options)
    {
        Render(target, options);

        return ValueTask.CompletedTask;
    }
}
