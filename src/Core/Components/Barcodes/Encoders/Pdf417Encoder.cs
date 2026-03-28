using System.Data;
using System.Diagnostics;
using System.Text;
using FluentUI.Blazor.Community.Components.Builders.Pdf417;
using FluentUI.Blazor.Community.Components.Enums;
using FluentUI.Blazor.Community.Components.Helpers;
using FluentUI.Blazor.Community.Components.Helpers.Pdf417;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to encode data into the PDF417 two-dimensional barcode format.
/// </summary>
/// <remarks>This encoder supports generating PDF417 barcodes using the specified payload and options. PDF417 is a
/// stacked linear barcode format commonly used for applications requiring high data capacity, such as identification
/// cards and transport tickets. Use this encoder when you need to create PDF417 barcodes from structured data within
/// the application.</remarks>
internal sealed class Pdf417Encoder : IBarcodeEncoder<Barcode1DStackedPayload, Pdf417Options>
{
    /// <summary>
    /// Specifies the maximum number of columns allowed.
    /// </summary>
    private const byte MaxColumns = 30;

    /// <summary>
    /// Represents the maximum number of rows allowed.
    /// </summary>
    private const byte MaxRows = 90;

    /// <summary>
    /// Represents the default number of columns used when no specific value is provided.
    /// </summary>
    private const byte DefaultColumns = 4;

    /// <summary>
    /// Provides a static instance of the MicroPdf417MatrixBuilder used for constructing MicroPDF417 barcode matrices.
    /// </summary>
    /// <remarks>This instance is intended for internal use to optimize matrix building operations and avoid
    /// repeated allocations.</remarks>
    private static readonly MicroPdf417MatrixBuilder MicroPdf417MatrixBuilder = new();

    /// <summary>
    /// Provides a singleton instance of the Pdf417CompactMatrixBuilder for constructing compact PDF417 barcode
    /// matrices.
    /// </summary>
    /// <remarks>This static, read-only instance is intended for internal use to ensure consistent matrix
    /// building behavior across the application.</remarks>
    private static readonly Pdf417CompactMatrixBuilder Pdf417CompactMatrixBuilder = new();

    /// <summary>
    /// Provides a singleton instance of the Pdf417NormalMatrixBuilder used for constructing normal PDF417 barcode
    /// matrices.
    /// </summary>
    private static readonly Pdf417NormalMatrixBuilder Pdf417NormalMatrixBuilder = new();

    /// <summary>
    /// Gets the singleton instance of the <see cref="Pdf417Encoder" /> class.
    /// </summary>
    public static Pdf417Encoder Instance { get; } = new();

    /// <inheritdoc />
    public Barcode1DStackedPayload Encode(string data, Pdf417Options options)
    {
        var dataCodewords = Pdf417EncoderCore.EncodeData(data);
        var errorCorrectionLevel = Pdf417EncoderCore.GetErrorCorrectionLevel(dataCodewords, options.ErrorLevel, out var errorCorrectionLength);

        var columns = options.Columns ?? DefaultColumns;
        var rows = options.Rows ?? (dataCodewords.Count + errorCorrectionLength + columns - 1) / columns;

        if (rows > MaxRows)
        {
            rows = MaxRows;
            columns = (dataCodewords.Count + errorCorrectionLength + rows - 1) / rows;

            if (columns > MaxColumns)
            {
                throw new InvalidOperationException("PDF417 barcode data overflow");
            }
        }

        // Force micro grid ?
        if (options.Mode == PDF417Mode.Micro)
        {
            if (options.MicroGrid is { } g)
            {
                rows = g.Rows;
                columns = g.Columns;
            }
            //else
            //{
            //    var (r, c) = MicroPdf417Grid.AutoSelectMicroGrid(totalRaw);
            //    rows = r;
            //    columns = c;
            //}
        }

        // 3. Complete codeword array (data + ECC + padding)
        var full = Pdf417EncoderCore.BuildFullCodewordArray(
            dataCodewords,
            rows,
            columns,
            errorCorrectionLevel);

        System.Diagnostics.Debug.WriteLine("FULL :" + string.Join(" ", full.Select(x => x.ToString("X2", System.Globalization.CultureInfo.InvariantCulture))));

        // 4. Matrix builder (Normal / Compact / Micro)
        Pdf417MatrixBuilderBase builder = options.Mode switch
        {
            PDF417Mode.Micro => MicroPdf417MatrixBuilder,
            PDF417Mode.Normal when options.Compact => Pdf417CompactMatrixBuilder,
            _ => Pdf417NormalMatrixBuilder
        };

        var widthRows = builder.BuildMatrix(full, columns, rows, options.ErrorLevel);

        // 5. Convert to boolean matrix.
        var moduleMatrix = BuildModuleMatrix(widthRows);

        var sb = new StringBuilder();

        for (var r = 0; r < moduleMatrix.GetLength(0); r++)
        {
            for (var c = 0; c < moduleMatrix.GetLength(1); c++)
            {
                sb.Append(moduleMatrix[r, c] == true ? "1" : "0");
            }

            sb.AppendLine();
        }

        Debug.WriteLine(sb);

        // 6. Payload
        return ToPayload(moduleMatrix, data);
    }

    /// <summary>
    /// Builds a two-dimensional boolean matrix representing a pattern of alternating modules based on the specified
    /// row-wise run-lengths.
    /// </summary>
    /// <remarks>The resulting matrix will have a number of rows equal to the length of the input array, and
    /// the number of columns is determined by the longest total run-length in any row. Rows shorter than the maximum
    /// width are padded with white modules (false) at the end.</remarks>
    /// <param name="widthRows">An array of integer arrays, where each inner array specifies the sequence of run-lengths for alternating module
    /// colors in a row. Each value represents the width of a consecutive run of modules, alternating between black and
    /// white, starting with black.</param>
    /// <returns>A two-dimensional boolean array where each element is set to true for a black module and false for a white
    /// module, as defined by the input run-lengths.</returns>
    private static bool[,] BuildModuleMatrix(int[][] widthRows)
    {
        var rows = widthRows.Length;
        var cols = 0;

        foreach (var r in widthRows)
        {
            var w = 0;

            foreach (var v in r)
            {
                w += v;
            }

            if (w > cols)
            {
                cols = w;
            }
        }

        var matrix = new bool[rows, cols];

        for (var r = 0; r < rows; r++)
        {
            var x = 0;
            var black = true;

            foreach (var w in widthRows[r])
            {
                for (var k = 0; k < w; k++)
                {
                    if (black)
                    {
                        matrix[r, x] = true;
                    }

                    x++;
                }

                black = !black;
            }
        }

        return matrix;
    }

    /// <summary>
    /// Converts a two-dimensional boolean matrix and associated data into a Barcode2DPayload object representing the
    /// encoded barcode structure.
    /// </summary>
    /// <param name="matrix">A two-dimensional boolean array where each element indicates whether the corresponding cell in the barcode is
    /// active (<see langword="true"/>) or inactive (<see langword="false"/>).</param>
    /// <param name="data">The data string to associate with the barcode payload.</param>
    /// <returns>A Barcode2DPayload object containing the encoded barcode structure, including active cell positions and the
    /// associated data.</returns>
    private static Barcode1DStackedPayload ToPayload(bool[,] matrix, string data)
    {
        var rows = matrix.GetLength(0);
        var cols = matrix.GetLength(1);
        var modules = MatrixHelper.Optimize(matrix);

        return new Barcode1DStackedPayload
        {
            Value = data,
            Rows = rows,
            Columns = cols,
            Modules = modules,
            Texts = []
        };
    }
}
