namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to compose ITF-14 barcodes, including the addition of bearer bars as required by the ITF-14
/// specification.
/// </summary>
/// <remarks>This class is intended for internal use within the barcode rendering system and extends the base 1D
/// barcode composer to support the specific requirements of ITF-14 barcodes. The composer ensures that bearer bars are
/// added to the barcode output, which are necessary for compliance with ITF-14 standards and to improve scanning
/// reliability.</remarks>
internal sealed class Itf14Composer : Barcode1DComposer<Itf14Options>
{
    /// <inheritdoc />
    protected override void AfterCompose(
        BarcodePayload<Barcode1DPayload> payload,
        Barcode1DPayload data,
        BarcodeRenderingOptions options,
        Itf14Options itf14Options)
    {
        var minX = data.Bars.Min(b => b.X);
        var maxX = data.Bars.Max(b => b.X + b.Width);
        var barHeight = data.Bars.Max(b => b.Height);

        var innerLeft = minX;
        var innerRight = maxX;
        var innerTop = 0.0;
        var innerBottom = barHeight;

        var thickness = barHeight / 6.0;
        var gap = 2 * thickness;

        var outerLeft = innerLeft - gap;
        var outerRight = innerRight + gap;
        var outerTop = innerTop - thickness;
        var outerBottom = innerBottom + thickness;

        payload.Shapes.Add(new BarcodeRectangle(
            outerLeft,
            outerTop,
            outerRight - outerLeft,
            thickness));

        payload.Shapes.Add(new BarcodeRectangle(
            outerLeft,
            outerBottom - thickness,
            outerRight - outerLeft,
            thickness));

        payload.Shapes.Add(new BarcodeRectangle(
            outerLeft,
            outerTop,
            thickness,
            outerBottom - outerTop));

        payload.Shapes.Add(new BarcodeRectangle(
            outerRight - thickness,
            outerTop,
            thickness,
            outerBottom - outerTop));
    }
}

