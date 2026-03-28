namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the grid configuration options for MicroPDF417 barcodes, including supported row and column counts and
/// their corresponding maximum codeword capacities.
/// </summary>
/// <remarks>This class provides predefined grid layouts used in the encoding and decoding of MicroPDF417
/// barcodes. Each configuration specifies the number of rows, columns, and the maximum number of codewords that can be
/// encoded. These configurations are based on the MicroPDF417 specification and are essential for selecting the
/// appropriate grid size during barcode generation or interpretation.</remarks>
internal class MicroPdf417Grid
{
    /// <summary>
    /// Provides a predefined set of microgrid configurations, each specifying the number of rows, columns, and the
    /// maximum codeword value.
    /// </summary>
    /// <remarks>Each tuple in the array represents a microgrid configuration with the format (Rows, Cols,
    /// MaxCW). These configurations can be used to select appropriate grid layouts or encoding parameters based on
    /// application requirements.</remarks>
    private static readonly (int Rows, int Cols, int MaxCW)[] MicroGrids =
    [
        ( 4, 11,  20),
        ( 4, 13,  26),
        ( 4, 15,  32),
        ( 4, 17,  38),
        ( 4, 19,  44),
        ( 6, 11,  32),
        ( 6, 13,  40),
        ( 6, 15,  48),
        ( 6, 17,  56),
        ( 6, 19,  64),
        ( 8, 11,  44),
        ( 8, 13,  54),
        ( 8, 15,  64),
        ( 8, 17,  74),
        ( 8, 19,  84),
        (10, 11,  56),
        (10, 13,  68),
        (10, 15,  80),
        (10, 17,  92),
        (10, 19, 104),
        (12, 11,  68),
        (12, 13,  82),
        (12, 15,  96),
        (12, 17, 110),
        (12, 19, 124)
    ];

    /// <summary>
    /// Selects the optimal MicroPDF417 grid dimensions to accommodate the specified number of codewords.
    /// </summary>
    /// <remarks>The method selects the smallest grid that can contain the specified codewords, preferring
    /// grids with a column-to-row ratio closest to 3.0 for optimal symbol shape.</remarks>
    /// <param name="totalCodewords">The total number of codewords to encode in the MicroPDF417 symbol. Must be less than or equal to the maximum
    /// capacity of available grid configurations.</param>
    /// <returns>A tuple containing the number of rows and columns for the selected MicroPDF417 grid that best fits the specified
    /// codeword count.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the specified number of codewords exceeds the maximum capacity supported by any available
    /// MicroPDF417 grid configuration.</exception>
    public static (int Rows, int Cols) AutoSelectMicroGrid(int totalCodewords)
    {
        const double targetRatio = 3.0;

        var candidates = MicroGrids
            .Where(g => totalCodewords <= g.MaxCW)
            .OrderBy(g => g.MaxCW) // plus petite capacité suffisante
            .ThenBy(g =>
            {
                var ratio = (double)g.Cols / g.Rows;

                return Math.Abs(ratio - targetRatio);
            })
            .ToList();

        if (candidates.Count == 0)
        {
            throw new InvalidOperationException(
                $"The data ({totalCodewords} codewords) are greater than the maximal capacity of the MicroPDF417.");
        }

        var (Rows, Cols, MaxCW) = candidates[0];

        return (Rows, Cols);
    }
}
