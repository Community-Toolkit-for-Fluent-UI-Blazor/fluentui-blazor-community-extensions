using System.Globalization;
using System.Runtime.CompilerServices;

namespace FluentUI.Blazor.Community.Components;

internal static class SignatureSvgExporter
{
    private static readonly CultureInfo InvariantCulture = CultureInfo.InvariantCulture;

    internal static (byte[] bytes, string mime, string filename) Export(
        int width,
        int height,
        List<SignatureStroke> strokes,
        SignatureSettings signatureSettings,
        SignatureWatermarkSettings watermarkSettings)
    {
        var builder = new DefaultInterpolatedStringHandler();
        builder.AppendLiteral("<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"");
        builder.AppendFormatted(width);
        builder.AppendLiteral("\" height=\"");
        builder.AppendFormatted(height);
        builder.AppendLiteral("\" viewBox=\"0 0 ");
        builder.AppendFormatted(width);
        builder.AppendLiteral(" ");
        builder.AppendFormatted(height);
        builder.AppendLiteral("\">");

        // Background
        if (signatureSettings.BackgroundImage is null)
        {
            builder.AppendLiteral("<rect width=\"100%\" height=\"100%\" fill=\"");
            builder.AppendFormatted(signatureSettings.BackgroundColor);
            builder.AppendLiteral("\"/>");
        }
        else
        {
            builder.AppendLiteral("<image href=\"data:image;base64,");
            builder.AppendFormatted(Convert.ToBase64String(signatureSettings.BackgroundImage));
            builder.AppendLiteral("\" x=\"0\" y=\"0\" width=\"");
            builder.AppendFormatted(width);
            builder.AppendLiteral("\" height=\"");
            builder.AppendFormatted(height);
            builder.AppendLiteral("\" />");
        }

        // Grid
        if (signatureSettings.ShowGrid && signatureSettings.GridSpacing > 0)
        {
            var gridColor = signatureSettings.GridColor;
            var op = signatureSettings.GridOpacity.ToString(InvariantCulture);

            if (signatureSettings.GridType == SignatureGridType.Lines)
            {
                for (var x = 0; x <= width; x += signatureSettings.GridSpacing)
                {
                    builder.AppendLiteral("<line x1=\"");
                    builder.AppendFormatted(x);
                    builder.AppendLiteral("\" y1=\"0\" x2=\"");
                    builder.AppendFormatted(x);
                    builder.AppendLiteral("\" y2=\"");
                    builder.AppendFormatted(height);
                    builder.AppendLiteral("\" stroke=\"");
                    builder.AppendFormatted(gridColor);
                    builder.AppendLiteral("\" stroke-opacity=\"");
                    builder.AppendFormatted(op);
                    builder.AppendLiteral("\" stroke-width=\"1\"/>");
                }

                for (var y = 0; y <= height; y += signatureSettings.GridSpacing)
                {
                    builder.AppendLiteral("<line x1=\"0\" y1=\"");
                    builder.AppendFormatted(y);
                    builder.AppendLiteral("\" x2=\"");
                    builder.AppendFormatted(width);
                    builder.AppendLiteral("\" y2=\"");
                    builder.AppendFormatted(y);
                    builder.AppendLiteral("\" stroke=\"");
                    builder.AppendFormatted(gridColor);
                    builder.AppendLiteral("\" stroke-opacity=\"");
                    builder.AppendFormatted(op);
                    builder.AppendLiteral("\" stroke-width=\"1\"/>");
                }
            }
            else
            {
                for (var x = 0; x <= width; x += signatureSettings.GridSpacing)
                {
                    for (var y = 0; y <= height; y += signatureSettings.GridSpacing)
                    {
                        builder.AppendLiteral("<circle cx=\"");
                        builder.AppendFormatted(x);
                        builder.AppendLiteral("\" cy=\"");
                        builder.AppendFormatted(y);
                        builder.AppendLiteral("\" r=\"1.5\" fill=\"");
                        builder.AppendFormatted(gridColor);
                        builder.AppendLiteral("\" fill-opacity=\"");
                        builder.AppendFormatted(op);
                        builder.AppendLiteral("\"/>");
                    }
                }
            }    
        }

        // Separator
        if (signatureSettings.ShowSeparatorLine)
        {
            var sepY = (int)(signatureSettings.SeparatorY * height);
            builder.AppendLiteral("<line x1=\"0\" y1=\"");
            builder.AppendFormatted(sepY);
            builder.AppendLiteral("\" x2=\"");
            builder.AppendFormatted(width);
            builder.AppendLiteral("\" y2=\"");
            builder.AppendFormatted(sepY);
            builder.AppendLiteral("\" stroke=\"");
            builder.AppendFormatted(signatureSettings.SeparatorLineColor);
            builder.AppendLiteral("\" stroke-width=\"1\" stroke-dasharray=\"6,6\"/>");
        }

        // Mask for eraser strokes
        var anyEraser = strokes.Exists(s => s.Eraser);

        if (anyEraser)
        {
            builder.AppendLiteral("<defs><mask id=\"eraserMask\">");
            builder.AppendLiteral("<rect width=\"100%\" height=\"100%\" fill=\"white\"/>");

            foreach (var stroke in strokes)
            {
                if (!stroke.Eraser || stroke.Points.Count < 2)
                {
                    continue;
                }

                var pathData = BuildSvgPathData(stroke);
                var opacity = stroke.Opacity.ToString(InvariantCulture);
                var dash = SvgDashArray(stroke.LineStyle, stroke.Width);
                builder.AppendLiteral("<path d=\"");
                builder.AppendFormatted(pathData);
                builder.AppendLiteral("\" fill=\"none\" stroke=\"black\" stroke-opacity=\"");
                builder.AppendFormatted(opacity);
                builder.AppendLiteral("\" stroke-width=\"");
                builder.AppendFormatted(stroke.Width.ToString(InvariantCulture));
                builder.AppendLiteral("\" stroke-linecap=\"round\" stroke-linejoin=\"round\" ");
                builder.AppendFormatted(dash);
                builder.AppendLiteral("/>");
            }

            builder.AppendLiteral("</mask></defs>");
            builder.AppendLiteral("<g mask=\"url(#eraserMask)\">");
        }

        // Strokes
        foreach (var stroke in strokes)
        {
            if (stroke.Eraser || stroke.Points.Count < 2)
            {
                continue;
            }

            var pathData = BuildSvgPathData(stroke);
            var opacity = stroke.Opacity.ToString(InvariantCulture);
            var dash = SvgDashArray(stroke.LineStyle, stroke.Width);
            builder.AppendLiteral("<path d=\"");
            builder.AppendFormatted(pathData);
            builder.AppendLiteral("\" fill=\"none\" stroke=\"");
            builder.AppendFormatted(stroke.Color);
            builder.AppendLiteral("\" stroke-opacity=\"");
            builder.AppendFormatted(opacity);
            builder.AppendLiteral("\" stroke-width=\"");
            builder.AppendFormatted(stroke.Width.ToString(InvariantCulture));
            builder.AppendLiteral("\" stroke-linecap=\"round\" stroke-linejoin=\"round\" ");
            builder.AppendFormatted(dash);
            builder.AppendLiteral("/>");
        }

        if (anyEraser)
        {
            builder.AppendLiteral("</g>");
        }

        // Watermark
        if (watermarkSettings.Image is not null &&
            watermarkSettings.Image.Length > 0)
        {
            var b64wm = Convert.ToBase64String(watermarkSettings.Image);
            var wmOpacity = watermarkSettings.Opacity.ToString(InvariantCulture);
            builder.AppendLiteral("<image href=\"data:image;base64,");
            builder.AppendFormatted(b64wm);
            builder.AppendLiteral("\" x=\"0\" y=\"0\" width=\"");
            builder.AppendFormatted(width);
            builder.AppendLiteral("\" height=\"");
            builder.AppendFormatted(height);
            builder.AppendLiteral("\" opacity=\"");
            builder.AppendFormatted(wmOpacity);
            builder.AppendLiteral("\"/>");
        }

        if (!string.IsNullOrWhiteSpace(watermarkSettings.Text))
        {
            var wmOpacity = watermarkSettings.Opacity.ToString(InvariantCulture);
            builder.AppendLiteral("<g opacity=\"");
            builder.AppendFormatted(wmOpacity);
            builder.AppendLiteral("\" transform=\"translate(");
            builder.AppendFormatted(width / 2);
            builder.AppendLiteral(",");
            builder.AppendFormatted(height / 2);
            builder.AppendLiteral(") rotate(-30)\">");
            builder.AppendLiteral("<text x=\"0\" y=\"0\" text-anchor=\"middle\" dominant-baseline=\"middle\" font-family=\"Arial\" font-size=\"32\" font-weight=\"700\" fill=\"#000000\">");
            builder.AppendFormatted(System.Security.SecurityElement.Escape(watermarkSettings.Text));
            builder.AppendLiteral("</text></g>");
        }

        builder.AppendLiteral("</svg>");

        var svgString = builder.ToStringAndClear();
        var svgBytes = System.Text.Encoding.UTF8.GetBytes(svgString);

        return (svgBytes, "image/svg+xml", "signature.svg");
    }

    private static string SvgDashArray(SignatureLineStyle style, float width)
    {
        return style switch
        {
            SignatureLineStyle.Dashed => $"""stroke-dasharray="{(width * 2).ToString(InvariantCulture)},{(width * 2).ToString(InvariantCulture)}" """,
            SignatureLineStyle.Dotted => $"""stroke-dasharray="{width.ToString(InvariantCulture)},{(width * 2).ToString(InvariantCulture)}" """,
            _ => string.Empty
        };
    }

    private static string BuildSvgPathData(SignatureStroke s)
    {
        var sb = new System.Text.StringBuilder();
        sb.Append(InvariantCulture, $"M {s.Points[0].X.ToString(InvariantCulture)} {s.Points[0].Y.ToString(InvariantCulture)} ");

        if (s.Smooth && s.Points.Count >= 3)
        {
            for (var i = 1; i < s.Points.Count - 1; i++)
            {
                var p1 = s.Points[i];
                var p2 = s.Points[i + 1];
                var mx = (p1.X + p2.X) / 2f;
                var my = (p1.Y + p2.Y) / 2f;

                sb.Append(InvariantCulture, $"Q {p1.X.ToString(InvariantCulture)} {p1.Y.ToString(InvariantCulture)} {mx.ToString(InvariantCulture)} {my.ToString(InvariantCulture)} ");
            }

            var last = s.Points[^1];
            sb.Append(InvariantCulture, $"L {last.X.ToString(InvariantCulture)} {last.Y.ToString(InvariantCulture)} ");
        }
        else
        {
            for (var i = 1; i < s.Points.Count; i++)
            {
                var p = s.Points[i];
                sb.Append(InvariantCulture, $"L {p.X.ToString(InvariantCulture)} {p.Y.ToString(InvariantCulture)} ");
            }
        }

        return sb.ToString().TrimEnd();
    }
}
