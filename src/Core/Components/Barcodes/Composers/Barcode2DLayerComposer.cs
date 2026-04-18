namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to compose 2D barcode payloads using specified barcode and rendering options.
/// </summary>
/// <remarks>This class implements the IBarcodeComposer interface for 2D barcode payloads, allowing for flexible
/// configuration of barcode generation through generic options. It is intended for internal use within the barcode
/// generation infrastructure.</remarks>
/// <typeparam name="TBarcodeOptions">The type that defines additional options specific to the barcode format being composed.</typeparam>
internal class Barcode2DLayerComposer<TBarcodeOptions> : IBarcodeLayerComposer<Barcode2DPayload, TBarcodeOptions>
    where TBarcodeOptions : IBarcode2DOptions
{
    /// <inheritdoc />
    public BarcodePayload<Barcode2DPayload> Compose(
        Barcode2DPayload data,
        BarcodeRenderingOptions options,
        TBarcodeOptions barcodeOptions)
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));

        var quiet = new BarcodeQuietZonePayload()
        {
            Width = 8 * barcodeOptions.ModuleSize,
            Height = 8 * barcodeOptions.ModuleSize
        };

        var width = ComputeWidth(data, quiet, barcodeOptions.ModuleSize);
        var height = ComputeHeight(data, quiet, barcodeOptions.ModuleSize, options.Label);

        var payload = new BarcodePayload<Barcode2DPayload>
        {
            Data = data,
            QuietZone = quiet,
            Width = width,
            Height = height,
            Background = options.Background,
            Foreground = options.Foreground,
            View = options.View,
            LabelOptions = options.Label
        };

        AfterCompose(payload, data, options, barcodeOptions);
        UpdatePayload(payload, barcodeOptions.ModuleSize);

        return payload;
    }

    /// <summary>
    /// Calculates the total width required to render a 2D barcode, including the barcode data, quiet zone, and
    /// optional label.
    /// </summary>
    /// <param name="data">The payload containing the 2D barcode data, including the number of rows to be rendered.</param>
    /// <param name="quiet">The payload specifying the quiet zone dimensions to be added around the barcode.</param>
    /// <param name="moduleSize">The height, in device-independent units, of a single barcode module (cell). Must be a positive value.</param>
    /// <returns>The total height, in device-independent units, required to render the barcode with the specified data, quiet
    /// zone, and label options.</returns>
    private static double ComputeWidth(
        Barcode2DPayload data,
        BarcodeQuietZonePayload quiet,
        double moduleSize)
    {
        return (data.Columns * moduleSize) + quiet.Width;
    }

    /// <summary>
    /// Calculates the total height required to render a 2D barcode, including the barcode data, quiet zone, and
    /// optional label.
    /// </summary>
    /// <param name="data">The payload containing the 2D barcode data, including the number of rows to be rendered.</param>
    /// <param name="quiet">The payload specifying the quiet zone dimensions to be added around the barcode.</param>
    /// <param name="moduleSize">The height, in device-independent units, of a single barcode module (cell). Must be a positive value.</param>
    /// <param name="label">The label options specifying whether a label is enabled and its vertical offset and font size.</param>
    /// <returns>The total height, in device-independent units, required to render the barcode with the specified data, quiet
    /// zone, and label options.</returns>
    private static double ComputeHeight(
        Barcode2DPayload data,
        BarcodeQuietZonePayload quiet,
        double moduleSize,
        BarcodeLabelOptions label)
    {
        var baseHeight = data.Rows * moduleSize;

        if (label.Enabled && data.Texts.Count > 0)
        {
            baseHeight += label.OffsetY + label.FontSize;
        }

        return baseHeight + quiet.Height;
    }

    /// <summary>
    /// Performs additional processing after composing the barcode payload, allowing customization of the barcode
    /// generation workflow.
    /// </summary>
    /// <remarks>Override this method in a derived class to implement custom logic that should execute after
    /// the barcode payload has been composed. This method is called as part of the barcode generation process and
    /// provides access to the composed payload, input data, rendering options, and barcode-specific options.</remarks>
    /// <param name="payload">The composed barcode payload containing the data and structure for the barcode.</param>
    /// <param name="data">The data object representing the 2D barcode content to be encoded.</param>
    /// <param name="options">The rendering options that specify visual settings for barcode generation.</param>
    /// <param name="barcodeOptions">The barcode-specific options that influence encoding or appearance.</param>
    protected virtual void AfterCompose(
        BarcodePayload<Barcode2DPayload> payload,
        Barcode2DPayload data,
        BarcodeRenderingOptions options,
        TBarcodeOptions barcodeOptions)
    {
    }

    /// <summary>
    /// Updates the barcode payload with shape and label positioning information based on the specified module size.
    /// </summary>
    /// <remarks>This method recalculates the positions of barcode modules and label text based on the current
    /// payload data and label options. It should be called whenever the module size or payload data changes to ensure
    /// correct rendering.</remarks>
    /// <param name="payload">The barcode payload to update with calculated shapes and label positions. Must not be null.</param>
    /// <param name="moduleSize">The size, in device-independent units, of each barcode module. Must be a positive value.</param>
    private static void UpdatePayload(BarcodePayload<Barcode2DPayload> payload, double moduleSize)
    {
        var texts = payload.Data.Texts;
        var labelOptions = payload.LabelOptions;
        var qx = 4 * moduleSize;
        var qy = 4 * moduleSize;

        foreach (var cell in payload.Data.Modules)
        {
            payload.Shapes.Add(new BarcodeRectangle(
               qx + cell.X * moduleSize,
               qy + cell.Y * moduleSize,
               cell.Width * moduleSize,
               cell.Height * moduleSize));
        }

        if (labelOptions.Enabled && texts.Count > 0)
        {
            var contentBottom = payload.QuietZone?.Y ?? 0 + payload.Data.Rows * moduleSize;

            var labelY = contentBottom + labelOptions.OffsetY;

            foreach (var t in texts)
            {
                t.Y = labelY;
            }
        }
    }
}
