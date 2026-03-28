namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Specifies the encoding mode used for data in a PDF417 barcode symbol.
/// </summary>
/// <remarks>Use this enumeration to select the appropriate encoding mode based on the type of data to be encoded
/// in the PDF417 barcode. Numeric mode is optimized for digit-only data, Text mode for alphanumeric and common
/// punctuation, and Byte mode for arbitrary binary data.</remarks>
internal enum Pdf417EncodingMode
{
    /// <summary>
    /// Specifies numeric values or options.
    /// </summary>
    Numeric,

    /// <summary>
    /// Specifies alphanumeric characters.
    /// </summary>
    Text,

    /// <summary>
    /// Specifies binary data.
    /// </summary>
    Byte
}
