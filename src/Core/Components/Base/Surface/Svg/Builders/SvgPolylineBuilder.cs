using FluentUI.Blazor.Community.Components.Extensions;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a builder for configuring and constructing an SVG polyline element using a fluent interface.
/// </summary>
/// <remarks>This class enables the creation and customization of SVG polyline elements as part of an SVG
/// document. It is intended to be used within the builder pattern, allowing for chained configuration of SVG elements.
/// Instances of this class are typically created through the parent SvgBuilder.</remarks>
public sealed class SvgPolylineBuilder : SvgElementBuilderBase<SvgPolylineBuilder, SvgPolyline>
{
    /// <summary>
    /// Initializes a new instance of the SvgPolylineBuilder class for configuring an SVG polyline element within the builder pattern.
    /// </summary>
    /// <param name="parent">The parent SvgBuilder instance that this polyline builder is associated with.</param>
    /// <param name="element">The element representing the SVG polyline to be configured. Cannot be null.</param>
    public SvgPolylineBuilder(SvgBuilder parent, SvgPolyline element)
        : base(parent, element)
    {
    }

    /// <summary>
    /// Specifies the collection of points that define the vertices of the polyline.
    /// </summary>
    /// <param name="points">An enumerable collection of tuples, each containing the X and Y coordinates of a point to be included in the
    /// polyline.</param>
    /// <returns>The current instance of <see cref="SvgPolylineBuilder"/> with the specified points applied.</returns>
    public SvgPolylineBuilder WithPoints(IEnumerable<(double X, double Y)> points)
    {
        var fullPoints = points.Select(p => $"{p.X.ToSvg()},{p.Y.ToSvg()}");

        return WithAttribute("points", string.Join(" ", fullPoints));
    }
}
