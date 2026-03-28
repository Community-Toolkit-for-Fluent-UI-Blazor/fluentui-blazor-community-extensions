namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides lookup tables for EAN-13 barcode encoding parity patterns.
/// </summary>
/// <remarks>This class contains static data used to determine the encoding parity for the first digit of an
/// EAN-13 barcode. It is intended for internal use in barcode generation or decoding processes.</remarks>
internal static class Ean13Table
{
    /// <summary>
    /// Provides the parity patterns used for encoding the left-side digits of EAN-13 barcodes.
    /// </summary>
    /// <remarks>Each string in the array represents the sequence of encoding patterns ('L', 'G') for a
    /// specific leading digit (0–9) in the EAN-13 barcode format. The index corresponds to the digit value.</remarks>
    public static readonly string[] Parity =
    [
        "LLLLLL", // 0
        "LLGLGG", // 1
        "LLGGLG", // 2
        "LLGGGL", // 3
        "LGLLGG", // 4
        "LGGLLG", // 5
        "LGGGLL", // 6
        "LGLGLG", // 7
        "LGLGGL", // 8
        "LGGLGL"  // 9
    ];
}

