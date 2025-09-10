using SkiaSharp;

// BUG: SkiaSharp is not supported in .NET 9.0 WASM yet. Woking on server side only for now.
// System.Drawing doesn't work in WASM.

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a utility class for exporting signature images using SkiaSharp.
/// </summary>
internal static class SignatureSkiaExporter
{
    /// <summary>
    /// Exports a signature image based on the provided dimensions, strokes, and settings.
    /// </summary>
    /// <param name="format">The format of the exported image.</param>
    /// <param name="width">The width of the exported image, in pixels. Must be greater than 0.</param>
    /// <param name="height">The height of the exported image, in pixels. Must be greater than 0.</param>
    /// <param name="strokes">A collection of signature strokes that define the signature to be exported. Cannot be null.</param>
    /// <param name="quality">The quality of the exported image, as a percentage (1-100). Higher values indicate better quality.</param>
    /// <param name="signatureSettings">The settings that define the appearance and behavior of the signature. Cannot be null.</param>
    /// <param name="watermarkSettings">The settings for applying a watermark to the exported image. Cannot be null.</param>
    /// <returns>A tuple containing the exported image data as a byte array, the MIME type of the image, and the suggested
    /// filename.</returns>
    internal static (byte[] bytes, string mime, string filename) Export(
        SignatureExportFormat format,
        int width,
        int height,
        List<SignatureStroke> strokes,
        int quality,
        SignatureSettings signatureSettings,
        SignatureWatermarkSettings watermarkSettings)
    {
        var data = format != SignatureExportFormat.Pdf ? ExportInternal(format, width, height, strokes, quality, signatureSettings, watermarkSettings) :
            ExportPdf(width, height, strokes, signatureSettings, watermarkSettings);

        return (data, format switch
        {
            SignatureExportFormat.Png => "image/png",
            SignatureExportFormat.Jpeg => "image/jpeg",
            SignatureExportFormat.Webp => "image/webp",
            SignatureExportFormat.Pdf => "application/pdf",
            _ => "application/octet-stream",
        }, format switch
        {
            SignatureExportFormat.Pdf => "signature.pdf",
            SignatureExportFormat.Png => "signature.png",
            SignatureExportFormat.Jpeg => "signature.jpg",
            SignatureExportFormat.Webp => "signature.webp",
            _ => "signature.bin",
        });
    }

    /// <summary>
    /// Exports a signature as a PDF document.
    /// </summary>
    /// <param name="width">The width of the exported image, in pixels. Must be greater than 0.</param>
    /// <param name="height">The height of the exported image, in pixels. Must be greater than 0.</param>
    /// <param name="strokes">A collection of signature strokes that define the signature to be exported. Cannot be null.</param>
    /// <param name="signatureSettings">The settings that define the appearance and behavior of the signature. Cannot be null.</param>
    /// <param name="watermarkSettings">The settings for applying a watermark to the exported image. Cannot be null.</param>
    /// <returns>Returns the pdf as a <see cref="byte"/> array.</returns>
    private static byte[] ExportPdf(
        int width,
        int height,
        List<SignatureStroke> strokes,
        SignatureSettings signatureSettings,
        SignatureWatermarkSettings watermarkSettings)
    {
        using var ms = new MemoryStream();
        using (var doc = SKDocument.CreatePdf(ms, new SKDocumentPdfMetadata()))
        {
            using var canvas = doc.BeginPage(width, height);
            canvas.Clear(ParseColor(signatureSettings.BackgroundColor));

            if (signatureSettings.BackgroundImage is not null &&
                signatureSettings.BackgroundImage.Length > 0)
            {
                using var bmp = SKBitmap.Decode(signatureSettings.BackgroundImage);

                if (bmp is not null)
                {
                    canvas.DrawBitmap(bmp, new SKRect(0, 0, width, height));
                }
            }

            DrawGrid(canvas, width, height, signatureSettings);
            DrawSeparator(canvas, width, height, signatureSettings);
            DrawStrokes(canvas, strokes, signatureSettings);
            DrawWatermark(canvas, width, height, watermarkSettings);
            doc.EndPage();
        }

        return ms.ToArray();
    }

