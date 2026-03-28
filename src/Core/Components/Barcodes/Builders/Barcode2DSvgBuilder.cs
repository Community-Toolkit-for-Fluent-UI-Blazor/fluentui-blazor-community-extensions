using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to build an SVG representation of a 2D barcode using the specified payload and SVG builder.
/// </summary>
/// <remarks>This class is intended for internal use when rendering 2D barcodes as SVG graphics. It processes the
/// barcode payload, including background, foreground, quiet zones, and optional text elements, and adds the
/// corresponding SVG elements to the provided builder. The class does not expose any public members and is not intended
/// to be used directly by application code.</remarks>
internal static class Barcode2DSvgBuilder
{
    /// <summary>
    /// Builds the SVG representation of a 2D barcode using the specified builder and payload.
    /// </summary>
    /// <remarks>This method adds background, barcode modules, and optional text elements to the SVG output
    /// based on the provided payload. The payload must contain all necessary information for rendering the barcode,
    /// including dimensions, color settings, and shape definitions.</remarks>
    /// <param name="builder">The SVG builder used to construct the barcode elements. Cannot be null.</param>
    /// <param name="payload">The payload containing barcode data, dimensions, colors, and shapes to render. Cannot be null.</param>
    public static void Build(
        SvgBuilder builder,
        BarcodePayload<Barcode2DPayload> payload)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(payload);

        var width = payload.Width;
        var height = payload.Height;

        var background = payload.Background;
        var foreground = payload.Foreground;
        var quiet = payload.QuietZone;

        var offsetX = quiet?.X ?? 0;
        var offsetY = quiet?.Y ?? 0;

        var group = builder.AddGroup();

        // 1. Fond complet (inclut la quiet zone)
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

        // 2. Modules décalés par la quiet zone
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

        // 3. Textes (déjà décalés correctement)
        foreach (var t in payload.Data.Texts)
        {
            group
                .AddText(
                    t.X + offsetX,
                    t.Y + offsetY,
                    t.Text)
                .WithFontFamily(payload.LabelOptions.FontFamily)
                .WithFontSize(payload.LabelOptions.FontSize)
                .WithFill(payload.LabelOptions.Color)
                .WithTextAnchor(t.Anchor)
                .WithDominantBaseline("alphabetic")
                .Close();
        }

        group.Close();
    }
}

