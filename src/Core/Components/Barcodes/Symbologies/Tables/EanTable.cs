namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides lookup tables for the encoding patterns used in EAN-13 and EAN-8 barcodes.
/// </summary>
/// <remarks>This class contains static arrays representing the bit patterns for the L-code (left odd parity),
/// G-code (left even parity), and R-code (right parity) digit encodings as defined by the EAN barcode standards. These
/// tables are typically used when generating or decoding EAN barcodes to map numeric digits to their corresponding
/// barcode representations.</remarks>
internal static class EanTable
{
    /// <summary>
    /// Represents the set of left-side (L-code) encoding patterns for digits 0 through 9 in the EAN-13 barcode
    /// symbology.
    /// </summary>
    public static readonly string[] L =
    {
        "0001101", // 0
        "0011001", // 1
        "0010011", // 2
        "0111101", // 3
        "0100011", // 4
        "0110001", // 5
        "0101111", // 6
        "0111011", // 7
        "0110111", // 8
        "0001011"  // 9
    };

    /// <summary>
    /// Represents the set of 7-bit binary encoding patterns for the 'G' parity used in EAN-13 barcodes for digits 0
    /// through 9.
    /// </summary>
    public static readonly string[] G =
    {
        "0100111", // 0
        "0110011", // 1
        "0011011", // 2
        "0100001", // 3
        "0011101", // 4
        "0111001", // 5
        "0000101", // 6
        "0010001", // 7
        "0001001", // 8
        "0010111"  // 9
    };

    /// <summary>
    /// Represents the set of left-side (L-code) encoding patterns for digits 0 through 9 in the EAN-13 barcode
    /// symbology.
    /// </summary>
    public static readonly string[] R =
    {
        "1110010", // 0
        "1100110", // 1
        "1101100", // 2
        "1000010", // 3
        "1011100", // 4
        "1001110", // 5
        "1010000", // 6
        "1000100", // 7
        "1001000", // 8
        "1110100"  // 9
    };
}
