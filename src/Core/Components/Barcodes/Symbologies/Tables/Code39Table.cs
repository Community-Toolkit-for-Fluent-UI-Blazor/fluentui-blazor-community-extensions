namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides the character-to-pattern mapping table for the Code 39 barcode symbology.
/// </summary>
/// <remarks>This class contains a static dictionary that maps each valid Code 39 character to its corresponding
/// barcode pattern, using the standard narrow ('n') and wide ('w') encoding. The table includes all alphanumeric
/// characters and special symbols supported by the Code 39 specification, including the start/stop character
/// ('*').</remarks>
internal static class Code39Table
{
    /// <summary>
    /// Provides the mapping of Code 39 barcode characters to their corresponding bar and space patterns.
    /// </summary>
    /// <remarks>Each key in the dictionary represents a valid Code 39 character, and the associated string
    /// value specifies the sequence of narrow and wide bars and spaces for that character. The pattern uses 'n' for
    /// narrow and 'w' for wide elements, following the standard Code 39 encoding scheme. The asterisk ('*') character
    /// is used as the start and stop delimiter in Code 39 barcodes.</remarks>
    public static readonly Dictionary<char, string> Patterns = new()
    {
        ['0'] = "nnnwwnwnn",
        ['1'] = "wnnwnnnnw",
        ['2'] = "nnwwnnnnw",
        ['3'] = "wnwwnnnnn",
        ['4'] = "nnnwwnnnw",
        ['5'] = "wnnwwnnnn",
        ['6'] = "nnwwwnnnn",
        ['7'] = "nnnwnnwnw",
        ['8'] = "wnnwnnwnn",
        ['9'] = "nnwwnnwnn",

        ['A'] = "wnnnnwnnw",
        ['B'] = "nnwnnwnnw",
        ['C'] = "wnwnnwnnn",
        ['D'] = "nnnnwwnnw",
        ['E'] = "wnnnwwnnn",
        ['F'] = "nnwnwwnnn",
        ['G'] = "nnnnnwwnw",
        ['H'] = "wnnnnwwnn",
        ['I'] = "nnwnnwwnn",
        ['J'] = "nnnnwwwnn",

        ['K'] = "wnnnnnnww",
        ['L'] = "nnwnnnnww",
        ['M'] = "wnwnnnnwn",
        ['N'] = "nnnnwnnww",
        ['O'] = "wnnnwnnwn",
        ['P'] = "nnwnwnnwn",
        ['Q'] = "nnnnnnwww",
        ['R'] = "wnnnnnwwn",
        ['S'] = "nnwnnnwwn",
        ['T'] = "nnnnwnwwn",

        ['U'] = "wwnnnnnnw",
        ['V'] = "nwwnnnnnw",
        ['W'] = "wwwnnnnnn",
        ['X'] = "nwnnwnnnw",
        ['Y'] = "wwnnwnnnn",
        ['Z'] = "nwwnwnnnn",

        ['-'] = "nwnnnnwnw",
        ['.'] = "wwnnnnwnn",
        [' '] = "nwwnnnwnn",
        ['$'] = "nwnwnwnnn",
        ['/'] = "nwnwnnnwn",
        ['+'] = "nwnnnwnwn",
        ['%'] = "nnnwnwnwn",

        ['*'] = "nwnnwnwnn" // start/stop
    };
}
