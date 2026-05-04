using System.Globalization;
using System.Xml.Linq;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to import signature strokes from SVG path data.
/// </summary>
/// <remarks>This class parses SVG path data and converts it into a collection of signature strokes for use in
/// digital signature scenarios. It is designed to be used where SVG-based signature capture or import is required.
/// Instances of this class are thread-safe for concurrent use.</remarks>
public sealed partial class SvgSignatureImporter : SvgSurfaceImporterBase<StrokeLayerPayload>
{
    /// <inheritdoc />
    protected override StrokeLayerPayload ParseContent(XElement content)
    {
        var strokes = new List<StrokePayload>();

        foreach (var g in content.Elements("g"))
        {
            var stroke = ParseStroke(g);

            if (stroke is not null)
            {
                strokes.Add(stroke);
            }
        }

        return new StrokeLayerPayload { Strokes = strokes };
    }

    /// <summary>
    /// Parses a stroke definition from the specified XML element and returns a corresponding StrokePayload instance if
    /// valid.
    /// </summary>
    /// <remarks>The method requires the input element to contain at least two points to be considered a valid
    /// stroke. If the element does not meet this requirement, the method returns null.</remarks>
    /// <param name="g">The XML element representing a stroke group, expected to contain stroke attributes and child path elements.</param>
    /// <returns>A StrokePayload object containing the parsed stroke data if the element defines at least two points; otherwise,
    /// null.</returns>
    private static StrokePayload? ParseStroke(XElement g)
    {
        var id = g.Attribute("data-stroke-id")?.Value ?? Guid.NewGuid().ToString();
        var blendMode = ParseBlendMode(g);
        var pen = ParsePen(g);
        var points = ParsePoints(g);

        if (points.Count < 2)
        {
            return null;
        }

        return new StrokePayload
        {
            Id = id,
            BlendMode = !string.IsNullOrEmpty(blendMode) ? blendMode : "source-over",
            Pen = pen,
            Points = points
        };
    }

    /// <summary>
    /// Extracts the value of the 'mix-blend-mode' CSS property from the specified XML element's style attribute.
    /// </summary>
    /// <remarks>The method searches for the 'mix-blend-mode' property within the 'style' attribute of the
    /// provided element. If the property is not present or the 'style' attribute is missing, the method returns
    /// null.</remarks>
    /// <param name="g">The XML element containing a 'style' attribute to parse for the 'mix-blend-mode' property.</param>
    /// <returns>A string representing the blend mode value if found; otherwise, null.</returns>
    private static string? ParseBlendMode(XElement g)
    {
        var style = g.Attribute("style")?.Value;

        if (style is null)
        {
            return null;
        }

        // style="mix-blend-mode:multiply;"
        var prefix = "mix-blend-mode:";
        var idx = style.IndexOf(prefix, StringComparison.OrdinalIgnoreCase);

        if (idx < 0)
        {
            return null;
        }

        var start = idx + prefix.Length;
        var end = style.IndexOf(';', start);

        if (end < 0)
        {
            end = style.Length;
        }

        return style[start..end].Trim();
    }

    /// <summary>
    /// Parses pen attributes from the specified SVG group element and returns a corresponding PenPayload instance.
    /// </summary>
    /// <remarks>The method extracts attributes such as stroke color, opacity, line cap, line join, dash
    /// array, and shadow from the first &lt;path&gt; element within the provided group. Default values are used if specific
    /// attributes are missing.</remarks>
    /// <param name="g">The SVG group element containing one or more &lt;path&gt; elements from which pen attributes are extracted.</param>
    /// <returns>A PenPayload object representing the parsed pen attributes. If no &lt;path&gt; element is found, returns a PenPayload
    /// with default values.</returns>
    private static PenPayload ParsePen(XElement g)
    {
        // Les <path> contiennent les attributs du pen
        var firstPath = g.Element("path");
        if (firstPath is null)
        {
            return new PenPayload();
        }

        var color = firstPath.Attribute("stroke")?.Value ?? "#000";
        var opacity = TryParseDouble(firstPath, "opacity") ?? 1.0;
        var linecap = firstPath.Attribute("stroke-linecap")?.Value ?? "round";
        var linejoin = firstPath.Attribute("stroke-linejoin")?.Value ?? "round";
        var dashArray = SignatureMathUtils.ToDashArray(firstPath.Attribute("stroke-dasharray")?.Value);

        // Shadow
        var shadow = ParseShadow(g);

        return new PenPayload
        {
            Color = color,
            Opacity = opacity,
            LineCap = Enum.TryParse<LineCap>(linecap, out var lc) ? lc : LineCap.Round,
            LineJoin = Enum.TryParse<LineJoin>(linejoin, out var lj) ? lj : LineJoin.Round,
            DashArray = dashArray,
            Shadow = shadow
        };
    }

