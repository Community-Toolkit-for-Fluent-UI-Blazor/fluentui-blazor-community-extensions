namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Specifies the available encoding modes for PDF417 barcodes.
/// </summary>
/// <remarks>Use this enumeration to select the desired encoding mode when generating or interpreting PDF417
/// barcodes. The mode determines how input data is processed and encoded within the barcode symbol.</remarks>
public enum PDF417Mode
{
    /// <summary>
    /// Represents the normal PDF-417 encoding mode.
    /// </summary>
    Normal,

    /// <summary>
    /// Represents the compact PDF-417 encoding mode.
    /// </summary>
    Compact,

    /// <summary>
    /// Represents the micro PDF-417 encoding mode.
    /// </summary>
    Micro
}
