namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides access to the codeword patterns used for encoding and decoding PDF417 barcodes.
/// </summary>
/// <remarks>This static class contains the mapping of codeword indices and clusters to their corresponding bar
/// and space patterns, as defined by the PDF417 barcode specification. The patterns are used internally for barcode
/// symbol recognition and generation. This class is intended for use by PDF417 barcode processing components and is not
/// typically used directly by application code.</remarks>
internal static class Pdf417CodewordPatterns
{
    /// <summary>
    /// Represents the start pattern sequence used for barcode encoding.
    /// </summary>
    /// <remarks>The pattern defines the sequence of bar and space widths required at the beginning of a
    /// barcode symbol. The specific values correspond to the standard start pattern for the supported barcode
    /// type.</remarks>
    public static readonly int[] StartPattern = { 8, 1, 1, 1, 1, 1, 1, 3 };

    /// <summary>
    /// Represents the stop pattern sequence used for barcode or symbol recognition.
    /// </summary>
    /// <remarks>This array defines the specific pattern of module widths that indicates the end of a barcode
    /// or symbol. The pattern is typically used in decoding algorithms to identify the termination of encoded
    /// data.</remarks>
    public static readonly int[] StopPattern = { 7, 1, 1, 3, 1, 1, 1, 2, 1 };

    /// <summary>
    /// Represents the micro start pattern as an array of integers, typically used for barcode or signal processing
    /// applications.
    /// </summary>
    /// <remarks>The pattern defines a specific sequence of values that can be used to identify or validate
    /// the start of a data stream. The exact interpretation of the pattern depends on the context in which it is
    /// used.</remarks>
    public static readonly int[] MicroStartPattern = { 1, 1, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 1, 1, 1 };

    /// <summary>
    /// Represents a predefined micro stop pattern as an array of integers.
    /// </summary>
    /// <remarks>This pattern can be used in scenarios where a fixed sequence of micro stops is required, such
    /// as in barcode generation or signal processing. The array contains a sequence of ones, indicating uniform stop
    /// intervals.</remarks>
    public static readonly int[] MicroStopPattern = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

