using System.Globalization;
using System.Xml.Linq;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a base class for importing surfaces from SVG documents.
/// </summary>
/// <remarks>This abstract class defines the common functionality for SVG surface importers. Derived classes
/// should implement the parsing logic specific to their requirements.</remarks>
/// <typeparam name="TPayload">The type of the payload associated with the imported surface.</typeparam>
public abstract class SvgSurfaceImporterBase<TPayload> : SurfaceImporter<TPayload>
{
    /// <summary>
    /// Represents the svg document being imported.
    /// </summary>
    private XDocument? _document;

    /// <summary>
    /// Represents the invariant culture, which is culture-insensitive and associated with the English language but not
    /// with any country or region.
    /// </summary>
    /// <remarks>The invariant culture is used in operations that require culture-independent results, such as
    /// formatting and parsing operations that must yield consistent results regardless of the system's culture
    /// settings.</remarks>
    protected static readonly CultureInfo s_culture = CultureInfo.InvariantCulture;

    /// <inheritdoc />
    protected override string ErrorMessage => "The input is not a valid SVG document.";

    /// <summary>
    /// Attempts to parse the value of the specified attribute from the given XML element as a double-precision
    /// floating-point number.
    /// </summary>
    /// <param name="root">The XML element containing the attribute to parse.</param>
    /// <param name="attribute">The name of the attribute whose value is to be parsed as a double.</param>
    /// <returns>A double-precision floating-point number if the attribute exists and can be parsed; otherwise, null.</returns>
    private static double? TryParseDouble(XElement root, string attribute)
    {
        var value = root.Attribute(attribute)?.Value;

        if (value is null)
        {
            return null;
        }

        return double.TryParse(value, NumberStyles.Any, s_culture, out var result) ? result : null;
    }

    /// <summary>
    /// Attempts to parse the value of the specified attribute from the given XML element as an integer.
    /// </summary>
    /// <param name="root">The XML element containing the attribute to parse.</param>
    /// <param name="attribute">The name of the attribute whose value is to be parsed as a double.</param>
    /// <returns>A double-precision floating-point number if the attribute exists and can be parsed; otherwise, null.</returns>
    private static int? TryParseInt(XElement root, string attribute)
    {
        var v = root.Attribute(attribute)?.Value;

        return int.TryParse(v, NumberStyles.Any, s_culture, out var r) ? r : null;
    }

    /// <summary>
    /// Attempts to parse the value of the specified attribute as a Boolean value.
    /// </summary>
    /// <remarks>The method returns null if the attribute is missing or its value cannot be parsed as a
    /// Boolean. Parsing is case-insensitive and follows the standard Boolean values recognized by .NET, such as "true"
    /// or "false".</remarks>
    /// <param name="root">The XML element containing the attribute to parse.</param>
    /// <param name="attribute">The name of the attribute whose value is to be parsed as a Boolean.</param>
    /// <returns>A Boolean value if the attribute exists and can be parsed as a Boolean; otherwise, null.</returns>
    private static bool? TryParseBool(XElement root, string attribute)
    {
        var v = root.Attribute(attribute)?.Value;

        return bool.TryParse(v, out var r) ? r : null;
    }

    /// <summary>
    /// Attempts to parse the value of the specified XML attribute as an enumeration of type TEnum.
    /// </summary>
    /// <remarks>Parsing is case-sensitive and fails if the attribute is missing or its value does not match a
    /// valid enumeration name.</remarks>
    /// <typeparam name="TEnum">The enumeration type to parse. Must be a value type that is an enumeration.</typeparam>
    /// <param name="root">The XML element containing the attribute to parse.</param>
    /// <param name="attribute">The name of the attribute whose value is to be parsed as an enumeration.</param>
    /// <returns>A nullable value of type TEnum if parsing succeeds; otherwise, null.</returns>
    private static TEnum? TryParseEnum<TEnum>(XElement root, string attribute) where TEnum : struct
    {
        var v = root.Attribute(attribute)?.Value;

        return Enum.TryParse<TEnum>(v, out var r) ? r : null;
    }

    /// <inheritdoc />
    public sealed override bool IsValidInput(string input)
    {
        try
        {
            _document = XDocument.Parse(input);

            return _document.Root?.Name.LocalName == "svg";
        }
        catch
        {
            return false;
        }
    }

    /// <inheritdoc />
    protected sealed override async ValueTask<ImportSurfaceResult<TPayload>> ParseAsync(string input)
    {
        if (_document is null)
        {
            return new ImportSurfaceResult<TPayload> { ErrorMessage = ErrorMessage };
        }

        var result = new SurfacePayload<TPayload>
        {
            Background = ParseBackground(_document),
            Grid = ParseGrid(_document),
            Axes = ParseAxes(_document),
            Content = ParseContent(_document),
            View = ParseView(_document),
            Watermark = ParseWatermark(_document)
        };

        return new ImportSurfaceResult<TPayload>(result);
    }

