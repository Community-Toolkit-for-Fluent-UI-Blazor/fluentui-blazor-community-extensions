using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Builders.Pdf417;

/// <summary>
/// Provides a base class for building the matrix representation of a PDF417 or MicroPDF417 barcode from a sequence of
/// codewords.
/// </summary>
/// <remarks>This abstract class defines the core structure and extensibility points for generating the barcode
/// matrix, including indicator codewords and compactness options. Derived classes should implement the indicator logic
/// and specify barcode characteristics such as start and stop codewords. This class is intended for internal use in
/// barcode encoding workflows.</remarks>
internal abstract class Pdf417MatrixBuilderBase
{
    /// <summary>
    /// Gets the codeword value that marks the start of the encoded data sequence for the implementing barcode
    /// symbology.
    /// </summary>
    /// <remarks>The start codeword is specific to each barcode format and is used to indicate the beginning
    /// of the data payload during encoding and decoding operations.</remarks>
    protected abstract int[] StartCodeword { get; }

    /// <summary>
    /// Gets the integer value that represents the stop codeword for the encoding process.
    /// </summary>
    /// <remarks>The stop codeword is used to indicate the end of encoded data in derived implementations. The
    /// specific value and its interpretation depend on the encoding scheme implemented by the subclass.</remarks>
    protected abstract int[] StopCodeword { get; }

    /// <summary>
    /// Gets a value indicating whether compact indicators are used in the component.
    /// </summary>
    protected abstract bool UseCompactIndicators { get; }

    /// <summary>
    /// Gets a value indicating whether the current implementation represents a micro variant.
    /// </summary>
    protected abstract bool IsMicro { get; }

    /// <summary>
    /// Builds a two-dimensional matrix representing the encoded PDF417 barcode from the specified codewords and layout
    /// parameters.
    /// </summary>
    /// <remarks>If the number of codewords is less than the total required for the specified matrix size,
    /// padding codewords are used to fill the remaining space. The resulting matrix includes start/stop patterns and
    /// row indicators as required by the PDF417 specification.</remarks>
    /// <param name="codewords">The sequence of codewords to encode into the matrix. The list must not be null and should contain the data and
    /// error correction codewords for the barcode.</param>
    /// <param name="columns">The number of data columns in the barcode matrix. Must be a positive integer.</param>
    /// <param name="rows">The number of rows in the barcode matrix. Must be a positive integer.</param>
    /// <param name="correctionLevel">The error correction level to apply when building the matrix, which may influence the row indicators and overall structure.</param>
    /// <returns>A jagged array of integers where each inner array represents a row of the barcode matrix, with each element
    /// corresponding to a module pattern.</returns>
    public int[][] BuildMatrix(
        int[] codewords,
        int columns,
        int rows,
        PDF417ErrorCorrectionLevel correctionLevel)
    {
        var matrix = new int[rows][];

        for (var r = 0; r < rows; r++)
        {
            var cluster = (r % 3) * 3;

            var line = new List<int[]>
            {
                StartCodeword,
                Pdf417CodewordPatterns.Get(GetLeftIndicator(r, rows, columns, correctionLevel, cluster), cluster)
            };

            for (var c = 0; c < columns; c++)
            {
                var index = r * columns + c;
                var cw = codewords[index];
                line.Add(Pdf417CodewordPatterns.Get(cw, cluster));
            }

            line.Add(Pdf417CodewordPatterns.Get(GetRightIndicator(r, rows, columns, correctionLevel, cluster), cluster));
            line.Add(StopCodeword);

            matrix[r] = Flatten(line);
        }

        return matrix;
    }

    /// <summary>
    /// Calculates the left indicator value for a specified cell within a grid layout.
    /// </summary>
    /// <param name="row">The zero-based index of the row for which to calculate the left indicator.</param>
    /// <param name="rows">The total number of rows in the grid. Must be greater than zero.</param>
    /// <param name="cols">The total number of columns in the grid. Must be greater than zero.</param>
    /// <param name="correctionLevel">The error correction level to consider when calculating the left indicator value.</param>
    /// <param name="cluster">The cluster index (0, 3, or 6) to determine the appropriate codeword pattern for the left indicator.</param>
    /// <returns>An integer representing the left indicator value for the specified cell.</returns>
    protected abstract int GetLeftIndicator(
        int row,
        int rows,
        int cols,
        PDF417ErrorCorrectionLevel correctionLevel,
        int cluster);

    /// <summary>
    /// Calculates the right indicator value for a specified cell within a grid layout.
    /// </summary>
    /// <param name="row">The zero-based index of the row for which to calculate the right indicator.</param>
    /// <param name="rows">The total number of rows in the grid. Must be greater than zero.</param>
    /// <param name="cols">The total number of columns in the grid. Must be greater than zero.</param>
    /// <param name="correctionLevel">The error correction level to consider when calculating the right indicator value.</param>
    /// <param name="cluster">The cluster index (0, 3, or 6) to determine the appropriate codeword pattern for the left indicator.</param>
    /// <returns>An integer representing the right indicator value for the specified cell.</returns>
    protected abstract int GetRightIndicator(
        int row,
        int rows,
        int cols,
        PDF417ErrorCorrectionLevel correctionLevel,
        int cluster);

    /// <summary>
    /// Concatenates the elements of multiple integer arrays into a single array.
    /// </summary>
    /// <param name="blocks">A list of integer arrays to be combined into a single array. Cannot be null, and none of the arrays within the
    /// list can be null.</param>
    /// <returns>An array containing all the integers from the input arrays, in the order they appear in the list. Returns an
    /// empty array if the list is empty.</returns>
    private static int[] Flatten(List<int[]> blocks)
    {
        var total = 0;

        foreach (var b in blocks)
        {
            total += b.Length;
        }

        var result = new int[total];
        var p = 0;

        foreach (var b in blocks)
        {
            Array.Copy(b, 0, result, p, b.Length);
            p += b.Length;
        }

        return result;
    }
}
