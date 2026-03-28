using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Builders.Pdf417;

/// <summary>
/// Provides a matrix builder for standard (non-compact) PDF417 barcodes, generating the normal barcode matrix structure
/// according to the PDF417 specification.
/// </summary>
/// <remarks>This class implements the logic for constructing the matrix of a standard PDF417 barcode, including
/// the calculation of start and stop codewords and row indicators. It is intended for use within barcode encoding
/// workflows that require the full PDF417 format, as opposed to compact or micro variants.</remarks>
internal sealed class Pdf417NormalMatrixBuilder : Pdf417MatrixBuilderBase
{
    /// <inheritdoc />
    protected override int[] StartCodeword => Pdf417CodewordPatterns.StartPattern;

    /// <inheritdoc />
    protected override int[] StopCodeword => Pdf417CodewordPatterns.StopPattern;

    /// <inheritdoc />
    protected override bool UseCompactIndicators => false;

    /// <inheritdoc />
    protected override bool IsMicro => false;

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
