using System.Text;
using FluentUI.Blazor.Community.Components.Extensions;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a builder for constructing SVG path elements using a fluent interface.
/// </summary>
/// <remarks>Use this class to incrementally build the 'd' attribute of an SVG path by chaining method calls that
/// represent path commands, such as MoveTo, LineTo, and CubicBezierTo. The builder allows setting common path
/// attributes like fill, stroke, and stroke width. Once the path is fully defined, call Close to finalize the path and
/// return to the parent SVG builder. This class is not thread-safe.</remarks>
public sealed class SvgPathBuilder : SvgElementBuilderBase<SvgPathBuilder, SvgPath>
{
    /// <summary>
    /// Represents the internal StringBuilder used to construct the 'd' attribute of the SVG path element.
    /// </summary>
    private readonly StringBuilder _dPath = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="SvgPathBuilder"/> class with the specified parent builder and path element.
    /// </summary>
    /// <param name="parent">The parent <see cref="SvgBuilder"/> that this path builder is associated with. Cannot be null.</param>
    /// <param name="path">The <see cref="SvgPath"/> element to configure with this builder. Cannot be null.</param>
    public SvgPathBuilder(SvgBuilder parent, SvgPath path)
        : base(parent, path)
    {
    }

    /// <summary>
    /// Appends a 'move to' (M) command to the current SVG path, setting the starting point for subsequent path
    /// commands.
    /// </summary>
    /// <remarks>Use this method to begin a new subpath at the specified coordinates. Subsequent path commands
    /// will start from this point.</remarks>
    /// <param name="x">The x-coordinate of the new starting point for the path.</param>
    /// <param name="y">The y-coordinate of the new starting point for the path.</param>
    /// <returns>The current instance of <see cref="SvgPathBuilder"/>, enabling method chaining.</returns>
    public SvgPathBuilder MoveTo(double x, double y)
    {
        _dPath.Append("M ")
              .Append(x.ToSvg())
              .Append(' ')
              .Append(y.ToSvg())
              .Append(' ');

        return this;
    }

    /// <summary>
    /// Appends a straight line segment from the current point to the specified coordinates in the path definition.
    /// </summary>
    /// <remarks>This method adds an absolute 'LineTo' (L) command to the SVG path. The coordinates are
    /// interpreted in the user coordinate system.</remarks>
    /// <param name="x">The x-coordinate of the end point of the line segment.</param>
    /// <param name="y">The y-coordinate of the end point of the line segment.</param>
    /// <returns>The current instance of <see cref="SvgPathBuilder"/>, enabling method chaining.</returns>
    public SvgPathBuilder LineTo(double x, double y)
    {
        _dPath.Append("L ")
              .Append(x.ToSvg())
              .Append(' ')
              .Append(y.ToSvg())
              .Append(' ');

        return this;
    }

    /// <summary>
    /// Appends a horizontal line command to the current SVG path, moving the cursor to the specified x-coordinate.
    /// </summary>
    /// <remarks>This method adds an 'H' command to the SVG path data, which draws a horizontal line from the
    /// current position to the specified x-coordinate. The vertical position remains unchanged.</remarks>
    /// <param name="x">The absolute x-coordinate to move the cursor to in the SVG path.</param>
    /// <returns>The current instance of <see cref="SvgPathBuilder"/>, enabling method chaining.</returns>
    public SvgPathBuilder HorizontalTo(double x)
    {
        _dPath.Append("H ")
              .Append(x.ToSvg())
              .Append(' ');

        return this;
    }

    /// <summary>
    /// Appends a vertical line command to the current SVG path, moving the current point vertically to the specified
    /// y-coordinate.
    /// </summary>
    /// <remarks>This method adds a 'V' command to the SVG path data, which moves the current point vertically
    /// to the specified y-coordinate without changing the x-coordinate. This is useful for constructing SVG paths that
    /// require vertical movements.</remarks>
    /// <param name="y">The y-coordinate to move to, in user space units.</param>
    /// <returns>The current instance of <see cref="SvgPathBuilder"/> with the vertical line command appended.</returns>
    public SvgPathBuilder VerticalTo(double y)
    {
        _dPath.Append("V ")
              .Append(y.ToSvg())
              .Append(' ');

        return this;
    }

