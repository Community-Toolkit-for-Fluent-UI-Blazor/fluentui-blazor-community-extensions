using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to render one-dimensional barcodes using the specified payload and rendering options.
/// </summary>
/// <remarks>This class implements the IBarcodeRenderer interface for 1D barcode payloads, enabling the
/// composition of barcode rendering data including quiet zones and optional labels. Instances of this class are
/// immutable and thread-safe.</remarks>
internal class Barcode1DLayerComposer<TBarcodeOptions> : IBarcodeLayerComposer<Barcode1DPayload, TBarcodeOptions>
{
    /// <summary>
    /// Generates a barcode payload with calculated dimensions, quiet zone, and optional label based on the provided
    /// data and rendering options.
    /// </summary>
    /// <remarks>The returned payload includes all necessary information for rendering a 1D barcode according
    /// to the specified options. If label rendering is disabled in the options, the label property of the payload will
    /// be null.</remarks>
    /// <param name="data">The barcode data to encode. Must contain the information to be represented in the barcode.</param>
    /// <param name="options">The rendering options that specify visual settings such as quiet zone size and label configuration. Cannot be
    /// null.</param>
    /// <param name="barcodeOptions">The options used to configure the barcode generation</param>
    /// <returns>A <see cref="BarcodePayload{Barcode1DPayload}"/> instance containing the encoded data, calculated width and height, quiet
    /// zone, and label if enabled.</returns>
    public BarcodePayload<Barcode1DPayload> Compose(
        Barcode1DPayload data,
        BarcodeRenderingOptions options,
        TBarcodeOptions barcodeOptions)
    {
        var quiet = ComputeQuietZone(options.QuietZone);
        var width = ComputeWidth(data, quiet, options.Label);
        var height = ComputeHeight(data, quiet, options.Label);

        var payload = new BarcodePayload<Barcode1DPayload>
        {
            Width = width,
            Height = height,
            Data = data,
            QuietZone = quiet,
            Background = options.Background,
            Foreground = options.Foreground,
            View = options.View,
            LabelOptions = options.Label,
        };

        AfterCompose(payload, data, options, barcodeOptions);
        UpdatePayload(payload);

        return payload;
    }

    /// <summary>
    /// Updates the dimensions and label position of the specified barcode payload based on its bars, shapes, and quiet
    /// zone.
    /// </summary>
    /// <remarks>This method recalculates the width and height of the payload to encompass all bars, shapes,
    /// and the quiet zone. If a label is present, its vertical position is adjusted to align with the updated payload
    /// height.</remarks>
    /// <param name="payload">The barcode payload to update. Must not be null and should contain valid bar and shape data.</param>
    private static void UpdatePayload(BarcodePayload<Barcode1DPayload> payload)
    {
        var bars = payload.Data.Bars;
        var shapes = payload.Shapes;
        var texts = payload.Data.Texts;
        var labelOptions = payload.LabelOptions;

        var minX = bars.Min(b => b.X);
        var maxX = bars.Max(b => b.X + b.Width);
        var minY = 0.0;
        var maxY = bars.Max(b => b.Height);

        if (shapes.Count > 0)
        {
            minX = Math.Min(minX, shapes.Min(s => s.X));
            maxX = Math.Max(maxX, shapes.Max(s => s.X + s.Width));
            minY = Math.Min(minY, shapes.Min(s => s.Y));
            maxY = Math.Max(maxY, shapes.Max(s => s.Y + s.Height));
        }

        var dx = minX < 0 ? -minX : 0;
        var dy = minY < 0 ? -minY : 0;

        if (dx != 0 || dy != 0)
        {
            foreach (var b in bars)
            {
                b.X += dx;
                b.Y += dy;
            }

            foreach (var s in shapes)
            {
                s.X += dx;
                s.Y += dy;
            }

            foreach (var t in texts)
            {
                t.X += dx;
                t.Y += dy;
            }

            minX += dx;
            maxX += dx;
            minY += dy;
            maxY += dy;
        }

        var contentBottom = maxY;

        if (labelOptions.Enabled && texts.Count > 0)
        {
            var labelY = contentBottom + labelOptions.OffsetY;

            foreach (var t in texts)
            {
                t.Y = labelY;
            }

            maxY = labelY;
        }

        var quietWidth = payload.QuietZone?.Width ?? 0;
        var quietHeight = payload.QuietZone?.Height ?? 0;

        payload.Width = (maxX - minX) + quietWidth;
        payload.Height = maxY + quietHeight;
    }