    /// <summary>
    /// Exports a signature image based on the provided dimensions, strokes, and settings.
    /// </summary>
    /// <param name="width">The width of the exported image, in pixels. Must be greater than 0.</param>
    /// <param name="height">The height of the exported image, in pixels. Must be greater than 0.</param>
    /// <param name="strokes">A collection of signature strokes that define the signature to be exported. Cannot be null.</param>
    /// <param name="quality">The quality of the exported image, as a percentage (1-100). Higher values indicate better quality.</param>
    /// <param name="signatureSettings">The settings that define the appearance and behavior of the signature. Cannot be null.</param>
    /// <param name="watermarkSettings">The settings for applying a watermark to the exported image. Cannot be null.</param>
    /// <param name="format">The format of the exported image.</param>
    /// <returns>A byte array containing the exported image data.</returns>
    private static byte[] ExportInternal(
        SignatureExportFormat format,
        int width,
        int height,
        List<SignatureStroke> strokes,
        int quality,
        SignatureSettings signatureSettings,
        SignatureWatermarkSettings watermarkSettings)
    {
        var encodedFormat = format switch
        {
            SignatureExportFormat.Png => SKEncodedImageFormat.Png,
            SignatureExportFormat.Jpeg => SKEncodedImageFormat.Jpeg,
            SignatureExportFormat.Webp => SKEncodedImageFormat.Webp,
            _ => SKEncodedImageFormat.Png,
        };

        using var bitmap = new SKBitmap(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
        using var surface = SKSurface.Create(bitmap.Info);
        var canvas = surface.Canvas;
        canvas.Clear(ParseColor(signatureSettings.BackgroundColor));

        if (signatureSettings.BackgroundImage is not null && signatureSettings.BackgroundImage.Length > 0)
        {
            using var bgImage = SKImage.FromEncodedData(signatureSettings.BackgroundImage);
            var destRect = new SKRect(0, 0, width, height);
            var sourceRect = new SKRect(0, 0, bgImage.Width, bgImage.Height);

            using var paint = new SKPaint
            {
                IsAntialias = true,
            };

            canvas.DrawImage(bgImage, sourceRect, destRect, new SKSamplingOptions(SKCubicResampler.Mitchell), paint);
        }

        DrawGrid(canvas, width, height, signatureSettings);
        DrawSeparator(canvas, width, height, signatureSettings);
        DrawStrokes(canvas, strokes, signatureSettings);
        DrawWatermark(canvas, width, height, watermarkSettings);

        using var image = surface.Snapshot();
        using var data = image.Encode(encodedFormat, quality);

        return data.ToArray();
    }

    /// <summary>
    /// Parses a hexadecimal color string and returns the corresponding <see cref="SKColor"/> value.
    /// </summary>
    /// <remarks>This method attempts to parse the provided hexadecimal color string using <see
    /// cref="SKColor.TryParse(string, out SKColor)"/>. If the parsing fails, the method returns <see
    /// cref="SKColors.Transparent"/>.</remarks>
    /// <param name="hex">The hexadecimal color string to parse. The string should be in a valid hex color format, such as "#RRGGBB" or
    /// "#AARRGGBB".</param>
    /// <returns>The parsed <see cref="SKColor"/> if the input string is valid; otherwise, <see cref="SKColors.Transparent"/>.</returns>
    private static SKColor ParseColor(string hex)
    {
        if (SKColor.TryParse(hex, out var c))
        {
            return c;
        }

        return SKColors.Transparent;
    }

    /// <summary>
    /// Draws a grid on the provided canvas based on the specified settings.
    /// </summary>
    /// <param name="canvas">Canvas where the grid will be drawn.</param>
    /// <param name="width">The width of the exported image, in pixels. Must be greater than 0.</param>
    /// <param name="height">The height of the exported image, in pixels. Must be greater than 0.</param>
    /// <param name="settings">The settings that define the appearance and behavior of the signature. Cannot be null.</param>
    private static void DrawGrid(SKCanvas canvas, int width, int height, SignatureSettings settings)
    {
        if (!settings.ShowGrid || settings.GridSpacing <= 0)
        {
            return;
        }

        using var paint = new SKPaint
        {
            Color = ParseColor(settings.GridColor).WithAlpha((byte)(settings.GridOpacity * 255)),
            IsStroke = true,
            StrokeWidth = 1,
            IsAntialias = true
        };

        switch (settings.GridType)
        {
            case SignatureGridType.Lines:
                DrawGridLines(canvas, width, height, paint, settings.GridSpacing);
                break;
            case SignatureGridType.Dots:
                DrawGridDots(canvas, width, height, paint, settings.GridSpacing);
                break;
        }
    }

    /// <summary>
    /// Draws a grid of dots on the provided canvas.
    /// </summary>
    /// <param name="canvas">Canvas where the grid will be drawn.</param>
    /// <param name="width">The width of the exported image, in pixels. Must be greater than 0.</param>
    /// <param name="height">The height of the exported image, in pixels. Must be greater than 0.</param>
    /// <param name="paint">Paint information.</param>
    /// <param name="gridSpacing">Spacing between each dot.</param>
    private static void DrawGridDots(SKCanvas canvas, int width, int height, SKPaint paint, int gridSpacing)
    {
        paint.IsStroke = false;

        for (var x = 0; x <= width; x += gridSpacing)
        {
            for (var y = 0; y <= height; y += gridSpacing)
            {
                canvas.DrawCircle(x, y, 1.5f, paint);
            }
        }
    }

    /// <summary>
    /// Draws a grid of lines on the provided canvas.
    /// </summary>
    /// <param name="canvas">Canvas where the grid will be drawn.</param>
    /// <param name="width">The width of the exported image, in pixels. Must be greater than 0.</param>
    /// <param name="height">The height of the exported image, in pixels. Must be greater than 0.</param>
    /// <param name="paint">Paint information.</param>
    /// <param name="gridSpacing">Spacing between each dot.</param>
    private static void DrawGridLines(SKCanvas canvas, int width, int height, SKPaint paint, int gridSpacing)
    {
        for (var x = 0; x <= width; x += gridSpacing)
        {
            canvas.DrawLine(x, 0, x, height, paint);
        }

        for (var y = 0; y <= height; y += gridSpacing)
        {
            canvas.DrawLine(0, y, width, y, paint);
        }
    }

    /// <summary>
    /// Draws a separator line on the provided canvas based on the specified settings.
    /// </summary>
    /// <param name="canvas">Canvas where the grid will be drawn.</param>
    /// <param name="width">The width of the exported image, in pixels. Must be greater than 0.</param>
    /// <param name="height">The height of the exported image, in pixels. Must be greater than 0.</param>
    /// <param name="settings">The settings that define the appearance and behavior of the signature. Cannot be null.</param>
    private static void DrawSeparator(SKCanvas canvas, int width, int height, SignatureSettings settings)
    {
        if (!settings.ShowSeparatorLine)
        {
            return;
        }

        using var paintSep = new SKPaint
        {
            Color = ParseColor(settings.SeparatorLineColor),
            IsStroke = true,
            StrokeWidth = 1,
            PathEffect = SKPathEffect.CreateDash([6, 6], 0),
            IsAntialias = true
        };

        var y = (float)(settings.SeparatorY * height);
        canvas.DrawLine(0, y, width, y, paintSep);
    }

    /// <summary>
    /// Draws a watermark on the provided canvas based on the specified settings.
    /// </summary>
    /// <param name="canvas">Canvas where the grid will be drawn.</param>
    /// <param name="width">The width of the exported image, in pixels. Must be greater than 0.</param>
    /// <param name="height">The height of the exported image, in pixels. Must be greater than 0.</param>
    /// <param name="settings">The settings that define the appearance and behavior of the signature. Cannot be null.</param>
    private static void DrawWatermark(SKCanvas canvas, int width, int height, SignatureWatermarkSettings settings)
    {
        if (settings.Image is not null && settings.Image.Length > 0)
        {
            using var bmp = SKBitmap.Decode(settings.Image);

            if (bmp is not null)
            {
                using var p = new SKPaint { Color = SKColors.White.WithAlpha((byte)(settings.Opacity * 255)) };
                var rect = new SKRect(0, 0, width, height);
                canvas.DrawBitmap(bmp, rect, p);
            }
        }

        if (!string.IsNullOrWhiteSpace(settings.Text))
        {
            using var font = new SKFont
            {
                Size = 32,
                Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold)
            };

            using var paint = new SKPaint
            {
                Color = ParseColor(settings.Color).WithAlpha((byte)(settings.Opacity * 255)),
                IsAntialias = true
            };

            var text = settings.Text;
            font.MeasureText(text, out var bounds);
            canvas.Save();
            canvas.RotateDegrees(-30, width / 2f, height / 2f);
            canvas.DrawText(text, width / 2f - bounds.MidX, height / 2f - bounds.MidY, font, paint);
            canvas.Restore();
        }
    }