    /// <summary>
    /// Parses shadow-related attributes from the specified XML element and returns a corresponding ShadowPayload
    /// instance.
    /// </summary>
    /// <remarks>If an expected attribute is not present or cannot be parsed, a default value is assigned for
    /// that property in the returned ShadowPayload.</remarks>
    /// <param name="g">The XML element containing shadow attribute data to parse. Must not be null.</param>
    /// <returns>A ShadowPayload object populated with values extracted from the provided XML element. Default values are used if
    /// attributes are missing or invalid.</returns>
    private static ShadowPayload ParseShadow(XElement g)
    {
        var enabled = TryParseBool(g, "data-shadow-enabled") ?? false;

        return new ShadowPayload
        {
            Enabled = enabled,
            Color = g.Attribute("data-shadow-color")?.Value ?? "#000000",
            Opacity = TryParseDouble(g, "data-shadow-opacity") ?? 0,
            Blur = TryParseDouble(g, "data-shadow-blur") ?? 0,
            OffsetX = TryParseDouble(g, "data-shadow-offsetX") ?? 0,
            OffsetY = TryParseDouble(g, "data-shadow-offsetY") ?? 0
        };
    }

    /// <summary>
    /// Parses the specified SVG group element and extracts stroke point data from its child path elements.
    /// </summary>
    /// <remarks>Each path element is expected to have a 'd' attribute describing a line segment and may
    /// include a 'stroke-width' attribute. Only path elements with valid 'd' attributes are processed.</remarks>
    /// <param name="g">The SVG group element containing path elements to parse for stroke point data. Cannot be null.</param>
    /// <returns>A list of stroke point payloads representing the start and end points of each path element found in the group.
    /// The list is empty if no valid path elements are present.</returns>
    private static List<StrokePointPayload> ParsePoints(XElement g)
    {
        var points = new List<StrokePointPayload>();

        foreach (var path in g.Elements("path"))
        {
            var d = path.Attribute("d")?.Value;

            if (string.IsNullOrWhiteSpace(d))
            {
                continue;
            }

            // stroke-width = largeur moyenne du segment
            var w = TryParseDouble(path, "stroke-width") ?? 1.0;

            // d="M x0 y0 L x1 y1"
            var coords = ParsePathD(d);

            if (coords is null)
            {
                continue;
            }

            var (x0, y0, x1, y1) = coords.Value;

            points.Add(new StrokePointPayload { X = x0, Y = y0, W = w });
            points.Add(new StrokePointPayload { X = x1, Y = y1, W = w });
        }

        return points;
    }

    /// <summary>
    /// Parses a path data string in the format "M {x0} {y0} L {x1} {y1}" and extracts the corresponding coordinate
    /// values.
    /// </summary>
    /// <remarks>The method expects the input string to strictly follow the specified format with
    /// space-separated values. Parsing fails if the format is incorrect or if any coordinate value cannot be converted
    /// to a double.</remarks>
    /// <param name="d">The path data string to parse. Must be in the format "M {x0} {y0} L {x1} {y1}" with numeric values for
    /// coordinates.</param>
    /// <returns>A tuple containing the parsed coordinates (x0, y0, x1, y1) if the input string is valid; otherwise, null.</returns>
    private static (double x0, double y0, double x1, double y1)? ParsePathD(string d)
    {
        // Format exact exporté :
        // M {x0} {y0} L {x1} {y1}
        var parts = d.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < 5)
        {
            return null;
        }

        if (parts[0] != "M" || parts[3] != "L")
        {
            return null;
        }

        if (!double.TryParse(parts[1], NumberStyles.Any, s_culture, out var x0))
        {
            return null;
        }

        if (!double.TryParse(parts[2], NumberStyles.Any, s_culture, out var y0))
        {
            return null;
        }

        if (!double.TryParse(parts[4], NumberStyles.Any, s_culture, out var x1))
        {
            return null;
        }

        if (!double.TryParse(parts[5], NumberStyles.Any, s_culture, out var y1))
        {
            return null;
        }

        return (x0, y0, x1, y1);
    }

    /// <summary>
    /// Attempts to parse the value of the specified attribute as a double-precision floating-point number.
    /// </summary>
    /// <remarks>Parsing uses culture-specific formatting as defined by the current culture
    /// settings.</remarks>
    /// <param name="e">The XML element containing the attribute to parse.</param>
    /// <param name="attr">The name of the attribute whose value is to be parsed as a double.</param>
    /// <returns>A double-precision floating-point number if the attribute value is successfully parsed; otherwise, null.</returns>
    private static double? TryParseDouble(XElement e, string attr)
    {
        var v = e.Attribute(attr)?.Value;

        return double.TryParse(v, NumberStyles.Any, s_culture, out var r) ? r : null;
    }

    /// <summary>
    /// Attempts to parse the value of the specified attribute as a Boolean value.
    /// </summary>
    /// <remarks>Returns null if the attribute is missing or its value cannot be parsed as a Boolean. This
    /// method does not throw an exception for missing or invalid attributes.</remarks>
    /// <param name="e">The XML element containing the attribute to parse.</param>
    /// <param name="attr">The name of the attribute whose value is to be parsed as a Boolean.</param>
    /// <returns>A Boolean value if the attribute exists and can be parsed; otherwise, null.</returns>
    private static bool? TryParseBool(XElement e, string attr)
    {
        var v = e.Attribute(attr)?.Value;

        return bool.TryParse(v, out var r) ? r : null;
    }
}
