namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides the parity patterns used for encoding UPC-E barcodes for number systems 0 and 1.
/// </summary>
/// <remarks>Each entry in the array represents a set of parity patterns for a specific number system. The inner
/// arrays contain six-character strings, where each character is either 'L' or 'G', corresponding to the encoding
/// pattern for each digit position based on the checksum value. These patterns are used to determine how each digit in
/// a UPC-E barcode is encoded.</remarks>
internal static class UpcETable
{
    // Parity patterns for UPC-E (system 0 and 1)
    // Each entry is a 6-character string of 'L' or 'G'
    public static readonly string[][] Parity =
    {
        // Number system 0
        [
            "LLLLLL", // checksum 0
            "LLGLGG", // 1
            "LLGGLG", // 2
            "LLGGGL", // 3
            "LGLLGG", // 4
            "LGGLLG", // 5
            "LGGGLL", // 6
            "LGLGLG", // 7
            "LGLGGL", // 8
            "LGGLGL"  // 9
        ],

        // Number system 1
        [
            "GGGGGG", // checksum 0
            "GGLGLG", // 1
            "GGGLGL", // 2
            "GGGLLG", // 3
            "GLGGGL", // 4
            "GLLGGL", // 5
            "GLLLGG", // 6
            "GLGLGG", // 7
            "GLGGLG", // 8
            "GLLGLG"  // 9
        ]
    };
}