    /// <summary>
    /// Appends an absolute cubic Bézier curve command to the current SVG path using the specified control and end
    /// points.
    /// </summary>
    /// <remarks>This method adds a 'C' command to the SVG path data, representing an absolute cubic Bézier
    /// curve segment. The curve starts at the current point and ends at the specified end point, using the provided
    /// control points to define its shape.</remarks>
    /// <param name="x1">The x-coordinate of the first control point that defines the curve.</param>
    /// <param name="y1">The y-coordinate of the first control point that defines the curve.</param>
    /// <param name="x2">The x-coordinate of the second control point that defines the curve.</param>
    /// <param name="y2">The y-coordinate of the second control point that defines the curve.</param>
    /// <param name="x">The x-coordinate of the end point of the curve.</param>
    /// <param name="y">The y-coordinate of the end point of the curve.</param>
    /// <returns>The current instance of <see cref="SvgPathBuilder"/>, allowing for method chaining.</returns>
    public SvgPathBuilder CubicBezierTo(
        double x1, double y1,
        double x2, double y2,
        double x, double y)
    {
        _dPath.Append("C ")
          .Append(x1.ToSvg()).Append(' ').Append(y1.ToSvg()).Append(' ')
          .Append(x2.ToSvg()).Append(' ').Append(y2.ToSvg()).Append(' ')
          .Append(x.ToSvg()).Append(' ').Append(y.ToSvg()).Append(' ');

        return this;
    }

    /// <summary>
    /// Appends a smooth cubic Bézier curve command to the current SVG path using the specified control point and end
    /// point coordinates.
    /// </summary>
    /// <remarks>This method adds an 'S' command to the SVG path, which creates a smooth cubic Bézier curve.
    /// The first control point is automatically calculated as the reflection of the previous control point, if any, or
    /// coincides with the current point if none exists.</remarks>
    /// <param name="x2">The x-coordinate of the control point for the curve.</param>
    /// <param name="y2">The y-coordinate of the control point for the curve.</param>
    /// <param name="x">The x-coordinate of the end point of the curve.</param>
    /// <param name="y">The y-coordinate of the end point of the curve.</param>
    /// <returns>The current instance of <see cref="SvgPathBuilder"/>, enabling method chaining.</returns>
    public SvgPathBuilder SmoothCubicBezierTo(double x2, double y2, double x, double y)
    {
        _dPath.Append("S ")
            .Append(x2.ToSvg())
            .Append(' ')
            .Append(y2.ToSvg())
            .Append(' ')
            .Append(x.ToSvg())
            .Append(' ')
            .Append(y.ToSvg())
            .Append(' ');

        return this;
    }

    /// <summary>
    /// Appends a quadratic Bézier curve segment to the current SVG path using the specified control point and end point
    /// coordinates.
    /// </summary>
    /// <remarks>This method adds a 'Q' command to the SVG path data, representing a quadratic Bézier curve
    /// from the current point to the specified end point, using the given control point.</remarks>
    /// <param name="x1">The x-coordinate of the control point that defines the curvature of the Bézier segment.</param>
    /// <param name="y1">The y-coordinate of the control point that defines the curvature of the Bézier segment.</param>
    /// <param name="x">The x-coordinate of the end point of the Bézier segment.</param>
    /// <param name="y">The y-coordinate of the end point of the Bézier segment.</param>
    /// <returns>The current instance of <see cref="SvgPathBuilder"/>, allowing for method chaining.</returns>
    public SvgPathBuilder QuadraticBezierTo(double x1, double y1, double x, double y)
    {
        _dPath.Append("Q ")
            .Append(x1.ToSvg())
            .Append(' ')
            .Append(y1.ToSvg())
            .Append(' ')
            .Append(x.ToSvg())
            .Append(' ')
            .Append(y.ToSvg())
            .Append(' ');

        return this;
    }

