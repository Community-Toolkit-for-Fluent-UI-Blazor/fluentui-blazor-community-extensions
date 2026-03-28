using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Builders.Pdf417;

/// <summary>
/// Provides a matrix builder for generating compact PDF417 barcodes using the standard PDF417 encoding scheme.
/// </summary>
/// <remarks>This class implements the logic required to construct the codeword matrix for compact PDF417
/// barcodes, including the calculation of start and stop codewords and the use of compact indicators. It is intended
/// for internal use as part of the barcode generation process and is not thread-safe.</remarks>
internal sealed class Pdf417CompactMatrixBuilder : Pdf417MatrixBuilderBase
{
    /// <inheritdoc />
    protected override int[] StartCodeword => Pdf417CodewordPatterns.StartPattern;

    /// <inheritdoc />
    protected override int[] StopCodeword => Pdf417CodewordPatterns.StopPattern;

    /// <inheritdoc />
    protected override bool UseCompactIndicators => true;

    /// <inheritdoc />
    protected override bool IsMicro => false;

    /// <inheritdoc />
    /// <inheritdoc />
    protected override int GetLeftIndicator(
        int row,
        int rows,
        int cols,
        PDF417ErrorCorrectionLevel correctionLevel,
        int cluster)
    {
        var lastRow = rows - 1;
        var left = 30 * (row / 3);

        if (cluster == 0)
        {
            left += lastRow / 3;
        }
        else if (cluster == 1)
        {
            left += ((int)correctionLevel * 3) + (lastRow % 3);
        }
        else
        {
            left += cols - 1;
        }

        return left;
    }

    /// <inheritdoc />
    protected override int GetRightIndicator(
        int row,
        int rows,
        int cols,
        PDF417ErrorCorrectionLevel correctionLevel,
        int cluster)
    {
        var lastRow = rows - 1;
        var right = 30 * (row / 3);

        if (cluster == 0)
        {
            right += cols - 1;
        }
        else if (cluster == 1)
        {
            right += lastRow / 3;
        }
        else
        {
            right += ((int)correctionLevel * 3) + (lastRow % 3);
        }

        return right;
    }
}
