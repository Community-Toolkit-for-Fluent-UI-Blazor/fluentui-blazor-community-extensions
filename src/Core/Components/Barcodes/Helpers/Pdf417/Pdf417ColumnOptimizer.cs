namespace FluentUI.Blazor.Community.Components.Helpers.Pdf417;

/// <summary>
/// Provides utility methods for determining the optimal number of columns and rows for arranging codewords in a PDF417
/// barcode.
/// </summary>
/// <remarks>This class is intended for internal use in PDF417 barcode layout calculations. The methods help
/// balance the barcode's aspect ratio by selecting appropriate column and row counts based on the total number of
/// codewords.</remarks>
internal static class Pdf417ColumnOptimizer
{
    /// <summary>
    /// Calculates the recommended number of columns for arranging codewords in a matrix based on the total number of
    /// codewords.
    /// </summary>
    /// <remarks>This method uses a 3:1 aspect ratio heuristic to determine the optimal column count for a
    /// matrix layout. The result is rounded to the nearest integer and then clamped to the valid range.</remarks>
    /// <param name="totalCodewords">The total number of codewords to be arranged. Must be a non-negative integer.</param>
    /// <returns>The number of columns to use for the matrix layout. The value is constrained to be between 2 and 30, inclusive.</returns>
    public static int ChooseColumns(int totalCodewords)
    {
        // Ratio 3:1 → width ≈ sqrt(totalCW * 3)
        var cols = (int)Math.Round(Math.Sqrt(totalCodewords * 3.0));

        if (cols < 2)
        {
            cols = 2;
        }

        if (cols > 30)
        {
            cols = 30;
        }

        return cols;
    }

    /// <summary>
    /// Calculates the minimum number of rows required to arrange the specified number of codewords into the given
    /// number of columns.
    /// </summary>
    /// <param name="totalCodewords">The total number of codewords to be distributed across the rows and columns. Must be non-negative.</param>
    /// <param name="columns">The number of columns to use when arranging the codewords. Must be greater than zero.</param>
    /// <returns>The minimum number of rows needed to fit all codewords into the specified number of columns.</returns>
    public static int ChooseRows(int totalCodewords, double columns)
    {
        return (int)Math.Ceiling(totalCodewords / columns);
    }
}
