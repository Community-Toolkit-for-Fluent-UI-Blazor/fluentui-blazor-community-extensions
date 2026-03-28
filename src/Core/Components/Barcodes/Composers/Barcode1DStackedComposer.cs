using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to compose 1D stacked barcode payloads with calculated dimensions, quiet zones, and optional
/// label rendering, based on specified data and rendering options.
/// </summary>
/// <remarks>This class implements the IBarcodeComposer interface for 1D barcode payloads, supporting the
/// calculation of barcode size, quiet zone, and label positioning according to the provided options. It is intended for
/// use in scenarios where flexible barcode rendering and configuration are required. The class supports extensibility
/// through the AfterCompose method, which can be overridden to perform additional processing after
/// composition.</remarks>
/// <typeparam name="TBarcodeOptions">The type of options used to configure barcode generation and rendering. This allows customization of barcode
/// behavior and appearance.</typeparam>
internal class Barcode1DStackedComposer<TBarcodeOptions>
    : IBarcodeComposer<Barcode1DStackedPayload, TBarcodeOptions> where TBarcodeOptions : IBarcode1DStackedOptions
{
    /// <inheritdoc />
    public BarcodePayload<Barcode1DStackedPayload> Compose(
       Barcode1DStackedPayload data,
       BarcodeRenderingOptions options,
       TBarcodeOptions barcodeOptions)
    {
        var quiet = ComputeQuietZone(options.QuietZone);
        var width = ComputeWidth(data, quiet, options.Label, barcodeOptions.ModuleWidth);
        var height = ComputeHeight(data, quiet, options.Label, barcodeOptions.ModuleHeight);

        var payload = new BarcodePayload<Barcode1DStackedPayload>
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
        UpdatePayload(payload, barcodeOptions);

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
    /// <param name="options">The options used to configure the barcode generation, which may influence how the payload is updated.</param>
    private static void UpdatePayload(
        BarcodePayload<Barcode1DStackedPayload> payload,
        TBarcodeOptions options)
    {
        var texts = payload.Data.Texts;
        var labelOptions = payload.LabelOptions;

        var minX = 0.0;
        var maxX = (double)payload.Data.Columns;
        var maxY = (double)payload.Data.Rows;

        if (texts.Count > 0)
        {
            var glyph = labelOptions.FontSize * 0.6;

            foreach (var t in texts)
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

                    default:
                        left = t.X - glyph * 0.5;
                        right = t.X + glyph * 0.5;
                        break;
                }

                minX = Math.Min(minX, left);
                maxX = Math.Max(maxX, right);
            }
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

        var qx = payload.QuietZone?.X ?? 0;
        var qy = payload.QuietZone?.Y ?? 0;

        foreach (var cell in payload.Data.Modules)
        {
            payload.Shapes.Add(new BarcodeRectangle(
               qx + cell.X * options.ModuleWidth,
               qy + cell.Y * options.ModuleHeight,
               cell.Width * options.ModuleWidth,
               cell.Height * options.ModuleHeight));
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
        BarcodePayload<Barcode1DStackedPayload> payload,
        Barcode1DStackedPayload data,
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
    /// <param name="width">The width of a single module in the barcode</param>
    /// <returns>Returns the total width of the barcode.</returns>
    private static double ComputeWidth(
        Barcode1DStackedPayload data,
        BarcodeQuietZonePayload quiet,
        BarcodeLabelOptions label,
        double width)
    {
        var minX = 0.0;
        var maxX = data.Columns * width;

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

                    default:
                        left = t.X - glyph * 0.5;
                        right = t.X + glyph * 0.5;
                        break;
                }

                minX = Math.Min(minX, left);
                maxX = Math.Max(maxX, right);
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
    /// <param name="height">The height of a single module in the barcode</param>
    /// <returns>The total height, in device-independent units, needed to render the barcode with the specified quiet zone and
    /// label options.</returns>
    private static double ComputeHeight(
        Barcode1DStackedPayload data,
        BarcodeQuietZonePayload quiet,
        BarcodeLabelOptions label,
        double height)
    {
        var baseHeight = 0.0;

        if (label.Enabled)
        {
            baseHeight += label.FontSize + label.OffsetY;
        }

        return (data.Rows * height) + baseHeight + quiet.Height;
    }
}
