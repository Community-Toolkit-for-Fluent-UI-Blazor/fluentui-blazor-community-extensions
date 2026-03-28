using FluentUI.Blazor.Community.Components.Extensions;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a builder for configuring and adding attributes to an SVG circle element in a fluent manner.
/// </summary>
/// <remarks>Use this builder to set common SVG circle attributes such as fill color, stroke color, and stroke
/// width. Once configuration is complete, call Close to return to the parent SVG builder and continue constructing the
/// SVG document. This class is sealed and not intended for inheritance.</remarks>
public sealed class SvgCircleBuilder : SvgElementBuilderBase<SvgCircleBuilder, SvgCircle>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SvgCircleBuilder"/> class with the specified parent builder and circle element.
    /// </summary>
    /// <param name="parent">The parent SvgBuilder that this builder is associated with. Cannot be null.</param>
    /// <param name="circle">The SvgCircle element to configure with this builder. Cannot be null.</param>
    public SvgCircleBuilder(SvgBuilder parent, SvgCircle circle)
        : base(parent, circle)
    {
    }

    /// <summary>
    /// Sets the radius of the SVG circle element.
    /// </summary>
    /// <param name="radius">The radius of the circle. Must be a non-negative value representing the distance from the center to the edge of
    /// the circle.</param>
    /// <returns>The current <see cref="SvgCircleBuilder"/> instance with the updated radius attribute.</returns>
    public SvgCircleBuilder WithRadius(double radius)
    {
        return WithAttribute("r", radius.ToSvg());
    }

    /// <summary>
    /// Sets the x-coordinate of the center of the circle.
    /// </summary>
    /// <param name="cx">The x-coordinate of the center of the circle, in user space units.</param>
    /// <returns>The current instance of <see cref="SvgCircleBuilder"/> with the updated center coordinates.</returns>
    public SvgCircleBuilder WithCx(double cx)
    {
        return WithAttribute("cx", cx.ToSvg());
    }

    /// <summary>
    /// Sets the y-coordinate of the center of the circle.
    /// </summary>
    /// <param name="cy">The y-coordinate of the center of the circle, in user space units.</param>
    /// <returns>The current instance of <see cref="SvgCircleBuilder"/> with the updated center coordinates.</returns>
    public SvgCircleBuilder WithCy(double cy)
    {
        return WithAttribute("cy", cy.ToSvg());
    }
}
