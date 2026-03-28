using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides methods to generate and append the SVG representation of a 1D barcode to an SVG builder.
/// </summary>
/// <remarks>This class is intended for internal use when rendering 1D barcodes as SVG elements. It supports
/// rendering barcode bars, background, labels, and additional shapes based on the provided payload
/// configuration.</remarks>
internal static class Barcode1DSvgBuilder
{
    /// <summary>
    /// Appends the SVG representation of the given 1D barcode payload to the provided SVG builder.
    /// </summary>
    /// <param name="builder">The SVG builder to append elements to.</param>
    /// <param name="payload">The composed barcode payload containing geometry and rendering options.</param>
    public static void Build(
        SvgBuilder builder,
        BarcodePayload<Barcode1DPayload> payload)
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
        var quietWidth = quiet?.Width ?? 0;
        var quietHeight = quiet?.Height ?? 0;

        var contentWidth = width - quietWidth;
        var barsHeight = payload.Data.Bars.Max(b => b.Height);

        var group = builder.AddGroup();

        if (!string.IsNullOrWhiteSpace(background.Color))
        {
            group
                .AddRect(offsetX, offsetY, contentWidth, barsHeight)
                .WithFill(background.Color)
                .Close();
        }

        var barColor = string.IsNullOrWhiteSpace(foreground.Color)
            ? StylesVariables.Colors.Brand.Foreground2
            : foreground.Color;

        // Barres
        foreach (var bar in payload.Data.Bars)
        {
            group
                .AddRect(
                    bar.X + offsetX,
                    bar.Y + offsetY,
                    bar.Width,
                    bar.Height)
                .WithFill(barColor)
                .Close();
        }

        // Texte (label)
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

        foreach (var shape in payload.Shapes)
        {
            switch (shape)
            {
                case BarcodeRectangle r:
                    group
                        .AddRect(
                            r.X + offsetX,
                            r.Y + offsetY,
                            r.Width,
                            r.Height)
                        .WithFill(barColor)
                        .Close();
                    break;
            }
        }

        group.Close();
    }
}