    /// <summary>
    /// Appends a smooth quadratic Bézier curve command to the current SVG path, using the current point as the starting
    /// point and the specified coordinates as the end point.
    /// </summary>
    /// <remarks>This method generates a 'T' command in the SVG path data, which creates a smooth quadratic
    /// Bézier curve by automatically reflecting the previous control point. If there is no previous quadratic Bézier
    /// command, the current point is used as the control point.</remarks>
    /// <param name="x">The x-coordinate of the end point of the quadratic Bézier curve, in user space.</param>
    /// <param name="y">The y-coordinate of the end point of the quadratic Bézier curve, in user space.</param>
    /// <returns>The current instance of the <see cref="SvgPathBuilder"/>, allowing for method chaining.</returns>
    public SvgPathBuilder SmoothQuadraticBezierTo(double x, double y)
    {
        _dPath.Append("T ")
            .Append(x.ToSvg())
            .Append(' ')
            .Append(y.ToSvg())
            .Append(' ');

        return this;
    }

    /// <summary>
    /// Appends an elliptical arc command to the current SVG path using the specified radii, rotation, arc flags, and
    /// endpoint coordinates.
    /// </summary>
    /// <remarks>This method corresponds to the SVG 'A' (arc) path command and allows for the construction of
    /// complex elliptical arcs within an SVG path definition.</remarks>
    /// <param name="rx">The x-axis radius of the ellipse used to draw the arc. Must be non-negative.</param>
    /// <param name="ry">The y-axis radius of the ellipse used to draw the arc. Must be non-negative.</param>
    /// <param name="xAxisRotation">The rotation angle, in degrees, of the ellipse's x-axis relative to the x-axis of the user coordinate system.</param>
    /// <param name="largeArc">A value indicating whether the larger arc (if <see langword="true"/>) or the smaller arc (if <see
    /// langword="false"/>) should be drawn.</param>
    /// <param name="sweep">A value indicating whether the arc should be drawn in a positive-angle (clockwise, if <see langword="true"/>) or
    /// negative-angle (counterclockwise, if <see langword="false"/>) direction.</param>
    /// <param name="x">The x-coordinate of the endpoint of the arc.</param>
    /// <param name="y">The y-coordinate of the endpoint of the arc.</param>
    /// <returns>The current <see cref="SvgPathBuilder"/> instance with the arc command appended, enabling method chaining.</returns>
    public SvgPathBuilder ArcTo(
        double rx,
        double ry,
        double xAxisRotation,
        bool largeArc,
        bool sweep,
        double x,
        double y)
    {
        _dPath.Append("A ")
          .Append(rx.ToSvg())
          .Append(' ')
          .Append(ry.ToSvg())
          .Append(' ')
          .Append(xAxisRotation.ToSvg())
          .Append(' ')
          .Append(largeArc ? "1 " : "0 ")
          .Append(sweep ? "1 " : "0 ")
          .Append(x.ToSvg())
          .Append(' ')
          .Append(y.ToSvg())
          .Append(' ');

        return this;
    }

    /// <summary>
    /// Sets the value of the 'stroke-dasharray' attribute for the SVG path being built.
    /// </summary>
    /// <remarks>The 'stroke-dasharray' attribute controls the pattern of dashes and gaps used to render the
    /// outline of the SVG path. For example, a value of "5,2" creates a pattern of 5 units of dash followed by 2 units
    /// of gap.</remarks>
    /// <param name="value">The dash pattern to apply to the stroke, specified as a string of comma- or space-separated numbers. Each number
    /// represents the length of dashes and gaps in the stroke pattern.</param>
    /// <returns>The current instance of <see cref="SvgPathBuilder"/>, enabling method chaining.</returns>
    public SvgPathBuilder WithStrokeDashArray(string value)
    {
        return WithAttribute("stroke-dasharray", value);
    }

    /// <summary>
    /// Closes the current subpath by drawing a straight line from the current point
    ///  back to the starting point of the subpath, and marks the path as closed.
    /// </summary>
    /// <returns></returns>
    public SvgPathBuilder ClosePath()
    {
        _dPath.Append('Z').Append(' ');

        return this;
    }

    /// <inheritdoc />
    public override SvgBuilder Close()
    {
        WithAttribute("d", _dPath.ToString().Trim());

        return base.Close();
    }
}
