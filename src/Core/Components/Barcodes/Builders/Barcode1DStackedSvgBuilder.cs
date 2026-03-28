using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides methods for constructing SVG representations of 1D stacked barcodes using the specified payload and
/// rendering options.
/// </summary>
/// <remarks>This class is intended for internal use in rendering barcode components as SVG graphics. It applies
/// background, foreground, and label options from the payload to generate the visual structure of the barcode. The
/// class is static and cannot be instantiated.</remarks>
internal static class Barcode1DStackedSvgBuilder
{
    /// <summary>
    /// Builds the SVG representation of a 1D stacked barcode using the specified builder and payload.
    /// </summary>
    /// <remarks>This method adds the barcode shapes, background, and label text to the provided SVG builder
    /// based on the configuration in the payload. The method does not return a value; the SVG content is built directly
    /// into the supplied builder instance.</remarks>
    /// <param name="builder">The SVG builder used to construct the barcode elements. Cannot be null.</param>
    /// <param name="payload">The payload containing barcode data, rendering options, and visual properties. Cannot be null.</param>
    public static void Build(
        SvgBuilder builder,
        BarcodePayload<Barcode1DStackedPayload> payload)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(payload);

        var width = payload.Width;
        var height = payload.Height;

        var background = payload.Background;
        var foreground = payload.Foreground;
        var quiet = payload.QuietZone;
        var labelOptions = payload.LabelOptions;

        var offsetX = quiet?.X ?? 0;
        var offsetY = quiet?.Y ?? 0;

        var group = builder.AddGroup();

        if (!string.IsNullOrWhiteSpace(background.Color))
        {
            group
                .AddRect(0, 0, width, height)
                .WithFill(background.Color)
                .Close();
        }

        var fillColor = string.IsNullOrWhiteSpace(foreground.Color)
            ? StylesVariables.Colors.Brand.Foreground2
            : foreground.Color;

        foreach (var shape in payload.Shapes)
        {
            group
                .AddRect(
                    shape.X + offsetX,
                    shape.Y + offsetY,
                    shape.Width,
                    shape.Height)
                .WithFill(fillColor)
                .Close();
        }

        foreach (var t in payload.Data.Texts)
        {
            group
                .AddText(
                    t.X + offsetX,
                    t.Y + offsetY,
                    t.Text)
                .WithFontFamily(labelOptions.FontFamily)
                .WithFontSize(labelOptions.FontSize)
                .WithFill(labelOptions.Color)
                .WithTextAnchor(t.Anchor)
                .WithDominantBaseline("alphabetic")
                .Close();
        }

        group.Close();
    }
}
