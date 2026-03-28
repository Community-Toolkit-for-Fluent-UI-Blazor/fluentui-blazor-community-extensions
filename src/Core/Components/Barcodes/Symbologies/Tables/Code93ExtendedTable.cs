namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a lookup table for mapping characters to their Code 93 Extended barcode representations.
/// </summary>
/// <remarks>This class supports encoding of the full ASCII character set for Code 93 barcodes by mapping each
/// character to its corresponding Code 93 Extended sequence. It is intended for internal use in barcode generation and
/// is not thread-safe for modification.</remarks>
internal static class Code93ExtendedTable
{
    /// <summary>
    /// Provides a mapping from ASCII characters to their corresponding encoded string representations for use in
    /// barcode or symbol encoding schemes.
    /// </summary>
    /// <remarks>This dictionary is typically used to translate individual ASCII characters into specific
    /// codewords or patterns required by certain barcode standards. The mapping includes control characters, digits,
    /// letters, and various symbols, each associated with a unique encoded string. The mapping is static and
    /// read-only.</remarks>
    private static readonly Dictionary<char, string> Map = new()
    {
        // ASCII 0–31 (control chars)
        ['\0'] = "(%)U",
        ['\x01'] = "($)A",
        ['\x02'] = "($)B",
        ['\x03'] = "($)C",
        ['\x04'] = "($)D",
        ['\x05'] = "($)E",
        ['\x06'] = "($)F",
        ['\x07'] = "($)G",
        ['\x08'] = "($)H",
        ['\x09'] = "($)I",
        ['\x0A'] = "($)J",
        ['\x0B'] = "($)K",
        ['\x0C'] = "($)L",
        ['\x0D'] = "($)M",
        ['\x0E'] = "($)N",
        ['\x0F'] = "($)O",
        ['\x10'] = "($)P",
        ['\x11'] = "($)Q",
        ['\x12'] = "($)R",
        ['\x13'] = "($)S",
        ['\x14'] = "($)T",
        ['\x15'] = "($)U",
        ['\x16'] = "($)V",
        ['\x17'] = "($)W",
        ['\x18'] = "($)X",
        ['\x19'] = "($)Y",
        ['\x1A'] = "($)Z",
        ['\x1B'] = "(%)A",
        ['\x1C'] = "(%)B",
        ['\x1D'] = "(%)C",
        ['\x1E'] = "(%)D",
        ['\x1F'] = "(%)E",

        // ASCII 32–47
        [' '] = " ",
        ['!'] = "(/)A",
        ['"'] = "(/)B",
        ['#'] = "(/)C",
        ['$'] = "(/)D",
        ['%'] = "(/)E",
        ['&'] = "(/)F",
        ['\''] = "(/)G",
        ['('] = "(/)H",
        [')'] = "(/)I",
        ['*'] = "(/)J",
        ['+'] = "(/)K",
        [','] = "(/)L",
        ['-'] = "-",
        ['.'] = ".",
        ['/'] = "/",

        // ASCII 48–57 (digits)
        ['0'] = "0",
        ['1'] = "1",
        ['2'] = "2",
        ['3'] = "3",
        ['4'] = "4",
        ['5'] = "5",
        ['6'] = "6",
        ['7'] = "7",
        ['8'] = "8",
        ['9'] = "9",

        // ASCII 58–64
        [':'] = "(/)Z",
        [';'] = "(%)F",
        ['<'] = "(%)G",
        ['='] = "(%)H",
        ['>'] = "(%)I",
        ['?'] = "(%)J",
        ['@'] = "(%)V",

        // ASCII 65–90 (A–Z)
        ['A'] = "A",
        ['B'] = "B",
        ['C'] = "C",
        ['D'] = "D",
        ['E'] = "E",
        ['F'] = "F",
        ['G'] = "G",
        ['H'] = "H",
        ['I'] = "I",
        ['J'] = "J",
        ['K'] = "K",
        ['L'] = "L",
        ['M'] = "M",
        ['N'] = "N",
        ['O'] = "O",
        ['P'] = "P",
        ['Q'] = "Q",
        ['R'] = "R",
        ['S'] = "S",
        ['T'] = "T",
        ['U'] = "U",
        ['V'] = "V",
        ['W'] = "W",
        ['X'] = "X",
        ['Y'] = "Y",
        ['Z'] = "Z",

        // ASCII 91–96
        ['['] = "(%)K",
        ['\\'] = "(%)L",
        [']'] = "(%)M",
        ['^'] = "(%)N",
        ['_'] = "(%)O",
        ['`'] = "(%)W",

        // ASCII 97–122 (a–z)
        ['a'] = "(+)A",
        ['b'] = "(+)B",
        ['c'] = "(+)C",
        ['d'] = "(+)D",
        ['e'] = "(+)E",
        ['f'] = "(+)F",
        ['g'] = "(+)G",
        ['h'] = "(+)H",
        ['i'] = "(+)I",
        ['j'] = "(+)J",
        ['k'] = "(+)K",
        ['l'] = "(+)L",
        ['m'] = "(+)M",
        ['n'] = "(+)N",
        ['o'] = "(+)O",
        ['p'] = "(+)P",
        ['q'] = "(+)Q",
        ['r'] = "(+)R",
        ['s'] = "(+)S",
        ['t'] = "(+)T",
        ['u'] = "(+)U",
        ['v'] = "(+)V",
        ['w'] = "(+)W",
        ['x'] = "(+)X",
        ['y'] = "(+)Y",
        ['z'] = "(+)Z",

        // ASCII 123–127
        ['{'] = "(%)P",
        ['|'] = "(%)Q",
        ['}'] = "(%)R",
        ['~'] = "(%)S",
        ['\x7F'] = "(%)T"
    };

    /// <summary>
    /// Attempts to retrieve the string sequence associated with the specified character.
    /// </summary>
    /// <param name="c">The character key to locate in the mapping.</param>
    /// <param name="seq">When this method returns, contains the string sequence associated with the specified character, if the character
    /// is found; otherwise, null. This parameter is passed uninitialized.</param>
    /// <returns>true if the mapping contains an entry for the specified character; otherwise, false.</returns>
    public static bool TryGet(char c, out string? seq) => Map.TryGetValue(c, out seq);
}
