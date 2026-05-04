using System.Text;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality for building SVG payloads representing signature stroke layers for export operations.
/// </summary>
/// <remarks>This class specializes the SVG document payload builder to handle signature stroke layers, enabling
/// the generation of SVG content suitable for digital signature scenarios. Use this class when exporting signature data
/// as SVG, typically in applications requiring handwritten input or electronic signing features.</remarks>
public sealed class SignatureSvgPayloadBuilder : SvgDocumentPayloadBuilderBase<StrokeLayerPayload>
{
    /// <inheritdoc />
    protected override void WriteContent(
        StringBuilder sb,
        StrokeLayerPayload payload,
        SurfaceExportOptions options)
    {
        if (payload.Strokes is null || payload.Strokes.Count == 0)
        {
            return;
        }

        foreach (var stroke in payload.Strokes)
        {
            WriteStroke(sb, stroke);
        }
    }

    /// <summary>
    /// Writes the SVG representation of a stroke to the specified StringBuilder, including path segments, styling, and
    /// optional shadow effects.
    /// </summary>
    /// <remarks>The method generates SVG markup for each segment of the stroke, reflecting dynamic width and
    /// style. If the stroke includes a shadow, a filter is applied. The stroke is skipped if it contains fewer than two
    /// points.</remarks>
    /// <param name="sb">The StringBuilder to which the SVG markup for the stroke will be appended.</param>
    /// <param name="stroke">The stroke payload containing the points, pen settings, and rendering options to be written as SVG.</param>
    private static void WriteStroke(StringBuilder sb, StrokePayload stroke)
    {
        if (stroke.Points.Count < 2)
        {
            return;
        }

        var pen = stroke.Pen;
        var shadowId = $"shadow-{stroke.Id}";

        if (pen.Shadow.Enabled && pen.Shadow.Opacity > 0)
        {
            WriteShadowFilter(sb, pen.Shadow, shadowId);
        }

        sb.Append("<g id=\"surface-content\" ");
        sb.Append(s_culture, $"data-stroke-id=\"{stroke.Id}\" ");
        sb.Append(s_culture, $"data-shadow-enabled=\"{pen.Shadow.Enabled}\" ");
        sb.Append(s_culture, $"data-shadow-color=\"{pen.Shadow.Color}\" ");
        sb.Append(s_culture, $"data-shadow-opacity=\"{pen.Shadow.Opacity}\" ");
        sb.Append(s_culture, $"data-shadow-blur=\"{pen.Shadow.Blur}\" ");
        sb.Append(s_culture, $"data-shadow-offsetX=\"{pen.Shadow.OffsetX}\" ");
        sb.Append(s_culture, $"data-shadow-offsetY=\"{pen.Shadow.OffsetY}\" ");

        if (!string.IsNullOrWhiteSpace(stroke.BlendMode))
        {
            sb.Append(s_culture, $"style=\"mix-blend-mode:{stroke.BlendMode};\" ");
        }

        if (pen.Shadow.Enabled && pen.Shadow.Opacity > 0)
        {
            sb.Append(s_culture, $"filter=\"url(#{shadowId})\" ");
        }

        sb.AppendLine(">");

        for (var i = 0; i < stroke.Points.Count - 1; i++)
        {
            var p0 = stroke.Points[i];
            var p1 = stroke.Points[i + 1];
            var w = (p0.W + p1.W) / 2;

            sb.Append("<path ");

            sb.Append(s_culture, $"stroke=\"{pen.Color}\" ");
            sb.Append(s_culture, $"stroke-width=\"{w}\" ");
            sb.Append(s_culture, $"opacity=\"{pen.Opacity}\" ");
            sb.Append(s_culture, $"stroke-linecap=\"{pen.LineCap}\" ");
            sb.Append(s_culture, $"stroke-linejoin=\"{pen.LineJoin}\" ");
            sb.Append("fill=\"none\" ");

            if (pen.DashArray.Length > 0)
            {
                sb.Append(s_culture, $"stroke-dasharray=\"{string.Join(",", pen.DashArray)}\" ");
            }

            sb.Append("d=\"");
            sb.Append(s_culture, $"M {p0.X} {p0.Y} ");
            sb.Append(s_culture, $"L {p1.X} {p1.Y} ");
            sb.AppendLine("\" />");
        }

        sb.AppendLine("</g>");
    }

    /// <summary>
    /// Writes an SVG filter definition for a drop shadow effect to the specified StringBuilder.
    /// </summary>
    /// <remarks>The filter is written with extended bounds to ensure the shadow is not clipped. The resulting
    /// markup can be used in SVG documents to apply drop shadow effects to elements.</remarks>
    /// <param name="sb">The StringBuilder to which the SVG filter markup will be appended. Must not be null.</param>
    /// <param name="shadow">The shadow payload containing the parameters for the drop shadow effect, such as offset, blur, color, and
    /// opacity.</param>
    /// <param name="id">The unique identifier for the filter element. Used as the value of the filter's 'id' attribute.</param>
    private static void WriteShadowFilter(StringBuilder sb, ShadowPayload shadow, string id)
    {
        sb.AppendLine(s_culture, $"<filter id=\"{id}\" x=\"-50%\" y=\"-50%\" width=\"200%\" height=\"200%\">");
        sb.AppendLine(s_culture, $"<feDropShadow dx=\"{shadow.OffsetX}\" ")
          .AppendLine(s_culture, $"dy=\"{shadow.OffsetY}\" ")
          .AppendLine(s_culture, $"stdDeviation=\"{shadow.Blur}\" ")
          .AppendLine(s_culture, $"flood-color=\"{shadow.Color}\" ")
          .AppendLine(s_culture, $"flood-opacity=\"{shadow.Opacity}\" />");
        sb.AppendLine("</filter>");
    }
}
