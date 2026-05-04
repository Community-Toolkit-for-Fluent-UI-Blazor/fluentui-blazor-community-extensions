using FluentUI.Blazor.Community.Components.Extensions;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a builder for configuring and constructing SVG rectangle elements using a fluent interface.
/// </summary>
/// <remarks>Use this class to set attributes such as fill color, stroke color, stroke width, and corner radii for
/// an SVG rectangle. The builder pattern enables chaining of configuration methods for concise and readable SVG element
/// creation. Once configuration is complete, call Close to return to the parent builder and continue constructing the
/// SVG document.</remarks>
public sealed class SvgRectBuilder : SvgElementBuilderBase<SvgRectBuilder, SvgRect>
{
    /// <summary>
    /// Initializes a new instance of the SvgRectBuilder class for configuring an SVG rectangle element within the
    /// builder pattern.
    /// </summary>
    /// <param name="parent">The parent SvgBuilder instance that this rectangle builder is associated with. Cannot be null.</param>
    /// <param name="rect">The SvgRect object representing the rectangle element to be configured. Cannot be null.</param>
    public SvgRectBuilder(SvgBuilder parent, SvgRect rect)
        : base(parent, rect)
    {
    }

    /// <summary>
    /// Sets the horizontal corner radius for the rectangle element.
    /// </summary>
    /// <remarks>Setting a non-zero value for the horizontal corner radius will result in rounded corners
    /// along the x-axis. If the value is zero, corners remain sharp.</remarks>
    /// <param name="rx">The horizontal radius, in user units, used to round the corners of the rectangle. Must be non-negative.</param>
    /// <returns>The current instance of <see cref="SvgRectBuilder"/> with the updated horizontal corner radius.</returns>
    public SvgRectBuilder WithRx(double rx)
    {
        return WithAttribute("rx", rx.ToSvg());
    }

    /// <summary>
    /// Sets the vertical corner radius for the rectangle element being built.
    /// </summary>
    /// <param name="ry">The vertical radius, in user units, to use for the rectangle's rounded corners. Must be non-negative.</param>
    /// <returns>The current instance of <see cref="SvgRectBuilder"/>, enabling method chaining.</returns>
    public SvgRectBuilder WithRy(double ry)
    {
        return WithAttribute("ry", ry.ToSvg());
    }

    /// <summary>
    /// Sets the width attribute for the SVG rectangle being built.
    /// </summary>
    /// <param name="width">The width of the rectangle, specified in user units. Must be a non-negative value.</param>
    /// <returns>The current <see cref="SvgRectBuilder"/> instance with the updated width attribute.</returns>
    public SvgRectBuilder WithWidth(double width)
    {
        return WithAttribute("width", width.ToSvg());
    }

    /// <summary>
    /// Sets the height attribute for the SVG rectangle being built.
    /// </summary>
    /// <param name="height">The height value to assign to the rectangle, specified in user units. Must be a non-negative number.</param>
    /// <returns>The current <see cref="SvgRectBuilder"/> instance with the updated height attribute.</returns>
    public SvgRectBuilder WithHeight(double height)
    {
        return WithAttribute("height", height.ToSvg());
    }

    /// <summary>
    /// Sets the x-coordinate of the rectangle in the SVG element being built.
    /// </summary>
    /// <param name="x">The x-coordinate value to assign to the rectangle. Represents the horizontal position in user units.</param>
    /// <returns>The current <see cref="SvgRectBuilder"/> instance with the updated x-coordinate.</returns>
    public SvgRectBuilder WithX(double x)
    {
        return WithAttribute("x", x.ToSvg());
    }

    /// <summary>
    /// Sets the y-coordinate of the rectangle in the SVG element being built.
    /// </summary>
    /// <param name="y">The y-coordinate value to assign to the rectangle. Represents the vertical position in user units.</param>
    /// <returns>The current instance of <see cref="SvgRectBuilder"/> with the updated y-coordinate, enabling method chaining.</returns>
    public SvgRectBuilder WithY(double y)
    {
        return WithAttribute("y", y.ToSvg());
    }
}

