namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides an SVG render target specialized for rendering barcode layers as SVG images.
/// </summary>
/// <remarks>This class supports rendering a single barcode layer, either one-dimensional or two-dimensional, to
/// an SVG output. Attempting to add more than one layer or an unsupported layer type will result in an exception. Use
/// this render target when you need to generate SVG representations of barcodes for display or export
/// purposes.</remarks>
public sealed class BarcodeSvgRenderTarget : SvgRenderTarget
{
    /// <inheritdoc />
    protected override void ValidateLayer(ILayer layer)
    {
        if (HasLayers)
        {
            throw new InvalidOperationException("The BarcodeSvgRenderTarget doesn't accept more than one layer.");
        }

        if (layer is not BarcodeLayer<Barcode1DPayload> &&
            layer is not BarcodeLayer<Barcode2DPayload> &&
            layer is not BarcodeLayer<Barcode1DStackedPayload>)
        {
            throw new InvalidOperationException("This Layer is not supported for a barcode.");
        }
    }

    /// <inheritdoc />
    protected override (double width, double height) ComputeViewBox(ILayer layer)
    {
        return layer.LayerPayload switch
        {
            BarcodePayload<Barcode1DPayload> p => (p.Width, p.Height),
            BarcodePayload<Barcode2DPayload> p => (p.Width, p.Width),
            BarcodePayload<Barcode1DStackedPayload> p => (p.Width, p.Height),
            _ => throw new NotSupportedException("Unsupported payload.")
        };
    }

    /// <inheritdoc />
    protected override void RenderLayer(SvgBuilder builder, ILayer layer)
    {
        switch (layer)
        {
            case BarcodeLayer<Barcode1DPayload> l1D:
                Barcode1DSvgBuilder.Build(builder, (BarcodePayload<Barcode1DPayload>)l1D.LayerPayload);
                break;

            case BarcodeLayer<Barcode2DPayload> l2D:
                Barcode2DSvgBuilder.Build(builder, (BarcodePayload<Barcode2DPayload>)l2D.LayerPayload);
                break;

            case BarcodeLayer<Barcode1DStackedPayload> lStacked:
                Barcode1DStackedSvgBuilder.Build(builder, (BarcodePayload<Barcode1DStackedPayload>)lStacked.LayerPayload);
                break;
        }
    }
}

