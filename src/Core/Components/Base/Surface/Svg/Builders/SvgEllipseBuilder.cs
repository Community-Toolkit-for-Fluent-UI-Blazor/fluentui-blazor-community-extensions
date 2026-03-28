using FluentUI.Blazor.Community.Components.Extensions;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a builder for configuring and adding attributes to an SVG circle element in a fluent manner.
/// </summary>
/// <remarks>Use this builder to set common SVG circle attributes such as fill color, stroke color, and stroke
/// width. Once configuration is complete, call Close to return to the parent SVG builder and continue constructing the
/// SVG document. This class is sealed and not intended for inheritance.</remarks>
public sealed class SvgEllipseBuilder : SvgElementBuilderBase<SvgEllipseBuilder, SvgEllipse>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SvgEllipseBuilder"/> class with the specified parent builder and circle element.
    /// </summary>
    /// <param name="parent">The parent SvgBuilder that this builder is associated with. Cannot be null.</param>
    /// <param name="ellipse">The SvgEllipse element to configure with this builder. Cannot be null.</param>
    public SvgEllipseBuilder(SvgBuilder parent, SvgEllipse ellipse)
        : base(parent, ellipse)
    {
    }

    /// <summary>
    /// Sets the horizontal radius of the ellipse element in the SVG builder.
    /// </summary>
    /// <param name="radius">The horizontal radius of the ellipse. Must be a non-negative value.</param>
    /// <returns>The current <see cref="SvgEllipseBuilder"/> instance with the updated horizontal radius attribute.</returns>
    public SvgEllipseBuilder WithRx(double radius)
    {
        return WithAttribute("rx", radius.ToSvg());
    }

    /// <summary>
    /// Sets the vertical radius of the ellipse element in the SVG builder.
    /// </summary>
    /// <param name="radius">The vertical radius of the ellipse. Must be a non-negative value.</param>
    /// <returns>The current <see cref="SvgEllipseBuilder"/> instance with the updated vertical radius attribute.</returns>
    public SvgEllipseBuilder WithRy(double radius)
    {
        return WithAttribute("ry", radius.ToSvg());
    }

    /// <summary>
    /// Sets the x-coordinate of the center of the circle.
    /// </summary>
    /// <param name="cx">The x-coordinate of the center of the circle, in user space units.</param>
    /// <returns>The current instance of <see cref="SvgCircleBuilder"/> with the updated center coordinates.</returns>
    public SvgEllipseBuilder WithCx(double cx)
    {
        return WithAttribute("cx", cx.ToSvg());
    }

    /// <summary>
    /// Sets the y-coordinate of the center of the circle.
    /// </summary>
    /// <param name="cy">The y-coordinate of the center of the circle, in user space units.</param>
    /// <returns>The current instance of <see cref="SvgCircleBuilder"/> with the updated center coordinates.</returns>
    public SvgEllipseBuilder WithCy(double cy)
    {
        return WithAttribute("cy", cy.ToSvg());
    }
}