    /// <summary>
    /// Parses a watermark payload from the specified XML document.
    /// </summary>
    /// <param name="document">The XML document containing the watermark data to parse. Cannot be null.</param>
    /// <returns>A <see cref="WatermarkPayload"/> instance representing the parsed watermark data if parsing is successful;
    /// otherwise, <see langword="null"/>.</returns>
    /// <exception cref="NotImplementedException">The method is not implemented.</exception>
    private static WatermarkPayload? ParseWatermark(XDocument document)
    {
        var g = document.Root!.Descendants().FirstOrDefault(e => (string?)e.Attribute("id") == "surface-watermark");

        if (g is null)
        {
            return null;
        }

        var payload = new WatermarkPayload
        {
            Opacity = TryParseDouble(g, "opacity") ?? 1.0,
            Repeat = TryParseBool(g, "data-reapeat") ?? false,
            Mode = TryParseEnum<WatermarkMode>(g, "data-mode") ?? WatermarkMode.Text,
            Color = g.Attribute("data-color")?.Value ?? "#000000",
            Text = g.Attribute("data-text")?.Value ?? "",
            TextOpacity = TryParseDouble(g, "data-text-opacity") ?? 1.0,
            FontSize = TryParseDouble(g, "data-font-size") ?? 16,
            FontFamily = g.Attribute("data-font-family")?.Value ?? "Arial",
            FontWeight = g.Attribute("data-font-weight")?.Value ?? "normal",
            LetterSpacing = TryParseDouble(g, "data-letter-spacing") ?? 0,
            Scale = TryParseDouble(g, "data-scale") ?? 1.0,
            Rotation = TryParseDouble(g, "data-rotation") ?? 0,
            ImageUrl = g.Attribute("data-url")?.Value ?? "",
            ImageOpacity = TryParseDouble(g, "data-image-opacity") ?? 1.0,
            PositionX = TryParseDouble(g, "data-position-x") ?? 0,
            PositionY = TryParseDouble(g, "data-position-y") ?? 0,
            RepeatSpacingX = TryParseDouble(g, "data-repeat-spacing-x") ?? 200,
            RepeatSpacingY = TryParseDouble(g, "data-repeat-spacing-y") ?? 200,
            HorizontalAlignment = TryParseInt(g, "data-horizontal-alignement") ?? 1,
            VerticalAlignment = TryParseInt(g, "data-vertical-alignement") ?? 1,
            VisualBias = TryParseDouble(g, "data-visual-bias") ?? 0
        };

        return payload;
    }

    /// <summary>
    /// Parses the specified XML document and extracts a view payload if available.
    /// </summary>
    /// <param name="document">The XML document to parse for view payload information. Cannot be null.</param>
    /// <returns>A <see cref="ViewPayload"/> instance containing the parsed view data if successful; otherwise, <see
    /// langword="null"/> if the document does not contain a valid view payload.</returns>
    /// <exception cref="NotImplementedException">Thrown in all cases as the method is not yet implemented.</exception>
    private static ViewPayload? ParseView(XDocument document)
    {
        var root = document.Root;

        if (root is null)
        {
            return null;
        }

        var widthAttr = root.Attribute("width")?.Value;
        var heightAttr = root.Attribute("height")?.Value;

        if (!double.TryParse(widthAttr, NumberStyles.Any, s_culture, out var width))
        {
            return null;
        }

        if (!double.TryParse(heightAttr, NumberStyles.Any, s_culture, out var height))
        {
            return null;
        }

        var renderWidth = TryParseDouble(root, "data-view-render-width");
        var renderHeight = TryParseDouble(root, "data-view-render-height");
        var scale = TryParseDouble(root, "data-view-scale");
        var dpi = TryParseDouble(root, "data-view-dpi");
        var offsetX = TryParseDouble(root, "data-view-offsetX");
        var offsetY = TryParseDouble(root, "data-view-offsetY");

        return new ViewPayload
        {
            Width = width,
            Height = height,
            RenderWidth = renderWidth ?? width,
            RenderHeight = renderHeight ?? height,
            Scale = scale ?? 1.0,
            Dpi = dpi ?? 96,
            OffsetX = offsetX ?? 0,
            OffsetY = offsetY ?? 0
        };
    }

