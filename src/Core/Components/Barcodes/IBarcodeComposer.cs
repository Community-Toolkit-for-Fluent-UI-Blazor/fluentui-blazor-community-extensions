namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines a contract for rendering barcode payloads with customizable rendering options.
/// </summary>
/// <remarks>Implementations of this interface allow for flexible barcode rendering by supporting various options
/// such as quiet zones, labels, and foreground styles. This enables consistent barcode generation across different
/// formats and rendering strategies.</remarks>
/// <typeparam name="TPayload">The type of the data used to generate the barcode payload.</typeparam>
/// <typeparam name="TBarcodeOptions">The type of the options used to configure the barcode generation process.</typeparam>
public interface IBarcodeLayerComposer<TPayload, TBarcodeOptions>
{
    /// <summary>
    /// Generates a barcode payload from the specified data using the provided rendering and barcode options.
    /// </summary>
    /// <remarks>Ensure that the provided data and options are valid for the intended barcode format. Invalid
    /// or incompatible parameters may result in an exception or an unusable barcode payload.</remarks>
    /// <param name="data">The data to encode into the barcode. The type and structure of the data must be compatible with the barcode
    /// format.</param>
    /// <param name="options">The rendering options that determine the visual appearance of the generated barcode, such as size, colors, and
    /// resolution.</param>
    /// <param name="barcodeOptions">The barcode-specific options that configure encoding parameters, error correction, or other barcode format
    /// features.</param>
    /// <returns>A <see cref="BarcodePayload{TPayload}"/> instance containing the encoded barcode data and associated metadata.</returns>
    BarcodePayload<TPayload> Compose(
        TPayload data,
        BarcodeRenderingOptions options,
        TBarcodeOptions barcodeOptions);
}
