namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides encoding tables and constants for representing digits and control codes in the Interleaved 2 of 5 (ITF)
/// barcode symbology.
/// </summary>
/// <remarks>This class contains static members used to map numeric digits to their corresponding narrow and wide
/// bar patterns, as well as the start and stop codes required for ITF barcode generation. It is intended for internal
/// use in barcode encoding processes.</remarks>
internal static class ItfTable
{
    // N = narrow, W = wide
    public static readonly string[] Digit =
    {
        "NNWWN", // 0
        "WNNNW", // 1
        "NWNNW", // 2
        "WWNNN", // 3
        "NNWNW", // 4
        "WNWNW", // 5
        "NWWNN", // 6
        "WWWNN", // 7
        "NWNWN", // 8
        "WNNWN"  // 9
    };

    /// <summary>
    /// Represents the start code value "1010".
    /// </summary>
    public const string Start = "1010";

    /// <summary>
    /// Represents the stop code value "11101".
    /// </summary>
    public const string Stop = "11101";
}