    /// <summary>
    /// Parses the specified XML document to extract axes information and returns an associated payload object.
    /// </summary>
    /// <param name="document">The XML document containing axes data to parse. Cannot be null.</param>
    /// <returns>An instance of AxesPayload containing the parsed axes information from the XML document.</returns>
    private static AxesPayload? ParseAxes(XDocument document)
    {
        var g = document.Root!.Descendants().FirstOrDefault(e => ((string?)e.Attribute("id"))?.StartsWith("surface-axes-") == true);

        if (g is null)
        {
            return null;
        }

        var id = g.Attribute("id")!.Value;
        var layer = id.EndsWith("background") ? GridLayer.Background : GridLayer.Foreground;
        var color = g.Attribute("stroke")?.Value ?? "#000";
        var opacity = double.TryParse(g.Attribute("opacity")?.Value, NumberStyles.Any, s_culture, out var o) ? o : 1.0;
        var strokeWidth = double.TryParse(g.Attribute("stroke-width")?.Value, NumberStyles.Any, s_culture, out var sw) ? sw : 1.0;

        return new AxesPayload
        {
            Color = color,
            Opacity = opacity,
            StrokeWidth = strokeWidth,
            Layer = layer,
            DashArray = SignatureMathUtils.ToDashArray(g.Attribute("stroke-dasharray")?.Value)
        };
    }

    /// <summary>
    /// Parses the specified XML document and extracts grid data into a GridPayload object.
    /// </summary>
    /// <param name="document">The XML document containing the grid data to parse. Cannot be null.</param>
    /// <returns>A GridPayload object representing the parsed grid data, or null if the input does not contain valid grid
    /// information.</returns>
    private static GridPayload? ParseGrid(XDocument document)
    {
        var g = document.Root!.Descendants().FirstOrDefault(e => ((string?)e.Attribute("id"))?.StartsWith("surface-grid-") == true);

        if (g is null)
        {
            return null;
        }

        var payload = new GridPayload
        {
            Color = g.Attribute("stroke")?.Value ?? "#000",
            Opacity = double.TryParse(g.Attribute("opacity")?.Value, NumberStyles.Any, s_culture, out var o) ? o : 1.0,
            StrokeWidth = double.TryParse(g.Attribute("stroke-width")?.Value, NumberStyles.Any, s_culture, out var sw) ? sw : 1.0,
            DashArray = SignatureMathUtils.ToDashArray(g.Attribute("stroke-dasharray")?.Value),
            DisplayMode = Enum.Parse<GridDisplayMode>(g.Attribute("data-display-mode")?.Value ?? "Lines"),
            CellSize = double.Parse(g.Attribute("data-cell-size")?.Value ?? "20", s_culture),
            BoldEvery = int.Parse(g.Attribute("data-bold-every")?.Value ?? "5", s_culture),
            PointRadius = double.Parse(g.Attribute("data-point-radius")?.Value ?? "1.5", s_culture),
            Layer = g.Attribute("id")!.Value.EndsWith("background") ? GridLayer.Background : GridLayer.Foreground
        };

        return payload;
    }

    /// <summary>
    /// Parses the background information from the specified XML document and returns a corresponding background payload
    /// if found.
    /// </summary>
    /// <remarks>The method searches for an element with the attribute 'id' equal to 'surface-background' and
    /// extracts its 'fill' attribute as the background color. If no such element exists, the method returns
    /// null.</remarks>
    /// <param name="document">The XML document to parse for background information. Must not be null and should contain an element with the
    /// attribute 'id' set to 'surface-background'.</param>
    /// <returns>A BackgroundPayload object containing the background color if a suitable element is found; otherwise, null.</returns>
    private static BackgroundPayload? ParseBackground(XDocument document)
    {
        var rect = document.Root!.Descendants().FirstOrDefault(e => (string?)e.Attribute("id") == "surface-background");

        if (rect is null)
        {
            return null;
        }

        return new BackgroundPayload
        {
            Color = rect.Attribute("fill")?.Value ?? "transparent",
        };
    }

    /// <summary>
    /// Asynchronously parses the specified input string and returns the resulting payload.
    /// </summary>
    /// <param name="document">The input string to parse. Cannot be null or empty.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the parsed payload of type TPayload.</returns>
    private TPayload? ParseContent(XDocument document)
    {
        var content = document.Root!.Descendants().FirstOrDefault(e => (string?)e.Attribute("id") == "surface-content");

        if (content is null)
        {
            return default;
        }

        return ParseContent(content);
    }

    /// <summary>
    /// Parses the specified XML element and returns a value of type TPayload representing the extracted content.
    /// </summary>
    /// <remarks>Implementations should define how the XML content is interpreted and converted to TPayload.
    /// The behavior may vary depending on the structure and schema of the XML provided.</remarks>
    /// <param name="content">The XML element containing the content to parse. Must not be null.</param>
    /// <returns>A value of type TPayload that represents the parsed content from the specified XML element.</returns>
    protected abstract TPayload ParseContent(XElement content);
}
