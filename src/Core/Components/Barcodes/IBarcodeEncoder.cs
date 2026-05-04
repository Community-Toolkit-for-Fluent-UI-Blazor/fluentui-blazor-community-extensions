namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines a contract for encoding data into a barcode payload using specified encoding options.
/// </summary>
/// <remarks>Implementations of this interface provide a way to convert input data into a barcode format, allowing
/// customization through encoding options. The specific barcode symbology and payload type depend on the
/// implementation.</remarks>
/// <typeparam name="TPayload">The type representing the encoded barcode payload returned by the encoder.</typeparam>
/// <typeparam name="TOptions">The type representing the options or settings used to control the encoding process.</typeparam>
public interface IBarcodeEncoder<TPayload, TOptions>
{
    /// <summary>
    /// Encodes the specified data using the provided options and returns the resulting payload.
    /// </summary>
    /// <param name="data">The data to encode. Cannot be null.</param>
    /// <param name="options">The options that configure the encoding process. Cannot be null.</param>
    /// <returns>The encoded payload resulting from the operation.</returns>
    TPayload Encode(string data, TOptions options);
}
