namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides lookup tables for Codabar barcode encoding, including character patterns and valid start/stop characters.
/// </summary>
/// <remarks>This class supplies static data structures used to map Codabar characters to their corresponding
/// barcode patterns and to identify valid start and stop characters for Codabar barcodes. It is intended for internal
/// use in barcode generation or validation routines.</remarks>
internal static class CodabarTable
{
    /// <summary>
    /// Provides a mapping of supported characters to their corresponding barcode patterns for the symbology.
    /// </summary>
    /// <remarks>Each key represents a character that can be encoded, and the associated string value
    /// specifies the pattern of narrow ('N') and wide ('W') bars and spaces used to represent that character in the
    /// barcode. This dictionary is typically used when generating or interpreting barcodes that follow this encoding
    /// scheme.</remarks>
    public static readonly Dictionary<char, string> Patterns = new()
    {
        { '0', "NNNNNWW" }, // 1111122
        { '1', "NNNNWWN" }, // 1111221
        { '2', "NNNWNWN" }, // 1112112
        { '3', "WWNNNNN" }, // 2211111
        { '4', "NNWWNNW" }, // 1121121
        { '5', "WNNNNNW" }, // 2111121
        { '6', "NWNNNNW" }, // 1211112
        { '7', "NWNWNNW" }, // 1211211
        { '8', "NWWNNNN" }, // 1221111
        { '9', "WNNWNNN" }, // 2112111

        { '-', "NNNWWNN" }, // 1112211
        { '$', "NNWWNNN" }, // 1122111
        { ':', "WNNNWWN" }, // 2111212
        { '/', "WNWNNWN" }, // 2121112
        { '.', "WNWNWNN" }, // 2121211
        { '+', "NNWWWWW" }, // 1122222

        { 'A', "NNWWNWN" }, // 1122121
        { 'B', "NWNWNNW" }, // 1212112
        { 'C', "NNNWNWW" }, // 1112122
        { 'D', "NNNWWWN" }  // 1112221
    };

    /// <summary>
    /// Represents the set of valid start and stop characters.
    /// </summary>
    /// <remarks>This set can be used to validate or identify characters that are designated as start or stop
    /// markers in a given context. The set is read-only and contains the characters 'A', 'B', 'C', and 'D'.</remarks>
    public static readonly char[] StartStop = ['A', 'B', 'C', 'D'];
}

