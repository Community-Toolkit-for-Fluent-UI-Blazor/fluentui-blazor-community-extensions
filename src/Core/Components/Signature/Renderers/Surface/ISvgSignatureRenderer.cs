namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines a contract for rendering a collection of signature strokes as an SVG image.
/// </summary>
/// <remarks>Implementations of this interface convert digital signature input, represented as a series of
/// strokes, into a Scalable Vector Graphics (SVG) format. This is typically used to display or export handwritten
/// signatures in web applications or documents.</remarks>
public interface ISvgSignatureRenderer
{
    /// <summary>
    /// Renders the provided signature strokes into an SVG string based on the specified options and dimensions.
    /// </summary>
    /// <param name="strokes">A read-only list of signature strokes, where each stroke consists of a series of points representing the path of the pen or stylus.</param>
    /// <param name="options">Configuration options that influence the rendering of the SVG, such as pen color, stroke width, background settings, and other visual properties.</param>
    /// <param name="width">The width of the resulting SVG image, which may affect the scaling and layout of the rendered signature.</param>
    /// <param name="height">The height of the resulting SVG image, which may affect the scaling and layout of the rendered signature.</param>
    /// <returns>A string containing the SVG markup that represents the rendered signature based on the input strokes and options.</returns>
    string RenderSvg(
        IReadOnlyList<SignatureStroke> strokes,
        SignatureRenderingOptions options,
        double width,
        double height);
}