    /// <summary>
    /// Draws the signature strokes on the provided canvas based on the specified settings.
    /// </summary>
    /// <param name="canvas">Canvas where the grid will be drawn.</param>
    /// <param name="strokes">Strokes to draw on the canvas.</param>
    /// <param name="settings">The settings that define the appearance and behavior of the signature. Cannot be null.</param>
    private static void DrawStrokes(SKCanvas canvas, List<SignatureStroke> strokes, SignatureSettings settings)
    {
        canvas.SaveLayer(null);

        foreach (var s in strokes.Where(x => !x.Eraser))
        {
            if (s.Points.Count < 2)
            {
                continue;
            }

            using var paint = new SKPaint
            {
                IsStroke = true,
                Color = ParseColor(s.Color).WithAlpha((byte)(s.Opacity * 255)),
                StrokeWidth = s.Width,
                IsAntialias = true,
                StrokeCap = SKStrokeCap.Round,
                StrokeJoin = SKStrokeJoin.Round,
            };

            if (s.LineStyle == SignatureLineStyle.Dashed)
            {
                paint.PathEffect = SKPathEffect.CreateDash([s.Width * 2, s.Width * 2], 0);
            }
            else if (s.LineStyle == SignatureLineStyle.Dotted)
            {
                paint.PathEffect = SKPathEffect.CreateDash([s.Width, s.Width * 2], 0);
            }

            if (settings.UseShadow && !s.Eraser)
            {
                paint.ImageFilter = SKImageFilter.CreateDropShadow(0, 0, 1.5f, 1.5f, ParseColor(settings.ShadowColor).WithAlpha((byte)(255 * settings.ShadowOpacity)));
            }

            using var path = new SKPath();
            path.MoveTo(s.Points[0].X, s.Points[0].Y);

            if (s.Smooth &&
                s.Points.Count >= 3)
            {
                for (var i = 1; i < s.Points.Count - 1; i++)
                {
                    var p1 = s.Points[i];
                    var p2 = s.Points[i + 1];
                    path.QuadTo(p1.X, p1.Y, (p1.X + p2.X) / 2f, (p1.Y + p2.Y) / 2f);
                }

                var last = s.Points[^1];
                path.LineTo(last.X, last.Y);
            }
            else
            {
                for (var i = 1; i < s.Points.Count; i++)
                {
                    var p = s.Points[i];
                    path.LineTo(p.X, p.Y);
                }
            }

            canvas.DrawPath(path, paint);
        }

        foreach (var s in strokes.Where(x => x.Eraser))
        {
            if (s.Points.Count < 2)
            {
                continue;
            }

            using var paint = new SKPaint
            {
                BlendMode = SKBlendMode.Clear,
                IsStroke = true,
                IsAntialias = true,
                StrokeCap = SKStrokeCap.Round,
                StrokeJoin = SKStrokeJoin.Round,
            };

            if (s.LineStyle == SignatureLineStyle.Dashed)
            {
                paint.PathEffect = SKPathEffect.CreateDash([s.Width * 2, s.Width * 2], 0);
            }
            else if (s.LineStyle == SignatureLineStyle.Dotted)
            {
                paint.PathEffect = SKPathEffect.CreateDash([s.Width, s.Width * 2], 0);
            }

            using var path = new SKPath();
            path.MoveTo(s.Points[0].X, s.Points[0].Y);

            if (s.Smooth &&
                s.Points.Count >= 3)
            {
                for (var i = 1; i < s.Points.Count - 1; i++)
                {
                    var p1 = s.Points[i];
                    var p2 = s.Points[i + 1];
                    path.QuadTo(p1.X, p1.Y, (p1.X + p2.X) / 2f, (p1.Y + p2.Y) / 2f);
                }

                var last = s.Points[^1];
                path.LineTo(last.X, last.Y);
            }
            else
            {
                for (var i = 1; i < s.Points.Count; i++)
                {
                    var p = s.Points[i];
                    path.LineTo(p.X, p.Y);
                }
            }

            canvas.DrawPath(path, paint);
        }

        canvas.Restore();
    }
}