    /// <summary>
    /// Performs additional processing after the barcode composition is complete.
    /// </summary>
    /// <param name="payload">The composed barcode payload containing the result of the barcode generation process.</param>
    /// <param name="data">The original data used to generate the barcode payload.</param>
    /// <param name="options">The rendering options that influence how the barcode is composed and displayed.</param>
    /// <param name="barcodeOptions">The options used to configure the barcode generation</param>
    protected virtual void AfterCompose(
        BarcodePayload<Barcode1DPayload> payload,
        Barcode1DPayload data,
        BarcodeRenderingOptions options,
        TBarcodeOptions barcodeOptions)
    {
    }

    /// <summary>
    /// Calculates the quiet zone dimensions for a barcode based on the specified options.
    /// </summary>
    /// <param name="options">The options that determine whether the quiet zone is enabled and specify the padding values to use.</param>
    /// <returns>A BarcodeQuietZonePayload containing the calculated quiet zone dimensions. If the quiet zone is not enabled, all
    /// values are set to zero.</returns>
    private static BarcodeQuietZonePayload ComputeQuietZone(BarcodeQuietZoneOptions options)
    {
        if (!options.Enabled)
        {
            return new BarcodeQuietZonePayload { X = 0, Y = 0, Width = 0, Height = 0 };
        }

        return new BarcodeQuietZonePayload
        {
            X = options.Padding.Left,
            Y = options.Padding.Top,
            Width = options.Padding.Left + options.Padding.Right,
            Height = options.Padding.Top + options.Padding.Bottom
        };
    }

    /// <summary>
    /// Calculates the total width of the barcode based on the data and quiet zone dimensions.
    /// </summary>
    /// <param name="data">The data representing the bars of the barcode.</param>
    /// <param name="quiet">The quiet zone dimensions that contribute to the overall width of the barcode.</param>
    /// <param name="label">The options of the label.</param>
    /// <returns>Returns the total width of the barcode.</returns>
    private static double ComputeWidth(
        Barcode1DPayload data,
        BarcodeQuietZonePayload quiet,
        BarcodeLabelOptions label)
    {
        var minX = data.Bars.Min(b => b.X);
        var maxX = data.Bars.Max(b => b.X + b.Width);

        if (data.Texts.Count > 0)
        {
            var glyph = label.FontSize * 0.6;

            foreach (var t in data.Texts)
            {
                double left, right;

                switch (t.Anchor)
                {
                    case SvgTextAnchor.Start:
                        left = t.X;
                        right = t.X + glyph;
                        break;

                    case SvgTextAnchor.End:
                        left = t.X - glyph;
                        right = t.X;
                        break;

                    default: // Middle
                        left = t.X - glyph * 0.5;
                        right = t.X + glyph * 0.5;
                        break;
                }

                if (left < minX)
                {
                    minX = left;
                }

                if (right > maxX)
                {
                    maxX = right;
                }
            }
        }

        return (maxX - minX) + quiet.Width;
    }

    /// <summary>
    /// Calculates the total height required to render a 1D barcode, including quiet zones and optional label space.
    /// </summary>
    /// <remarks>If the label is enabled, its font size and vertical offset are included in the total height
    /// calculation. The quiet zone height is always added to the result.</remarks>
    /// <param name="data">The barcode payload containing the bar definitions to be rendered.</param>
    /// <param name="quiet">The payload specifying the quiet zone dimensions to be added around the barcode.</param>
    /// <param name="label">The label options that determine whether a label is rendered and its associated size and offset.</param>
    /// <returns>The total height, in device-independent units, needed to render the barcode with the specified quiet zone and
    /// label options.</returns>
    private static double ComputeHeight(
        Barcode1DPayload data,
        BarcodeQuietZonePayload quiet,
        BarcodeLabelOptions label)
    {
        var baseHeight = data.Bars.Max(b => b.Height);

        if (label.Enabled)
        {
            baseHeight += label.FontSize + label.OffsetY;
        }

        return baseHeight + quiet.Height;
    }
}
