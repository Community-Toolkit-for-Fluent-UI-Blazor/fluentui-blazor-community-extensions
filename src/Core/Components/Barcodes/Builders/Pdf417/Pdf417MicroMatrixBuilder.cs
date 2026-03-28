using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Builders.Pdf417;

/// <summary>
/// Provides matrix-building logic specific to the MicroPDF417 barcode format.
/// </summary>
/// <remarks>This class implements the MicroPDF417 variant of the matrix builder, supplying the appropriate start
/// and stop codewords, indicator calculations, and format-specific behaviors. It is intended for internal use when
/// generating MicroPDF417 barcodes and should not be used directly for other barcode types.</remarks>
internal sealed class MicroPdf417MatrixBuilder : Pdf417MatrixBuilderBase
{
    /// <inheritdoc />
    protected override int[] StartCodeword => Pdf417CodewordPatterns.MicroStartPattern;

    /// <inheritdoc />
    protected override int[] StopCodeword => Pdf417CodewordPatterns.MicroStopPattern;

    /// <inheritdoc />
    protected override bool UseCompactIndicators => false;

    /// <inheritdoc />
    protected override bool IsMicro => true;

    /// <inheritdoc />
    protected override int GetLeftIndicator(
        int row,
        int rows,
        int cols,
        PDF417ErrorCorrectionLevel level,
        int cluster)
    {
        // MicroPDF417 left indicator = (row << 3) + (cols - 1)
        return (row << 3) + (cols - 1);
    }

    /// <inheritdoc />
    protected override int GetRightIndicator(
        int row,
        int rows,
        int cols,
        PDF417ErrorCorrectionLevel level,
        int cluster)
    {
        // MicroPDF417 right indicator = (rows << 3) + row
        return (rows << 3) + row;
    }
}

