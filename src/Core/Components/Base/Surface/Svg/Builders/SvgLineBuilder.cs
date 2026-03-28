using FluentUI.Blazor.Community.Components.Extensions;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a builder for configuring and constructing an SVG line element within an SVG document.
/// </summary>
/// <remarks>Use this builder to fluently set properties and attributes on an <see cref="SvgLine"/> element as
/// part of an SVG composition. This class is typically used in conjunction with <see cref="SvgBuilder"/> to create
/// complex SVG graphics in a structured and type-safe manner.</remarks>
public partial class SvgLineBuilder : SvgElementBuilderBase<SvgLineBuilder, SvgLine>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SvgLineBuilder"/> class with the specified
    ///  parent builder and line element.
    /// </summary>
    /// <param name="parent">The parent SvgBuilder that this builder is associated with. Cannot be null.</param>
    /// <param name="line">The SvgLine element to configure with this builder. Cannot be null.</param>
    public SvgLineBuilder(SvgBuilder parent, SvgLine line)
        : base(parent, line)
    {
    }

    /// <summary>
    /// Sets the value of the 'x1' attribute for the SVG line element being built.
    /// </summary>
    /// <param name="x1">The x-coordinate of the start point of the line.</param>
    /// <returns>The current <see cref="SvgLineBuilder"/> instance with the updated 'x1' attribute.</returns>
    public SvgLineBuilder WithX1(double x1)
    {
        return WithAttribute("x1", x1.ToSvg());
    }

    /// <summary>
    /// Sets the value of the 'x2' attribute for the SVG line element being built.
    /// </summary>
    /// <param name="x2">The x-coordinate of the end point of the line.</param>
    /// <returns>The current <see cref="SvgLineBuilder"/> instance with the updated 'x2' attribute.</returns>
    public SvgLineBuilder WithX2(double x2)
    {
        return WithAttribute("x2", x2.ToSvg());
    }

    /// <summary>
    /// Sets the value of the 'y1' attribute for the SVG line element being built.
    /// </summary>
    /// <param name="y1">The y-coordinate of the start point of the line.</param>
    /// <returns>The current <see cref="SvgLineBuilder"/> instance with the updated 'y1' attribute.</returns>
    public SvgLineBuilder WithY1(double y1)
    {
        return WithAttribute("y1", y1.ToSvg());
    }

    /// <summary>
    /// Sets the value of the 'y2' attribute for the SVG line element being built.
    /// </summary>
    /// <param name="y2">The y-coordinate of the end point of the line.</param>
    /// <returns>The current <see cref="SvgLineBuilder"/> instance with the updated 'y2' attribute.</returns>
    public SvgLineBuilder WithY2(double y2)
    {
        return WithAttribute("y2", y2.ToSvg());
    }
}
