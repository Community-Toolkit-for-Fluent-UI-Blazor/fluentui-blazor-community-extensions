namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides lookup tables and utility methods for mapping characters and symbol patterns used in the Code 93 barcode
/// symbology.
/// </summary>
/// <remarks>This class contains static data and methods to facilitate encoding and decoding operations for Code
/// 93 barcodes, including symbol pattern definitions and character-to-value mappings. It is intended for internal use
/// within barcode processing components and is not designed for direct use in application code.</remarks>
internal static class Code93Table
{
    /// <summary>
    /// Represents the set of symbol patterns used in the Code 93 barcode symbology.
    /// </summary>
    /// <remarks>Each element in the array corresponds to a Code 93 symbol, including digits, uppercase
    /// letters, special characters, and the start/stop symbol. The pattern for each symbol is defined as a string of
    /// digits representing the bar and space widths according to the Code 93 specification. The order of symbols in the
    /// array matches their encoding values in the Code 93 standard.</remarks>
    public static readonly Code93Symbol[] Symbols =
    [
        new() { Pattern = "131112" }, // 0
        new() { Pattern = "111213" }, // 1
        new() { Pattern = "111312" }, // 2
        new() { Pattern = "111411" }, // 3
        new() { Pattern = "121113" }, // 4
        new() { Pattern = "121212" }, // 5
        new() { Pattern = "121311" }, // 6
        new() { Pattern = "111114" }, // 7
        new() { Pattern = "131211" }, // 8
        new() { Pattern = "141111" }, // 9

        new() { Pattern = "211113" }, // 10 A
        new() { Pattern = "211212" }, // 11 B
        new() { Pattern = "211311" }, // 12 C
        new() { Pattern = "221112" }, // 13 D
        new() { Pattern = "221211" }, // 14 E
        new() { Pattern = "231111" }, // 15 F
        new() { Pattern = "112113" }, // 16 G
        new() { Pattern = "112212" }, // 17 H
        new() { Pattern = "112311" }, // 18 I
        new() { Pattern = "122112" }, // 19 J

        new() { Pattern = "132111" }, // 20 K
        new() { Pattern = "111123" }, // 21 L
        new() { Pattern = "111222" }, // 22 M
        new() { Pattern = "111321" }, // 23 N
        new() { Pattern = "121122" }, // 24 O
        new() { Pattern = "131121" }, // 25 P
        new() { Pattern = "212112" }, // 26 Q
        new() { Pattern = "212211" }, // 27 R
        new() { Pattern = "211122" }, // 28 S
        new() { Pattern = "211221" }, // 29 T

        new() { Pattern = "221121" }, // 30 U
        new() { Pattern = "222111" }, // 31 V
        new() { Pattern = "112122" }, // 32 W
        new() { Pattern = "112221" }, // 33 X
        new() { Pattern = "122121" }, // 34 Y
        new() { Pattern = "123111" }, // 35 Z
        new() { Pattern = "121131" }, // 36 -
        new() { Pattern = "311112" }, // 37 .
        new() { Pattern = "311211" }, // 38 space
        new() { Pattern = "321111" }, // 39 $

        new() { Pattern = "112131" }, // 40 /
        new() { Pattern = "113121" }, // 41 +
        new() { Pattern = "211131" }, // 42 %
        new() { Pattern = "121221" }, // 43 ($)
        new() { Pattern = "312111" }, // 44 (/)
        new() { Pattern = "311121" }, // 45 (+)
        new() { Pattern = "122211" }, // 46 (%)

        // 47 = START/STOP (*)
        new() { Pattern = "111141" }  // *
    ];

    /// <summary>
    /// Provides a mapping from supported character strings to their corresponding integer values for encoding and
    /// decoding operations.
    /// </summary>
    /// <remarks>This dictionary includes mappings for digits (0–9), uppercase letters (A–Z), and a set of
    /// special characters commonly used in barcode or symbol encoding schemes. The mapping is typically used to convert
    /// characters to their numeric representations for processing or validation purposes.</remarks>
    private static readonly Dictionary<string, int> CharToValue = new()
    {
        // 0–9
        ["0"] = 0,
        ["1"] = 1,
        ["2"] = 2,
        ["3"] = 3,
        ["4"] = 4,
        ["5"] = 5,
        ["6"] = 6,
        ["7"] = 7,
        ["8"] = 8,
        ["9"] = 9,

        // A–Z
        ["A"] = 10,
        ["B"] = 11,
        ["C"] = 12,
        ["D"] = 13,
        ["E"] = 14,
        ["F"] = 15,
        ["G"] = 16,
        ["H"] = 17,
        ["I"] = 18,
        ["J"] = 19,
        ["K"] = 20,
        ["L"] = 21,
        ["M"] = 22,
        ["N"] = 23,
        ["O"] = 24,
        ["P"] = 25,
        ["Q"] = 26,
        ["R"] = 27,
        ["S"] = 28,
        ["T"] = 29,
        ["U"] = 30,
        ["V"] = 31,
        ["W"] = 32,
        ["X"] = 33,
        ["Y"] = 34,
        ["Z"] = 35,

        ["-"] = 36,
        ["."] = 37,
        [" "] = 38,
        ["$"] = 39,
        ["/"] = 40,
        ["+"] = 41,
        ["%"] = 42,

        // Start/Stop
        ["*"] = 47
    };

    /// <summary>
    /// Attempts to retrieve the integer value associated with the specified character key.
    /// </summary>
    /// <param name="c">The character key whose associated value is to be retrieved.</param>
    /// <param name="value">When this method returns, contains the integer value associated with the specified character key, if the key is
    /// found; otherwise, zero. This parameter is passed uninitialized.</param>
    /// <returns>true if the character key was found and its value was retrieved; otherwise, false.</returns>
    public static bool TryGetValue(string c, out int value) => CharToValue.TryGetValue(c, out value);
}
