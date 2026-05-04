namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Specifies the error correction levels available for PDF417 barcodes.
/// </summary>
/// <remarks>Higher error correction levels increase the barcode's ability to recover from damage or data loss,
/// but also increase the size of the barcode. The appropriate level should be chosen based on the expected level of
/// damage or distortion in the barcode's usage environment.</remarks>
public enum PDF417ErrorCorrectionLevel
{
    /// <summary>
    /// Specifies that the value should be determined automatically based on context or default behavior.
    /// </summary>
    Auto = -1,

    /// <summary>
    /// Indicates the lowest level of error correction.
    /// </summary>
    Level0 = 0,

    /// <summary>
    /// Indicates the first level of error correction.
    /// </summary>
    Level1 = 1,

    /// <summary>
    /// Indicates the second level of error correction.
    /// </summary>
    Level2 = 2,

    /// <summary>
    /// Indicates the third level of error correction.
    /// </summary>
    Level3 = 3,

    /// <summary>
    /// Indicates the fourth level of error correction.
    /// </summary>
    Level4 = 4,

    /// <summary>
    /// Indicates the fifth level of error correction.
    /// </summary>
    Level5 = 5,

    /// <summary>
    /// Indicates the six level of error correction.
    /// </summary>
    Level6 = 6,

    /// <summary>
    /// Indicates the seven level of error correction.
    /// </summary>
    Level7 = 7,

    /// <summary>
    /// Indicates the highest level of error correction.
    /// </summary>
    Level8 = 8
}