    /// <summary>
    /// Provides a static lookup table of integer patterns, organized as a nested dictionary structure for efficient
    /// retrieval based on two integer keys.
    /// </summary>
    /// <remarks>The outer dictionary maps an integer key to an inner dictionary, which in turn maps another
    /// integer key to an array of integers representing a specific pattern. This structure is typically used to store
    /// and retrieve precomputed or fixed pattern data for algorithms that require fast access to such patterns. The
    /// data is read-only and intended for internal use within the component or algorithm that requires these
    /// patterns.</remarks>
    private static readonly Dictionary<int, Dictionary<int, int[]>> Patterns =
        new Dictionary<int, Dictionary<int, int[]>>
        {
            [0] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 1, 1, 1, 1, 3, 6 },
                [3] = new[] { 5, 1, 1, 1, 1, 1, 2, 5 },
                [6] = new[] { 2, 1, 1, 1, 1, 1, 5, 5 },
            },
            [1] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 1, 1, 1, 1, 4, 4 },
                [3] = new[] { 6, 1, 1, 1, 1, 1, 3, 3 },
                [6] = new[] { 3, 1, 1, 1, 1, 1, 6, 3 },
            },
            [2] = new Dictionary<int, int[]>
            {
                [0] = new[] { 5, 1, 1, 1, 1, 1, 5, 2 },
                [3] = new[] { 4, 1, 1, 1, 1, 2, 1, 6 },
                [6] = new[] { 1, 1, 1, 1, 1, 2, 4, 6 },
            },
            [3] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 1, 1, 1, 2, 3, 5 },
                [3] = new[] { 5, 1, 1, 1, 1, 2, 2, 4 },
                [6] = new[] { 2, 1, 1, 1, 1, 2, 5, 4 },
            },
            [4] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 1, 1, 1, 2, 4, 3 },
                [3] = new[] { 6, 1, 1, 1, 1, 2, 3, 2 },
                [6] = new[] { 3, 1, 1, 1, 1, 2, 6, 2 },
            },
            [5] = new Dictionary<int, int[]>
            {
                [0] = new[] { 5, 1, 1, 1, 1, 2, 5, 1 },
                [3] = new[] { 4, 1, 1, 1, 1, 3, 1, 5 },
                [6] = new[] { 1, 1, 1, 1, 1, 3, 4, 5 },
            },
            [6] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 1, 1, 3, 2, 6 },
                [3] = new[] { 5, 1, 1, 1, 1, 3, 2, 3 },
                [6] = new[] { 2, 1, 1, 1, 1, 3, 5, 3 },
            },
            [7] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 1, 1, 1, 3, 3, 4 },
                [3] = new[] { 6, 1, 1, 1, 1, 3, 3, 1 },
                [6] = new[] { 3, 1, 1, 1, 1, 3, 6, 1 },
            },
            [8] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 1, 1, 4, 2, 5 },
                [3] = new[] { 4, 1, 1, 1, 1, 4, 1, 4 },
                [6] = new[] { 1, 1, 1, 1, 1, 4, 4, 4 },
            },
            [9] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 1, 1, 5, 1, 6 },
                [3] = new[] { 5, 1, 1, 1, 1, 4, 2, 2 },
                [6] = new[] { 2, 1, 1, 1, 1, 4, 5, 2 },
            },
            [10] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 1, 1, 5, 2, 4 },
                [3] = new[] { 4, 1, 1, 1, 1, 5, 1, 3 },
                [6] = new[] { 1, 1, 1, 1, 1, 5, 4, 3 },
            },
            [11] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 1, 1, 6, 1, 5 },
                [3] = new[] { 5, 1, 1, 1, 1, 5, 2, 1 },
                [6] = new[] { 6, 1, 1, 1, 2, 1, 1, 4 },
            },
            [12] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 1, 2, 1, 3, 6 },
                [3] = new[] { 4, 1, 1, 1, 1, 6, 1, 2 },
                [6] = new[] { 1, 1, 1, 1, 2, 1, 5, 5 },
            },
            [13] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 1, 1, 2, 1, 4, 4 },
                [3] = new[] { 4, 1, 1, 1, 2, 1, 2, 5 },
                [6] = new[] { 2, 1, 1, 1, 2, 1, 6, 3 },
            },
            [14] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 1, 1, 2, 1, 5, 2 },
                [3] = new[] { 5, 1, 1, 1, 2, 1, 3, 3 },
                [6] = new[] { 6, 1, 1, 1, 2, 2, 1, 3 },
            },
            [15] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 1, 2, 2, 3, 5 },
                [3] = new[] { 6, 1, 1, 1, 2, 1, 4, 1 },
                [6] = new[] { 1, 1, 1, 1, 2, 2, 5, 4 },
            },
            [16] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 1, 1, 2, 2, 4, 3 },
                [3] = new[] { 3, 1, 1, 1, 2, 2, 1, 6 },
                [6] = new[] { 2, 1, 1, 1, 2, 2, 6, 2 },
            },
            [17] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 1, 1, 2, 2, 5, 1 },
                [3] = new[] { 4, 1, 1, 1, 2, 2, 2, 4 },
                [6] = new[] { 6, 1, 1, 1, 2, 3, 1, 2 },
            },
            [18] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 1, 2, 3, 2, 6 },
                [3] = new[] { 5, 1, 1, 1, 2, 2, 3, 2 },
                [6] = new[] { 1, 1, 1, 1, 2, 3, 5, 3 },
            },
            [19] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 1, 2, 3, 3, 4 },
                [3] = new[] { 3, 1, 1, 1, 2, 3, 1, 5 },
                [6] = new[] { 2, 1, 1, 1, 2, 3, 6, 1 },
            },
            [20] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 1, 2, 4, 2, 5 },
                [3] = new[] { 4, 1, 1, 1, 2, 3, 2, 3 },
                [6] = new[] { 6, 1, 1, 1, 2, 4, 1, 1 },
            },
            [21] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 1, 3, 1, 3, 6 },
                [3] = new[] { 5, 1, 1, 1, 2, 3, 3, 1 },
                [6] = new[] { 1, 1, 1, 1, 2, 4, 5, 2 },
            },
            [22] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 1, 3, 1, 4, 4 },
                [3] = new[] { 3, 1, 1, 1, 2, 4, 1, 4 },
                [6] = new[] { 5, 1, 1, 1, 3, 1, 1, 4 },
            },
            [23] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 1, 1, 3, 1, 5, 2 },
                [3] = new[] { 4, 1, 1, 1, 2, 4, 2, 2 },
                [6] = new[] { 6, 1, 1, 1, 3, 1, 2, 2 },
            },
            [24] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 1, 3, 2, 3, 5 },
                [3] = new[] { 3, 1, 1, 1, 2, 5, 1, 3 },
                [6] = new[] { 1, 1, 1, 1, 3, 1, 6, 3 },
            },
            [25] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 1, 3, 2, 4, 3 },
                [3] = new[] { 4, 1, 1, 1, 2, 5, 2, 1 },
                [6] = new[] { 5, 1, 1, 1, 3, 2, 1, 3 },
            },
            [26] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 1, 1, 3, 2, 5, 1 },
                [3] = new[] { 3, 1, 1, 1, 2, 6, 1, 2 },
                [6] = new[] { 6, 1, 1, 1, 3, 2, 2, 1 },
            },
            [27] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 1, 3, 3, 3, 4 },
                [3] = new[] { 3, 1, 1, 1, 3, 1, 2, 5 },
                [6] = new[] { 1, 1, 1, 1, 3, 2, 6, 2 },
            },
            [28] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 1, 3, 3, 4, 2 },
                [3] = new[] { 4, 1, 1, 1, 3, 1, 3, 3 },
                [6] = new[] { 5, 1, 1, 1, 3, 3, 1, 2 },
            },
            [29] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 1, 4, 1, 4, 4 },
                [3] = new[] { 5, 1, 1, 1, 3, 1, 4, 1 },
                [6] = new[] { 1, 1, 1, 1, 3, 3, 6, 1 },
            },
            [30] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 1, 4, 1, 5, 2 },
                [3] = new[] { 2, 1, 1, 1, 3, 2, 1, 6 },
                [6] = new[] { 5, 1, 1, 1, 3, 4, 1, 1 },
            },
            [31] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 1, 4, 2, 4, 3 },
                [3] = new[] { 3, 1, 1, 1, 3, 2, 2, 4 },
                [6] = new[] { 4, 1, 1, 1, 4, 1, 1, 4 },
            },
            [32] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 1, 4, 2, 5, 1 },
                [3] = new[] { 4, 1, 1, 1, 3, 2, 3, 2 },
                [6] = new[] { 5, 1, 1, 1, 4, 1, 2, 2 },
            },
            [33] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 1, 5, 1, 5, 2 },
                [3] = new[] { 2, 1, 1, 1, 3, 3, 1, 5 },
                [6] = new[] { 4, 1, 1, 1, 4, 2, 1, 3 },
            },
            [34] = new Dictionary<int, int[]>
            {
                [0] = new[] { 5, 1, 1, 1, 6, 1, 1, 1 },
                [3] = new[] { 3, 1, 1, 1, 3, 3, 2, 3 },
                [6] = new[] { 5, 1, 1, 1, 4, 2, 2, 1 },
            },
            [35] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 1, 2, 1, 1, 3, 5 },
                [3] = new[] { 4, 1, 1, 1, 3, 3, 3, 1 },
                [6] = new[] { 4, 1, 1, 1, 4, 3, 1, 2 },
            },
            [36] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 1, 2, 1, 1, 4, 3 },
                [3] = new[] { 2, 1, 1, 1, 3, 4, 1, 4 },
                [6] = new[] { 4, 1, 1, 1, 4, 4, 1, 1 },
            },
            [37] = new Dictionary<int, int[]>
            {
                [0] = new[] { 5, 1, 1, 2, 1, 1, 5, 1 },
                [3] = new[] { 3, 1, 1, 1, 3, 4, 2, 2 },
                [6] = new[] { 3, 1, 1, 1, 5, 1, 1, 4 },
            },
            [38] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 2, 1, 2, 2, 6 },
                [3] = new[] { 2, 1, 1, 1, 3, 5, 1, 3 },
                [6] = new[] { 4, 1, 1, 1, 5, 1, 2, 2 },
            },
            [39] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 1, 2, 1, 2, 3, 4 },
                [3] = new[] { 3, 1, 1, 1, 3, 5, 2, 1 },
                [6] = new[] { 3, 1, 1, 1, 5, 2, 1, 3 },
            },
            [40] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 1, 2, 1, 2, 4, 2 },
                [3] = new[] { 2, 1, 1, 1, 3, 6, 1, 2 },
                [6] = new[] { 4, 1, 1, 1, 5, 2, 2, 1 },
            },
            [41] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 2, 1, 3, 2, 5 },
                [3] = new[] { 2, 1, 1, 1, 4, 1, 2, 5 },
                [6] = new[] { 3, 1, 1, 1, 5, 3, 1, 2 },
            },
            [42] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 1, 2, 1, 3, 3, 3 },
                [3] = new[] { 3, 1, 1, 1, 4, 1, 3, 3 },
                [6] = new[] { 3, 1, 1, 1, 5, 4, 1, 1 },
            },
            [43] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 2, 1, 4, 1, 6 },
                [3] = new[] { 4, 1, 1, 1, 4, 1, 4, 1 },
                [6] = new[] { 2, 1, 1, 1, 6, 1, 1, 4 },
            },
            [44] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 2, 1, 4, 2, 4 },
                [3] = new[] { 1, 1, 1, 1, 4, 2, 1, 6 },
                [6] = new[] { 3, 1, 1, 1, 6, 1, 2, 2 },
            },
            [45] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 1, 2, 1, 4, 3, 2 },
                [3] = new[] { 2, 1, 1, 1, 4, 2, 2, 4 },
                [6] = new[] { 2, 1, 1, 1, 6, 2, 1, 3 },
            },
            [46] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 2, 1, 5, 1, 5 },
                [3] = new[] { 3, 1, 1, 1, 4, 2, 3, 2 },
                [6] = new[] { 3, 1, 1, 1, 6, 2, 2, 1 },
            },
            [47] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 2, 1, 5, 2, 3 },
                [3] = new[] { 1, 1, 1, 1, 4, 3, 1, 5 },
                [6] = new[] { 2, 1, 1, 1, 6, 3, 1, 2 },
            },
            [48] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 2, 1, 6, 1, 4 },
                [3] = new[] { 2, 1, 1, 1, 4, 3, 2, 3 },
                [6] = new[] { 1, 1, 1, 2, 1, 1, 4, 6 },
            },
            [49] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 2, 2, 1, 3, 5 },
                [3] = new[] { 3, 1, 1, 1, 4, 3, 3, 1 },
                [6] = new[] { 2, 1, 1, 2, 1, 1, 5, 4 },
            },
            [50] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 1, 2, 2, 1, 4, 3 },
                [3] = new[] { 1, 1, 1, 1, 4, 4, 1, 4 },
                [6] = new[] { 3, 1, 1, 2, 1, 1, 6, 2 },
            },
            [51] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 1, 2, 2, 1, 5, 1 },
                [3] = new[] { 2, 1, 1, 1, 4, 4, 2, 2 },
                [6] = new[] { 1, 1, 1, 2, 1, 2, 4, 5 },
            },
            [52] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 2, 2, 2, 2, 6 },
                [3] = new[] { 1, 1, 1, 1, 4, 5, 1, 3 },
                [6] = new[] { 2, 1, 1, 2, 1, 2, 5, 3 },
            },
            [53] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 2, 2, 2, 3, 4 },
                [3] = new[] { 2, 1, 1, 1, 4, 5, 2, 1 },
                [6] = new[] { 3, 1, 1, 2, 1, 2, 6, 1 },
            },
            [54] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 1, 2, 2, 2, 4, 2 },
                [3] = new[] { 1, 1, 1, 1, 5, 1, 2, 5 },
                [6] = new[] { 1, 1, 1, 2, 1, 3, 4, 4 },
            },
            [55] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 2, 2, 3, 2, 5 },
                [3] = new[] { 2, 1, 1, 1, 5, 1, 3, 3 },
                [6] = new[] { 2, 1, 1, 2, 1, 3, 5, 2 },
            },
            [56] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 2, 2, 3, 3, 3 },
                [3] = new[] { 3, 1, 1, 1, 5, 1, 4, 1 },
                [6] = new[] { 1, 1, 1, 2, 1, 4, 4, 3 },
            },
            [57] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 1, 2, 2, 3, 4, 1 },
                [3] = new[] { 1, 1, 1, 1, 5, 2, 2, 4 },
                [6] = new[] { 2, 1, 1, 2, 1, 4, 5, 1 },
            },
            [58] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 2, 2, 4, 2, 4 },
                [3] = new[] { 2, 1, 1, 1, 5, 2, 3, 2 },
                [6] = new[] { 1, 1, 1, 2, 1, 5, 4, 2 },
            },
            [59] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 2, 2, 4, 3, 2 },
                [3] = new[] { 1, 1, 1, 1, 5, 3, 2, 3 },
                [6] = new[] { 6, 1, 1, 2, 2, 1, 1, 3 },
            },
            [60] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 2, 3, 1, 3, 5 },
                [3] = new[] { 2, 1, 1, 1, 5, 3, 3, 1 },
                [6] = new[] { 1, 1, 1, 2, 2, 1, 5, 4 },
            },
            [61] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 2, 3, 1, 4, 3 },
                [3] = new[] { 1, 1, 1, 1, 5, 4, 2, 2 },
                [6] = new[] { 2, 1, 1, 2, 2, 1, 6, 2 },
            },
            [62] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 1, 2, 3, 1, 5, 1 },
                [3] = new[] { 1, 1, 1, 1, 6, 1, 3, 3 },
                [6] = new[] { 6, 1, 1, 2, 2, 2, 1, 2 },
            },
            [63] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 2, 3, 2, 3, 4 },
                [3] = new[] { 2, 1, 1, 1, 6, 1, 4, 1 },
                [6] = new[] { 1, 1, 1, 2, 2, 2, 5, 3 },
            },
            [64] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 2, 3, 2, 4, 2 },
                [3] = new[] { 1, 1, 1, 1, 6, 2, 3, 2 },
                [6] = new[] { 2, 1, 1, 2, 2, 2, 6, 1 },
            },
            [65] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 2, 3, 3, 3, 3 },
                [3] = new[] { 1, 1, 1, 1, 6, 3, 3, 1 },
                [6] = new[] { 6, 1, 1, 2, 2, 3, 1, 1 },
            },
            [66] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 2, 3, 3, 4, 1 },
                [3] = new[] { 4, 1, 1, 2, 1, 1, 1, 6 },
                [6] = new[] { 1, 1, 1, 2, 2, 3, 5, 2 },
            },
            [67] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 2, 4, 1, 4, 3 },
                [3] = new[] { 5, 1, 1, 2, 1, 1, 2, 4 },
                [6] = new[] { 1, 1, 1, 2, 2, 4, 5, 1 },
            },
            [68] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 2, 4, 1, 5, 1 },
                [3] = new[] { 6, 1, 1, 2, 1, 1, 3, 2 },
                [6] = new[] { 5, 1, 1, 2, 3, 1, 1, 3 },
            },
            [69] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 2, 4, 2, 4, 2 },
                [3] = new[] { 4, 1, 1, 2, 1, 2, 1, 5 },
                [6] = new[] { 6, 1, 1, 2, 3, 1, 2, 1 },
            },
            [70] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 2, 4, 3, 4, 1 },
                [3] = new[] { 5, 1, 1, 2, 1, 2, 2, 3 },
                [6] = new[] { 1, 1, 1, 2, 3, 1, 6, 2 },
            },
            [71] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 3, 1, 1, 2, 6 },
                [3] = new[] { 6, 1, 1, 2, 1, 2, 3, 1 },
                [6] = new[] { 5, 1, 1, 2, 3, 2, 1, 2 },
            },
            [72] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 1, 3, 1, 1, 3, 4 },
                [3] = new[] { 4, 1, 1, 2, 1, 3, 1, 4 },
                [6] = new[] { 1, 1, 1, 2, 3, 2, 6, 1 },
            },
            [73] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 1, 3, 1, 1, 4, 2 },
                [3] = new[] { 5, 1, 1, 2, 1, 3, 2, 2 },
                [6] = new[] { 5, 1, 1, 2, 3, 3, 1, 1 },
            },
            [74] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 3, 1, 2, 2, 5 },
                [3] = new[] { 4, 1, 1, 2, 1, 4, 1, 3 },
                [6] = new[] { 4, 1, 1, 2, 4, 1, 1, 3 },
            },
            [75] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 1, 3, 1, 2, 3, 3 },
                [3] = new[] { 5, 1, 1, 2, 1, 4, 2, 1 },
                [6] = new[] { 5, 1, 1, 2, 4, 1, 2, 1 },
            },
            [76] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 1, 3, 1, 2, 4, 1 },
                [3] = new[] { 4, 1, 1, 2, 1, 5, 1, 2 },
                [6] = new[] { 4, 1, 1, 2, 4, 2, 1, 2 },
            },
            [77] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 3, 1, 3, 1, 6 },
                [3] = new[] { 4, 1, 1, 2, 1, 6, 1, 1 },
                [6] = new[] { 4, 1, 1, 2, 4, 3, 1, 1 },
            },
            [78] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 3, 1, 3, 2, 4 },
                [3] = new[] { 3, 1, 1, 2, 2, 1, 1, 6 },
                [6] = new[] { 3, 1, 1, 2, 5, 1, 1, 3 },
            },
            [79] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 1, 3, 1, 3, 3, 2 },
                [3] = new[] { 4, 1, 1, 2, 2, 1, 2, 4 },
                [6] = new[] { 4, 1, 1, 2, 5, 1, 2, 1 },
            },
            [80] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 3, 1, 4, 1, 5 },
                [3] = new[] { 5, 1, 1, 2, 2, 1, 3, 2 },
                [6] = new[] { 3, 1, 1, 2, 5, 2, 1, 2 },
            },
            [81] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 3, 1, 4, 2, 3 },
                [3] = new[] { 3, 1, 1, 2, 2, 2, 1, 5 },
                [6] = new[] { 3, 1, 1, 2, 5, 3, 1, 1 },
            },
            [82] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 3, 1, 5, 1, 4 },
                [3] = new[] { 4, 1, 1, 2, 2, 2, 2, 3 },
                [6] = new[] { 2, 1, 1, 2, 6, 1, 1, 3 },
            },
            [83] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 3, 1, 6, 1, 3 },
                [3] = new[] { 5, 1, 1, 2, 2, 2, 3, 1 },
                [6] = new[] { 3, 1, 1, 2, 6, 1, 2, 1 },
            },
            [84] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 3, 2, 1, 2, 6 },
                [3] = new[] { 3, 1, 1, 2, 2, 3, 1, 4 },
                [6] = new[] { 2, 1, 1, 2, 6, 2, 1, 2 },
            },
            [85] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 3, 2, 1, 3, 4 },
                [3] = new[] { 4, 1, 1, 2, 2, 3, 2, 2 },
                [6] = new[] { 2, 1, 1, 2, 6, 3, 1, 1 },
            },
            [86] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 1, 3, 2, 1, 4, 2 },
                [3] = new[] { 3, 1, 1, 2, 2, 4, 1, 3 },
                [6] = new[] { 1, 1, 1, 3, 1, 1, 4, 5 },
            },
            [87] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 3, 2, 2, 2, 5 },
                [3] = new[] { 4, 1, 1, 2, 2, 4, 2, 1 },
                [6] = new[] { 2, 1, 1, 3, 1, 1, 5, 3 },
            },
            [88] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 3, 2, 2, 3, 3 },
                [3] = new[] { 3, 1, 1, 2, 2, 5, 1, 2 },
                [6] = new[] { 3, 1, 1, 3, 1, 1, 6, 1 },
            },
            [89] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 1, 3, 2, 2, 4, 1 },
                [3] = new[] { 3, 1, 1, 2, 2, 6, 1, 1 },
                [6] = new[] { 1, 1, 1, 3, 1, 2, 4, 4 },
            },
            [90] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 3, 2, 3, 2, 4 },
                [3] = new[] { 2, 1, 1, 2, 3, 1, 1, 6 },
                [6] = new[] { 2, 1, 1, 3, 1, 2, 5, 2 },
            },
            [91] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 3, 2, 3, 3, 2 },
                [3] = new[] { 3, 1, 1, 2, 3, 1, 2, 4 },
                [6] = new[] { 1, 1, 1, 3, 1, 3, 4, 3 },
            },
            [92] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 3, 2, 4, 2, 3 },
                [3] = new[] { 4, 1, 1, 2, 3, 1, 3, 2 },
                [6] = new[] { 2, 1, 1, 3, 1, 3, 5, 1 },
            },
            [93] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 3, 2, 5, 2, 2 },
                [3] = new[] { 2, 1, 1, 2, 3, 2, 1, 5 },
                [6] = new[] { 1, 1, 1, 3, 1, 4, 4, 2 },
            },
            [94] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 3, 3, 1, 3, 4 },
                [3] = new[] { 3, 1, 1, 2, 3, 2, 2, 3 },
                [6] = new[] { 1, 1, 1, 3, 1, 5, 4, 1 },
            },
            [95] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 3, 3, 1, 4, 2 },
                [3] = new[] { 4, 1, 1, 2, 3, 2, 3, 1 },
                [6] = new[] { 6, 1, 1, 3, 2, 1, 1, 2 },
            },
            [96] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 3, 3, 2, 3, 3 },
                [3] = new[] { 2, 1, 1, 2, 3, 3, 1, 4 },
                [6] = new[] { 1, 1, 1, 3, 2, 1, 5, 3 },
            },
            [97] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 3, 3, 2, 4, 1 },
                [3] = new[] { 3, 1, 1, 2, 3, 3, 2, 2 },
                [6] = new[] { 2, 1, 1, 3, 2, 1, 6, 1 },
            },
            [98] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 3, 3, 3, 3, 2 },
                [3] = new[] { 2, 1, 1, 2, 3, 4, 1, 3 },
                [6] = new[] { 6, 1, 1, 3, 2, 2, 1, 1 },
            },
            [99] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 3, 4, 1, 4, 2 },
                [3] = new[] { 3, 1, 1, 2, 3, 4, 2, 1 },
                [6] = new[] { 1, 1, 1, 3, 2, 2, 5, 2 },
            },
            [100] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 4, 1, 1, 2, 5 },
                [3] = new[] { 2, 1, 1, 2, 3, 5, 1, 2 },
                [6] = new[] { 1, 1, 1, 3, 2, 3, 5, 1 },
            },
            [101] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 1, 4, 1, 1, 3, 3 },
                [3] = new[] { 2, 1, 1, 2, 3, 6, 1, 1 },
                [6] = new[] { 5, 1, 1, 3, 3, 1, 1, 2 },
            },
            [102] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 1, 4, 1, 1, 4, 1 },
                [3] = new[] { 1, 1, 1, 2, 4, 1, 1, 6 },
                [6] = new[] { 1, 1, 1, 3, 3, 1, 6, 1 },
            },
            [103] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 4, 1, 2, 1, 6 },
                [3] = new[] { 2, 1, 1, 2, 4, 1, 2, 4 },
                [6] = new[] { 5, 1, 1, 3, 3, 2, 1, 1 },
            },
            [104] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 4, 1, 2, 2, 4 },
                [3] = new[] { 3, 1, 1, 2, 4, 1, 3, 2 },
                [6] = new[] { 4, 1, 1, 3, 4, 1, 1, 2 },
            },
            [105] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 1, 4, 1, 2, 3, 2 },
                [3] = new[] { 1, 1, 1, 2, 4, 2, 1, 5 },
                [6] = new[] { 4, 1, 1, 3, 4, 2, 1, 1 },
            },
            [106] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 4, 1, 3, 1, 5 },
                [3] = new[] { 2, 1, 1, 2, 4, 2, 2, 3 },
                [6] = new[] { 3, 1, 1, 3, 5, 1, 1, 2 },
            },
            [107] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 4, 1, 3, 2, 3 },
                [3] = new[] { 3, 1, 1, 2, 4, 2, 3, 1 },
                [6] = new[] { 3, 1, 1, 3, 5, 2, 1, 1 },
            },
            [108] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 1, 4, 1, 3, 3, 1 },
                [3] = new[] { 1, 1, 1, 2, 4, 3, 1, 4 },
                [6] = new[] { 2, 1, 1, 3, 6, 1, 1, 2 },
            },
            [109] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 4, 1, 4, 1, 4 },
                [3] = new[] { 2, 1, 1, 2, 4, 3, 2, 2 },
                [6] = new[] { 2, 1, 1, 3, 6, 2, 1, 1 },
            },
            [110] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 4, 1, 4, 2, 2 },
                [3] = new[] { 1, 1, 1, 2, 4, 4, 1, 3 },
                [6] = new[] { 1, 1, 1, 4, 1, 1, 4, 4 },
            },
            [111] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 4, 1, 5, 1, 3 },
                [3] = new[] { 2, 1, 1, 2, 4, 4, 2, 1 },
                [6] = new[] { 2, 1, 1, 4, 1, 1, 5, 2 },
            },
            [112] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 4, 1, 5, 2, 1 },
                [3] = new[] { 1, 1, 1, 2, 4, 5, 1, 2 },
                [6] = new[] { 1, 1, 1, 4, 1, 2, 4, 3 },
            },
            [113] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 4, 2, 1, 2, 5 },
                [3] = new[] { 1, 1, 1, 2, 5, 1, 2, 4 },
                [6] = new[] { 2, 1, 1, 4, 1, 2, 5, 1 },
            },
            [114] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 4, 2, 1, 3, 3 },
                [3] = new[] { 2, 1, 1, 2, 5, 1, 3, 2 },
                [6] = new[] { 1, 1, 1, 4, 1, 3, 4, 2 },
            },
            [115] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 1, 4, 2, 1, 4, 1 },
                [3] = new[] { 1, 1, 1, 2, 5, 2, 2, 3 },
                [6] = new[] { 1, 1, 1, 4, 1, 4, 4, 1 },
            },
            [116] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 4, 2, 2, 2, 4 },
                [3] = new[] { 2, 1, 1, 2, 5, 2, 3, 1 },
                [6] = new[] { 6, 1, 1, 4, 2, 1, 1, 1 },
            },
            [117] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 4, 2, 2, 3, 2 },
                [3] = new[] { 1, 1, 1, 2, 5, 3, 2, 2 },
                [6] = new[] { 1, 1, 1, 4, 2, 1, 5, 2 },
            },
            [118] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 4, 2, 3, 2, 3 },
                [3] = new[] { 1, 1, 1, 2, 5, 4, 2, 1 },
                [6] = new[] { 1, 1, 1, 4, 2, 2, 5, 1 },
            },
            [119] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 4, 2, 3, 3, 1 },
                [3] = new[] { 1, 1, 1, 2, 6, 1, 3, 2 },
                [6] = new[] { 5, 1, 1, 4, 3, 1, 1, 1 },
            },
            [120] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 4, 2, 4, 2, 2 },
                [3] = new[] { 1, 1, 1, 2, 6, 2, 3, 1 },
                [6] = new[] { 4, 1, 1, 4, 4, 1, 1, 1 },
            },
            [121] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 4, 2, 5, 2, 1 },
                [3] = new[] { 4, 1, 1, 3, 1, 1, 1, 5 },
                [6] = new[] { 3, 1, 1, 4, 5, 1, 1, 1 },
            },
            [122] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 4, 3, 1, 4, 1 },
                [3] = new[] { 5, 1, 1, 3, 1, 1, 2, 3 },
                [6] = new[] { 1, 1, 1, 5, 1, 1, 4, 3 },
            },
            [123] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 4, 3, 3, 3, 1 },
                [3] = new[] { 6, 1, 1, 3, 1, 1, 3, 1 },
                [6] = new[] { 2, 1, 1, 5, 1, 1, 5, 1 },
            },
            [124] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 5, 1, 1, 1, 6 },
                [3] = new[] { 4, 1, 1, 3, 1, 2, 1, 4 },
                [6] = new[] { 1, 1, 1, 5, 1, 2, 4, 2 },
            },
            [125] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 5, 1, 1, 2, 4 },
                [3] = new[] { 5, 1, 1, 3, 1, 2, 2, 2 },
                [6] = new[] { 1, 1, 1, 5, 1, 3, 4, 1 },
            },
            [126] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 1, 5, 1, 1, 3, 2 },
                [3] = new[] { 4, 1, 1, 3, 1, 3, 1, 3 },
                [6] = new[] { 1, 1, 1, 5, 2, 1, 5, 1 },
            },
            [127] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 5, 1, 2, 1, 5 },
                [3] = new[] { 5, 1, 1, 3, 1, 3, 2, 1 },
                [6] = new[] { 1, 1, 1, 6, 1, 1, 4, 2 },
            },
            [128] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 5, 1, 2, 2, 3 },
                [3] = new[] { 4, 1, 1, 3, 1, 4, 1, 2 },
                [6] = new[] { 1, 1, 1, 6, 1, 2, 4, 1 },
            },
            [129] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 1, 5, 1, 2, 3, 1 },
                [3] = new[] { 4, 1, 1, 3, 1, 5, 1, 1 },
                [6] = new[] { 1, 2, 1, 1, 1, 1, 4, 6 },
            },
            [130] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 5, 1, 3, 1, 4 },
                [3] = new[] { 3, 1, 1, 3, 2, 1, 1, 5 },
                [6] = new[] { 2, 2, 1, 1, 1, 1, 5, 4 },
            },
            [131] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 5, 1, 3, 2, 2 },
                [3] = new[] { 4, 1, 1, 3, 2, 1, 2, 3 },
                [6] = new[] { 3, 2, 1, 1, 1, 1, 6, 2 },
            },
            [132] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 5, 1, 4, 1, 3 },
                [3] = new[] { 5, 1, 1, 3, 2, 1, 3, 1 },
                [6] = new[] { 1, 2, 1, 1, 1, 2, 4, 5 },
            },
            [133] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 5, 1, 4, 2, 1 },
                [3] = new[] { 3, 1, 1, 3, 2, 2, 1, 4 },
                [6] = new[] { 2, 2, 1, 1, 1, 2, 5, 3 },
            },
            [134] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 5, 1, 5, 1, 2 },
                [3] = new[] { 4, 1, 1, 3, 2, 2, 2, 2 },
                [6] = new[] { 3, 2, 1, 1, 1, 2, 6, 1 },
            },
            [135] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 5, 2, 1, 2, 4 },
                [3] = new[] { 3, 1, 1, 3, 2, 3, 1, 3 },
                [6] = new[] { 1, 2, 1, 1, 1, 3, 4, 4 },
            },
            [136] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 5, 2, 2, 2, 3 },
                [3] = new[] { 4, 1, 1, 3, 2, 3, 2, 1 },
                [6] = new[] { 2, 2, 1, 1, 1, 3, 5, 2 },
            },
            [137] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 5, 2, 3, 2, 2 },
                [3] = new[] { 3, 1, 1, 3, 2, 4, 1, 2 },
                [6] = new[] { 1, 2, 1, 1, 1, 4, 4, 3 },
            },
            [138] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 6, 1, 1, 1, 5 },
                [3] = new[] { 3, 1, 1, 3, 2, 5, 1, 1 },
                [6] = new[] { 2, 2, 1, 1, 1, 4, 5, 1 },
            },
            [139] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 1, 6, 1, 1, 3, 1 },
                [3] = new[] { 2, 1, 1, 3, 3, 1, 1, 5 },
                [6] = new[] { 1, 2, 1, 1, 1, 5, 4, 2 },
            },
            [140] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 6, 1, 2, 2, 2 },
                [3] = new[] { 3, 1, 1, 3, 3, 1, 2, 3 },
                [6] = new[] { 6, 2, 1, 1, 2, 1, 1, 3 },
            },
            [141] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 1, 6, 1, 3, 2, 1 },
                [3] = new[] { 4, 1, 1, 3, 3, 1, 3, 1 },
                [6] = new[] { 1, 2, 1, 1, 2, 1, 5, 4 },
            },
            [142] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 1, 6, 1, 5, 1, 1 },
                [3] = new[] { 2, 1, 1, 3, 3, 2, 1, 4 },
                [6] = new[] { 2, 2, 1, 1, 2, 1, 6, 2 },
            },
            [143] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 1, 1, 1, 1, 3, 5 },
                [3] = new[] { 3, 1, 1, 3, 3, 2, 2, 2 },
                [6] = new[] { 6, 2, 1, 1, 2, 2, 1, 2 },
            },
            [144] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 2, 1, 1, 1, 1, 4, 3 },
                [3] = new[] { 2, 1, 1, 3, 3, 3, 1, 3 },
                [6] = new[] { 1, 2, 1, 1, 2, 2, 5, 3 },
            },
            [145] = new Dictionary<int, int[]>
            {
                [0] = new[] { 5, 2, 1, 1, 1, 1, 5, 1 },
                [3] = new[] { 3, 1, 1, 3, 3, 3, 2, 1 },
                [6] = new[] { 2, 2, 1, 1, 2, 2, 6, 1 },
            },
            [146] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 1, 1, 1, 2, 2, 6 },
                [3] = new[] { 2, 1, 1, 3, 3, 4, 1, 2 },
                [6] = new[] { 6, 2, 1, 1, 2, 3, 1, 1 },
            },
            [147] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 1, 1, 1, 2, 3, 4 },
                [3] = new[] { 2, 1, 1, 3, 3, 5, 1, 1 },
                [6] = new[] { 1, 2, 1, 1, 2, 3, 5, 2 },
            },
            [148] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 2, 1, 1, 1, 2, 4, 2 },
                [3] = new[] { 1, 1, 1, 3, 4, 1, 1, 5 },
                [6] = new[] { 1, 2, 1, 1, 2, 4, 5, 1 },
            },
            [149] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 1, 1, 1, 3, 2, 5 },
                [3] = new[] { 2, 1, 1, 3, 4, 1, 2, 3 },
                [6] = new[] { 5, 2, 1, 1, 3, 1, 1, 3 },
            },
            [150] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 1, 1, 1, 3, 3, 3 },
                [3] = new[] { 3, 1, 1, 3, 4, 1, 3, 1 },
                [6] = new[] { 6, 2, 1, 1, 3, 1, 2, 1 },
            },
            [151] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 2, 1, 1, 1, 3, 4, 1 },
                [3] = new[] { 1, 1, 1, 3, 4, 2, 1, 4 },
                [6] = new[] { 1, 2, 1, 1, 3, 1, 6, 2 },
            },
            [152] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 1, 1, 4, 1, 6 },
                [3] = new[] { 2, 1, 1, 3, 4, 2, 2, 2 },
                [6] = new[] { 5, 2, 1, 1, 3, 2, 1, 2 },
            },
            [153] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 1, 1, 1, 4, 2, 4 },
                [3] = new[] { 1, 1, 1, 3, 4, 3, 1, 3 },
                [6] = new[] { 1, 2, 1, 1, 3, 2, 6, 1 },
            },
            [154] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 1, 1, 5, 1, 5 },
                [3] = new[] { 2, 1, 1, 3, 4, 3, 2, 1 },
                [6] = new[] { 5, 2, 1, 1, 3, 3, 1, 1 },
            },
            [155] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 1, 1, 2, 1, 3, 5 },
                [3] = new[] { 1, 1, 1, 3, 4, 4, 1, 2 },
                [6] = new[] { 4, 2, 1, 1, 4, 1, 1, 3 },
            },
            [156] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 1, 1, 2, 1, 4, 3 },
                [3] = new[] { 1, 1, 1, 3, 4, 5, 1, 1 },
                [6] = new[] { 5, 2, 1, 1, 4, 1, 2, 1 },
            },
            [157] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 2, 1, 1, 2, 1, 5, 1 },
                [3] = new[] { 1, 1, 1, 3, 5, 1, 2, 3 },
                [6] = new[] { 4, 2, 1, 1, 4, 2, 1, 2 },
            },
            [158] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 1, 2, 2, 2, 6 },
                [3] = new[] { 2, 1, 1, 3, 5, 1, 3, 1 },
                [6] = new[] { 4, 2, 1, 1, 4, 3, 1, 1 },
            },
            [159] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 1, 1, 2, 2, 3, 4 },
                [3] = new[] { 1, 1, 1, 3, 5, 2, 2, 2 },
                [6] = new[] { 3, 2, 1, 1, 5, 1, 1, 3 },
            },
            [160] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 1, 1, 2, 2, 4, 2 },
                [3] = new[] { 1, 1, 1, 3, 5, 3, 2, 1 },
                [6] = new[] { 4, 2, 1, 1, 5, 1, 2, 1 },
            },
            [161] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 1, 2, 3, 2, 5 },
                [3] = new[] { 1, 1, 1, 3, 6, 1, 3, 1 },
                [6] = new[] { 3, 2, 1, 1, 5, 2, 1, 2 },
            },
            [162] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 1, 1, 2, 3, 3, 3 },
                [3] = new[] { 4, 1, 1, 4, 1, 1, 1, 4 },
                [6] = new[] { 3, 2, 1, 1, 5, 3, 1, 1 },
            },
            [163] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 1, 2, 4, 2, 4 },
                [3] = new[] { 5, 1, 1, 4, 1, 1, 2, 2 },
                [6] = new[] { 2, 2, 1, 1, 6, 1, 1, 3 },
            },
            [164] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 1, 2, 5, 2, 3 },
                [3] = new[] { 4, 1, 1, 4, 1, 2, 1, 3 },
                [6] = new[] { 3, 2, 1, 1, 6, 1, 2, 1 },
            },
            [165] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 1, 3, 1, 3, 5 },
                [3] = new[] { 5, 1, 1, 4, 1, 2, 2, 1 },
                [6] = new[] { 2, 2, 1, 1, 6, 2, 1, 2 },
            },
            [166] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 1, 1, 3, 1, 4, 3 },
                [3] = new[] { 4, 1, 1, 4, 1, 3, 1, 2 },
                [6] = new[] { 2, 2, 1, 1, 6, 3, 1, 1 },
            },
            [167] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 1, 1, 3, 1, 5, 1 },
                [3] = new[] { 4, 1, 1, 4, 1, 4, 1, 1 },
                [6] = new[] { 2, 1, 2, 1, 1, 1, 4, 5 },
            },
            [168] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 1, 3, 2, 3, 4 },
                [3] = new[] { 3, 1, 1, 4, 2, 1, 1, 4 },
                [6] = new[] { 3, 1, 2, 1, 1, 1, 5, 3 },
            },
            [169] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 1, 1, 3, 2, 4, 2 },
                [3] = new[] { 4, 1, 1, 4, 2, 1, 2, 2 },
                [6] = new[] { 4, 1, 2, 1, 1, 1, 6, 1 },
            },
            [170] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 1, 3, 3, 3, 3 },
                [3] = new[] { 3, 1, 1, 4, 2, 2, 1, 3 },
                [6] = new[] { 1, 1, 2, 1, 1, 2, 3, 6 },
            },
            [171] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 1, 3, 4, 3, 2 },
                [3] = new[] { 4, 1, 1, 4, 2, 2, 2, 1 },
                [6] = new[] { 2, 1, 2, 1, 1, 2, 4, 4 },
            },
            [172] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 1, 4, 1, 4, 3 },
                [3] = new[] { 3, 1, 1, 4, 2, 3, 1, 2 },
                [6] = new[] { 3, 1, 2, 1, 1, 2, 5, 2 },
            },
            [173] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 1, 1, 4, 1, 5, 1 },
                [3] = new[] { 3, 1, 1, 4, 2, 4, 1, 1 },
                [6] = new[] { 1, 1, 2, 1, 1, 3, 3, 5 },
            },
            [174] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 1, 4, 2, 4, 2 },
                [3] = new[] { 2, 1, 1, 4, 3, 1, 1, 4 },
                [6] = new[] { 2, 1, 2, 1, 1, 3, 4, 3 },
            },
            [175] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 1, 5, 1, 5, 1 },
                [3] = new[] { 3, 1, 1, 4, 3, 1, 2, 2 },
                [6] = new[] { 3, 1, 2, 1, 1, 3, 5, 1 },
            },
            [176] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 2, 1, 1, 1, 2, 6 },
                [3] = new[] { 2, 1, 1, 4, 3, 2, 1, 3 },
                [6] = new[] { 1, 1, 2, 1, 1, 4, 3, 4 },
            },
            [177] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 2, 1, 1, 1, 3, 4 },
                [3] = new[] { 3, 1, 1, 4, 3, 2, 2, 1 },
                [6] = new[] { 2, 1, 2, 1, 1, 4, 4, 2 },
            },
            [178] = new Dictionary<int, int[]>
            {
                [0] = new[] { 5, 1, 2, 1, 1, 1, 4, 2 },
                [3] = new[] { 2, 1, 1, 4, 3, 3, 1, 2 },
                [6] = new[] { 1, 1, 2, 1, 1, 5, 3, 3 },
            },
            [179] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 2, 1, 1, 2, 2, 5 },
                [3] = new[] { 2, 1, 1, 4, 3, 4, 1, 1 },
                [6] = new[] { 2, 1, 2, 1, 1, 5, 4, 1 },
            },
            [180] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 2, 1, 1, 2, 3, 3 },
                [3] = new[] { 1, 1, 1, 4, 4, 1, 1, 4 },
                [6] = new[] { 1, 1, 2, 1, 1, 6, 3, 2 },
            },
            [181] = new Dictionary<int, int[]>
            {
                [0] = new[] { 5, 1, 2, 1, 1, 2, 4, 1 },
                [3] = new[] { 2, 1, 1, 4, 4, 1, 2, 2 },
                [6] = new[] { 1, 2, 1, 2, 1, 1, 4, 5 },
            },
            [182] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 2, 1, 1, 3, 1, 6 },
                [3] = new[] { 1, 1, 1, 4, 4, 2, 1, 3 },
                [6] = new[] { 2, 2, 1, 2, 1, 1, 5, 3 },
            },
            [183] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 2, 1, 1, 3, 2, 4 },
                [3] = new[] { 2, 1, 1, 4, 4, 2, 2, 1 },
                [6] = new[] { 3, 2, 1, 2, 1, 1, 6, 1 },
            },
            [184] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 2, 1, 1, 3, 3, 2 },
                [3] = new[] { 1, 1, 1, 4, 4, 3, 1, 2 },
                [6] = new[] { 1, 1, 2, 1, 2, 1, 4, 5 },
            },
            [185] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 2, 1, 1, 4, 1, 5 },
                [3] = new[] { 1, 1, 1, 4, 4, 4, 1, 1 },
                [6] = new[] { 1, 2, 1, 2, 1, 2, 4, 4 },
            },
            [186] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 2, 1, 1, 4, 2, 3 },
                [3] = new[] { 1, 1, 1, 4, 5, 1, 2, 2 },
                [6] = new[] { 2, 2, 1, 2, 1, 2, 5, 2 },
            },
            [187] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 2, 1, 1, 4, 3, 1 },
                [3] = new[] { 1, 1, 1, 4, 5, 2, 2, 1 },
                [6] = new[] { 1, 1, 2, 1, 2, 2, 4, 4 },
            },
            [188] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 2, 1, 1, 5, 1, 4 },
                [3] = new[] { 4, 1, 1, 5, 1, 1, 1, 3 },
                [6] = new[] { 2, 1, 2, 1, 2, 2, 5, 2 },
            },
            [189] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 2, 1, 1, 5, 2, 2 },
                [3] = new[] { 5, 1, 1, 5, 1, 1, 2, 1 },
                [6] = new[] { 2, 2, 1, 2, 1, 3, 5, 1 },
            },
            [190] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 1, 2, 1, 1, 2, 6 },
                [3] = new[] { 4, 1, 1, 5, 1, 2, 1, 2 },
                [6] = new[] { 1, 1, 2, 1, 2, 3, 4, 3 },
            },
            [191] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 1, 2, 1, 1, 3, 4 },
                [3] = new[] { 4, 1, 1, 5, 1, 3, 1, 1 },
                [6] = new[] { 1, 2, 1, 2, 1, 4, 4, 2 },
            },
            [192] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 2, 1, 2, 1, 1, 4, 2 },
                [3] = new[] { 3, 1, 1, 5, 2, 1, 1, 3 },
                [6] = new[] { 1, 1, 2, 1, 2, 4, 4, 2 },
            },
            [193] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 2, 1, 2, 1, 2, 6 },
                [3] = new[] { 4, 1, 1, 5, 2, 1, 2, 1 },
                [6] = new[] { 1, 2, 1, 2, 1, 5, 4, 1 },
            },
            [194] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 1, 2, 1, 2, 2, 5 },
                [3] = new[] { 3, 1, 1, 5, 2, 2, 1, 2 },
                [6] = new[] { 1, 1, 2, 1, 2, 5, 4, 1 },
            },
            [195] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 1, 2, 1, 2, 3, 3 },
                [3] = new[] { 3, 1, 1, 5, 2, 3, 1, 1 },
                [6] = new[] { 6, 2, 1, 2, 2, 1, 1, 2 },
            },
            [196] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 2, 1, 2, 1, 2, 4, 1 },
                [3] = new[] { 2, 1, 1, 5, 3, 1, 1, 3 },
                [6] = new[] { 1, 2, 1, 2, 2, 1, 5, 3 },
            },
            [197] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 2, 1, 2, 2, 2, 5 },
                [3] = new[] { 3, 1, 1, 5, 3, 1, 2, 1 },
                [6] = new[] { 2, 2, 1, 2, 2, 1, 6, 1 },
            },
            [198] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 2, 1, 2, 2, 3, 3 },
                [3] = new[] { 2, 1, 1, 5, 3, 2, 1, 2 },
                [6] = new[] { 6, 1, 2, 1, 3, 1, 1, 2 },
            },
            [199] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 2, 1, 2, 2, 4, 1 },
                [3] = new[] { 2, 1, 1, 5, 3, 3, 1, 1 },
                [6] = new[] { 6, 2, 1, 2, 2, 2, 1, 1 },
            },
            [200] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 1, 2, 3, 1, 6 },
                [3] = new[] { 1, 1, 1, 5, 4, 1, 1, 3 },
                [6] = new[] { 1, 1, 2, 1, 3, 1, 5, 3 },
            },
            [201] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 2, 1, 4, 1, 5 },
                [3] = new[] { 2, 1, 1, 5, 4, 1, 2, 1 },
                [6] = new[] { 1, 2, 1, 2, 2, 2, 5, 2 },
            },
            [202] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 1, 2, 1, 4, 2, 3 },
                [3] = new[] { 1, 1, 1, 5, 4, 2, 1, 2 },
                [6] = new[] { 6, 1, 2, 1, 3, 2, 1, 1 },
            },
            [203] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 1, 2, 1, 4, 3, 1 },
                [3] = new[] { 1, 1, 1, 5, 4, 3, 1, 1 },
                [6] = new[] { 1, 1, 2, 1, 3, 2, 5, 2 },
            },
            [204] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 1, 2, 4, 1, 5 },
                [3] = new[] { 4, 1, 1, 6, 1, 1, 1, 2 },
                [6] = new[] { 1, 2, 1, 2, 2, 3, 5, 1 },
            },
            [205] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 2, 1, 2, 4, 2, 3 },
                [3] = new[] { 4, 1, 1, 6, 1, 2, 1, 1 },
                [6] = new[] { 1, 1, 2, 1, 3, 3, 5, 1 },
            },
            [206] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 1, 2, 5, 1, 4 },
                [3] = new[] { 3, 1, 1, 6, 2, 1, 1, 2 },
                [6] = new[] { 5, 2, 1, 2, 3, 1, 1, 2 },
            },
            [207] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 2, 2, 1, 2, 6 },
                [3] = new[] { 3, 1, 1, 6, 2, 2, 1, 1 },
                [6] = new[] { 1, 2, 1, 2, 3, 1, 6, 1 },
            },
            [208] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 1, 2, 2, 1, 3, 4 },
                [3] = new[] { 2, 1, 1, 6, 3, 1, 1, 2 },
                [6] = new[] { 5, 1, 2, 1, 4, 1, 1, 2 },
            },
            [209] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 1, 2, 2, 1, 4, 2 },
                [3] = new[] { 2, 1, 1, 6, 3, 2, 1, 1 },
                [6] = new[] { 5, 2, 1, 2, 3, 2, 1, 1 },
            },
            [210] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 1, 3, 1, 2, 6 },
                [3] = new[] { 4, 2, 1, 1, 1, 1, 1, 6 },
                [6] = new[] { 1, 1, 2, 1, 4, 1, 6, 1 },
            },
            [211] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 2, 2, 2, 2, 5 },
                [3] = new[] { 5, 2, 1, 1, 1, 1, 2, 4 },
                [6] = new[] { 5, 1, 2, 1, 4, 2, 1, 1 },
            },
            [212] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 1, 2, 2, 2, 3, 3 },
                [3] = new[] { 6, 2, 1, 1, 1, 1, 3, 2 },
                [6] = new[] { 4, 2, 1, 2, 4, 1, 1, 2 },
            },
            [213] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 1, 2, 2, 2, 4, 1 },
                [3] = new[] { 4, 2, 1, 1, 1, 2, 1, 5 },
                [6] = new[] { 4, 1, 2, 1, 5, 1, 1, 2 },
            },
            [214] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 1, 3, 2, 2, 5 },
                [3] = new[] { 5, 2, 1, 1, 1, 2, 2, 3 },
                [6] = new[] { 4, 2, 1, 2, 4, 2, 1, 1 },
            },
            [215] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 2, 1, 3, 2, 3, 3 },
                [3] = new[] { 6, 2, 1, 1, 1, 2, 3, 1 },
                [6] = new[] { 4, 1, 2, 1, 5, 2, 1, 1 },
            },
            [216] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 2, 1, 3, 2, 4, 1 },
                [3] = new[] { 4, 2, 1, 1, 1, 3, 1, 4 },
                [6] = new[] { 3, 2, 1, 2, 5, 1, 1, 2 },
            },
            [217] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 1, 3, 3, 2, 4 },
                [3] = new[] { 5, 2, 1, 1, 1, 3, 2, 2 },
                [6] = new[] { 3, 1, 2, 1, 6, 1, 1, 2 },
            },
            [218] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 2, 2, 4, 2, 3 },
                [3] = new[] { 4, 2, 1, 1, 1, 4, 1, 3 },
                [6] = new[] { 3, 2, 1, 2, 5, 2, 1, 1 },
            },
            [219] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 1, 3, 4, 2, 3 },
                [3] = new[] { 5, 2, 1, 1, 1, 4, 2, 1 },
                [6] = new[] { 3, 1, 2, 1, 6, 2, 1, 1 },
            },
            [220] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 2, 3, 1, 3, 4 },
                [3] = new[] { 4, 2, 1, 1, 1, 5, 1, 2 },
                [6] = new[] { 2, 2, 1, 2, 6, 1, 1, 2 },
            },
            [221] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 1, 2, 3, 1, 4, 2 },
                [3] = new[] { 4, 2, 1, 1, 1, 6, 1, 1 },
                [6] = new[] { 2, 2, 1, 2, 6, 2, 1, 1 },
            },
            [222] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 1, 4, 1, 3, 4 },
                [3] = new[] { 3, 2, 1, 1, 2, 1, 1, 6 },
                [6] = new[] { 1, 1, 2, 2, 1, 1, 3, 6 },
            },
            [223] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 2, 3, 2, 3, 3 },
                [3] = new[] { 4, 2, 1, 1, 2, 1, 2, 4 },
                [6] = new[] { 2, 1, 2, 2, 1, 1, 4, 4 },
            },
            [224] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 1, 2, 3, 2, 4, 1 },
                [3] = new[] { 5, 2, 1, 1, 2, 1, 3, 2 },
                [6] = new[] { 3, 1, 2, 2, 1, 1, 5, 2 },
            },
            [225] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 1, 4, 2, 3, 3 },
                [3] = new[] { 3, 2, 1, 1, 2, 2, 1, 5 },
                [6] = new[] { 1, 1, 2, 2, 1, 2, 3, 5 },
            },
            [226] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 2, 1, 4, 2, 4, 1 },
                [3] = new[] { 4, 2, 1, 1, 2, 2, 2, 3 },
                [6] = new[] { 2, 1, 2, 2, 1, 2, 4, 3 },
            },
            [227] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 1, 4, 3, 3, 2 },
                [3] = new[] { 5, 2, 1, 1, 2, 2, 3, 1 },
                [6] = new[] { 3, 1, 2, 2, 1, 2, 5, 1 },
            },
            [228] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 2, 4, 1, 4, 2 },
                [3] = new[] { 3, 2, 1, 1, 2, 3, 1, 4 },
                [6] = new[] { 1, 1, 2, 2, 1, 3, 3, 4 },
            },
            [229] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 1, 5, 1, 4, 2 },
                [3] = new[] { 4, 2, 1, 1, 2, 3, 2, 2 },
                [6] = new[] { 2, 1, 2, 2, 1, 3, 4, 2 },
            },
            [230] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 2, 4, 2, 4, 1 },
                [3] = new[] { 3, 2, 1, 1, 2, 4, 1, 3 },
                [6] = new[] { 1, 1, 2, 2, 1, 4, 3, 3 },
            },
            [231] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 1, 5, 2, 4, 1 },
                [3] = new[] { 4, 2, 1, 1, 2, 4, 2, 1 },
                [6] = new[] { 2, 1, 2, 2, 1, 4, 4, 1 },
            },
            [232] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 2, 2, 1, 1, 2, 5 },
                [3] = new[] { 3, 2, 1, 1, 2, 5, 1, 2 },
                [6] = new[] { 1, 1, 2, 2, 1, 5, 3, 2 },
            },
            [233] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 2, 2, 1, 1, 3, 3 },
                [3] = new[] { 3, 2, 1, 1, 2, 6, 1, 1 },
                [6] = new[] { 1, 1, 2, 2, 1, 6, 3, 1 },
            },
            [234] = new Dictionary<int, int[]>
            {
                [0] = new[] { 5, 1, 2, 2, 1, 1, 4, 1 },
                [3] = new[] { 2, 2, 1, 1, 3, 1, 1, 6 },
                [6] = new[] { 1, 2, 1, 3, 1, 1, 4, 4 },
            },
            [235] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 2, 2, 1, 2, 1, 6 },
                [3] = new[] { 3, 2, 1, 1, 3, 1, 2, 4 },
                [6] = new[] { 2, 2, 1, 3, 1, 1, 5, 2 },
            },
            [236] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 2, 2, 1, 2, 2, 4 },
                [3] = new[] { 4, 2, 1, 1, 3, 1, 3, 2 },
                [6] = new[] { 1, 1, 2, 2, 2, 1, 4, 4 },
            },
            [237] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 2, 2, 1, 2, 3, 2 },
                [3] = new[] { 2, 2, 1, 1, 3, 2, 1, 5 },
                [6] = new[] { 1, 2, 1, 3, 1, 2, 4, 3 },
            },
            [238] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 2, 2, 1, 3, 1, 5 },
                [3] = new[] { 3, 2, 1, 1, 3, 2, 2, 3 },
                [6] = new[] { 2, 2, 1, 3, 1, 2, 5, 1 },
            },
            [239] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 2, 2, 1, 3, 2, 3 },
                [3] = new[] { 4, 2, 1, 1, 3, 2, 3, 1 },
                [6] = new[] { 1, 1, 2, 2, 2, 2, 4, 3 },
            },
            [240] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 2, 2, 1, 3, 3, 1 },
                [3] = new[] { 2, 2, 1, 1, 3, 3, 1, 4 },
                [6] = new[] { 2, 1, 2, 2, 2, 2, 5, 1 },
            },
            [241] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 2, 2, 1, 4, 1, 4 },
                [3] = new[] { 3, 2, 1, 1, 3, 3, 2, 2 },
                [6] = new[] { 1, 1, 2, 2, 2, 3, 4, 2 },
            },
            [242] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 2, 2, 1, 4, 2, 2 },
                [3] = new[] { 2, 2, 1, 1, 3, 4, 1, 3 },
                [6] = new[] { 1, 2, 1, 3, 1, 4, 4, 1 },
            },
            [243] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 2, 2, 1, 5, 1, 3 },
                [3] = new[] { 3, 2, 1, 1, 3, 4, 2, 1 },
                [6] = new[] { 1, 1, 2, 2, 2, 4, 4, 1 },
            },
            [244] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 2, 2, 1, 6, 1, 2 },
                [3] = new[] { 2, 2, 1, 1, 3, 5, 1, 2 },
                [6] = new[] { 6, 2, 1, 3, 2, 1, 1, 1 },
            },
            [245] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 1, 3, 1, 1, 2, 5 },
                [3] = new[] { 2, 2, 1, 1, 3, 6, 1, 1 },
                [6] = new[] { 1, 2, 1, 3, 2, 1, 5, 2 },
            },
            [246] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 1, 3, 1, 1, 3, 3 },
                [3] = new[] { 1, 2, 1, 1, 4, 1, 1, 6 },
                [6] = new[] { 6, 1, 2, 2, 3, 1, 1, 1 },
            },
            [247] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 2, 1, 3, 1, 1, 4, 1 },
                [3] = new[] { 2, 2, 1, 1, 4, 1, 2, 4 },
                [6] = new[] { 1, 1, 2, 2, 3, 1, 5, 2 },
            },
            [248] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 2, 2, 2, 1, 2, 5 },
                [3] = new[] { 3, 2, 1, 1, 4, 1, 3, 2 },
                [6] = new[] { 1, 2, 1, 3, 2, 2, 5, 1 },
            },
            [249] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 1, 3, 1, 2, 2, 4 },
                [3] = new[] { 1, 2, 1, 1, 4, 2, 1, 5 },
                [6] = new[] { 1, 1, 2, 2, 3, 2, 5, 1 },
            },
            [250] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 1, 3, 1, 2, 3, 2 },
                [3] = new[] { 2, 2, 1, 1, 4, 2, 2, 3 },
                [6] = new[] { 5, 2, 1, 3, 3, 1, 1, 1 },
            },
            [251] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 2, 2, 2, 1, 6 },
                [3] = new[] { 3, 2, 1, 1, 4, 2, 3, 1 },
                [6] = new[] { 5, 1, 2, 2, 4, 1, 1, 1 },
            },
            [252] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 3, 1, 3, 1, 5 },
                [3] = new[] { 1, 2, 1, 1, 4, 3, 1, 4 },
                [6] = new[] { 4, 2, 1, 3, 4, 1, 1, 1 },
            },
            [253] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 2, 2, 2, 2, 3, 2 },
                [3] = new[] { 2, 2, 1, 1, 4, 3, 2, 2 },
                [6] = new[] { 4, 1, 2, 2, 5, 1, 1, 1 },
            },
            [254] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 1, 3, 1, 3, 3, 1 },
                [3] = new[] { 1, 2, 1, 1, 4, 4, 1, 3 },
                [6] = new[] { 3, 2, 1, 3, 5, 1, 1, 1 },
            },
            [255] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 2, 2, 3, 1, 5 },
                [3] = new[] { 2, 2, 1, 1, 4, 4, 2, 1 },
                [6] = new[] { 3, 1, 2, 2, 6, 1, 1, 1 },
            },
            [256] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 3, 1, 4, 1, 4 },
                [3] = new[] { 1, 2, 1, 1, 4, 5, 1, 2 },
                [6] = new[] { 2, 2, 1, 3, 6, 1, 1, 1 },
            },
            [257] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 1, 3, 1, 4, 2, 2 },
                [3] = new[] { 1, 2, 1, 1, 5, 1, 2, 4 },
                [6] = new[] { 1, 1, 2, 3, 1, 1, 3, 5 },
            },
            [258] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 2, 2, 4, 1, 4 },
                [3] = new[] { 2, 2, 1, 1, 5, 1, 3, 2 },
                [6] = new[] { 2, 1, 2, 3, 1, 1, 4, 3 },
            },
            [259] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 2, 2, 2, 4, 2, 2 },
                [3] = new[] { 1, 2, 1, 1, 5, 2, 2, 3 },
                [6] = new[] { 3, 1, 2, 3, 1, 1, 5, 1 },
            },
            [260] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 1, 3, 1, 5, 2, 1 },
                [3] = new[] { 2, 2, 1, 1, 5, 2, 3, 1 },
                [6] = new[] { 1, 1, 2, 3, 1, 2, 3, 4 },
            },
            [261] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 3, 1, 6, 1, 2 },
                [3] = new[] { 1, 2, 1, 1, 5, 3, 2, 2 },
                [6] = new[] { 2, 1, 2, 3, 1, 2, 4, 2 },
            },
            [262] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 3, 2, 1, 2, 5 },
                [3] = new[] { 1, 2, 1, 1, 5, 4, 2, 1 },
                [6] = new[] { 1, 1, 2, 3, 1, 3, 3, 3 },
            },
            [263] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 1, 3, 2, 1, 3, 3 },
                [3] = new[] { 1, 2, 1, 1, 6, 1, 3, 2 },
                [6] = new[] { 2, 1, 2, 3, 1, 3, 4, 1 },
            },
            [264] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 1, 3, 2, 1, 4, 1 },
                [3] = new[] { 1, 2, 1, 1, 6, 2, 3, 1 },
                [6] = new[] { 1, 1, 2, 3, 1, 4, 3, 2 },
            },
            [265] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 2, 3, 1, 2, 5 },
                [3] = new[] { 5, 1, 2, 1, 1, 1, 1, 5 },
                [6] = new[] { 1, 1, 2, 3, 1, 5, 3, 1 },
            },
            [266] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 3, 2, 2, 2, 4 },
                [3] = new[] { 6, 1, 2, 1, 1, 1, 2, 3 },
                [6] = new[] { 1, 2, 1, 4, 1, 1, 4, 3 },
            },
            [267] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 1, 3, 2, 2, 3, 2 },
                [3] = new[] { 1, 1, 2, 1, 1, 1, 6, 4 },
                [6] = new[] { 2, 2, 1, 4, 1, 1, 5, 1 },
            },
            [268] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 2, 3, 2, 2, 4 },
                [3] = new[] { 5, 1, 2, 1, 1, 2, 1, 4 },
                [6] = new[] { 1, 1, 2, 3, 2, 1, 4, 3 },
            },
            [269] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 2, 2, 3, 2, 3, 2 },
                [3] = new[] { 6, 1, 2, 1, 1, 2, 2, 2 },
                [6] = new[] { 1, 2, 1, 4, 1, 2, 4, 2 },
            },
            [270] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 1, 3, 2, 3, 3, 1 },
                [3] = new[] { 1, 1, 2, 1, 1, 2, 6, 3 },
                [6] = new[] { 1, 1, 2, 3, 2, 2, 4, 2 },
            },
            [271] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 2, 3, 3, 2, 3 },
                [3] = new[] { 5, 1, 2, 1, 1, 3, 1, 3 },
                [6] = new[] { 1, 2, 1, 4, 1, 3, 4, 1 },
            },
            [272] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 3, 2, 4, 2, 2 },
                [3] = new[] { 6, 1, 2, 1, 1, 3, 2, 1 },
                [6] = new[] { 1, 1, 2, 3, 2, 3, 4, 1 },
            },
            [273] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 3, 2, 5, 2, 1 },
                [3] = new[] { 1, 1, 2, 1, 1, 3, 6, 2 },
                [6] = new[] { 1, 2, 1, 4, 2, 1, 5, 1 },
            },
            [274] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 3, 3, 1, 3, 3 },
                [3] = new[] { 5, 1, 2, 1, 1, 4, 1, 2 },
                [6] = new[] { 1, 1, 2, 3, 3, 1, 5, 1 },
            },
            [275] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 1, 3, 3, 1, 4, 1 },
                [3] = new[] { 5, 1, 2, 1, 1, 5, 1, 1 },
                [6] = new[] { 1, 1, 2, 4, 1, 1, 3, 4 },
            },
            [276] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 2, 4, 1, 3, 3 },
                [3] = new[] { 4, 2, 1, 2, 1, 1, 1, 5 },
                [6] = new[] { 2, 1, 2, 4, 1, 1, 4, 2 },
            },
            [277] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 3, 3, 2, 3, 2 },
                [3] = new[] { 5, 2, 1, 2, 1, 1, 2, 3 },
                [6] = new[] { 1, 1, 2, 4, 1, 2, 3, 3 },
            },
            [278] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 2, 4, 2, 3, 2 },
                [3] = new[] { 6, 2, 1, 2, 1, 1, 3, 1 },
                [6] = new[] { 2, 1, 2, 4, 1, 2, 4, 1 },
            },
            [279] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 3, 3, 3, 3, 1 },
                [3] = new[] { 4, 1, 2, 1, 2, 1, 1, 5 },
                [6] = new[] { 1, 1, 2, 4, 1, 3, 3, 2 },
            },
            [280] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 2, 4, 3, 3, 1 },
                [3] = new[] { 4, 2, 1, 2, 1, 2, 1, 4 },
                [6] = new[] { 1, 1, 2, 4, 1, 4, 3, 1 },
            },
            [281] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 2, 5, 1, 4, 1 },
                [3] = new[] { 6, 1, 2, 1, 2, 1, 3, 1 },
                [6] = new[] { 1, 2, 1, 5, 1, 1, 4, 2 },
            },
            [282] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 2, 3, 1, 1, 1, 6 },
                [3] = new[] { 4, 1, 2, 1, 2, 2, 1, 4 },
                [6] = new[] { 1, 1, 2, 4, 2, 1, 4, 2 },
            },
            [283] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 2, 3, 1, 1, 2, 4 },
                [3] = new[] { 5, 1, 2, 1, 2, 2, 2, 2 },
                [6] = new[] { 1, 2, 1, 5, 1, 2, 4, 1 },
            },
            [284] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 2, 3, 1, 1, 3, 2 },
                [3] = new[] { 5, 2, 1, 2, 1, 3, 2, 1 },
                [6] = new[] { 1, 1, 2, 4, 2, 2, 4, 1 },
            },
            [285] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 2, 3, 1, 2, 1, 5 },
                [3] = new[] { 4, 1, 2, 1, 2, 3, 1, 3 },
                [6] = new[] { 1, 1, 2, 5, 1, 1, 3, 3 },
            },
            [286] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 2, 3, 1, 2, 2, 3 },
                [3] = new[] { 4, 2, 1, 2, 1, 4, 1, 2 },
                [6] = new[] { 2, 1, 2, 5, 1, 1, 4, 1 },
            },
            [287] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 2, 3, 1, 2, 3, 1 },
                [3] = new[] { 4, 1, 2, 1, 2, 4, 1, 2 },
                [6] = new[] { 1, 1, 2, 5, 1, 2, 3, 2 },
            },
            [288] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 2, 3, 1, 3, 1, 4 },
                [3] = new[] { 4, 2, 1, 2, 1, 5, 1, 1 },
                [6] = new[] { 1, 1, 2, 5, 1, 3, 3, 1 },
            },
            [289] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 2, 3, 1, 3, 2, 2 },
                [3] = new[] { 4, 1, 2, 1, 2, 5, 1, 1 },
                [6] = new[] { 1, 2, 1, 6, 1, 1, 4, 1 },
            },
            [290] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 2, 3, 1, 4, 1, 3 },
                [3] = new[] { 3, 2, 1, 2, 2, 1, 1, 5 },
                [6] = new[] { 1, 1, 2, 5, 2, 1, 4, 1 },
            },
            [291] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 2, 3, 1, 4, 2, 1 },
                [3] = new[] { 4, 2, 1, 2, 2, 1, 2, 3 },
                [6] = new[] { 1, 1, 2, 6, 1, 1, 3, 2 },
            },
            [292] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 2, 3, 1, 5, 1, 2 },
                [3] = new[] { 5, 2, 1, 2, 2, 1, 3, 1 },
                [6] = new[] { 1, 1, 2, 6, 1, 2, 3, 1 },
            },
            [293] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 2, 3, 1, 6, 1, 1 },
                [3] = new[] { 3, 1, 2, 1, 3, 1, 1, 5 },
                [6] = new[] { 1, 3, 1, 1, 1, 1, 4, 5 },
            },
            [294] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 4, 1, 1, 1, 6 },
                [3] = new[] { 3, 2, 1, 2, 2, 2, 1, 4 },
                [6] = new[] { 2, 3, 1, 1, 1, 1, 5, 3 },
            },
            [295] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 1, 4, 1, 1, 2, 4 },
                [3] = new[] { 4, 2, 1, 2, 2, 2, 2, 2 },
                [6] = new[] { 3, 3, 1, 1, 1, 1, 6, 1 },
            },
            [296] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 1, 4, 1, 1, 3, 2 },
                [3] = new[] { 3, 1, 2, 1, 3, 2, 1, 4 },
                [6] = new[] { 1, 3, 1, 1, 1, 2, 4, 4 },
            },
            [297] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 3, 2, 1, 1, 6 },
                [3] = new[] { 4, 1, 2, 1, 3, 2, 2, 2 },
                [6] = new[] { 2, 3, 1, 1, 1, 2, 5, 2 },
            },
            [298] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 4, 1, 2, 1, 5 },
                [3] = new[] { 4, 2, 1, 2, 2, 3, 2, 1 },
                [6] = new[] { 1, 3, 1, 1, 1, 3, 4, 3 },
            },
            [299] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 1, 4, 1, 2, 2, 3 },
                [3] = new[] { 3, 1, 2, 1, 3, 3, 1, 3 },
                [6] = new[] { 2, 3, 1, 1, 1, 3, 5, 1 },
            },
            [300] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 1, 4, 1, 2, 3, 1 },
                [3] = new[] { 3, 2, 1, 2, 2, 4, 1, 2 },
                [6] = new[] { 1, 3, 1, 1, 1, 4, 4, 2 },
            },
            [301] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 3, 2, 2, 1, 5 },
                [3] = new[] { 3, 1, 2, 1, 3, 4, 1, 2 },
                [6] = new[] { 1, 3, 1, 1, 1, 5, 4, 1 },
            },
            [302] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 2, 3, 2, 2, 2, 3 },
                [3] = new[] { 3, 2, 1, 2, 2, 5, 1, 1 },
                [6] = new[] { 6, 3, 1, 1, 2, 1, 1, 2 },
            },
            [303] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 2, 3, 2, 2, 3, 1 },
                [3] = new[] { 3, 1, 2, 1, 3, 5, 1, 1 },
                [6] = new[] { 1, 3, 1, 1, 2, 1, 5, 3 },
            },
            [304] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 3, 2, 3, 1, 4 },
                [3] = new[] { 2, 2, 1, 2, 3, 1, 1, 5 },
                [6] = new[] { 2, 3, 1, 1, 2, 1, 6, 1 },
            },
            [305] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 4, 1, 4, 1, 3 },
                [3] = new[] { 3, 2, 1, 2, 3, 1, 2, 3 },
                [6] = new[] { 6, 3, 1, 1, 2, 2, 1, 1 },
            },
            [306] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 1, 4, 1, 4, 2, 1 },
                [3] = new[] { 4, 2, 1, 2, 3, 1, 3, 1 },
                [6] = new[] { 1, 3, 1, 1, 2, 2, 5, 2 },
            },
            [307] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 3, 2, 4, 1, 3 },
                [3] = new[] { 2, 1, 2, 1, 4, 1, 1, 5 },
                [6] = new[] { 1, 3, 1, 1, 2, 3, 5, 1 },
            },
            [308] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 2, 3, 2, 4, 2, 1 },
                [3] = new[] { 2, 2, 1, 2, 3, 2, 1, 4 },
                [6] = new[] { 5, 3, 1, 1, 3, 1, 1, 2 },
            },
            [309] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 3, 2, 5, 1, 2 },
                [3] = new[] { 3, 2, 1, 2, 3, 2, 2, 2 },
                [6] = new[] { 1, 3, 1, 1, 3, 1, 6, 1 },
            },
            [310] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 4, 2, 1, 2, 4 },
                [3] = new[] { 2, 1, 2, 1, 4, 2, 1, 4 },
                [6] = new[] { 5, 3, 1, 1, 3, 2, 1, 1 },
            },
            [311] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 1, 4, 2, 1, 3, 2 },
                [3] = new[] { 3, 1, 2, 1, 4, 2, 2, 2 },
                [6] = new[] { 4, 3, 1, 1, 4, 1, 1, 2 },
            },
            [312] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 3, 3, 1, 2, 4 },
                [3] = new[] { 3, 2, 1, 2, 3, 3, 2, 1 },
                [6] = new[] { 4, 3, 1, 1, 4, 2, 1, 1 },
            },
            [313] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 4, 2, 2, 2, 3 },
                [3] = new[] { 2, 1, 2, 1, 4, 3, 1, 3 },
                [6] = new[] { 3, 3, 1, 1, 5, 1, 1, 2 },
            },
            [314] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 1, 4, 2, 2, 3, 1 },
                [3] = new[] { 2, 2, 1, 2, 3, 4, 1, 2 },
                [6] = new[] { 3, 3, 1, 1, 5, 2, 1, 1 },
            },
            [315] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 3, 3, 2, 2, 3 },
                [3] = new[] { 2, 1, 2, 1, 4, 4, 1, 2 },
                [6] = new[] { 2, 3, 1, 1, 6, 1, 1, 2 },
            },
            [316] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 2, 3, 3, 2, 3, 1 },
                [3] = new[] { 2, 2, 1, 2, 3, 5, 1, 1 },
                [6] = new[] { 2, 3, 1, 1, 6, 2, 1, 1 },
            },
            [317] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 3, 3, 3, 2, 2 },
                [3] = new[] { 2, 1, 2, 1, 4, 5, 1, 1 },
                [6] = new[] { 1, 2, 2, 1, 1, 1, 3, 6 },
            },
            [318] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 4, 2, 4, 2, 1 },
                [3] = new[] { 1, 2, 1, 2, 4, 1, 1, 5 },
                [6] = new[] { 2, 2, 2, 1, 1, 1, 4, 4 },
            },
            [319] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 3, 3, 4, 2, 1 },
                [3] = new[] { 2, 2, 1, 2, 4, 1, 2, 3 },
                [6] = new[] { 3, 2, 2, 1, 1, 1, 5, 2 },
            },
            [320] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 3, 4, 1, 3, 2 },
                [3] = new[] { 3, 2, 1, 2, 4, 1, 3, 1 },
                [6] = new[] { 1, 2, 2, 1, 1, 2, 3, 5 },
            },
            [321] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 3, 4, 2, 3, 1 },
                [3] = new[] { 1, 1, 2, 1, 5, 1, 1, 5 },
                [6] = new[] { 2, 2, 2, 1, 1, 2, 4, 3 },
            },
            [322] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 2, 4, 1, 1, 1, 5 },
                [3] = new[] { 1, 2, 1, 2, 4, 2, 1, 4 },
                [6] = new[] { 3, 2, 2, 1, 1, 2, 5, 1 },
            },
            [323] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 2, 4, 1, 1, 2, 3 },
                [3] = new[] { 2, 2, 1, 2, 4, 2, 2, 2 },
                [6] = new[] { 1, 2, 2, 1, 1, 3, 3, 4 },
            },
            [324] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 2, 4, 1, 1, 3, 1 },
                [3] = new[] { 1, 1, 2, 1, 5, 2, 1, 4 },
                [6] = new[] { 2, 2, 2, 1, 1, 3, 4, 2 },
            },
            [325] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 2, 4, 1, 2, 1, 4 },
                [3] = new[] { 2, 1, 2, 1, 5, 2, 2, 2 },
                [6] = new[] { 1, 2, 2, 1, 1, 4, 3, 3 },
            },
            [326] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 2, 4, 1, 2, 2, 2 },
                [3] = new[] { 2, 2, 1, 2, 4, 3, 2, 1 },
                [6] = new[] { 2, 2, 2, 1, 1, 4, 4, 1 },
            },
            [327] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 2, 4, 1, 3, 1, 3 },
                [3] = new[] { 1, 1, 2, 1, 5, 3, 1, 3 },
                [6] = new[] { 1, 2, 2, 1, 1, 5, 3, 2 },
            },
            [328] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 2, 4, 1, 3, 2, 1 },
                [3] = new[] { 1, 2, 1, 2, 4, 4, 1, 2 },
                [6] = new[] { 1, 2, 2, 1, 1, 6, 3, 1 },
            },
            [329] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 2, 4, 1, 4, 1, 2 },
                [3] = new[] { 1, 1, 2, 1, 5, 4, 1, 2 },
                [6] = new[] { 1, 3, 1, 2, 1, 1, 4, 4 },
            },
            [330] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 2, 4, 1, 5, 1, 1 },
                [3] = new[] { 1, 2, 1, 2, 4, 5, 1, 1 },
                [6] = new[] { 2, 3, 1, 2, 1, 1, 5, 2 },
            },
            [331] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 5, 1, 1, 1, 5 },
                [3] = new[] { 1, 2, 1, 2, 5, 1, 2, 3 },
                [6] = new[] { 1, 2, 2, 1, 2, 1, 4, 4 },
            },
            [332] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 1, 5, 1, 1, 2, 3 },
                [3] = new[] { 2, 2, 1, 2, 5, 1, 3, 1 },
                [6] = new[] { 1, 3, 1, 2, 1, 2, 4, 3 },
            },
            [333] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 1, 5, 1, 1, 3, 1 },
                [3] = new[] { 1, 1, 2, 1, 6, 1, 2, 3 },
                [6] = new[] { 2, 3, 1, 2, 1, 2, 5, 1 },
            },
            [334] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 4, 2, 1, 1, 5 },
                [3] = new[] { 1, 2, 1, 2, 5, 2, 2, 2 },
                [6] = new[] { 1, 2, 2, 1, 2, 2, 4, 3 },
            },
            [335] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 5, 1, 2, 1, 4 },
                [3] = new[] { 1, 1, 2, 1, 6, 2, 2, 2 },
                [6] = new[] { 2, 2, 2, 1, 2, 2, 5, 1 },
            },
            [336] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 1, 5, 1, 2, 2, 2 },
                [3] = new[] { 1, 2, 1, 2, 5, 3, 2, 1 },
                [6] = new[] { 1, 2, 2, 1, 2, 3, 4, 2 },
            },
            [337] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 4, 2, 2, 1, 4 },
                [3] = new[] { 1, 1, 2, 1, 6, 3, 2, 1 },
                [6] = new[] { 1, 3, 1, 2, 1, 4, 4, 1 },
            },
            [338] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 2, 4, 2, 2, 2, 2 },
                [3] = new[] { 1, 2, 1, 2, 6, 1, 3, 1 },
                [6] = new[] { 1, 2, 2, 1, 2, 4, 4, 1 },
            },
            [339] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 1, 5, 1, 3, 2, 1 },
                [3] = new[] { 5, 1, 2, 2, 1, 1, 1, 4 },
                [6] = new[] { 6, 3, 1, 2, 2, 1, 1, 1 },
            },
            [340] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 4, 2, 3, 1, 3 },
                [3] = new[] { 6, 1, 2, 2, 1, 1, 2, 2 },
                [6] = new[] { 1, 3, 1, 2, 2, 1, 5, 2 },
            },
            [341] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 5, 1, 4, 1, 2 },
                [3] = new[] { 1, 1, 2, 2, 1, 1, 6, 3 },
                [6] = new[] { 6, 2, 2, 1, 3, 1, 1, 1 },
            },
            [342] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 4, 2, 4, 1, 2 },
                [3] = new[] { 5, 1, 2, 2, 1, 2, 1, 3 },
                [6] = new[] { 1, 2, 2, 1, 3, 1, 5, 2 },
            },
            [343] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 5, 1, 5, 1, 1 },
                [3] = new[] { 6, 1, 2, 2, 1, 2, 2, 1 },
                [6] = new[] { 1, 3, 1, 2, 2, 2, 5, 1 },
            },
            [344] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 5, 2, 1, 2, 3 },
                [3] = new[] { 1, 1, 2, 2, 1, 2, 6, 2 },
                [6] = new[] { 1, 2, 2, 1, 3, 2, 5, 1 },
            },
            [345] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 4, 3, 1, 2, 3 },
                [3] = new[] { 5, 1, 2, 2, 1, 3, 1, 2 },
                [6] = new[] { 5, 3, 1, 2, 3, 1, 1, 1 },
            },
            [346] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 4, 3, 2, 2, 2 },
                [3] = new[] { 1, 1, 2, 2, 1, 3, 6, 1 },
                [6] = new[] { 5, 2, 2, 1, 4, 1, 1, 1 },
            },
            [347] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 4, 3, 3, 2, 1 },
                [3] = new[] { 5, 1, 2, 2, 1, 4, 1, 1 },
                [6] = new[] { 4, 3, 1, 2, 4, 1, 1, 1 },
            },
            [348] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 2, 5, 1, 1, 2, 2 },
                [3] = new[] { 4, 2, 1, 3, 1, 1, 1, 4 },
                [6] = new[] { 4, 2, 2, 1, 5, 1, 1, 1 },
            },
            [349] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 2, 5, 1, 2, 2, 1 },
                [3] = new[] { 5, 2, 1, 3, 1, 1, 2, 2 },
                [6] = new[] { 3, 3, 1, 2, 5, 1, 1, 1 },
            },
            [350] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 2, 5, 1, 4, 1, 1 },
                [3] = new[] { 4, 1, 2, 2, 2, 1, 1, 4 },
                [6] = new[] { 3, 2, 2, 1, 6, 1, 1, 1 },
            },
            [351] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 1, 6, 1, 1, 2, 2 },
                [3] = new[] { 4, 2, 1, 3, 1, 2, 1, 3 },
                [6] = new[] { 2, 3, 1, 2, 6, 1, 1, 1 },
            },
            [352] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 1, 6, 1, 2, 1, 3 },
                [3] = new[] { 5, 2, 1, 3, 1, 2, 2, 1 },
                [6] = new[] { 2, 1, 3, 1, 1, 1, 3, 5 },
            },
            [353] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 5, 2, 2, 1, 3 },
                [3] = new[] { 4, 1, 2, 2, 2, 2, 1, 3 },
                [6] = new[] { 3, 1, 3, 1, 1, 1, 4, 3 },
            },
            [354] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 5, 2, 3, 1, 2 },
                [3] = new[] { 5, 1, 2, 2, 2, 2, 2, 1 },
                [6] = new[] { 4, 1, 3, 1, 1, 1, 5, 1 },
            },
            [355] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 2, 5, 2, 4, 1, 1 },
                [3] = new[] { 4, 1, 2, 2, 2, 3, 1, 2 },
                [6] = new[] { 1, 1, 3, 1, 1, 2, 2, 6 },
            },
            [356] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 1, 1, 1, 1, 2, 6 },
                [3] = new[] { 4, 2, 1, 3, 1, 4, 1, 1 },
                [6] = new[] { 2, 1, 3, 1, 1, 2, 3, 4 },
            },
            [357] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 1, 1, 1, 1, 3, 4 },
                [3] = new[] { 4, 1, 2, 2, 2, 4, 1, 1 },
                [6] = new[] { 3, 1, 3, 1, 1, 2, 4, 2 },
            },
            [358] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 3, 1, 1, 1, 1, 4, 2 },
                [3] = new[] { 3, 2, 1, 3, 2, 1, 1, 4 },
                [6] = new[] { 1, 1, 3, 1, 1, 3, 2, 5 },
            },
            [359] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 1, 1, 1, 2, 2, 5 },
                [3] = new[] { 4, 2, 1, 3, 2, 1, 2, 2 },
                [6] = new[] { 2, 1, 3, 1, 1, 3, 3, 3 },
            },
            [360] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 1, 1, 1, 2, 3, 3 },
                [3] = new[] { 3, 1, 2, 2, 3, 1, 1, 4 },
                [6] = new[] { 3, 1, 3, 1, 1, 3, 4, 1 },
            },
            [361] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 1, 1, 3, 1, 6 },
                [3] = new[] { 3, 2, 1, 3, 2, 2, 1, 3 },
                [6] = new[] { 1, 1, 3, 1, 1, 4, 2, 4 },
            },
            [362] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 1, 1, 1, 3, 2, 4 },
                [3] = new[] { 4, 2, 1, 3, 2, 2, 2, 1 },
                [6] = new[] { 2, 1, 3, 1, 1, 4, 3, 2 },
            },
            [363] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 1, 1, 1, 3, 3, 2 },
                [3] = new[] { 3, 1, 2, 2, 3, 2, 1, 3 },
                [6] = new[] { 1, 1, 3, 1, 1, 5, 2, 3 },
            },
            [364] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 1, 1, 4, 1, 5 },
                [3] = new[] { 4, 1, 2, 2, 3, 2, 2, 1 },
                [6] = new[] { 2, 1, 3, 1, 1, 5, 3, 1 },
            },
            [365] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 1, 1, 1, 4, 2, 3 },
                [3] = new[] { 3, 1, 2, 2, 3, 3, 1, 2 },
                [6] = new[] { 1, 1, 3, 1, 1, 6, 2, 2 },
            },
            [366] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 1, 1, 5, 1, 4 },
                [3] = new[] { 3, 2, 1, 3, 2, 4, 1, 1 },
                [6] = new[] { 1, 2, 2, 2, 1, 1, 3, 5 },
            },
            [367] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 1, 1, 6, 1, 3 },
                [3] = new[] { 3, 1, 2, 2, 3, 4, 1, 1 },
                [6] = new[] { 2, 2, 2, 2, 1, 1, 4, 3 },
            },
            [368] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 1, 2, 1, 2, 6 },
                [3] = new[] { 2, 2, 1, 3, 3, 1, 1, 4 },
                [6] = new[] { 3, 2, 2, 2, 1, 1, 5, 1 },
            },
            [369] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 1, 1, 2, 1, 3, 4 },
                [3] = new[] { 3, 2, 1, 3, 3, 1, 2, 2 },
                [6] = new[] { 1, 1, 3, 1, 2, 1, 3, 5 },
            },
            [370] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 1, 1, 2, 1, 4, 2 },
                [3] = new[] { 2, 1, 2, 2, 4, 1, 1, 4 },
                [6] = new[] { 1, 2, 2, 2, 1, 2, 3, 4 },
            },
            [371] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 1, 2, 2, 2, 5 },
                [3] = new[] { 2, 2, 1, 3, 3, 2, 1, 3 },
                [6] = new[] { 2, 2, 2, 2, 1, 2, 4, 2 },
            },
            [372] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 1, 1, 2, 2, 3, 3 },
                [3] = new[] { 3, 2, 1, 3, 3, 2, 2, 1 },
                [6] = new[] { 1, 1, 3, 1, 2, 2, 3, 4 },
            },
            [373] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 1, 1, 2, 2, 4, 1 },
                [3] = new[] { 2, 1, 2, 2, 4, 2, 1, 3 },
                [6] = new[] { 2, 1, 3, 1, 2, 2, 4, 2 },
            },
            [374] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 1, 2, 3, 2, 4 },
                [3] = new[] { 3, 1, 2, 2, 4, 2, 2, 1 },
                [6] = new[] { 2, 2, 2, 2, 1, 3, 4, 1 },
            },
            [375] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 1, 1, 2, 3, 3, 2 },
                [3] = new[] { 2, 1, 2, 2, 4, 3, 1, 2 },
                [6] = new[] { 1, 1, 3, 1, 2, 3, 3, 3 },
            },
            [376] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 1, 2, 4, 2, 3 },
                [3] = new[] { 2, 2, 1, 3, 3, 4, 1, 1 },
                [6] = new[] { 1, 2, 2, 2, 1, 4, 3, 2 },
            },
            [377] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 1, 2, 5, 2, 2 },
                [3] = new[] { 2, 1, 2, 2, 4, 4, 1, 1 },
                [6] = new[] { 1, 1, 3, 1, 2, 4, 3, 2 },
            },
            [378] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 1, 3, 1, 3, 4 },
                [3] = new[] { 1, 2, 1, 3, 4, 1, 1, 4 },
                [6] = new[] { 1, 2, 2, 2, 1, 5, 3, 1 },
            },
            [379] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 1, 1, 3, 1, 4, 2 },
                [3] = new[] { 2, 2, 1, 3, 4, 1, 2, 2 },
                [6] = new[] { 1, 1, 3, 1, 2, 5, 3, 1 },
            },
            [380] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 1, 3, 2, 3, 3 },
                [3] = new[] { 1, 1, 2, 2, 5, 1, 1, 4 },
                [6] = new[] { 1, 3, 1, 3, 1, 1, 4, 3 },
            },
            [381] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 1, 1, 3, 2, 4, 1 },
                [3] = new[] { 1, 2, 1, 3, 4, 2, 1, 3 },
                [6] = new[] { 2, 3, 1, 3, 1, 1, 5, 1 },
            },
            [382] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 1, 3, 3, 3, 2 },
                [3] = new[] { 2, 2, 1, 3, 4, 2, 2, 1 },
                [6] = new[] { 1, 2, 2, 2, 2, 1, 4, 3 },
            },
            [383] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 1, 4, 1, 4, 2 },
                [3] = new[] { 1, 1, 2, 2, 5, 2, 1, 3 },
                [6] = new[] { 1, 3, 1, 3, 1, 2, 4, 2 },
            },
            [384] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 1, 4, 2, 4, 1 },
                [3] = new[] { 2, 1, 2, 2, 5, 2, 2, 1 },
                [6] = new[] { 1, 1, 3, 1, 3, 1, 4, 3 },
            },
            [385] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 2, 1, 1, 1, 2, 5 },
                [3] = new[] { 1, 1, 2, 2, 5, 3, 1, 2 },
                [6] = new[] { 1, 2, 2, 2, 2, 2, 4, 2 },
            },
            [386] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 2, 2, 1, 1, 1, 3, 3 },
                [3] = new[] { 1, 2, 1, 3, 4, 4, 1, 1 },
                [6] = new[] { 1, 3, 1, 3, 1, 3, 4, 1 },
            },
            [387] = new Dictionary<int, int[]>
            {
                [0] = new[] { 5, 2, 2, 1, 1, 1, 4, 1 },
                [3] = new[] { 1, 1, 2, 2, 5, 4, 1, 1 },
                [6] = new[] { 1, 1, 3, 1, 3, 2, 4, 2 },
            },
            [388] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 2, 1, 1, 2, 1, 6 },
                [3] = new[] { 1, 2, 1, 3, 5, 1, 2, 2 },
                [6] = new[] { 1, 2, 2, 2, 2, 3, 4, 1 },
            },
            [389] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 2, 1, 1, 2, 2, 4 },
                [3] = new[] { 1, 1, 2, 2, 6, 1, 2, 2 },
                [6] = new[] { 1, 1, 3, 1, 3, 3, 4, 1 },
            },
            [390] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 2, 2, 1, 1, 2, 3, 2 },
                [3] = new[] { 1, 2, 1, 3, 5, 2, 2, 1 },
                [6] = new[] { 1, 3, 1, 3, 2, 1, 5, 1 },
            },
            [391] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 2, 1, 1, 3, 1, 5 },
                [3] = new[] { 1, 1, 2, 2, 6, 2, 2, 1 },
                [6] = new[] { 1, 2, 2, 2, 3, 1, 5, 1 },
            },
            [392] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 2, 1, 1, 3, 2, 3 },
                [3] = new[] { 5, 1, 2, 3, 1, 1, 1, 3 },
                [6] = new[] { 1, 1, 3, 1, 4, 1, 5, 1 },
            },
            [393] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 2, 2, 1, 1, 3, 3, 1 },
                [3] = new[] { 6, 1, 2, 3, 1, 1, 2, 1 },
                [6] = new[] { 1, 1, 3, 2, 1, 1, 2, 6 },
            },
            [394] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 2, 1, 1, 4, 1, 4 },
                [3] = new[] { 1, 1, 2, 3, 1, 1, 6, 2 },
                [6] = new[] { 2, 1, 3, 2, 1, 1, 3, 4 },
            },
            [395] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 2, 1, 1, 4, 2, 2 },
                [3] = new[] { 5, 1, 2, 3, 1, 2, 1, 2 },
                [6] = new[] { 3, 1, 3, 2, 1, 1, 4, 2 },
            },
            [396] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 2, 1, 1, 5, 1, 3 },
                [3] = new[] { 1, 1, 2, 3, 1, 2, 6, 1 },
                [6] = new[] { 1, 1, 3, 2, 1, 2, 2, 5 },
            },
            [397] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 2, 1, 1, 5, 2, 1 },
                [3] = new[] { 5, 1, 2, 3, 1, 3, 1, 1 },
                [6] = new[] { 2, 1, 3, 2, 1, 2, 3, 3 },
            },
            [398] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 1, 2, 1, 1, 2, 5 },
                [3] = new[] { 4, 2, 1, 4, 1, 1, 1, 3 },
                [6] = new[] { 3, 1, 3, 2, 1, 2, 4, 1 },
            },
            [399] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 1, 2, 1, 1, 3, 3 },
                [3] = new[] { 5, 2, 1, 4, 1, 1, 2, 1 },
                [6] = new[] { 1, 1, 3, 2, 1, 3, 2, 4 },
            },
            [400] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 3, 1, 2, 1, 1, 4, 1 },
                [3] = new[] { 4, 1, 2, 3, 2, 1, 1, 3 },
                [6] = new[] { 2, 1, 3, 2, 1, 3, 3, 2 },
            },
            [401] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 2, 1, 2, 1, 2, 5 },
                [3] = new[] { 5, 1, 2, 3, 2, 1, 2, 1 },
                [6] = new[] { 1, 1, 3, 2, 1, 4, 2, 3 },
            },
            [402] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 1, 2, 1, 2, 2, 4 },
                [3] = new[] { 4, 1, 2, 3, 2, 2, 1, 2 },
                [6] = new[] { 2, 1, 3, 2, 1, 4, 3, 1 },
            },
            [403] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 1, 2, 1, 2, 3, 2 },
                [3] = new[] { 4, 2, 1, 4, 1, 3, 1, 1 },
                [6] = new[] { 1, 1, 3, 2, 1, 5, 2, 2 },
            },
            [404] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 2, 1, 2, 2, 1, 6 },
                [3] = new[] { 4, 1, 2, 3, 2, 3, 1, 1 },
                [6] = new[] { 1, 1, 3, 2, 1, 6, 2, 1 },
            },
            [405] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 2, 1, 3, 1, 5 },
                [3] = new[] { 3, 2, 1, 4, 2, 1, 1, 3 },
                [6] = new[] { 1, 2, 2, 3, 1, 1, 3, 4 },
            },
            [406] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 2, 1, 2, 2, 3, 2 },
                [3] = new[] { 4, 2, 1, 4, 2, 1, 2, 1 },
                [6] = new[] { 2, 2, 2, 3, 1, 1, 4, 2 },
            },
            [407] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 1, 2, 1, 3, 3, 1 },
                [3] = new[] { 3, 1, 2, 3, 3, 1, 1, 3 },
                [6] = new[] { 1, 1, 3, 2, 2, 1, 3, 4 },
            },
            [408] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 2, 1, 2, 3, 1, 5 },
                [3] = new[] { 3, 2, 1, 4, 2, 2, 1, 2 },
                [6] = new[] { 1, 2, 2, 3, 1, 2, 3, 3 },
            },
            [409] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 2, 1, 2, 3, 2, 3 },
                [3] = new[] { 3, 1, 2, 3, 3, 2, 1, 2 },
                [6] = new[] { 2, 2, 2, 3, 1, 2, 4, 1 },
            },
            [410] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 1, 2, 1, 4, 2, 2 },
                [3] = new[] { 3, 2, 1, 4, 2, 3, 1, 1 },
                [6] = new[] { 1, 1, 3, 2, 2, 2, 3, 3 },
            },
            [411] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 2, 1, 2, 4, 1, 4 },
                [3] = new[] { 3, 1, 2, 3, 3, 3, 1, 1 },
                [6] = new[] { 2, 1, 3, 2, 2, 2, 4, 1 },
            },
            [412] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 2, 1, 5, 1, 3 },
                [3] = new[] { 2, 2, 1, 4, 3, 1, 1, 3 },
                [6] = new[] { 1, 1, 3, 2, 2, 3, 3, 2 },
            },
            [413] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 2, 1, 2, 5, 1, 3 },
                [3] = new[] { 3, 2, 1, 4, 3, 1, 2, 1 },
                [6] = new[] { 1, 2, 2, 3, 1, 4, 3, 1 },
            },
            [414] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 2, 2, 1, 2, 5 },
                [3] = new[] { 2, 1, 2, 3, 4, 1, 1, 3 },
                [6] = new[] { 1, 1, 3, 2, 2, 4, 3, 1 },
            },
            [415] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 1, 2, 2, 1, 3, 3 },
                [3] = new[] { 3, 1, 2, 3, 4, 1, 2, 1 },
                [6] = new[] { 1, 3, 1, 4, 1, 1, 4, 2 },
            },
            [416] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 1, 2, 2, 1, 4, 1 },
                [3] = new[] { 2, 1, 2, 3, 4, 2, 1, 2 },
                [6] = new[] { 1, 2, 2, 3, 2, 1, 4, 2 },
            },
            [417] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 2, 1, 3, 1, 2, 5 },
                [3] = new[] { 2, 2, 1, 4, 3, 3, 1, 1 },
                [6] = new[] { 1, 3, 1, 4, 1, 2, 4, 1 },
            },
            [418] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 2, 2, 2, 2, 4 },
                [3] = new[] { 2, 1, 2, 3, 4, 3, 1, 1 },
                [6] = new[] { 1, 1, 3, 2, 3, 1, 4, 2 },
            },
            [419] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 2, 1, 3, 1, 4, 1 },
                [3] = new[] { 1, 2, 1, 4, 4, 1, 1, 3 },
                [6] = new[] { 1, 2, 2, 3, 2, 2, 4, 1 },
            },
            [420] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 2, 1, 3, 2, 2, 4 },
                [3] = new[] { 2, 2, 1, 4, 4, 1, 2, 1 },
                [6] = new[] { 1, 1, 3, 2, 3, 2, 4, 1 },
            },
            [421] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 2, 1, 3, 2, 3, 2 },
                [3] = new[] { 1, 1, 2, 3, 5, 1, 1, 3 },
                [6] = new[] { 1, 1, 3, 3, 1, 1, 2, 5 },
            },
            [422] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 1, 2, 2, 3, 3, 1 },
                [3] = new[] { 1, 2, 1, 4, 4, 2, 1, 2 },
                [6] = new[] { 2, 1, 3, 3, 1, 1, 3, 3 },
            },
            [423] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 2, 1, 3, 3, 2, 3 },
                [3] = new[] { 1, 1, 2, 3, 5, 2, 1, 2 },
                [6] = new[] { 3, 1, 3, 3, 1, 1, 4, 1 },
            },
            [424] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 2, 2, 4, 2, 2 },
                [3] = new[] { 1, 2, 1, 4, 4, 3, 1, 1 },
                [6] = new[] { 1, 1, 3, 3, 1, 2, 2, 4 },
            },
            [425] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 2, 1, 3, 4, 2, 2 },
                [3] = new[] { 1, 1, 2, 3, 5, 3, 1, 1 },
                [6] = new[] { 2, 1, 3, 3, 1, 2, 3, 2 },
            },
            [426] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 2, 3, 1, 3, 3 },
                [3] = new[] { 1, 2, 1, 4, 5, 1, 2, 1 },
                [6] = new[] { 1, 1, 3, 3, 1, 3, 2, 3 },
            },
            [427] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 1, 2, 3, 1, 4, 1 },
                [3] = new[] { 1, 1, 2, 3, 6, 1, 2, 1 },
                [6] = new[] { 2, 1, 3, 3, 1, 3, 3, 1 },
            },
            [428] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 2, 1, 4, 1, 3, 3 },
                [3] = new[] { 5, 1, 2, 4, 1, 1, 1, 2 },
                [6] = new[] { 1, 1, 3, 3, 1, 4, 2, 2 },
            },
            [429] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 2, 3, 2, 3, 2 },
                [3] = new[] { 1, 1, 2, 4, 1, 1, 6, 1 },
                [6] = new[] { 1, 1, 3, 3, 1, 5, 2, 1 },
            },
            [430] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 2, 1, 4, 2, 3, 2 },
                [3] = new[] { 5, 1, 2, 4, 1, 2, 1, 1 },
                [6] = new[] { 1, 2, 2, 4, 1, 1, 3, 3 },
            },
            [431] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 2, 3, 3, 3, 1 },
                [3] = new[] { 4, 2, 1, 5, 1, 1, 1, 2 },
                [6] = new[] { 2, 2, 2, 4, 1, 1, 4, 1 },
            },
            [432] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 2, 4, 1, 4, 1 },
                [3] = new[] { 4, 1, 2, 4, 2, 1, 1, 2 },
                [6] = new[] { 1, 1, 3, 3, 2, 1, 3, 3 },
            },
            [433] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 2, 1, 5, 1, 4, 1 },
                [3] = new[] { 4, 2, 1, 5, 1, 2, 1, 1 },
                [6] = new[] { 1, 2, 2, 4, 1, 2, 3, 2 },
            },
            [434] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 3, 1, 1, 1, 1, 6 },
                [3] = new[] { 4, 1, 2, 4, 2, 2, 1, 1 },
                [6] = new[] { 1, 1, 3, 3, 2, 2, 3, 2 },
            },
            [435] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 3, 1, 1, 1, 2, 4 },
                [3] = new[] { 3, 2, 1, 5, 2, 1, 1, 2 },
                [6] = new[] { 1, 2, 2, 4, 1, 3, 3, 1 },
            },
            [436] = new Dictionary<int, int[]>
            {
                [0] = new[] { 5, 1, 3, 1, 1, 1, 3, 2 },
                [3] = new[] { 3, 1, 2, 4, 3, 1, 1, 2 },
                [6] = new[] { 1, 1, 3, 3, 2, 3, 3, 1 },
            },
            [437] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 3, 1, 1, 2, 1, 5 },
                [3] = new[] { 3, 2, 1, 5, 2, 2, 1, 1 },
                [6] = new[] { 1, 3, 1, 5, 1, 1, 4, 1 },
            },
            [438] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 3, 1, 1, 2, 2, 3 },
                [3] = new[] { 3, 1, 2, 4, 3, 2, 1, 1 },
                [6] = new[] { 1, 2, 2, 4, 2, 1, 4, 1 },
            },
            [439] = new Dictionary<int, int[]>
            {
                [0] = new[] { 5, 1, 3, 1, 1, 2, 3, 1 },
                [3] = new[] { 2, 2, 1, 5, 3, 1, 1, 2 },
                [6] = new[] { 1, 1, 3, 3, 3, 1, 4, 1 },
            },
            [440] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 3, 1, 1, 3, 1, 4 },
                [3] = new[] { 2, 1, 2, 4, 4, 1, 1, 2 },
                [6] = new[] { 1, 1, 3, 4, 1, 1, 2, 4 },
            },
            [441] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 3, 1, 1, 3, 2, 2 },
                [3] = new[] { 2, 2, 1, 5, 3, 2, 1, 1 },
                [6] = new[] { 2, 1, 3, 4, 1, 1, 3, 2 },
            },
            [442] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 3, 1, 1, 4, 1, 3 },
                [3] = new[] { 2, 1, 2, 4, 4, 2, 1, 1 },
                [6] = new[] { 1, 1, 3, 4, 1, 2, 2, 3 },
            },
            [443] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 3, 1, 1, 4, 2, 1 },
                [3] = new[] { 1, 2, 1, 5, 4, 1, 1, 2 },
                [6] = new[] { 2, 1, 3, 4, 1, 2, 3, 1 },
            },
            [444] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 3, 1, 1, 5, 1, 2 },
                [3] = new[] { 1, 1, 2, 4, 5, 1, 1, 2 },
                [6] = new[] { 1, 1, 3, 4, 1, 3, 2, 2 },
            },
            [445] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 2, 2, 1, 1, 1, 6 },
                [3] = new[] { 1, 2, 1, 5, 4, 2, 1, 1 },
                [6] = new[] { 1, 1, 3, 4, 1, 4, 2, 1 },
            },
            [446] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 2, 2, 1, 1, 2, 4 },
                [3] = new[] { 1, 1, 2, 4, 5, 2, 1, 1 },
                [6] = new[] { 1, 2, 2, 5, 1, 1, 3, 2 },
            },
            [447] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 2, 2, 2, 1, 1, 3, 2 },
                [3] = new[] { 5, 1, 2, 5, 1, 1, 1, 1 },
                [6] = new[] { 1, 1, 3, 4, 2, 1, 3, 2 },
            },
            [448] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 3, 1, 2, 1, 1, 6 },
                [3] = new[] { 4, 2, 1, 6, 1, 1, 1, 1 },
                [6] = new[] { 1, 2, 2, 5, 1, 2, 3, 1 },
            },
            [449] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 2, 2, 1, 2, 1, 5 },
                [3] = new[] { 4, 1, 2, 5, 2, 1, 1, 1 },
                [6] = new[] { 1, 1, 3, 4, 2, 2, 3, 1 },
            },
            [450] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 3, 1, 2, 1, 3, 2 },
                [3] = new[] { 3, 2, 1, 6, 2, 1, 1, 1 },
                [6] = new[] { 1, 1, 3, 5, 1, 1, 2, 3 },
            },
            [451] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 2, 2, 2, 1, 2, 3, 1 },
                [3] = new[] { 3, 1, 2, 5, 3, 1, 1, 1 },
                [6] = new[] { 2, 1, 3, 5, 1, 1, 3, 1 },
            },
            [452] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 3, 1, 2, 2, 1, 5 },
                [3] = new[] { 2, 2, 1, 6, 3, 1, 1, 1 },
                [6] = new[] { 1, 1, 3, 5, 1, 2, 2, 2 },
            },
            [453] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 3, 1, 2, 2, 2, 3 },
                [3] = new[] { 2, 1, 2, 5, 4, 1, 1, 1 },
                [6] = new[] { 1, 1, 3, 5, 1, 3, 2, 1 },
            },
            [454] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 3, 1, 2, 2, 3, 1 },
                [3] = new[] { 4, 3, 1, 1, 1, 1, 1, 5 },
                [6] = new[] { 1, 2, 2, 6, 1, 1, 3, 1 },
            },
            [455] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 3, 1, 2, 3, 1, 4 },
                [3] = new[] { 5, 3, 1, 1, 1, 1, 2, 3 },
                [6] = new[] { 1, 1, 3, 5, 2, 1, 3, 1 },
            },
            [456] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 2, 2, 1, 4, 1, 3 },
                [3] = new[] { 6, 3, 1, 1, 1, 1, 3, 1 },
                [6] = new[] { 1, 1, 3, 6, 1, 1, 2, 2 },
            },
            [457] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 2, 2, 1, 4, 2, 1 },
                [3] = new[] { 4, 3, 1, 1, 1, 2, 1, 4 },
                [6] = new[] { 1, 1, 3, 6, 1, 2, 2, 1 },
            },
            [458] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 3, 1, 2, 4, 1, 3 },
                [3] = new[] { 5, 3, 1, 1, 1, 2, 2, 2 },
                [6] = new[] { 1, 4, 1, 1, 1, 1, 4, 4 },
            },
            [459] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 3, 1, 2, 4, 2, 1 },
                [3] = new[] { 4, 3, 1, 1, 1, 3, 1, 3 },
                [6] = new[] { 2, 4, 1, 1, 1, 1, 5, 2 },
            },
            [460] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 2, 2, 1, 6, 1, 1 },
                [3] = new[] { 5, 3, 1, 1, 1, 3, 2, 1 },
                [6] = new[] { 1, 4, 1, 1, 1, 2, 4, 3 },
            },
            [461] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 3, 1, 1, 1, 6 },
                [3] = new[] { 4, 3, 1, 1, 1, 4, 1, 2 },
                [6] = new[] { 2, 4, 1, 1, 1, 2, 5, 1 },
            },
            [462] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 1, 3, 1, 1, 2, 4 },
                [3] = new[] { 4, 3, 1, 1, 1, 5, 1, 1 },
                [6] = new[] { 1, 4, 1, 1, 1, 3, 4, 2 },
            },
            [463] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 1, 3, 1, 1, 3, 2 },
                [3] = new[] { 3, 3, 1, 1, 2, 1, 1, 5 },
                [6] = new[] { 1, 4, 1, 1, 1, 4, 4, 1 },
            },
            [464] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 2, 2, 2, 1, 1, 6 },
                [3] = new[] { 4, 3, 1, 1, 2, 1, 2, 3 },
                [6] = new[] { 1, 4, 1, 1, 2, 1, 5, 2 },
            },
            [465] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 3, 1, 2, 1, 5 },
                [3] = new[] { 5, 3, 1, 1, 2, 1, 3, 1 },
                [6] = new[] { 1, 4, 1, 1, 2, 2, 5, 1 },
            },
            [466] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 1, 3, 1, 2, 2, 3 },
                [3] = new[] { 3, 3, 1, 1, 2, 2, 1, 4 },
                [6] = new[] { 5, 4, 1, 1, 3, 1, 1, 1 },
            },
            [467] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 1, 3, 1, 2, 3, 1 },
                [3] = new[] { 4, 3, 1, 1, 2, 2, 2, 2 },
                [6] = new[] { 4, 4, 1, 1, 4, 1, 1, 1 },
            },
            [468] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 3, 1, 3, 1, 1, 6 },
                [3] = new[] { 3, 3, 1, 1, 2, 3, 1, 3 },
                [6] = new[] { 3, 4, 1, 1, 5, 1, 1, 1 },
            },
            [469] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 2, 2, 2, 2, 1, 5 },
                [3] = new[] { 4, 3, 1, 1, 2, 3, 2, 1 },
                [6] = new[] { 2, 4, 1, 1, 6, 1, 1, 1 },
            },
            [470] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 2, 2, 2, 2, 2, 3 },
                [3] = new[] { 3, 3, 1, 1, 2, 4, 1, 2 },
                [6] = new[] { 1, 3, 2, 1, 1, 1, 3, 5 },
            },
            [471] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 2, 2, 2, 2, 3, 1 },
                [3] = new[] { 3, 3, 1, 1, 2, 5, 1, 1 },
                [6] = new[] { 2, 3, 2, 1, 1, 1, 4, 3 },
            },
            [472] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 3, 1, 3, 2, 1, 5 },
                [3] = new[] { 2, 3, 1, 1, 3, 1, 1, 5 },
                [6] = new[] { 3, 3, 2, 1, 1, 1, 5, 1 },
            },
            [473] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 3, 1, 3, 2, 2, 3 },
                [3] = new[] { 3, 3, 1, 1, 3, 1, 2, 3 },
                [6] = new[] { 1, 3, 2, 1, 1, 2, 3, 4 },
            },
            [474] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 3, 1, 3, 2, 3, 1 },
                [3] = new[] { 4, 3, 1, 1, 3, 1, 3, 1 },
                [6] = new[] { 2, 3, 2, 1, 1, 2, 4, 2 },
            },
            [475] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 1, 3, 1, 4, 2, 1 },
                [3] = new[] { 2, 3, 1, 1, 3, 2, 1, 4 },
                [6] = new[] { 1, 3, 2, 1, 1, 3, 3, 3 },
            },
            [476] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 3, 1, 3, 3, 1, 4 },
                [3] = new[] { 3, 3, 1, 1, 3, 2, 2, 2 },
                [6] = new[] { 2, 3, 2, 1, 1, 3, 4, 1 },
            },
            [477] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 2, 2, 2, 4, 1, 3 },
                [3] = new[] { 2, 3, 1, 1, 3, 3, 1, 3 },
                [6] = new[] { 1, 3, 2, 1, 1, 4, 3, 2 },
            },
            [478] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 2, 2, 2, 4, 2, 1 },
                [3] = new[] { 3, 3, 1, 1, 3, 3, 2, 1 },
                [6] = new[] { 1, 3, 2, 1, 1, 5, 3, 1 },
            },
            [479] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 3, 1, 3, 4, 1, 3 },
                [3] = new[] { 2, 3, 1, 1, 3, 4, 1, 2 },
                [6] = new[] { 1, 4, 1, 2, 1, 1, 4, 3 },
            },
            [480] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 3, 1, 6, 1, 1 },
                [3] = new[] { 2, 3, 1, 1, 3, 5, 1, 1 },
                [6] = new[] { 2, 4, 1, 2, 1, 1, 5, 1 },
            },
            [481] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 3, 2, 1, 2, 4 },
                [3] = new[] { 1, 3, 1, 1, 4, 1, 1, 5 },
                [6] = new[] { 1, 3, 2, 1, 2, 1, 4, 3 },
            },
            [482] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 1, 3, 2, 1, 3, 2 },
                [3] = new[] { 2, 3, 1, 1, 4, 1, 2, 3 },
                [6] = new[] { 1, 4, 1, 2, 1, 2, 4, 2 },
            },
            [483] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 2, 2, 3, 1, 2, 4 },
                [3] = new[] { 3, 3, 1, 1, 4, 1, 3, 1 },
                [6] = new[] { 1, 3, 2, 1, 2, 2, 4, 2 },
            },
            [484] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 3, 2, 2, 2, 3 },
                [3] = new[] { 1, 3, 1, 1, 4, 2, 1, 4 },
                [6] = new[] { 1, 4, 1, 2, 1, 3, 4, 1 },
            },
            [485] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 1, 3, 2, 2, 3, 1 },
                [3] = new[] { 2, 3, 1, 1, 4, 2, 2, 2 },
                [6] = new[] { 1, 3, 2, 1, 2, 3, 4, 1 },
            },
            [486] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 3, 1, 4, 1, 2, 4 },
                [3] = new[] { 1, 3, 1, 1, 4, 3, 1, 3 },
                [6] = new[] { 1, 4, 1, 2, 2, 1, 5, 1 },
            },
            [487] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 2, 2, 3, 2, 2, 3 },
                [3] = new[] { 2, 3, 1, 1, 4, 3, 2, 1 },
                [6] = new[] { 1, 3, 2, 1, 3, 1, 5, 1 },
            },
            [488] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 2, 2, 3, 2, 3, 1 },
                [3] = new[] { 1, 3, 1, 1, 4, 4, 1, 2 },
                [6] = new[] { 1, 2, 3, 1, 1, 1, 2, 6 },
            },
            [489] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 3, 1, 4, 2, 2, 3 },
                [3] = new[] { 1, 3, 1, 1, 4, 5, 1, 1 },
                [6] = new[] { 2, 2, 3, 1, 1, 1, 3, 4 },
            },
            [490] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 3, 1, 4, 2, 3, 1 },
                [3] = new[] { 1, 3, 1, 1, 5, 1, 2, 3 },
                [6] = new[] { 3, 2, 3, 1, 1, 1, 4, 2 },
            },
            [491] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 3, 2, 4, 2, 1 },
                [3] = new[] { 2, 3, 1, 1, 5, 1, 3, 1 },
                [6] = new[] { 1, 2, 3, 1, 1, 2, 2, 5 },
            },
            [492] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 2, 2, 3, 4, 2, 1 },
                [3] = new[] { 1, 3, 1, 1, 5, 2, 2, 2 },
                [6] = new[] { 2, 2, 3, 1, 1, 2, 3, 3 },
            },
            [493] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 3, 3, 1, 3, 2 },
                [3] = new[] { 1, 3, 1, 1, 5, 3, 2, 1 },
                [6] = new[] { 3, 2, 3, 1, 1, 2, 4, 1 },
            },
            [494] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 2, 2, 4, 1, 3, 2 },
                [3] = new[] { 1, 3, 1, 1, 6, 1, 3, 1 },
                [6] = new[] { 1, 2, 3, 1, 1, 3, 2, 4 },
            },
            [495] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 3, 3, 2, 3, 1 },
                [3] = new[] { 5, 2, 2, 1, 1, 1, 1, 4 },
                [6] = new[] { 2, 2, 3, 1, 1, 3, 3, 2 },
            },
            [496] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 3, 1, 5, 1, 3, 2 },
                [3] = new[] { 6, 2, 2, 1, 1, 1, 2, 2 },
                [6] = new[] { 1, 2, 3, 1, 1, 4, 2, 3 },
            },
            [497] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 2, 2, 4, 2, 3, 1 },
                [3] = new[] { 1, 2, 2, 1, 1, 1, 6, 3 },
                [6] = new[] { 2, 2, 3, 1, 1, 4, 3, 1 },
            },
            [498] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 3, 2, 1, 1, 1, 5 },
                [3] = new[] { 5, 2, 2, 1, 1, 2, 1, 3 },
                [6] = new[] { 1, 2, 3, 1, 1, 5, 2, 2 },
            },
            [499] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 3, 2, 1, 1, 2, 3 },
                [3] = new[] { 6, 2, 2, 1, 1, 2, 2, 1 },
                [6] = new[] { 1, 2, 3, 1, 1, 6, 2, 1 },
            },
            [500] = new Dictionary<int, int[]>
            {
                [0] = new[] { 5, 1, 3, 2, 1, 1, 3, 1 },
                [3] = new[] { 1, 2, 2, 1, 1, 2, 6, 2 },
                [6] = new[] { 1, 3, 2, 2, 1, 1, 3, 4 },
            },
            [501] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 3, 2, 1, 2, 1, 4 },
                [3] = new[] { 5, 2, 2, 1, 1, 3, 1, 2 },
                [6] = new[] { 2, 3, 2, 2, 1, 1, 4, 2 },
            },
            [502] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 3, 2, 1, 2, 2, 2 },
                [3] = new[] { 1, 2, 2, 1, 1, 3, 6, 1 },
                [6] = new[] { 1, 2, 3, 1, 2, 1, 3, 4 },
            },
            [503] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 3, 2, 1, 3, 1, 3 },
                [3] = new[] { 5, 2, 2, 1, 1, 4, 1, 1 },
                [6] = new[] { 1, 3, 2, 2, 1, 2, 3, 3 },
            },
            [504] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 3, 2, 1, 3, 2, 1 },
                [3] = new[] { 4, 3, 1, 2, 1, 1, 1, 4 },
                [6] = new[] { 2, 3, 2, 2, 1, 2, 4, 1 },
            },
            [505] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 3, 2, 1, 4, 1, 2 },
                [3] = new[] { 5, 3, 1, 2, 1, 1, 2, 2 },
                [6] = new[] { 1, 2, 3, 1, 2, 2, 3, 3 },
            },
            [506] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 3, 2, 1, 5, 1, 1 },
                [3] = new[] { 4, 2, 2, 1, 2, 1, 1, 4 },
                [6] = new[] { 1, 3, 2, 2, 1, 3, 3, 2 },
            },
            [507] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 2, 3, 1, 1, 1, 5 },
                [3] = new[] { 4, 3, 1, 2, 1, 2, 1, 3 },
                [6] = new[] { 1, 2, 3, 1, 2, 3, 3, 2 },
            },
            [508] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 2, 3, 1, 1, 2, 3 },
                [3] = new[] { 5, 3, 1, 2, 1, 2, 2, 1 },
                [6] = new[] { 1, 3, 2, 2, 1, 4, 3, 1 },
            },
            [509] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 2, 2, 3, 1, 1, 3, 1 },
                [3] = new[] { 4, 2, 2, 1, 2, 2, 1, 3 },
                [6] = new[] { 1, 2, 3, 1, 2, 4, 3, 1 },
            },
            [510] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 3, 2, 2, 1, 1, 5 },
                [3] = new[] { 5, 2, 2, 1, 2, 2, 2, 1 },
                [6] = new[] { 1, 4, 1, 3, 1, 1, 4, 2 },
            },
            [511] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 2, 3, 1, 2, 1, 4 },
                [3] = new[] { 4, 2, 2, 1, 2, 3, 1, 2 },
                [6] = new[] { 1, 3, 2, 2, 2, 1, 4, 2 },
            },
            [512] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 3, 2, 2, 1, 3, 1 },
                [3] = new[] { 4, 3, 1, 2, 1, 4, 1, 1 },
                [6] = new[] { 1, 4, 1, 3, 1, 2, 4, 1 },
            },
            [513] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 3, 2, 2, 2, 1, 4 },
                [3] = new[] { 4, 2, 2, 1, 2, 4, 1, 1 },
                [6] = new[] { 1, 2, 3, 1, 3, 1, 4, 2 },
            },
            [514] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 3, 2, 2, 2, 2, 2 },
                [3] = new[] { 3, 3, 1, 2, 2, 1, 1, 4 },
                [6] = new[] { 1, 3, 2, 2, 2, 2, 4, 1 },
            },
            [515] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 2, 3, 1, 3, 2, 1 },
                [3] = new[] { 4, 3, 1, 2, 2, 1, 2, 2 },
                [6] = new[] { 1, 2, 3, 1, 3, 2, 4, 1 },
            },
            [516] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 3, 2, 2, 3, 1, 3 },
                [3] = new[] { 3, 2, 2, 1, 3, 1, 1, 4 },
                [6] = new[] { 2, 1, 4, 1, 1, 1, 2, 5 },
            },
            [517] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 2, 3, 1, 4, 1, 2 },
                [3] = new[] { 3, 3, 1, 2, 2, 2, 1, 3 },
                [6] = new[] { 3, 1, 4, 1, 1, 1, 3, 3 },
            },
            [518] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 3, 2, 2, 4, 1, 2 },
                [3] = new[] { 4, 3, 1, 2, 2, 2, 2, 1 },
                [6] = new[] { 4, 1, 4, 1, 1, 1, 4, 1 },
            },
            [519] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 2, 3, 1, 5, 1, 1 },
                [3] = new[] { 3, 2, 2, 1, 3, 2, 1, 3 },
                [6] = new[] { 1, 1, 4, 1, 1, 2, 1, 6 },
            },
            [520] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 3, 2, 2, 5, 1, 1 },
                [3] = new[] { 4, 2, 2, 1, 3, 2, 2, 1 },
                [6] = new[] { 2, 1, 4, 1, 1, 2, 2, 4 },
            },
            [521] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 4, 1, 1, 1, 5 },
                [3] = new[] { 3, 2, 2, 1, 3, 3, 1, 2 },
                [6] = new[] { 3, 1, 4, 1, 1, 2, 3, 2 },
            },
            [522] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 1, 4, 1, 1, 2, 3 },
                [3] = new[] { 3, 3, 1, 2, 2, 4, 1, 1 },
                [6] = new[] { 1, 1, 4, 1, 1, 3, 1, 5 },
            },
            [523] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 1, 4, 1, 1, 3, 1 },
                [3] = new[] { 3, 2, 2, 1, 3, 4, 1, 1 },
                [6] = new[] { 2, 1, 4, 1, 1, 3, 2, 3 },
            },
            [524] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 2, 3, 2, 1, 1, 5 },
                [3] = new[] { 2, 3, 1, 2, 3, 1, 1, 4 },
                [6] = new[] { 3, 1, 4, 1, 1, 3, 3, 1 },
            },
            [525] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 4, 1, 2, 1, 4 },
                [3] = new[] { 3, 3, 1, 2, 3, 1, 2, 2 },
                [6] = new[] { 1, 1, 4, 1, 1, 4, 1, 4 },
            },
            [526] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 1, 4, 1, 2, 2, 2 },
                [3] = new[] { 2, 2, 2, 1, 4, 1, 1, 4 },
                [6] = new[] { 2, 1, 4, 1, 1, 4, 2, 2 },
            },
            [527] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 3, 2, 3, 1, 1, 5 },
                [3] = new[] { 2, 3, 1, 2, 3, 2, 1, 3 },
                [6] = new[] { 1, 1, 4, 1, 1, 5, 1, 3 },
            },
            [528] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 2, 3, 2, 2, 1, 4 },
                [3] = new[] { 3, 3, 1, 2, 3, 2, 2, 1 },
                [6] = new[] { 2, 1, 4, 1, 1, 5, 2, 1 },
            },
            [529] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 2, 3, 2, 2, 2, 2 },
                [3] = new[] { 2, 2, 2, 1, 4, 2, 1, 3 },
                [6] = new[] { 1, 1, 4, 1, 1, 6, 1, 2 },
            },
            [530] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 1, 4, 1, 3, 2, 1 },
                [3] = new[] { 3, 2, 2, 1, 4, 2, 2, 1 },
                [6] = new[] { 1, 2, 3, 2, 1, 1, 2, 5 },
            },
            [531] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 3, 2, 3, 2, 1, 4 },
                [3] = new[] { 2, 2, 2, 1, 4, 3, 1, 2 },
                [6] = new[] { 2, 2, 3, 2, 1, 1, 3, 3 },
            },
            [532] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 3, 2, 3, 2, 2, 2 },
                [3] = new[] { 2, 3, 1, 2, 3, 4, 1, 1 },
                [6] = new[] { 3, 2, 3, 2, 1, 1, 4, 1 },
            },
            [533] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 4, 1, 4, 1, 2 },
                [3] = new[] { 2, 2, 2, 1, 4, 4, 1, 1 },
                [6] = new[] { 1, 1, 4, 1, 2, 1, 2, 5 },
            },
            [534] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 3, 2, 3, 3, 1, 3 },
                [3] = new[] { 1, 3, 1, 2, 4, 1, 1, 4 },
                [6] = new[] { 1, 2, 3, 2, 1, 2, 2, 4 },
            },
            [535] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 2, 3, 2, 4, 1, 2 },
                [3] = new[] { 2, 3, 1, 2, 4, 1, 2, 2 },
                [6] = new[] { 2, 2, 3, 2, 1, 2, 3, 2 },
            },
            [536] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 4, 1, 5, 1, 1 },
                [3] = new[] { 1, 2, 2, 1, 5, 1, 1, 4 },
                [6] = new[] { 1, 1, 4, 1, 2, 2, 2, 4 },
            },
            [537] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 2, 3, 2, 5, 1, 1 },
                [3] = new[] { 1, 3, 1, 2, 4, 2, 1, 3 },
                [6] = new[] { 2, 1, 4, 1, 2, 2, 3, 2 },
            },
            [538] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 4, 2, 1, 2, 3 },
                [3] = new[] { 2, 3, 1, 2, 4, 2, 2, 1 },
                [6] = new[] { 2, 2, 3, 2, 1, 3, 3, 1 },
            },
            [539] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 1, 4, 2, 1, 3, 1 },
                [3] = new[] { 1, 2, 2, 1, 5, 2, 1, 3 },
                [6] = new[] { 1, 1, 4, 1, 2, 3, 2, 3 },
            },
            [540] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 2, 3, 3, 1, 2, 3 },
                [3] = new[] { 2, 2, 2, 1, 5, 2, 2, 1 },
                [6] = new[] { 1, 2, 3, 2, 1, 4, 2, 2 },
            },
            [541] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 4, 2, 2, 2, 2 },
                [3] = new[] { 1, 2, 2, 1, 5, 3, 1, 2 },
                [6] = new[] { 1, 1, 4, 1, 2, 4, 2, 2 },
            },
            [542] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 3, 2, 4, 1, 2, 3 },
                [3] = new[] { 1, 3, 1, 2, 4, 4, 1, 1 },
                [6] = new[] { 1, 2, 3, 2, 1, 5, 2, 1 },
            },
            [543] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 2, 3, 3, 2, 2, 2 },
                [3] = new[] { 1, 2, 2, 1, 5, 4, 1, 1 },
                [6] = new[] { 1, 1, 4, 1, 2, 5, 2, 1 },
            },
            [544] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 4, 2, 3, 2, 1 },
                [3] = new[] { 1, 3, 1, 2, 5, 1, 2, 2 },
                [6] = new[] { 1, 3, 2, 3, 1, 1, 3, 3 },
            },
            [545] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 3, 2, 4, 2, 2, 2 },
                [3] = new[] { 1, 2, 2, 1, 6, 1, 2, 2 },
                [6] = new[] { 2, 3, 2, 3, 1, 1, 4, 1 },
            },
            [546] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 2, 3, 3, 3, 2, 1 },
                [3] = new[] { 1, 3, 1, 2, 5, 2, 2, 1 },
                [6] = new[] { 1, 2, 3, 2, 2, 1, 3, 3 },
            },
            [547] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 4, 3, 1, 3, 1 },
                [3] = new[] { 1, 2, 2, 1, 6, 2, 2, 1 },
                [6] = new[] { 1, 3, 2, 3, 1, 2, 3, 2 },
            },
            [548] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 3, 2, 5, 1, 3, 1 },
                [3] = new[] { 6, 1, 3, 1, 1, 1, 1, 3 },
                [6] = new[] { 1, 1, 4, 1, 3, 1, 3, 3 },
            },
            [549] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 3, 3, 1, 1, 1, 4 },
                [3] = new[] { 1, 1, 3, 1, 1, 1, 5, 4 },
                [6] = new[] { 1, 2, 3, 2, 2, 2, 3, 2 },
            },
            [550] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 3, 3, 1, 1, 2, 2 },
                [3] = new[] { 2, 1, 3, 1, 1, 1, 6, 2 },
                [6] = new[] { 1, 3, 2, 3, 1, 3, 3, 1 },
            },
            [551] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 3, 3, 1, 2, 1, 3 },
                [3] = new[] { 6, 1, 3, 1, 1, 2, 1, 2 },
                [6] = new[] { 1, 1, 4, 1, 3, 2, 3, 2 },
            },
            [552] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 3, 3, 1, 2, 2, 1 },
                [3] = new[] { 1, 1, 3, 1, 1, 2, 5, 3 },
                [6] = new[] { 1, 2, 3, 2, 2, 3, 3, 1 },
            },
            [553] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 3, 3, 1, 3, 1, 2 },
                [3] = new[] { 2, 1, 3, 1, 1, 2, 6, 1 },
                [6] = new[] { 1, 1, 4, 1, 3, 3, 3, 1 },
            },
            [554] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 3, 3, 1, 4, 1, 1 },
                [3] = new[] { 6, 1, 3, 1, 1, 3, 1, 1 },
                [6] = new[] { 1, 4, 1, 4, 1, 1, 4, 1 },
            },
            [555] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 2, 4, 1, 1, 1, 4 },
                [3] = new[] { 1, 1, 3, 1, 1, 3, 5, 2 },
                [6] = new[] { 1, 3, 2, 3, 2, 1, 4, 1 },
            },
            [556] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 2, 4, 1, 1, 2, 2 },
                [3] = new[] { 1, 1, 3, 1, 1, 4, 5, 1 },
                [6] = new[] { 1, 2, 3, 2, 3, 1, 4, 1 },
            },
            [557] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 3, 3, 2, 1, 1, 4 },
                [3] = new[] { 5, 2, 2, 2, 1, 1, 1, 3 },
                [6] = new[] { 1, 1, 4, 1, 4, 1, 4, 1 },
            },
            [558] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 2, 4, 1, 2, 1, 3 },
                [3] = new[] { 6, 2, 2, 2, 1, 1, 2, 1 },
                [6] = new[] { 1, 1, 4, 2, 1, 1, 1, 6 },
            },
            [559] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 2, 4, 1, 2, 2, 1 },
                [3] = new[] { 1, 2, 2, 2, 1, 1, 6, 2 },
                [6] = new[] { 2, 1, 4, 2, 1, 1, 2, 4 },
            },
            [560] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 3, 3, 2, 2, 1, 3 },
                [3] = new[] { 5, 1, 3, 1, 2, 1, 1, 3 },
                [6] = new[] { 3, 1, 4, 2, 1, 1, 3, 2 },
            },
            [561] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 3, 3, 2, 2, 2, 1 },
                [3] = new[] { 6, 1, 3, 1, 2, 1, 2, 1 },
                [6] = new[] { 1, 1, 4, 2, 1, 2, 1, 5 },
            },
            [562] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 3, 3, 2, 3, 1, 2 },
                [3] = new[] { 1, 1, 3, 1, 2, 1, 6, 2 },
                [6] = new[] { 2, 1, 4, 2, 1, 2, 2, 3 },
            },
            [563] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 2, 4, 1, 4, 1, 1 },
                [3] = new[] { 1, 2, 2, 2, 1, 2, 6, 1 },
                [6] = new[] { 3, 1, 4, 2, 1, 2, 3, 1 },
            },
            [564] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 3, 3, 2, 4, 1, 1 },
                [3] = new[] { 5, 1, 3, 1, 2, 2, 1, 2 },
                [6] = new[] { 1, 1, 4, 2, 1, 3, 1, 4 },
            },
            [565] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 5, 1, 1, 1, 4 },
                [3] = new[] { 5, 2, 2, 2, 1, 3, 1, 1 },
                [6] = new[] { 2, 1, 4, 2, 1, 3, 2, 2 },
            },
            [566] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 1, 5, 1, 1, 2, 2 },
                [3] = new[] { 1, 1, 3, 1, 2, 2, 6, 1 },
                [6] = new[] { 1, 1, 4, 2, 1, 4, 1, 3 },
            },
            [567] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 2, 4, 2, 1, 1, 4 },
                [3] = new[] { 5, 1, 3, 1, 2, 3, 1, 1 },
                [6] = new[] { 2, 1, 4, 2, 1, 4, 2, 1 },
            },
            [568] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 5, 1, 2, 1, 3 },
                [3] = new[] { 4, 3, 1, 3, 1, 1, 1, 3 },
                [6] = new[] { 1, 1, 4, 2, 1, 5, 1, 2 },
            },
            [569] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 1, 5, 1, 2, 2, 1 },
                [3] = new[] { 5, 3, 1, 3, 1, 1, 2, 1 },
                [6] = new[] { 1, 1, 4, 2, 1, 6, 1, 1 },
            },
            [570] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 3, 3, 3, 1, 1, 4 },
                [3] = new[] { 4, 2, 2, 2, 2, 1, 1, 3 },
                [6] = new[] { 1, 2, 3, 3, 1, 1, 2, 4 },
            },
            [571] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 2, 4, 2, 2, 1, 3 },
                [3] = new[] { 4, 3, 1, 3, 1, 2, 1, 2 },
                [6] = new[] { 2, 2, 3, 3, 1, 1, 3, 2 },
            },
            [572] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 2, 4, 2, 2, 2, 1 },
                [3] = new[] { 4, 1, 3, 1, 3, 1, 1, 3 },
                [6] = new[] { 1, 1, 4, 2, 2, 1, 2, 4 },
            },
            [573] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 3, 3, 3, 2, 1, 3 },
                [3] = new[] { 5, 1, 3, 1, 3, 1, 2, 1 },
                [6] = new[] { 1, 2, 3, 3, 1, 2, 2, 3 },
            },
            [574] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 3, 3, 3, 2, 2, 1 },
                [3] = new[] { 4, 3, 1, 3, 1, 3, 1, 1 },
                [6] = new[] { 2, 2, 3, 3, 1, 2, 3, 1 },
            },
            [575] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 5, 1, 4, 1, 1 },
                [3] = new[] { 4, 1, 3, 1, 3, 2, 1, 2 },
                [6] = new[] { 1, 1, 4, 2, 2, 2, 2, 3 },
            },
            [576] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 3, 3, 3, 3, 1, 2 },
                [3] = new[] { 4, 2, 2, 2, 2, 3, 1, 1 },
                [6] = new[] { 2, 1, 4, 2, 2, 2, 3, 1 },
            },
            [577] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 2, 4, 2, 4, 1, 1 },
                [3] = new[] { 4, 1, 3, 1, 3, 3, 1, 1 },
                [6] = new[] { 1, 1, 4, 2, 2, 3, 2, 2 },
            },
            [578] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 3, 3, 3, 4, 1, 1 },
                [3] = new[] { 3, 3, 1, 3, 2, 1, 1, 3 },
                [6] = new[] { 1, 2, 3, 3, 1, 4, 2, 1 },
            },
            [579] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 2, 4, 3, 1, 2, 2 },
                [3] = new[] { 4, 3, 1, 3, 2, 1, 2, 1 },
                [6] = new[] { 1, 1, 4, 2, 2, 4, 2, 1 },
            },
            [580] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 3, 3, 4, 1, 2, 2 },
                [3] = new[] { 3, 2, 2, 2, 3, 1, 1, 3 },
                [6] = new[] { 1, 3, 2, 4, 1, 1, 3, 2 },
            },
            [581] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 3, 3, 4, 2, 2, 1 },
                [3] = new[] { 3, 3, 1, 3, 2, 2, 1, 2 },
                [6] = new[] { 1, 2, 3, 3, 2, 1, 3, 2 },
            },
            [582] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 3, 4, 1, 1, 2, 1 },
                [3] = new[] { 3, 1, 3, 1, 4, 1, 1, 3 },
                [6] = new[] { 1, 3, 2, 4, 1, 2, 3, 1 },
            },
            [583] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 3, 4, 1, 3, 1, 1 },
                [3] = new[] { 3, 2, 2, 2, 3, 2, 1, 2 },
                [6] = new[] { 1, 1, 4, 2, 3, 1, 3, 2 },
            },
            [584] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 2, 5, 1, 1, 2, 1 },
                [3] = new[] { 3, 3, 1, 3, 2, 3, 1, 1 },
                [6] = new[] { 1, 2, 3, 3, 2, 2, 3, 1 },
            },
            [585] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 2, 5, 1, 2, 1, 2 },
                [3] = new[] { 3, 1, 3, 1, 4, 2, 1, 2 },
                [6] = new[] { 1, 1, 4, 2, 3, 2, 3, 1 },
            },
            [586] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 2, 5, 1, 3, 1, 1 },
                [3] = new[] { 3, 2, 2, 2, 3, 3, 1, 1 },
                [6] = new[] { 1, 1, 4, 3, 1, 1, 1, 5 },
            },
            [587] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 6, 1, 1, 1, 3 },
                [3] = new[] { 3, 1, 3, 1, 4, 3, 1, 1 },
                [6] = new[] { 2, 1, 4, 3, 1, 1, 2, 3 },
            },
            [588] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 2, 5, 2, 1, 1, 3 },
                [3] = new[] { 2, 3, 1, 3, 3, 1, 1, 3 },
                [6] = new[] { 3, 1, 4, 3, 1, 1, 3, 1 },
            },
            [589] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 3, 4, 3, 1, 1, 3 },
                [3] = new[] { 3, 3, 1, 3, 3, 1, 2, 1 },
                [6] = new[] { 1, 1, 4, 3, 1, 2, 1, 4 },
            },
            [590] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 1, 6, 1, 3, 1, 1 },
                [3] = new[] { 2, 2, 2, 2, 4, 1, 1, 3 },
                [6] = new[] { 2, 1, 4, 3, 1, 2, 2, 2 },
            },
            [591] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 2, 5, 2, 3, 1, 1 },
                [3] = new[] { 2, 3, 1, 3, 3, 2, 1, 2 },
                [6] = new[] { 1, 1, 4, 3, 1, 3, 1, 3 },
            },
            [592] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 4, 1, 1, 1, 1, 2, 5 },
                [3] = new[] { 2, 1, 3, 1, 5, 1, 1, 3 },
                [6] = new[] { 2, 1, 4, 3, 1, 3, 2, 1 },
            },
            [593] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 1, 1, 2, 1, 6 },
                [3] = new[] { 2, 2, 2, 2, 4, 2, 1, 2 },
                [6] = new[] { 1, 1, 4, 3, 1, 4, 1, 2 },
            },
            [594] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 4, 1, 1, 1, 2, 2, 4 },
                [3] = new[] { 2, 3, 1, 3, 3, 3, 1, 1 },
                [6] = new[] { 1, 1, 4, 3, 1, 5, 1, 1 },
            },
            [595] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 1, 1, 3, 1, 5 },
                [3] = new[] { 2, 1, 3, 1, 5, 2, 1, 2 },
                [6] = new[] { 1, 2, 3, 4, 1, 1, 2, 3 },
            },
            [596] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 4, 1, 1, 1, 3, 2, 3 },
                [3] = new[] { 2, 2, 2, 2, 4, 3, 1, 1 },
                [6] = new[] { 2, 2, 3, 4, 1, 1, 3, 1 },
            },
            [597] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 4, 1, 1, 1, 3, 3, 1 },
                [3] = new[] { 2, 1, 3, 1, 5, 3, 1, 1 },
                [6] = new[] { 1, 1, 4, 3, 2, 1, 2, 3 },
            },
            [598] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 1, 1, 4, 1, 4 },
                [3] = new[] { 1, 3, 1, 3, 4, 1, 1, 3 },
                [6] = new[] { 1, 2, 3, 4, 1, 2, 2, 2 },
            },
            [599] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 4, 1, 1, 1, 4, 2, 2 },
                [3] = new[] { 2, 3, 1, 3, 4, 1, 2, 1 },
                [6] = new[] { 1, 1, 4, 3, 2, 2, 2, 2 },
            },
            [600] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 1, 1, 5, 1, 3 },
                [3] = new[] { 1, 2, 2, 2, 5, 1, 1, 3 },
                [6] = new[] { 1, 2, 3, 4, 1, 3, 2, 1 },
            },
            [601] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 4, 1, 1, 1, 5, 2, 1 },
                [3] = new[] { 1, 3, 1, 3, 4, 2, 1, 2 },
                [6] = new[] { 1, 1, 4, 3, 2, 3, 2, 1 },
            },
            [602] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 1, 2, 1, 2, 5 },
                [3] = new[] { 1, 1, 3, 1, 6, 1, 1, 3 },
                [6] = new[] { 1, 3, 2, 5, 1, 1, 3, 1 },
            },
            [603] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 4, 1, 1, 2, 1, 3, 3 },
                [3] = new[] { 1, 2, 2, 2, 5, 2, 1, 2 },
                [6] = new[] { 1, 2, 3, 4, 2, 1, 3, 1 },
            },
            [604] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 4, 1, 1, 2, 1, 4, 1 },
                [3] = new[] { 1, 3, 1, 3, 4, 3, 1, 1 },
                [6] = new[] { 1, 1, 4, 3, 3, 1, 3, 1 },
            },
            [605] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 1, 2, 2, 2, 4 },
                [3] = new[] { 1, 1, 3, 1, 6, 2, 1, 2 },
                [6] = new[] { 1, 1, 4, 4, 1, 1, 1, 4 },
            },
            [606] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 4, 1, 1, 2, 2, 3, 2 },
                [3] = new[] { 1, 2, 2, 2, 5, 3, 1, 1 },
                [6] = new[] { 2, 1, 4, 4, 1, 1, 2, 2 },
            },
            [607] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 1, 2, 3, 2, 3 },
                [3] = new[] { 1, 1, 3, 1, 6, 3, 1, 1 },
                [6] = new[] { 1, 1, 4, 4, 1, 2, 1, 3 },
            },
            [608] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 4, 1, 1, 2, 3, 3, 1 },
                [3] = new[] { 1, 3, 1, 3, 5, 1, 2, 1 },
                [6] = new[] { 2, 1, 4, 4, 1, 2, 2, 1 },
            },
            [609] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 1, 2, 4, 2, 2 },
                [3] = new[] { 1, 2, 2, 2, 6, 1, 2, 1 },
                [6] = new[] { 1, 1, 4, 4, 1, 3, 1, 2 },
            },
            [610] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 1, 2, 5, 2, 1 },
                [3] = new[] { 6, 1, 3, 2, 1, 1, 1, 2 },
                [6] = new[] { 1, 1, 4, 4, 1, 4, 1, 1 },
            },
            [611] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 1, 3, 1, 3, 3 },
                [3] = new[] { 1, 1, 3, 2, 1, 1, 5, 3 },
                [6] = new[] { 1, 2, 3, 5, 1, 1, 2, 2 },
            },
            [612] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 4, 1, 1, 3, 1, 4, 1 },
                [3] = new[] { 2, 1, 3, 2, 1, 1, 6, 1 },
                [6] = new[] { 1, 1, 4, 4, 2, 1, 2, 2 },
            },
            [613] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 1, 3, 2, 3, 2 },
                [3] = new[] { 6, 1, 3, 2, 1, 2, 1, 1 },
                [6] = new[] { 1, 2, 3, 5, 1, 2, 2, 1 },
            },
            [614] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 1, 3, 3, 3, 1 },
                [3] = new[] { 1, 1, 3, 2, 1, 2, 5, 2 },
                [6] = new[] { 1, 1, 4, 4, 2, 2, 2, 1 },
            },
            [615] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 1, 4, 1, 4, 1 },
                [3] = new[] { 1, 1, 3, 2, 1, 3, 5, 1 },
                [6] = new[] { 1, 1, 4, 5, 1, 1, 1, 3 },
            },
            [616] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 2, 1, 1, 1, 1, 6 },
                [3] = new[] { 5, 2, 2, 3, 1, 1, 1, 2 },
                [6] = new[] { 2, 1, 4, 5, 1, 1, 2, 1 },
            },
            [617] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 2, 1, 1, 1, 2, 4 },
                [3] = new[] { 1, 2, 2, 3, 1, 1, 6, 1 },
                [6] = new[] { 1, 1, 4, 5, 1, 2, 1, 2 },
            },
            [618] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 3, 2, 1, 1, 1, 3, 2 },
                [3] = new[] { 5, 1, 3, 2, 2, 1, 1, 2 },
                [6] = new[] { 1, 1, 4, 5, 1, 3, 1, 1 },
            },
            [619] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 2, 1, 1, 2, 1, 5 },
                [3] = new[] { 5, 2, 2, 3, 1, 2, 1, 1 },
                [6] = new[] { 1, 2, 3, 6, 1, 1, 2, 1 },
            },
            [620] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 2, 1, 1, 2, 2, 3 },
                [3] = new[] { 1, 1, 3, 2, 2, 1, 6, 1 },
                [6] = new[] { 1, 1, 4, 5, 2, 1, 2, 1 },
            },
            [621] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 2, 1, 1, 3, 1, 4 },
                [3] = new[] { 5, 1, 3, 2, 2, 2, 1, 1 },
                [6] = new[] { 1, 5, 1, 1, 1, 1, 4, 3 },
            },
            [622] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 2, 1, 1, 3, 2, 2 },
                [3] = new[] { 4, 3, 1, 4, 1, 1, 1, 2 },
                [6] = new[] { 2, 5, 1, 1, 1, 1, 5, 1 },
            },
            [623] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 2, 1, 1, 4, 1, 3 },
                [3] = new[] { 4, 2, 2, 3, 2, 1, 1, 2 },
                [6] = new[] { 1, 5, 1, 1, 1, 2, 4, 2 },
            },
            [624] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 2, 1, 1, 4, 2, 1 },
                [3] = new[] { 4, 3, 1, 4, 1, 2, 1, 1 },
                [6] = new[] { 1, 5, 1, 1, 1, 3, 4, 1 },
            },
            [625] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 2, 1, 1, 5, 1, 2 },
                [3] = new[] { 4, 1, 3, 2, 3, 1, 1, 2 },
                [6] = new[] { 1, 5, 1, 1, 2, 1, 5, 1 },
            },
            [626] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 2, 1, 1, 1, 6 },
                [3] = new[] { 4, 2, 2, 3, 2, 2, 1, 1 },
                [6] = new[] { 1, 4, 2, 1, 1, 1, 3, 4 },
            },
            [627] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 4, 1, 2, 1, 1, 2, 4 },
                [3] = new[] { 4, 1, 3, 2, 3, 2, 1, 1 },
                [6] = new[] { 2, 4, 2, 1, 1, 1, 4, 2 },
            },
            [628] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 4, 1, 2, 1, 1, 3, 2 },
                [3] = new[] { 3, 3, 1, 4, 2, 1, 1, 2 },
                [6] = new[] { 1, 4, 2, 1, 1, 2, 3, 3 },
            },
            [629] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 2, 1, 2, 1, 1, 6 },
                [3] = new[] { 3, 2, 2, 3, 3, 1, 1, 2 },
                [6] = new[] { 2, 4, 2, 1, 1, 2, 4, 1 },
            },
            [630] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 2, 1, 2, 1, 5 },
                [3] = new[] { 3, 3, 1, 4, 2, 2, 1, 1 },
                [6] = new[] { 1, 4, 2, 1, 1, 3, 3, 2 },
            },
            [631] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 2, 1, 2, 1, 3, 2 },
                [3] = new[] { 3, 1, 3, 2, 4, 1, 1, 2 },
                [6] = new[] { 1, 4, 2, 1, 1, 4, 3, 1 },
            },
            [632] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 4, 1, 2, 1, 2, 3, 1 },
                [3] = new[] { 3, 2, 2, 3, 3, 2, 1, 1 },
                [6] = new[] { 1, 5, 1, 2, 1, 1, 4, 2 },
            },
            [633] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 2, 1, 2, 2, 1, 5 },
                [3] = new[] { 3, 1, 3, 2, 4, 2, 1, 1 },
                [6] = new[] { 1, 4, 2, 1, 2, 1, 4, 2 },
            },
            [634] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 2, 1, 2, 2, 2, 3 },
                [3] = new[] { 2, 3, 1, 4, 3, 1, 1, 2 },
                [6] = new[] { 1, 5, 1, 2, 1, 2, 4, 1 },
            },
            [635] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 2, 1, 2, 2, 3, 1 },
                [3] = new[] { 2, 2, 2, 3, 4, 1, 1, 2 },
                [6] = new[] { 1, 4, 2, 1, 2, 2, 4, 1 },
            },
            [636] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 2, 1, 2, 3, 1, 4 },
                [3] = new[] { 2, 3, 1, 4, 3, 2, 1, 1 },
                [6] = new[] { 1, 3, 3, 1, 1, 1, 2, 5 },
            },
            [637] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 2, 1, 4, 1, 3 },
                [3] = new[] { 2, 1, 3, 2, 5, 1, 1, 2 },
                [6] = new[] { 2, 3, 3, 1, 1, 1, 3, 3 },
            },
            [638] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 4, 1, 2, 1, 4, 2, 1 },
                [3] = new[] { 2, 2, 2, 3, 4, 2, 1, 1 },
                [6] = new[] { 3, 3, 3, 1, 1, 1, 4, 1 },
            },
            [639] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 2, 1, 2, 4, 1, 3 },
                [3] = new[] { 2, 1, 3, 2, 5, 2, 1, 1 },
                [6] = new[] { 1, 3, 3, 1, 1, 2, 2, 4 },
            },
            [640] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 2, 1, 2, 4, 2, 1 },
                [3] = new[] { 1, 3, 1, 4, 4, 1, 1, 2 },
                [6] = new[] { 2, 3, 3, 1, 1, 2, 3, 2 },
            },
            [641] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 2, 1, 6, 1, 1 },
                [3] = new[] { 1, 2, 2, 3, 5, 1, 1, 2 },
                [6] = new[] { 1, 3, 3, 1, 1, 3, 2, 3 },
            },
            [642] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 2, 2, 1, 2, 4 },
                [3] = new[] { 1, 3, 1, 4, 4, 2, 1, 1 },
                [6] = new[] { 2, 3, 3, 1, 1, 3, 3, 1 },
            },
            [643] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 4, 1, 2, 2, 1, 3, 2 },
                [3] = new[] { 1, 1, 3, 2, 6, 1, 1, 2 },
                [6] = new[] { 1, 3, 3, 1, 1, 4, 2, 2 },
            },
            [644] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 2, 1, 3, 1, 2, 4 },
                [3] = new[] { 1, 2, 2, 3, 5, 2, 1, 1 },
                [6] = new[] { 1, 3, 3, 1, 1, 5, 2, 1 },
            },
            [645] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 2, 2, 2, 2, 3 },
                [3] = new[] { 1, 1, 3, 2, 6, 2, 1, 1 },
                [6] = new[] { 1, 4, 2, 2, 1, 1, 3, 3 },
            },
            [646] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 4, 1, 2, 2, 2, 3, 1 },
                [3] = new[] { 6, 1, 3, 3, 1, 1, 1, 1 },
                [6] = new[] { 2, 4, 2, 2, 1, 1, 4, 1 },
            },
            [647] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 2, 1, 3, 2, 2, 3 },
                [3] = new[] { 1, 1, 3, 3, 1, 1, 5, 2 },
                [6] = new[] { 1, 3, 3, 1, 2, 1, 3, 3 },
            },
            [648] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 2, 1, 3, 2, 3, 1 },
                [3] = new[] { 1, 1, 3, 3, 1, 2, 5, 1 },
                [6] = new[] { 1, 4, 2, 2, 1, 2, 3, 2 },
            },
            [649] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 2, 1, 3, 3, 2, 2 },
                [3] = new[] { 5, 2, 2, 4, 1, 1, 1, 1 },
                [6] = new[] { 1, 3, 3, 1, 2, 2, 3, 2 },
            },
            [650] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 2, 2, 4, 2, 1 },
                [3] = new[] { 5, 1, 3, 3, 2, 1, 1, 1 },
                [6] = new[] { 1, 4, 2, 2, 1, 3, 3, 1 },
            },
            [651] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 2, 3, 1, 3, 2 },
                [3] = new[] { 4, 3, 1, 5, 1, 1, 1, 1 },
                [6] = new[] { 1, 3, 3, 1, 2, 3, 3, 1 },
            },
            [652] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 2, 1, 4, 1, 3, 2 },
                [3] = new[] { 4, 2, 2, 4, 2, 1, 1, 1 },
                [6] = new[] { 1, 5, 1, 3, 1, 1, 4, 1 },
            },
            [653] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 2, 3, 2, 3, 1 },
                [3] = new[] { 4, 1, 3, 3, 3, 1, 1, 1 },
                [6] = new[] { 1, 4, 2, 2, 2, 1, 4, 1 },
            },
            [654] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 2, 1, 4, 2, 3, 1 },
                [3] = new[] { 3, 3, 1, 5, 2, 1, 1, 1 },
                [6] = new[] { 1, 3, 3, 1, 3, 1, 4, 1 },
            },
            [655] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 3, 1, 1, 1, 1, 5 },
                [3] = new[] { 3, 2, 2, 4, 3, 1, 1, 1 },
                [6] = new[] { 1, 2, 4, 1, 1, 1, 1, 6 },
            },
            [656] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 2, 3, 1, 1, 1, 2, 3 },
                [3] = new[] { 3, 1, 3, 3, 4, 1, 1, 1 },
                [6] = new[] { 2, 2, 4, 1, 1, 1, 2, 4 },
            },
            [657] = new Dictionary<int, int[]>
            {
                [0] = new[] { 5, 2, 3, 1, 1, 1, 3, 1 },
                [3] = new[] { 2, 3, 1, 5, 3, 1, 1, 1 },
                [6] = new[] { 3, 2, 4, 1, 1, 1, 3, 2 },
            },
            [658] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 3, 1, 1, 2, 1, 4 },
                [3] = new[] { 2, 2, 2, 4, 4, 1, 1, 1 },
                [6] = new[] { 1, 2, 4, 1, 1, 2, 1, 5 },
            },
            [659] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 2, 3, 1, 1, 2, 2, 2 },
                [3] = new[] { 2, 1, 3, 3, 5, 1, 1, 1 },
                [6] = new[] { 2, 2, 4, 1, 1, 2, 2, 3 },
            },
            [660] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 3, 1, 1, 3, 1, 3 },
                [3] = new[] { 1, 3, 1, 5, 4, 1, 1, 1 },
                [6] = new[] { 3, 2, 4, 1, 1, 2, 3, 1 },
            },
            [661] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 2, 3, 1, 1, 3, 2, 1 },
                [3] = new[] { 1, 2, 2, 4, 5, 1, 1, 1 },
                [6] = new[] { 1, 2, 4, 1, 1, 3, 1, 4 },
            },
            [662] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 3, 1, 1, 4, 1, 2 },
                [3] = new[] { 1, 1, 3, 3, 6, 1, 1, 1 },
                [6] = new[] { 2, 2, 4, 1, 1, 3, 2, 2 },
            },
            [663] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 3, 1, 1, 5, 1, 1 },
                [3] = new[] { 1, 1, 3, 4, 1, 1, 5, 1 },
                [6] = new[] { 1, 2, 4, 1, 1, 4, 1, 3 },
            },
            [664] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 2, 2, 1, 1, 1, 5 },
                [3] = new[] { 4, 4, 1, 1, 1, 1, 1, 4 },
                [6] = new[] { 2, 2, 4, 1, 1, 4, 2, 1 },
            },
            [665] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 2, 2, 1, 1, 2, 3 },
                [3] = new[] { 5, 4, 1, 1, 1, 1, 2, 2 },
                [6] = new[] { 1, 2, 4, 1, 1, 5, 1, 2 },
            },
            [666] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 3, 1, 2, 1, 1, 5 },
                [3] = new[] { 4, 4, 1, 1, 1, 2, 1, 3 },
                [6] = new[] { 1, 2, 4, 1, 1, 6, 1, 1 },
            },
            [667] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 2, 2, 1, 2, 1, 4 },
                [3] = new[] { 5, 4, 1, 1, 1, 2, 2, 1 },
                [6] = new[] { 1, 3, 3, 2, 1, 1, 2, 4 },
            },
            [668] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 2, 2, 1, 2, 2, 2 },
                [3] = new[] { 4, 4, 1, 1, 1, 3, 1, 2 },
                [6] = new[] { 2, 3, 3, 2, 1, 1, 3, 2 },
            },
            [669] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 3, 1, 2, 2, 1, 4 },
                [3] = new[] { 4, 4, 1, 1, 1, 4, 1, 1 },
                [6] = new[] { 1, 2, 4, 1, 2, 1, 2, 4 },
            },
            [670] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 3, 1, 2, 2, 2, 2 },
                [3] = new[] { 3, 4, 1, 1, 2, 1, 1, 4 },
                [6] = new[] { 1, 3, 3, 2, 1, 2, 2, 3 },
            },
            [671] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 2, 2, 1, 3, 2, 1 },
                [3] = new[] { 4, 4, 1, 1, 2, 1, 2, 2 },
                [6] = new[] { 2, 3, 3, 2, 1, 2, 3, 1 },
            },
            [672] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 3, 1, 2, 3, 1, 3 },
                [3] = new[] { 3, 4, 1, 1, 2, 2, 1, 3 },
                [6] = new[] { 1, 2, 4, 1, 2, 2, 2, 3 },
            },
            [673] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 2, 2, 1, 4, 1, 2 },
                [3] = new[] { 4, 4, 1, 1, 2, 2, 2, 1 },
                [6] = new[] { 2, 2, 4, 1, 2, 2, 3, 1 },
            },
            [674] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 3, 1, 2, 4, 1, 2 },
                [3] = new[] { 3, 4, 1, 1, 2, 3, 1, 2 },
                [6] = new[] { 1, 2, 4, 1, 2, 3, 2, 2 },
            },
            [675] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 2, 2, 1, 5, 1, 1 },
                [3] = new[] { 3, 4, 1, 1, 2, 4, 1, 1 },
                [6] = new[] { 1, 3, 3, 2, 1, 4, 2, 1 },
            },
            [676] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 3, 1, 2, 5, 1, 1 },
                [3] = new[] { 2, 4, 1, 1, 3, 1, 1, 4 },
                [6] = new[] { 1, 2, 4, 1, 2, 4, 2, 1 },
            },
            [677] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 3, 1, 1, 1, 5 },
                [3] = new[] { 3, 4, 1, 1, 3, 1, 2, 2 },
                [6] = new[] { 1, 4, 2, 3, 1, 1, 3, 2 },
            },
            [678] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 4, 1, 3, 1, 1, 2, 3 },
                [3] = new[] { 2, 4, 1, 1, 3, 2, 1, 3 },
                [6] = new[] { 1, 3, 3, 2, 2, 1, 3, 2 },
            },
            [679] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 2, 2, 2, 1, 1, 5 },
                [3] = new[] { 3, 4, 1, 1, 3, 2, 2, 1 },
                [6] = new[] { 1, 4, 2, 3, 1, 2, 3, 1 },
            },
            [680] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 3, 1, 2, 1, 4 },
                [3] = new[] { 2, 4, 1, 1, 3, 3, 1, 2 },
                [6] = new[] { 1, 2, 4, 1, 3, 1, 3, 2 },
            },
            [681] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 2, 2, 2, 1, 3, 1 },
                [3] = new[] { 2, 4, 1, 1, 3, 4, 1, 1 },
                [6] = new[] { 1, 3, 3, 2, 2, 2, 3, 1 },
            },
            [682] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 3, 1, 3, 1, 1, 5 },
                [3] = new[] { 1, 4, 1, 1, 4, 1, 1, 4 },
                [6] = new[] { 1, 2, 4, 1, 3, 2, 3, 1 },
            },
            [683] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 2, 2, 2, 2, 1, 4 },
                [3] = new[] { 2, 4, 1, 1, 4, 1, 2, 2 },
                [6] = new[] { 2, 1, 5, 1, 1, 1, 1, 5 },
            },
            [684] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 2, 2, 2, 2, 2, 2 },
                [3] = new[] { 1, 4, 1, 1, 4, 2, 1, 3 },
                [6] = new[] { 3, 1, 5, 1, 1, 1, 2, 3 },
            },
            [685] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 4, 1, 3, 1, 3, 2, 1 },
                [3] = new[] { 2, 4, 1, 1, 4, 2, 2, 1 },
                [6] = new[] { 4, 1, 5, 1, 1, 1, 3, 1 },
            },
            [686] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 3, 1, 3, 2, 1, 4 },
                [3] = new[] { 1, 4, 1, 1, 4, 3, 1, 2 },
                [6] = new[] { 2, 1, 5, 1, 1, 2, 1, 4 },
            },
            [687] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 3, 1, 3, 2, 2, 2 },
                [3] = new[] { 1, 4, 1, 1, 4, 4, 1, 1 },
                [6] = new[] { 3, 1, 5, 1, 1, 2, 2, 2 },
            },
            [688] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 3, 1, 4, 1, 2 },
                [3] = new[] { 1, 4, 1, 1, 5, 1, 2, 2 },
                [6] = new[] { 2, 1, 5, 1, 1, 3, 1, 3 },
            },
            [689] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 3, 1, 3, 3, 1, 3 },
                [3] = new[] { 1, 4, 1, 1, 5, 2, 2, 1 },
                [6] = new[] { 3, 1, 5, 1, 1, 3, 2, 1 },
            },
            [690] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 2, 2, 2, 4, 1, 2 },
                [3] = new[] { 5, 3, 2, 1, 1, 1, 1, 3 },
                [6] = new[] { 2, 1, 5, 1, 1, 4, 1, 2 },
            },
            [691] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 3, 1, 5, 1, 1 },
                [3] = new[] { 6, 3, 2, 1, 1, 1, 2, 1 },
                [6] = new[] { 2, 1, 5, 1, 1, 5, 1, 1 },
            },
            [692] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 2, 2, 2, 5, 1, 1 },
                [3] = new[] { 1, 3, 2, 1, 1, 1, 6, 2 },
                [6] = new[] { 1, 2, 4, 2, 1, 1, 1, 5 },
            },
            [693] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 3, 2, 1, 2, 3 },
                [3] = new[] { 5, 3, 2, 1, 1, 2, 1, 2 },
                [6] = new[] { 2, 2, 4, 2, 1, 1, 2, 3 },
            },
            [694] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 4, 1, 3, 2, 1, 3, 1 },
                [3] = new[] { 1, 3, 2, 1, 1, 2, 6, 1 },
                [6] = new[] { 3, 2, 4, 2, 1, 1, 3, 1 },
            },
            [695] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 2, 2, 3, 1, 2, 3 },
                [3] = new[] { 5, 3, 2, 1, 1, 3, 1, 1 },
                [6] = new[] { 1, 1, 5, 1, 2, 1, 1, 5 },
            },
            [696] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 3, 2, 2, 2, 2 },
                [3] = new[] { 4, 4, 1, 2, 1, 1, 1, 3 },
                [6] = new[] { 1, 2, 4, 2, 1, 2, 1, 4 },
            },
            [697] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 3, 1, 4, 1, 2, 3 },
                [3] = new[] { 5, 4, 1, 2, 1, 1, 2, 1 },
                [6] = new[] { 2, 2, 4, 2, 1, 2, 2, 2 },
            },
            [698] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 2, 2, 3, 2, 2, 2 },
                [3] = new[] { 4, 3, 2, 1, 2, 1, 1, 3 },
                [6] = new[] { 1, 1, 5, 1, 2, 2, 1, 4 },
            },
            [699] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 3, 2, 3, 2, 1 },
                [3] = new[] { 4, 4, 1, 2, 1, 2, 1, 2 },
                [6] = new[] { 2, 1, 5, 1, 2, 2, 2, 2 },
            },
            [700] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 3, 1, 4, 2, 2, 2 },
                [3] = new[] { 4, 3, 2, 1, 2, 2, 1, 2 },
                [6] = new[] { 2, 2, 4, 2, 1, 3, 2, 1 },
            },
            [701] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 2, 2, 3, 3, 2, 1 },
                [3] = new[] { 4, 4, 1, 2, 1, 3, 1, 1 },
                [6] = new[] { 1, 1, 5, 1, 2, 3, 1, 3 },
            },
            [702] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 3, 3, 1, 3, 1 },
                [3] = new[] { 4, 3, 2, 1, 2, 3, 1, 1 },
                [6] = new[] { 1, 2, 4, 2, 1, 4, 1, 2 },
            },
            [703] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 2, 2, 4, 1, 3, 1 },
                [3] = new[] { 3, 4, 1, 2, 2, 1, 1, 3 },
                [6] = new[] { 1, 1, 5, 1, 2, 4, 1, 2 },
            },
            [704] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 3, 1, 5, 1, 3, 1 },
                [3] = new[] { 4, 4, 1, 2, 2, 1, 2, 1 },
                [6] = new[] { 1, 2, 4, 2, 1, 5, 1, 1 },
            },
            [705] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 4, 1, 1, 1, 1, 4 },
                [3] = new[] { 3, 3, 2, 1, 3, 1, 1, 3 },
                [6] = new[] { 1, 1, 5, 1, 2, 5, 1, 1 },
            },
            [706] = new Dictionary<int, int[]>
            {
                [0] = new[] { 5, 1, 4, 1, 1, 1, 2, 2 },
                [3] = new[] { 3, 4, 1, 2, 2, 2, 1, 2 },
                [6] = new[] { 1, 3, 3, 3, 1, 1, 2, 3 },
            },
            [707] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 4, 1, 1, 2, 1, 3 },
                [3] = new[] { 3, 3, 2, 1, 3, 2, 1, 2 },
                [6] = new[] { 2, 3, 3, 3, 1, 1, 3, 1 },
            },
            [708] = new Dictionary<int, int[]>
            {
                [0] = new[] { 5, 1, 4, 1, 1, 2, 2, 1 },
                [3] = new[] { 3, 4, 1, 2, 2, 3, 1, 1 },
                [6] = new[] { 1, 2, 4, 2, 2, 1, 2, 3 },
            },
            [709] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 4, 1, 1, 3, 1, 2 },
                [3] = new[] { 3, 3, 2, 1, 3, 3, 1, 1 },
                [6] = new[] { 1, 3, 3, 3, 1, 2, 2, 2 },
            },
            [710] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 4, 1, 1, 4, 1, 1 },
                [3] = new[] { 2, 4, 1, 2, 3, 1, 1, 3 },
                [6] = new[] { 1, 1, 5, 1, 3, 1, 2, 3 },
            },
            [711] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 3, 2, 1, 1, 1, 4 },
                [3] = new[] { 3, 4, 1, 2, 3, 1, 2, 1 },
                [6] = new[] { 1, 2, 4, 2, 2, 2, 2, 2 },
            },
            [712] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 2, 3, 2, 1, 1, 2, 2 },
                [3] = new[] { 2, 3, 2, 1, 4, 1, 1, 3 },
                [6] = new[] { 1, 3, 3, 3, 1, 3, 2, 1 },
            },
            [713] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 4, 1, 2, 1, 1, 4 },
                [3] = new[] { 2, 4, 1, 2, 3, 2, 1, 2 },
                [6] = new[] { 1, 1, 5, 1, 3, 2, 2, 2 },
            },
            [714] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 4, 1, 2, 1, 2, 2 },
                [3] = new[] { 2, 3, 2, 1, 4, 2, 1, 2 },
                [6] = new[] { 1, 2, 4, 2, 2, 3, 2, 1 },
            },
            [715] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 2, 3, 2, 1, 2, 2, 1 },
                [3] = new[] { 2, 4, 1, 2, 3, 3, 1, 1 },
                [6] = new[] { 1, 1, 5, 1, 3, 3, 2, 1 },
            },
            [716] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 4, 1, 2, 2, 1, 3 },
                [3] = new[] { 2, 3, 2, 1, 4, 3, 1, 1 },
                [6] = new[] { 1, 4, 2, 4, 1, 1, 3, 1 },
            },
            [717] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 4, 1, 2, 2, 2, 1 },
                [3] = new[] { 1, 4, 1, 2, 4, 1, 1, 3 },
                [6] = new[] { 1, 3, 3, 3, 2, 1, 3, 1 },
            },
            [718] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 4, 1, 2, 3, 1, 2 },
                [3] = new[] { 2, 4, 1, 2, 4, 1, 2, 1 },
                [6] = new[] { 1, 2, 4, 2, 3, 1, 3, 1 },
            },
            [719] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 3, 2, 1, 4, 1, 1 },
                [3] = new[] { 1, 3, 2, 1, 5, 1, 1, 3 },
                [6] = new[] { 1, 1, 5, 1, 4, 1, 3, 1 },
            },
            [720] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 4, 1, 2, 4, 1, 1 },
                [3] = new[] { 1, 4, 1, 2, 4, 2, 1, 2 },
                [6] = new[] { 2, 1, 5, 2, 1, 1, 1, 4 },
            },
            [721] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 2, 3, 1, 1, 1, 4 },
                [3] = new[] { 1, 3, 2, 1, 5, 2, 1, 2 },
                [6] = new[] { 3, 1, 5, 2, 1, 1, 2, 2 },
            },
            [722] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 2, 3, 1, 1, 2, 2 },
                [3] = new[] { 1, 4, 1, 2, 4, 3, 1, 1 },
                [6] = new[] { 2, 1, 5, 2, 1, 2, 1, 3 },
            },
            [723] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 3, 2, 2, 1, 1, 4 },
                [3] = new[] { 1, 3, 2, 1, 5, 3, 1, 1 },
                [6] = new[] { 3, 1, 5, 2, 1, 2, 2, 1 },
            },
            [724] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 2, 3, 1, 2, 1, 3 },
                [3] = new[] { 1, 4, 1, 2, 5, 1, 2, 1 },
                [6] = new[] { 2, 1, 5, 2, 1, 3, 1, 2 },
            },
            [725] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 2, 3, 1, 2, 2, 1 },
                [3] = new[] { 1, 3, 2, 1, 6, 1, 2, 1 },
                [6] = new[] { 2, 1, 5, 2, 1, 4, 1, 1 },
            },
            [726] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 4, 1, 3, 1, 1, 4 },
                [3] = new[] { 6, 2, 3, 1, 1, 1, 1, 2 },
                [6] = new[] { 1, 2, 4, 3, 1, 1, 1, 4 },
            },
            [727] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 3, 2, 2, 2, 1, 3 },
                [3] = new[] { 1, 2, 3, 1, 1, 1, 5, 3 },
                [6] = new[] { 2, 2, 4, 3, 1, 1, 2, 2 },
            },
            [728] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 3, 2, 2, 2, 2, 1 },
                [3] = new[] { 2, 2, 3, 1, 1, 1, 6, 1 },
                [6] = new[] { 1, 1, 5, 2, 2, 1, 1, 4 },
            },
            [729] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 4, 1, 3, 2, 1, 3 },
                [3] = new[] { 6, 2, 3, 1, 1, 2, 1, 1 },
                [6] = new[] { 1, 2, 4, 3, 1, 2, 1, 3 },
            },
            [730] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 4, 1, 3, 2, 2, 1 },
                [3] = new[] { 1, 2, 3, 1, 1, 2, 5, 2 },
                [6] = new[] { 2, 2, 4, 3, 1, 2, 2, 1 },
            },
            [731] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 2, 3, 1, 4, 1, 1 },
                [3] = new[] { 1, 2, 3, 1, 1, 3, 5, 1 },
                [6] = new[] { 1, 1, 5, 2, 2, 2, 1, 3 },
            },
            [732] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 4, 1, 3, 3, 1, 2 },
                [3] = new[] { 5, 3, 2, 2, 1, 1, 1, 2 },
                [6] = new[] { 2, 1, 5, 2, 2, 2, 2, 1 },
            },
            [733] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 3, 2, 2, 4, 1, 1 },
                [3] = new[] { 1, 3, 2, 2, 1, 1, 6, 1 },
                [6] = new[] { 1, 1, 5, 2, 2, 3, 1, 2 },
            },
            [734] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 4, 1, 3, 4, 1, 1 },
                [3] = new[] { 5, 2, 3, 1, 2, 1, 1, 2 },
                [6] = new[] { 1, 2, 4, 3, 1, 4, 1, 1 },
            },
            [735] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 4, 1, 1, 1, 4 },
                [3] = new[] { 5, 3, 2, 2, 1, 2, 1, 1 },
                [6] = new[] { 1, 1, 5, 2, 2, 4, 1, 1 },
            },
            [736] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 4, 1, 4, 1, 1, 2, 2 },
                [3] = new[] { 1, 2, 3, 1, 2, 1, 6, 1 },
                [6] = new[] { 1, 3, 3, 4, 1, 1, 2, 2 },
            },
            [737] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 2, 3, 2, 1, 1, 4 },
                [3] = new[] { 5, 2, 3, 1, 2, 2, 1, 1 },
                [6] = new[] { 1, 2, 4, 3, 2, 1, 2, 2 },
            },
            [738] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 4, 1, 2, 1, 3 },
                [3] = new[] { 4, 4, 1, 3, 1, 1, 1, 2 },
                [6] = new[] { 1, 3, 3, 4, 1, 2, 2, 1 },
            },
            [739] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 4, 1, 4, 1, 2, 2, 1 },
                [3] = new[] { 4, 3, 2, 2, 2, 1, 1, 2 },
                [6] = new[] { 1, 1, 5, 2, 3, 1, 2, 2 },
            },
            [740] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 3, 2, 3, 1, 1, 4 },
                [3] = new[] { 4, 4, 1, 3, 1, 2, 1, 1 },
                [6] = new[] { 1, 2, 4, 3, 2, 2, 2, 1 },
            },
            [741] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 2, 3, 2, 2, 1, 3 },
                [3] = new[] { 4, 2, 3, 1, 3, 1, 1, 2 },
                [6] = new[] { 1, 1, 5, 2, 3, 2, 2, 1 },
            },
            [742] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 2, 3, 2, 2, 2, 1 },
                [3] = new[] { 4, 3, 2, 2, 2, 2, 1, 1 },
                [6] = new[] { 2, 1, 5, 3, 1, 1, 1, 3 },
            },
            [743] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 4, 1, 4, 1, 1, 4 },
                [3] = new[] { 4, 2, 3, 1, 3, 2, 1, 1 },
                [6] = new[] { 3, 1, 5, 3, 1, 1, 2, 1 },
            },
            [744] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 3, 2, 3, 2, 1, 3 },
                [3] = new[] { 3, 4, 1, 3, 2, 1, 1, 2 },
                [6] = new[] { 2, 1, 5, 3, 1, 2, 1, 2 },
            },
            [745] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 3, 2, 3, 2, 2, 1 },
                [3] = new[] { 3, 3, 2, 2, 3, 1, 1, 2 },
                [6] = new[] { 2, 1, 5, 3, 1, 3, 1, 1 },
            },
            [746] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 4, 1, 4, 1, 1 },
                [3] = new[] { 3, 4, 1, 3, 2, 2, 1, 1 },
                [6] = new[] { 1, 2, 4, 4, 1, 1, 1, 3 },
            },
            [747] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 4, 1, 4, 2, 1, 3 },
                [3] = new[] { 3, 2, 3, 1, 4, 1, 1, 2 },
                [6] = new[] { 2, 2, 4, 4, 1, 1, 2, 1 },
            },
            [748] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 4, 1, 4, 2, 2, 1 },
                [3] = new[] { 3, 3, 2, 2, 3, 2, 1, 1 },
                [6] = new[] { 1, 1, 5, 3, 2, 1, 1, 3 },
            },
            [749] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 2, 3, 2, 4, 1, 1 },
                [3] = new[] { 3, 2, 3, 1, 4, 2, 1, 1 },
                [6] = new[] { 1, 2, 4, 4, 1, 2, 1, 2 },
            },
            [750] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 4, 1, 4, 3, 1, 2 },
                [3] = new[] { 2, 4, 1, 3, 3, 1, 1, 2 },
                [6] = new[] { 1, 1, 5, 3, 2, 2, 1, 2 },
            },
            [751] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 4, 2, 1, 2, 2 },
                [3] = new[] { 2, 3, 2, 2, 4, 1, 1, 2 },
                [6] = new[] { 1, 2, 4, 4, 1, 3, 1, 1 },
            },
            [752] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 2, 3, 3, 1, 2, 2 },
                [3] = new[] { 2, 4, 1, 3, 3, 2, 1, 1 },
                [6] = new[] { 1, 1, 5, 3, 2, 3, 1, 1 },
            },
            [753] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 4, 2, 2, 2, 1 },
                [3] = new[] { 2, 2, 3, 1, 5, 1, 1, 2 },
                [6] = new[] { 1, 3, 3, 5, 1, 1, 2, 1 },
            },
            [754] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 3, 2, 4, 1, 2, 2 },
                [3] = new[] { 2, 3, 2, 2, 4, 2, 1, 1 },
                [6] = new[] { 1, 2, 4, 4, 2, 1, 2, 1 },
            },
            [755] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 2, 3, 3, 2, 2, 1 },
                [3] = new[] { 2, 2, 3, 1, 5, 2, 1, 1 },
                [6] = new[] { 1, 1, 5, 3, 3, 1, 2, 1 },
            },
            [756] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 4, 1, 5, 1, 2, 2 },
                [3] = new[] { 1, 4, 1, 3, 4, 1, 1, 2 },
                [6] = new[] { 2, 1, 5, 4, 1, 1, 1, 2 },
            },
            [757] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 3, 2, 4, 2, 2, 1 },
                [3] = new[] { 1, 3, 2, 2, 5, 1, 1, 2 },
                [6] = new[] { 2, 1, 5, 4, 1, 2, 1, 1 },
            },
            [758] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 4, 1, 5, 2, 2, 1 },
                [3] = new[] { 1, 4, 1, 3, 4, 2, 1, 1 },
                [6] = new[] { 1, 2, 4, 5, 1, 1, 1, 2 },
            },
            [759] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 4, 2, 1, 1, 1, 3 },
                [3] = new[] { 1, 2, 3, 1, 6, 1, 1, 2 },
                [6] = new[] { 1, 1, 5, 4, 2, 1, 1, 2 },
            },
            [760] = new Dictionary<int, int[]>
            {
                [0] = new[] { 5, 1, 4, 2, 1, 1, 2, 1 },
                [3] = new[] { 1, 3, 2, 2, 5, 2, 1, 1 },
                [6] = new[] { 1, 2, 4, 5, 1, 2, 1, 1 },
            },
            [761] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 4, 2, 1, 2, 1, 2 },
                [3] = new[] { 1, 2, 3, 1, 6, 2, 1, 1 },
                [6] = new[] { 1, 1, 5, 4, 2, 2, 1, 1 },
            },
            [762] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 4, 2, 1, 3, 1, 1 },
                [3] = new[] { 1, 1, 4, 1, 1, 1, 4, 4 },
                [6] = new[] { 1, 6, 1, 1, 1, 1, 4, 2 },
            },
            [763] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 3, 3, 1, 1, 1, 3 },
                [3] = new[] { 2, 1, 4, 1, 1, 1, 5, 2 },
                [6] = new[] { 1, 6, 1, 1, 1, 2, 4, 1 },
            },
            [764] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 2, 3, 3, 1, 1, 2, 1 },
                [3] = new[] { 1, 1, 4, 1, 1, 2, 4, 3 },
                [6] = new[] { 1, 5, 2, 1, 1, 1, 3, 3 },
            },
            [765] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 4, 2, 2, 1, 1, 3 },
                [3] = new[] { 2, 1, 4, 1, 1, 2, 5, 1 },
                [6] = new[] { 2, 5, 2, 1, 1, 1, 4, 1 },
            },
            [766] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 4, 2, 2, 1, 2, 1 },
                [3] = new[] { 1, 1, 4, 1, 1, 3, 4, 2 },
                [6] = new[] { 1, 5, 2, 1, 1, 2, 3, 2 },
            },
            [767] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 4, 2, 2, 2, 1, 2 },
                [3] = new[] { 1, 1, 4, 1, 1, 4, 4, 1 },
                [6] = new[] { 1, 5, 2, 1, 1, 3, 3, 1 },
            },
            [768] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 3, 3, 1, 3, 1, 1 },
                [3] = new[] { 6, 2, 3, 2, 1, 1, 1, 1 },
                [6] = new[] { 1, 6, 1, 2, 1, 1, 4, 1 },
            },
            [769] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 4, 2, 2, 3, 1, 1 },
                [3] = new[] { 1, 2, 3, 2, 1, 1, 5, 2 },
                [6] = new[] { 1, 5, 2, 1, 2, 1, 4, 1 },
            },
            [770] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 2, 4, 1, 1, 1, 3 },
                [3] = new[] { 6, 1, 4, 1, 2, 1, 1, 1 },
                [6] = new[] { 1, 4, 3, 1, 1, 1, 2, 4 },
            },
            [771] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 2, 4, 1, 1, 2, 1 },
                [3] = new[] { 1, 1, 4, 1, 2, 1, 5, 2 },
                [6] = new[] { 2, 4, 3, 1, 1, 1, 3, 2 },
            },
            [772] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 3, 3, 2, 1, 1, 3 },
                [3] = new[] { 1, 2, 3, 2, 1, 2, 5, 1 },
                [6] = new[] { 1, 4, 3, 1, 1, 2, 2, 3 },
            },
            [773] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 2, 4, 1, 2, 1, 2 },
                [3] = new[] { 1, 1, 4, 1, 2, 2, 5, 1 },
                [6] = new[] { 2, 4, 3, 1, 1, 2, 3, 1 },
            },
            [774] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 4, 2, 3, 1, 1, 3 },
                [3] = new[] { 5, 3, 2, 3, 1, 1, 1, 1 },
                [6] = new[] { 1, 4, 3, 1, 1, 3, 2, 2 },
            },
            [775] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 3, 3, 2, 2, 1, 2 },
                [3] = new[] { 5, 2, 3, 2, 2, 1, 1, 1 },
                [6] = new[] { 1, 4, 3, 1, 1, 4, 2, 1 },
            },
            [776] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 2, 4, 1, 3, 1, 1 },
                [3] = new[] { 5, 1, 4, 1, 3, 1, 1, 1 },
                [6] = new[] { 1, 5, 2, 2, 1, 1, 3, 2 },
            },
            [777] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 4, 2, 3, 2, 1, 2 },
                [3] = new[] { 4, 4, 1, 4, 1, 1, 1, 1 },
                [6] = new[] { 1, 4, 3, 1, 2, 1, 3, 2 },
            },
            [778] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 3, 3, 2, 3, 1, 1 },
                [3] = new[] { 4, 3, 2, 3, 2, 1, 1, 1 },
                [6] = new[] { 1, 5, 2, 2, 1, 2, 3, 1 },
            },
            [779] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 4, 2, 3, 3, 1, 1 },
                [3] = new[] { 4, 2, 3, 2, 3, 1, 1, 1 },
                [6] = new[] { 1, 4, 3, 1, 2, 2, 3, 1 },
            },
            [780] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 5, 1, 1, 1, 3 },
                [3] = new[] { 4, 1, 4, 1, 4, 1, 1, 1 },
                [6] = new[] { 1, 3, 4, 1, 1, 1, 1, 5 },
            },
            [781] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 4, 1, 5, 1, 1, 2, 1 },
                [3] = new[] { 3, 4, 1, 4, 2, 1, 1, 1 },
                [6] = new[] { 2, 3, 4, 1, 1, 1, 2, 3 },
            },
            [782] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 2, 4, 2, 1, 1, 3 },
                [3] = new[] { 3, 3, 2, 3, 3, 1, 1, 1 },
                [6] = new[] { 3, 3, 4, 1, 1, 1, 3, 1 },
            },
            [783] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 2, 4, 2, 1, 2, 1 },
                [3] = new[] { 3, 2, 3, 2, 4, 1, 1, 1 },
                [6] = new[] { 1, 3, 4, 1, 1, 2, 1, 4 },
            },
            [784] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 3, 3, 3, 1, 1, 3 },
                [3] = new[] { 3, 1, 4, 1, 5, 1, 1, 1 },
                [6] = new[] { 2, 3, 4, 1, 1, 2, 2, 2 },
            },
            [785] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 2, 4, 2, 2, 1, 2 },
                [3] = new[] { 2, 4, 1, 4, 3, 1, 1, 1 },
                [6] = new[] { 1, 3, 4, 1, 1, 3, 1, 3 },
            },
            [786] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 1, 5, 1, 3, 1, 1 },
                [3] = new[] { 2, 3, 2, 3, 4, 1, 1, 1 },
                [6] = new[] { 2, 3, 4, 1, 1, 3, 2, 1 },
            },
            [787] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 4, 2, 4, 1, 1, 3 },
                [3] = new[] { 2, 2, 3, 2, 5, 1, 1, 1 },
                [6] = new[] { 1, 3, 4, 1, 1, 4, 1, 2 },
            },
            [788] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 3, 3, 3, 2, 1, 2 },
                [3] = new[] { 2, 1, 4, 1, 6, 1, 1, 1 },
                [6] = new[] { 1, 3, 4, 1, 1, 5, 1, 1 },
            },
            [789] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 2, 4, 2, 3, 1, 1 },
                [3] = new[] { 1, 4, 1, 4, 4, 1, 1, 1 },
                [6] = new[] { 1, 4, 3, 2, 1, 1, 2, 3 },
            },
            [790] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 4, 2, 4, 2, 1, 2 },
                [3] = new[] { 1, 3, 2, 3, 5, 1, 1, 1 },
                [6] = new[] { 2, 4, 3, 2, 1, 1, 3, 1 },
            },
            [791] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 3, 3, 3, 3, 1, 1 },
                [3] = new[] { 1, 2, 3, 2, 6, 1, 1, 1 },
                [6] = new[] { 1, 3, 4, 1, 2, 1, 2, 3 },
            },
            [792] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 4, 2, 4, 3, 1, 1 },
                [3] = new[] { 1, 1, 4, 2, 1, 1, 4, 3 },
                [6] = new[] { 2, 3, 4, 1, 2, 1, 3, 1 },
            },
            [793] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 2, 4, 3, 1, 2, 1 },
                [3] = new[] { 2, 1, 4, 2, 1, 1, 5, 1 },
                [6] = new[] { 1, 3, 4, 1, 2, 2, 2, 2 },
            },
            [794] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 4, 2, 5, 1, 2, 1 },
                [3] = new[] { 1, 1, 4, 2, 1, 2, 4, 2 },
                [6] = new[] { 1, 4, 3, 2, 1, 3, 2, 1 },
            },
            [795] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 4, 3, 1, 2, 1, 1 },
                [3] = new[] { 1, 1, 4, 2, 1, 3, 4, 1 },
                [6] = new[] { 1, 3, 4, 1, 2, 3, 2, 1 },
            },
            [796] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 4, 3, 2, 1, 1, 2 },
                [3] = new[] { 1, 2, 3, 3, 1, 1, 5, 1 },
                [6] = new[] { 1, 5, 2, 3, 1, 1, 3, 1 },
            },
            [797] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 4, 3, 2, 2, 1, 1 },
                [3] = new[] { 1, 1, 4, 2, 2, 1, 5, 1 },
                [6] = new[] { 1, 4, 3, 2, 2, 1, 3, 1 },
            },
            [798] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 3, 4, 2, 1, 1, 2 },
                [3] = new[] { 1, 1, 4, 3, 1, 1, 4, 2 },
                [6] = new[] { 1, 3, 4, 1, 3, 1, 3, 1 },
            },
            [799] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 4, 3, 3, 1, 1, 2 },
                [3] = new[] { 1, 1, 4, 3, 1, 2, 4, 1 },
                [6] = new[] { 2, 2, 5, 1, 1, 1, 1, 4 },
            },
            [800] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 4, 3, 3, 2, 1, 1 },
                [3] = new[] { 1, 1, 4, 4, 1, 1, 4, 1 },
                [6] = new[] { 3, 2, 5, 1, 1, 1, 2, 2 },
            },
            [801] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 2, 5, 2, 1, 1, 2 },
                [3] = new[] { 4, 5, 1, 1, 1, 1, 1, 3 },
                [6] = new[] { 2, 2, 5, 1, 1, 2, 1, 3 },
            },
            [802] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 3, 4, 3, 1, 1, 2 },
                [3] = new[] { 4, 5, 1, 1, 1, 2, 1, 2 },
                [6] = new[] { 3, 2, 5, 1, 1, 2, 2, 1 },
            },
            [803] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 4, 3, 4, 1, 1, 2 },
                [3] = new[] { 4, 5, 1, 1, 1, 3, 1, 1 },
                [6] = new[] { 2, 2, 5, 1, 1, 3, 1, 2 },
            },
            [804] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 1, 4, 3, 4, 2, 1, 1 },
                [3] = new[] { 3, 5, 1, 1, 2, 1, 1, 3 },
                [6] = new[] { 2, 2, 5, 1, 1, 4, 1, 1 },
            },
            [805] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 5, 1, 1, 1, 1, 1, 6 },
                [3] = new[] { 4, 5, 1, 1, 2, 1, 2, 1 },
                [6] = new[] { 1, 3, 4, 2, 1, 1, 1, 4 },
            },
            [806] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 5, 1, 1, 1, 2, 1, 5 },
                [3] = new[] { 3, 5, 1, 1, 2, 2, 1, 2 },
                [6] = new[] { 2, 3, 4, 2, 1, 1, 2, 2 },
            },
            [807] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 5, 1, 1, 1, 2, 2, 3 },
                [3] = new[] { 3, 5, 1, 1, 2, 3, 1, 1 },
                [6] = new[] { 1, 2, 5, 1, 2, 1, 1, 4 },
            },
            [808] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 5, 1, 1, 1, 3, 1, 4 },
                [3] = new[] { 2, 5, 1, 1, 3, 1, 1, 3 },
                [6] = new[] { 2, 2, 5, 1, 2, 1, 2, 2 },
            },
            [809] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 5, 1, 1, 1, 4, 1, 3 },
                [3] = new[] { 3, 5, 1, 1, 3, 1, 2, 1 },
                [6] = new[] { 2, 3, 4, 2, 1, 2, 2, 1 },
            },
            [810] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 5, 1, 1, 1, 5, 1, 2 },
                [3] = new[] { 2, 5, 1, 1, 3, 2, 1, 2 },
                [6] = new[] { 1, 2, 5, 1, 2, 2, 1, 3 },
            },
            [811] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 5, 1, 1, 2, 1, 2, 4 },
                [3] = new[] { 2, 5, 1, 1, 3, 3, 1, 1 },
                [6] = new[] { 1, 3, 4, 2, 1, 3, 1, 2 },
            },
            [812] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 5, 1, 1, 2, 2, 2, 3 },
                [3] = new[] { 1, 5, 1, 1, 4, 1, 1, 3 },
                [6] = new[] { 1, 2, 5, 1, 2, 3, 1, 2 },
            },
            [813] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 5, 1, 1, 2, 3, 2, 2 },
                [3] = new[] { 2, 5, 1, 1, 4, 1, 2, 1 },
                [6] = new[] { 1, 3, 4, 2, 1, 4, 1, 1 },
            },
            [814] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 5, 1, 1, 2, 4, 2, 1 },
                [3] = new[] { 1, 5, 1, 1, 4, 2, 1, 2 },
                [6] = new[] { 1, 2, 5, 1, 2, 4, 1, 1 },
            },
            [815] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 5, 1, 1, 3, 1, 3, 2 },
                [3] = new[] { 1, 5, 1, 1, 4, 3, 1, 1 },
                [6] = new[] { 1, 4, 3, 3, 1, 1, 2, 2 },
            },
            [816] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 5, 1, 1, 3, 2, 3, 1 },
                [3] = new[] { 1, 5, 1, 1, 5, 1, 2, 1 },
                [6] = new[] { 1, 3, 4, 2, 2, 1, 2, 2 },
            },
            [817] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 4, 2, 1, 1, 1, 1, 5 },
                [3] = new[] { 5, 4, 2, 1, 1, 1, 1, 2 },
                [6] = new[] { 1, 4, 3, 3, 1, 2, 2, 1 },
            },
            [818] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 4, 2, 1, 1, 2, 1, 4 },
                [3] = new[] { 1, 4, 2, 1, 1, 1, 6, 1 },
                [6] = new[] { 1, 2, 5, 1, 3, 1, 2, 2 },
            },
            [819] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 4, 2, 1, 1, 2, 2, 2 },
                [3] = new[] { 5, 4, 2, 1, 1, 2, 1, 1 },
                [6] = new[] { 1, 3, 4, 2, 2, 2, 2, 1 },
            },
            [820] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 4, 2, 1, 1, 3, 1, 3 },
                [3] = new[] { 4, 5, 1, 2, 1, 1, 1, 2 },
                [6] = new[] { 1, 2, 5, 1, 3, 2, 2, 1 },
            },
            [821] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 4, 2, 1, 1, 3, 2, 1 },
                [3] = new[] { 4, 4, 2, 1, 2, 1, 1, 2 },
                [6] = new[] { 3, 1, 6, 1, 1, 1, 1, 3 },
            },
            [822] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 4, 2, 1, 1, 4, 1, 2 },
                [3] = new[] { 4, 5, 1, 2, 1, 2, 1, 1 },
                [6] = new[] { 4, 1, 6, 1, 1, 1, 2, 1 },
            },
            [823] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 4, 2, 1, 1, 5, 1, 1 },
                [3] = new[] { 4, 4, 2, 1, 2, 2, 1, 1 },
                [6] = new[] { 3, 1, 6, 1, 1, 2, 1, 2 },
            },
            [824] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 5, 1, 2, 1, 1, 1, 5 },
                [3] = new[] { 3, 5, 1, 2, 2, 1, 1, 2 },
                [6] = new[] { 3, 1, 6, 1, 1, 3, 1, 1 },
            },
            [825] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 5, 1, 2, 1, 1, 2, 3 },
                [3] = new[] { 3, 4, 2, 1, 3, 1, 1, 2 },
                [6] = new[] { 2, 2, 5, 2, 1, 1, 1, 3 },
            },
            [826] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 2, 1, 2, 1, 1, 5 },
                [3] = new[] { 3, 5, 1, 2, 2, 2, 1, 1 },
                [6] = new[] { 3, 2, 5, 2, 1, 1, 2, 1 },
            },
            [827] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 4, 2, 1, 2, 1, 2, 3 },
                [3] = new[] { 3, 4, 2, 1, 3, 2, 1, 1 },
                [6] = new[] { 2, 1, 6, 1, 2, 1, 1, 3 },
            },
            [828] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 5, 1, 2, 1, 2, 2, 2 },
                [3] = new[] { 2, 5, 1, 2, 3, 1, 1, 2 },
                [6] = new[] { 2, 2, 5, 2, 1, 2, 1, 2 },
            },
            [829] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 2, 1, 2, 2, 1, 4 },
                [3] = new[] { 2, 4, 2, 1, 4, 1, 1, 2 },
                [6] = new[] { 2, 1, 6, 1, 2, 2, 1, 2 },
            },
            [830] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 4, 2, 1, 2, 2, 2, 2 },
                [3] = new[] { 2, 5, 1, 2, 3, 2, 1, 1 },
                [6] = new[] { 2, 2, 5, 2, 1, 3, 1, 1 },
            },
            [831] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 2, 1, 2, 3, 1, 3 },
                [3] = new[] { 2, 4, 2, 1, 4, 2, 1, 1 },
                [6] = new[] { 2, 1, 6, 1, 2, 3, 1, 1 },
            },
            [832] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 4, 2, 1, 2, 3, 2, 1 },
                [3] = new[] { 1, 5, 1, 2, 4, 1, 1, 2 },
                [6] = new[] { 1, 3, 4, 3, 1, 1, 1, 3 },
            },
            [833] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 2, 1, 2, 4, 1, 2 },
                [3] = new[] { 1, 4, 2, 1, 5, 1, 1, 2 },
                [6] = new[] { 2, 3, 4, 3, 1, 1, 2, 1 },
            },
            [834] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 5, 1, 2, 1, 5, 1, 1 },
                [3] = new[] { 1, 5, 1, 2, 4, 2, 1, 1 },
                [6] = new[] { 1, 2, 5, 2, 2, 1, 1, 3 },
            },
            [835] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 2, 1, 2, 5, 1, 1 },
                [3] = new[] { 1, 4, 2, 1, 5, 2, 1, 1 },
                [6] = new[] { 1, 3, 4, 3, 1, 2, 1, 2 },
            },
            [836] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 5, 1, 2, 2, 1, 2, 3 },
                [3] = new[] { 6, 3, 3, 1, 1, 1, 1, 1 },
                [6] = new[] { 1, 1, 6, 1, 3, 1, 1, 3 },
            },
            [837] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 5, 1, 2, 2, 1, 3, 1 },
                [3] = new[] { 1, 3, 3, 1, 1, 1, 5, 2 },
                [6] = new[] { 1, 2, 5, 2, 2, 2, 1, 2 },
            },
            [838] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 2, 1, 3, 1, 2, 3 },
                [3] = new[] { 1, 3, 3, 1, 1, 2, 5, 1 },
                [6] = new[] { 1, 3, 4, 3, 1, 3, 1, 1 },
            },
            [839] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 4, 2, 1, 3, 1, 3, 1 },
                [3] = new[] { 5, 4, 2, 2, 1, 1, 1, 1 },
                [6] = new[] { 1, 1, 6, 1, 3, 2, 1, 2 },
            },
            [840] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 2, 1, 3, 2, 2, 2 },
                [3] = new[] { 5, 3, 3, 1, 2, 1, 1, 1 },
                [6] = new[] { 1, 2, 5, 2, 2, 3, 1, 1 },
            },
            [841] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 5, 1, 2, 2, 3, 2, 1 },
                [3] = new[] { 4, 5, 1, 3, 1, 1, 1, 1 },
                [6] = new[] { 1, 1, 6, 1, 3, 3, 1, 1 },
            },
            [842] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 2, 1, 3, 3, 2, 1 },
                [3] = new[] { 4, 4, 2, 2, 2, 1, 1, 1 },
                [6] = new[] { 1, 4, 3, 4, 1, 1, 2, 1 },
            },
            [843] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 5, 1, 2, 3, 1, 3, 1 },
                [3] = new[] { 4, 3, 3, 1, 3, 1, 1, 1 },
                [6] = new[] { 1, 3, 4, 3, 2, 1, 2, 1 },
            },
            [844] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 2, 1, 4, 1, 3, 1 },
                [3] = new[] { 3, 5, 1, 3, 2, 1, 1, 1 },
                [6] = new[] { 1, 2, 5, 2, 3, 1, 2, 1 },
            },
            [845] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 3, 1, 1, 1, 1, 4 },
                [3] = new[] { 3, 4, 2, 2, 3, 1, 1, 1 },
                [6] = new[] { 1, 1, 6, 1, 4, 1, 2, 1 },
            },
            [846] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 3, 1, 1, 2, 1, 3 },
                [3] = new[] { 3, 3, 3, 1, 4, 1, 1, 1 },
                [6] = new[] { 3, 1, 6, 2, 1, 1, 1, 2 },
            },
            [847] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 3, 1, 1, 3, 1, 2 },
                [3] = new[] { 2, 5, 1, 3, 3, 1, 1, 1 },
                [6] = new[] { 3, 1, 6, 2, 1, 2, 1, 1 },
            },
            [848] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 3, 1, 1, 4, 1, 1 },
                [3] = new[] { 2, 4, 2, 2, 4, 1, 1, 1 },
                [6] = new[] { 2, 2, 5, 3, 1, 1, 1, 2 },
            },
            [849] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 4, 2, 2, 1, 1, 1, 4 },
                [3] = new[] { 2, 3, 3, 1, 5, 1, 1, 1 },
                [6] = new[] { 2, 1, 6, 2, 2, 1, 1, 2 },
            },
            [850] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 3, 1, 2, 1, 1, 4 },
                [3] = new[] { 1, 5, 1, 3, 4, 1, 1, 1 },
                [6] = new[] { 2, 2, 5, 3, 1, 2, 1, 1 },
            },
            [851] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 3, 1, 2, 1, 2, 2 },
                [3] = new[] { 1, 4, 2, 2, 5, 1, 1, 1 },
                [6] = new[] { 2, 1, 6, 2, 2, 2, 1, 1 },
            },
            [852] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 4, 2, 2, 1, 2, 2, 1 },
                [3] = new[] { 1, 3, 3, 1, 6, 1, 1, 1 },
                [6] = new[] { 1, 3, 4, 4, 1, 1, 1, 2 },
            },
            [853] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 3, 1, 2, 2, 1, 3 },
                [3] = new[] { 1, 2, 4, 1, 1, 1, 4, 3 },
                [6] = new[] { 1, 2, 5, 3, 2, 1, 1, 2 },
            },
            [854] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 3, 1, 2, 2, 2, 1 },
                [3] = new[] { 2, 2, 4, 1, 1, 1, 5, 1 },
                [6] = new[] { 1, 3, 4, 4, 1, 2, 1, 1 },
            },
            [855] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 3, 1, 2, 3, 1, 2 },
                [3] = new[] { 1, 2, 4, 1, 1, 2, 4, 2 },
                [6] = new[] { 1, 1, 6, 2, 3, 1, 1, 2 },
            },
            [856] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 4, 2, 2, 1, 4, 1, 1 },
                [3] = new[] { 1, 2, 4, 1, 1, 3, 4, 1 },
                [6] = new[] { 1, 2, 5, 3, 2, 2, 1, 1 },
            },
            [857] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 3, 1, 2, 4, 1, 1 },
                [3] = new[] { 1, 3, 3, 2, 1, 1, 5, 1 },
                [6] = new[] { 1, 1, 6, 2, 3, 2, 1, 1 },
            },
            [858] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 5, 1, 3, 1, 1, 1, 4 },
                [3] = new[] { 1, 2, 4, 1, 2, 1, 5, 1 },
                [6] = new[] { 3, 1, 6, 3, 1, 1, 1, 1 },
            },
            [859] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 2, 2, 2, 1, 1, 4 },
                [3] = new[] { 1, 1, 5, 1, 1, 1, 3, 4 },
                [6] = new[] { 2, 2, 5, 4, 1, 1, 1, 1 },
            },
            [860] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 5, 1, 3, 1, 2, 1, 3 },
                [3] = new[] { 2, 1, 5, 1, 1, 1, 4, 2 },
                [6] = new[] { 2, 1, 6, 3, 2, 1, 1, 1 },
            },
            [861] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 5, 1, 3, 1, 2, 2, 1 },
                [3] = new[] { 1, 1, 5, 1, 1, 2, 3, 3 },
                [6] = new[] { 1, 3, 4, 5, 1, 1, 1, 1 },
            },
            [862] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 3, 1, 3, 1, 1, 4 },
                [3] = new[] { 2, 1, 5, 1, 1, 2, 4, 1 },
                [6] = new[] { 1, 2, 5, 4, 2, 1, 1, 1 },
            },
            [863] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 2, 2, 2, 2, 1, 3 },
                [3] = new[] { 1, 1, 5, 1, 1, 3, 3, 2 },
                [6] = new[] { 1, 1, 6, 3, 3, 1, 1, 1 },
            },
            [864] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 5, 1, 3, 1, 3, 1, 2 },
                [3] = new[] { 1, 1, 5, 1, 1, 4, 3, 1 },
                [6] = new[] { 1, 6, 2, 1, 1, 1, 3, 2 },
            },
            [865] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 3, 1, 3, 2, 1, 3 },
                [3] = new[] { 1, 2, 4, 2, 1, 1, 4, 2 },
                [6] = new[] { 1, 6, 2, 1, 1, 2, 3, 1 },
            },
            [866] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 2, 2, 2, 3, 1, 2 },
                [3] = new[] { 1, 1, 5, 1, 2, 1, 4, 2 },
                [6] = new[] { 1, 5, 3, 1, 1, 1, 2, 3 },
            },
            [867] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 5, 1, 3, 1, 4, 1, 1 },
                [3] = new[] { 1, 2, 4, 2, 1, 2, 4, 1 },
                [6] = new[] { 2, 5, 3, 1, 1, 1, 3, 1 },
            },
            [868] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 3, 1, 3, 3, 1, 2 },
                [3] = new[] { 1, 1, 5, 1, 2, 2, 4, 1 },
                [6] = new[] { 1, 5, 3, 1, 1, 2, 2, 2 },
            },
            [869] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 2, 2, 2, 4, 1, 1 },
                [3] = new[] { 1, 1, 5, 2, 1, 1, 3, 3 },
                [6] = new[] { 1, 5, 3, 1, 1, 3, 2, 1 },
            },
            [870] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 5, 1, 3, 2, 1, 2, 2 },
                [3] = new[] { 2, 1, 5, 2, 1, 1, 4, 1 },
                [6] = new[] { 1, 6, 2, 2, 1, 1, 3, 1 },
            },
            [871] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 2, 2, 3, 1, 2, 2 },
                [3] = new[] { 1, 1, 5, 2, 1, 2, 3, 2 },
                [6] = new[] { 1, 5, 3, 1, 2, 1, 3, 1 },
            },
            [872] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 5, 1, 3, 2, 2, 2, 1 },
                [3] = new[] { 1, 1, 5, 2, 1, 3, 3, 1 },
                [6] = new[] { 1, 4, 4, 1, 1, 1, 1, 4 },
            },
            [873] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 3, 1, 4, 1, 2, 2 },
                [3] = new[] { 1, 2, 4, 3, 1, 1, 4, 1 },
                [6] = new[] { 2, 4, 4, 1, 1, 1, 2, 2 },
            },
            [874] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 2, 2, 3, 2, 2, 1 },
                [3] = new[] { 1, 1, 5, 2, 2, 1, 4, 1 },
                [6] = new[] { 1, 4, 4, 1, 1, 2, 1, 3 },
            },
            [875] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 3, 1, 4, 2, 2, 1 },
                [3] = new[] { 1, 1, 5, 3, 1, 1, 3, 2 },
                [6] = new[] { 2, 4, 4, 1, 1, 2, 2, 1 },
            },
            [876] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 2, 4, 1, 1, 1, 1, 3 },
                [3] = new[] { 1, 1, 5, 3, 1, 2, 3, 1 },
                [6] = new[] { 1, 4, 4, 1, 1, 3, 1, 2 },
            },
            [877] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 2, 4, 1, 1, 2, 1, 2 },
                [3] = new[] { 1, 1, 5, 4, 1, 1, 3, 1 },
                [6] = new[] { 1, 4, 4, 1, 1, 4, 1, 1 },
            },
            [878] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 2, 4, 1, 1, 3, 1, 1 },
                [3] = new[] { 3, 6, 1, 1, 2, 1, 1, 2 },
                [6] = new[] { 1, 5, 3, 2, 1, 1, 2, 2 },
            },
            [879] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 3, 2, 1, 1, 1, 3 },
                [3] = new[] { 3, 6, 1, 1, 2, 2, 1, 1 },
                [6] = new[] { 1, 4, 4, 1, 2, 1, 2, 2 },
            },
            [880] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 4, 1, 2, 1, 1, 3 },
                [3] = new[] { 2, 6, 1, 1, 3, 1, 1, 2 },
                [6] = new[] { 1, 5, 3, 2, 1, 2, 2, 1 },
            },
            [881] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 2, 4, 1, 2, 1, 2, 1 },
                [3] = new[] { 2, 6, 1, 1, 3, 2, 1, 1 },
                [6] = new[] { 1, 4, 4, 1, 2, 2, 2, 1 },
            },
            [882] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 4, 1, 2, 2, 1, 2 },
                [3] = new[] { 1, 6, 1, 1, 4, 1, 1, 2 },
                [6] = new[] { 2, 3, 5, 1, 1, 1, 1, 3 },
            },
            [883] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 3, 2, 1, 3, 1, 1 },
                [3] = new[] { 1, 6, 1, 1, 4, 2, 1, 1 },
                [6] = new[] { 3, 3, 5, 1, 1, 1, 2, 1 },
            },
            [884] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 4, 1, 2, 3, 1, 1 },
                [3] = new[] { 4, 5, 2, 1, 2, 1, 1, 1 },
                [6] = new[] { 2, 3, 5, 1, 1, 2, 1, 2 },
            },
            [885] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 4, 2, 3, 1, 1, 1, 3 },
                [3] = new[] { 3, 6, 1, 2, 2, 1, 1, 1 },
                [6] = new[] { 2, 3, 5, 1, 1, 3, 1, 1 },
            },
            [886] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 4, 2, 3, 1, 1, 2, 1 },
                [3] = new[] { 3, 5, 2, 1, 3, 1, 1, 1 },
                [6] = new[] { 1, 4, 4, 2, 1, 1, 1, 3 },
            },
            [887] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 3, 2, 2, 1, 1, 3 },
                [3] = new[] { 2, 6, 1, 2, 3, 1, 1, 1 },
                [6] = new[] { 2, 4, 4, 2, 1, 1, 2, 1 },
            },
            [888] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 3, 2, 2, 1, 2, 1 },
                [3] = new[] { 2, 5, 2, 1, 4, 1, 1, 1 },
                [6] = new[] { 1, 3, 5, 1, 2, 1, 1, 3 },
            },
            [889] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 4, 1, 3, 1, 1, 3 },
                [3] = new[] { 1, 6, 1, 2, 4, 1, 1, 1 },
                [6] = new[] { 2, 3, 5, 1, 2, 1, 2, 1 },
            },
            [890] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 3, 2, 2, 2, 1, 2 },
                [3] = new[] { 1, 5, 2, 1, 5, 1, 1, 1 },
                [6] = new[] { 1, 3, 5, 1, 2, 2, 1, 2 },
            },
            [891] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 4, 2, 3, 1, 3, 1, 1 },
                [3] = new[] { 1, 4, 3, 1, 1, 1, 5, 1 },
                [6] = new[] { 1, 4, 4, 2, 1, 3, 1, 1 },
            },
            [892] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 4, 1, 3, 2, 1, 2 },
                [3] = new[] { 1, 3, 4, 1, 1, 1, 4, 2 },
                [6] = new[] { 1, 3, 5, 1, 2, 3, 1, 1 },
            },
            [893] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 3, 2, 2, 3, 1, 1 },
                [3] = new[] { 1, 3, 4, 1, 1, 2, 4, 1 },
                [6] = new[] { 1, 5, 3, 3, 1, 1, 2, 1 },
            },
            [894] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 4, 1, 3, 3, 1, 1 },
                [3] = new[] { 1, 2, 5, 1, 1, 1, 3, 3 },
                [6] = new[] { 1, 4, 4, 2, 2, 1, 2, 1 },
            },
            [895] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 5, 1, 4, 1, 1, 1, 3 },
                [3] = new[] { 2, 2, 5, 1, 1, 1, 4, 1 },
                [6] = new[] { 1, 3, 5, 1, 3, 1, 2, 1 },
            },
            [896] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 5, 1, 4, 1, 1, 2, 1 },
                [3] = new[] { 1, 2, 5, 1, 1, 2, 3, 2 },
                [6] = new[] { 3, 2, 6, 1, 1, 1, 1, 2 },
            },
            [897] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 2, 3, 2, 1, 1, 3 },
                [3] = new[] { 1, 2, 5, 1, 1, 3, 3, 1 },
                [6] = new[] { 3, 2, 6, 1, 1, 2, 1, 1 },
            },
            [898] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 4, 2, 3, 2, 1, 2, 1 },
                [3] = new[] { 1, 3, 4, 2, 1, 1, 4, 1 },
                [6] = new[] { 2, 3, 5, 2, 1, 1, 1, 2 },
            },
            [899] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 3, 2, 3, 1, 1, 3 },
                [3] = new[] { 1, 2, 5, 1, 2, 1, 4, 1 },
                [6] = new[] { 2, 2, 6, 1, 2, 1, 1, 2 },
            },
            [900] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 2, 3, 2, 2, 1, 2 },
                [3] = new[] { 1, 1, 6, 1, 1, 1, 2, 4 },
                [6] = new[] { 2, 3, 5, 2, 1, 2, 1, 1 },
            },
            [901] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 5, 1, 4, 1, 3, 1, 1 },
                [3] = new[] { 2, 1, 6, 1, 1, 1, 3, 2 },
                [6] = new[] { 2, 2, 6, 1, 2, 2, 1, 1 },
            },
            [902] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 4, 1, 4, 1, 1, 3 },
                [3] = new[] { 1, 1, 6, 1, 1, 2, 2, 3 },
                [6] = new[] { 1, 4, 4, 3, 1, 1, 1, 2 },
            },
            [903] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 3, 2, 3, 2, 1, 2 },
                [3] = new[] { 2, 1, 6, 1, 1, 2, 3, 1 },
                [6] = new[] { 1, 3, 5, 2, 2, 1, 1, 2 },
            },
            [904] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 2, 3, 2, 3, 1, 1 },
                [3] = new[] { 1, 1, 6, 1, 1, 3, 2, 2 },
                [6] = new[] { 1, 4, 4, 3, 1, 2, 1, 1 },
            },
            [905] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 4, 1, 4, 2, 1, 2 },
                [3] = new[] { 1, 1, 6, 1, 1, 4, 2, 1 },
                [6] = new[] { 1, 2, 6, 1, 3, 1, 1, 2 },
            },
            [906] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 3, 2, 3, 3, 1, 1 },
                [3] = new[] { 1, 2, 5, 2, 1, 1, 3, 2 },
                [6] = new[] { 1, 3, 5, 2, 2, 2, 1, 1 },
            },
            [907] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 5, 1, 4, 2, 1, 2, 1 },
                [3] = new[] { 1, 1, 6, 1, 2, 1, 3, 2 },
                [6] = new[] { 1, 2, 6, 1, 3, 2, 1, 1 },
            },
            [908] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 4, 2, 3, 3, 1, 2, 1 },
                [3] = new[] { 1, 2, 5, 2, 1, 2, 3, 1 },
                [6] = new[] { 3, 2, 6, 2, 1, 1, 1, 1 },
            },
            [909] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 3, 3, 2, 4, 1, 2, 1 },
                [3] = new[] { 1, 1, 6, 1, 2, 2, 3, 1 },
                [6] = new[] { 2, 3, 5, 3, 1, 1, 1, 1 },
            },
            [910] = new Dictionary<int, int[]>
            {
                [0] = new[] { 1, 2, 4, 1, 5, 1, 2, 1 },
                [3] = new[] { 1, 1, 6, 2, 1, 1, 2, 3 },
                [6] = new[] { 2, 2, 6, 2, 2, 1, 1, 1 },
            },
            [911] = new Dictionary<int, int[]>
            {
                [0] = new[] { 5, 1, 5, 1, 1, 1, 1, 2 },
                [3] = new[] { 2, 1, 6, 2, 1, 1, 3, 1 },
                [6] = new[] { 1, 4, 4, 4, 1, 1, 1, 1 },
            },
            [912] = new Dictionary<int, int[]>
            {
                [0] = new[] { 5, 1, 5, 1, 1, 2, 1, 1 },
                [3] = new[] { 1, 1, 6, 2, 1, 2, 2, 2 },
                [6] = new[] { 1, 3, 5, 3, 2, 1, 1, 1 },
            },
            [913] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 2, 4, 2, 1, 1, 1, 2 },
                [3] = new[] { 1, 1, 6, 2, 1, 3, 2, 1 },
                [6] = new[] { 1, 2, 6, 2, 3, 1, 1, 1 },
            },
            [914] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 5, 1, 2, 1, 1, 2 },
                [3] = new[] { 1, 2, 5, 3, 1, 1, 3, 1 },
                [6] = new[] { 1, 6, 3, 1, 1, 1, 2, 2 },
            },
            [915] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 2, 4, 2, 1, 2, 1, 1 },
                [3] = new[] { 1, 1, 6, 2, 2, 1, 3, 1 },
                [6] = new[] { 1, 6, 3, 1, 1, 2, 2, 1 },
            },
            [916] = new Dictionary<int, int[]>
            {
                [0] = new[] { 4, 1, 5, 1, 2, 2, 1, 1 },
                [3] = new[] { 1, 1, 6, 3, 1, 1, 2, 2 },
                [6] = new[] { 1, 5, 4, 1, 1, 1, 1, 3 },
            },
            [917] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 3, 3, 1, 1, 1, 2 },
                [3] = new[] { 1, 1, 6, 3, 1, 2, 2, 1 },
                [6] = new[] { 2, 5, 4, 1, 1, 1, 2, 1 },
            },
            [918] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 4, 2, 2, 1, 1, 2 },
                [3] = new[] { 1, 4, 4, 1, 1, 1, 4, 1 },
                [6] = new[] { 1, 5, 4, 1, 1, 2, 1, 2 },
            },
            [919] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 3, 3, 3, 1, 2, 1, 1 },
                [3] = new[] { 1, 3, 5, 1, 1, 1, 3, 2 },
                [6] = new[] { 1, 5, 4, 1, 1, 3, 1, 1 },
            },
            [920] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 5, 1, 3, 1, 1, 2 },
                [3] = new[] { 1, 3, 5, 1, 1, 2, 3, 1 },
                [6] = new[] { 1, 6, 3, 2, 1, 1, 2, 1 },
            },
            [921] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 2, 4, 2, 2, 2, 1, 1 },
                [3] = new[] { 1, 2, 6, 1, 1, 1, 2, 3 },
                [6] = new[] { 1, 5, 4, 1, 2, 1, 2, 1 },
            },
            [922] = new Dictionary<int, int[]>
            {
                [0] = new[] { 3, 1, 5, 1, 3, 2, 1, 1 },
                [3] = new[] { 2, 2, 6, 1, 1, 1, 3, 1 },
                [6] = new[] { 2, 4, 5, 1, 1, 1, 1, 2 },
            },
            [923] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 4, 2, 4, 1, 1, 1, 2 },
                [3] = new[] { 1, 2, 6, 1, 1, 2, 2, 2 },
                [6] = new[] { 2, 4, 5, 1, 1, 2, 1, 1 },
            },
            [924] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 3, 3, 2, 1, 1, 2 },
                [3] = new[] { 1, 2, 6, 1, 1, 3, 2, 1 },
                [6] = new[] { 1, 5, 4, 2, 1, 1, 1, 2 },
            },
            [925] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 4, 2, 4, 1, 2, 1, 1 },
                [3] = new[] { 1, 3, 5, 2, 1, 1, 3, 1 },
                [6] = new[] { 1, 4, 5, 1, 2, 1, 1, 2 },
            },
            [926] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 2, 4, 2, 3, 1, 1, 2 },
                [3] = new[] { 1, 2, 6, 1, 2, 1, 3, 1 },
                [6] = new[] { 1, 5, 4, 2, 1, 2, 1, 1 },
            },
            [927] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 3, 3, 3, 2, 2, 1, 1 },
                [3] = new[] { 1, 2, 6, 2, 1, 1, 2, 2 },
                [6] = new[] { 1, 4, 5, 1, 2, 2, 1, 1 },
            },
            [928] = new Dictionary<int, int[]>
            {
                [0] = new[] { 2, 1, 5, 1, 4, 1, 1, 2 },
                [3] = new[] { 1, 2, 6, 2, 1, 2, 2, 1 },
                [6] = new[] { 3, 3, 6, 1, 1, 1, 1, 1 },
            },
        };

    /// <summary>
    /// Retrieves the integer array associated with the specified index and cluster from the Patterns collection.
    /// </summary>
    /// <param name="index">The zero-based index identifying the pattern group to access.</param>
    /// <param name="cluster">The zero-based cluster index within the specified pattern group.</param>
    /// <returns>An array of integers corresponding to the specified index and cluster in the Patterns collection.</returns>
    public static int[] Get(int index, int cluster)
    {
        return Patterns[index][cluster];
    }
}
