using System.Text;
using FluentUI.Blazor.Community.Components.Enums;
using FluentUI.Blazor.Community.Components.Extensions;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a fluent API for constructing and configuring SVG (Scalable Vector Graphics) documents programmatically.
/// </summary>
/// <remarks>Use this class to build SVG markup by chaining method calls that add and configure SVG elements such
/// as rectangles, circles, ellipses, lines, polylines, polygons, paths, groups, and definitions. The builder maintains
/// the structure and attributes of the SVG document, and generates the final SVG markup as a string when the Build
/// method is called. This class is not thread-safe.</remarks>
public sealed class SvgBuilder
{
    /// <summary>
    /// Represents the root element of the SVG document associated with this instance.
    /// </summary>
    private readonly SvgRoot _root;

    /// <summary>
    /// Initializes a new instance of the <see cref="SvgBuilder"/> class, setting up the root SVG element with necessary namespaces and version attributes.
    /// </summary>
    public SvgBuilder()
    {
        _root = new SvgRoot();
        _root.Attributes["xmlns"] = "http://www.w3.org/2000/svg";
        _root.Attributes["version"] = "1.1";
    }

    /// <summary>
    /// Sets the value of the 'id' attribute for the root SVG element.
    /// </summary>
    /// <param name="id">The identifier to assign to the root SVG element. Cannot be null.</param>
    /// <returns>The current instance of <see cref="SvgBuilder"/>, enabling method chaining.</returns>
    public SvgBuilder WithId(string? id)
    {
        if (!string.IsNullOrEmpty(id))
        {
            _root.Attributes["id"] = id!;
        }

        return this;
    }

    /// <summary>
    /// Sets a custom data attribute 'data-svg-id' on the root SVG element.
    /// </summary>
    /// <param name="id">The value to assign to the 'data-svg-id' attribute. Cannot be null.</param>
    /// <returns>The current instance of <see cref="SvgBuilder"/>, enabling method chaining.</returns>
    public SvgBuilder WithDataId(string? id)
    {
        if (!string.IsNullOrEmpty(id))
        {
            _root.Attributes["data-svg-id"] = id!;
        }

        return this;
    }

    /// <summary>
    /// Sets the rendered width of the SVG element in user units.
    /// </summary>
    /// <remarks>This method updates the "width" attribute of the root SVG element. If called multiple times,
    /// the last value specified will be used.</remarks>
    /// <param name="width">The width, in user units, to assign to the SVG element. Must be a non-negative value.</param>
    /// <returns>The current instance of <see cref="SvgBuilder"/>, enabling method chaining.</returns>
    public SvgBuilder RenderedWidth(double width)
    {
        _root.Attributes["width"] = width.ToSvg();

        return this;
    }

    /// <summary>
    /// Sets the shape rendering.
    /// </summary>
    /// <param name="value">Value for the 'shape-rendering' attribute.</param>
    /// <returns></returns>
    public SvgBuilder ShapeRendering(string value)
    {
        _root.Attributes["shape-rendering"] = value;

        return this;
    }

    /// <summary>
    /// Sets the shape rendering.
    /// </summary>
    /// <param name="value">The <see cref="SvgShapeRendering"/> value to assign to the 'shape-rendering' attribute. If the value is <see cref="SvgShapeRendering.None"/>, the attribute will not be set.</param>
    /// <returns>Returns the current instance of <see cref="SvgBuilder"/>, enabling method chaining.</returns>
    public SvgBuilder ShapeRendering(SvgShapeRendering value)
    {
        if (value == SvgShapeRendering.None)
        {
            return this;
        }

        var attributeValue = value switch
        {
            SvgShapeRendering.Auto => "auto",
            SvgShapeRendering.OptimizeSpeed => "optimizeSpeed",
            SvgShapeRendering.CrispEdges => "crispEdges",
            SvgShapeRendering.GeometricPrecision => "geometricPrecision",
            SvgShapeRendering.OptimizeQuality => "optimizeQuality",
            _ => throw new ArgumentOutOfRangeException(nameof(value), $"Unsupported shape rendering value: {value}")
        };

        _root.Attributes["shape-rendering"] = attributeValue;

        return this;
    }

    /// <summary>
    /// Sets the 'preserveAspectRatio' attribute for the root SVG element.
    /// </summary>
    /// <remarks>Use this method to control how the SVG content is scaled and aligned within its viewport. The
    /// value should follow the SVG specification for the 'preserveAspectRatio' attribute.</remarks>
    /// <param name="value">The value to assign to the 'preserveAspectRatio' attribute. Defaults to "xMidYMid meet" if not specified.</param>
    /// <returns>The current instance of <see cref="SvgBuilder"/>, enabling method chaining.</returns>
    public SvgBuilder PreserveAspectRatio(string value = "xMidYMid meet")
    {
        _root.Attributes["preserveAspectRatio"] = value;

        return this;
    }

    /// <summary>
    /// Sets the rendered height of the SVG element in pixels.
    /// </summary>
    /// <param name="height">The height, in pixels, to assign to the SVG element. Must be a non-negative value.</param>
    /// <returns>The current instance of <see cref="SvgBuilder"/> to allow method chaining.</returns>
    public SvgBuilder RenderedHeight(double height)
    {
        _root.Attributes["height"] = height.ToSvg();

        return this;
    }

    /// <summary>
    /// Sets the SVG viewBox attribute to define the position and dimension of the viewport for the SVG content.
    /// </summary>
    /// <remarks>The viewBox attribute enables scaling and panning of SVG content by specifying the coordinate
    /// system and visible area. Setting appropriate values allows for responsive and properly scaled SVG
    /// graphics.</remarks>
    /// <param name="x">The x-coordinate of the viewBox's origin.</param>
    /// <param name="y">The y-coordinate of the viewBox's origin.</param>
    /// <param name="width">The width of the viewBox. Must be a positive value.</param>
    /// <param name="height">The height of the viewBox. Must be a positive value.</param>
    /// <returns>The current instance of the SvgBuilder with the updated viewBox attribute.</returns>
    public SvgBuilder ViewBox(double x, double y, double width, double height)
    {
        _root.Attributes["viewBox"] = $"{x.ToSvg()} {y.ToSvg()} {width.ToSvg()} {height.ToSvg()}";

        return this;
    }

    /// <summary>
    /// Adds a rectangle element to the current SVG structure at the specified position and size.
    /// </summary>
    /// <param name="x">The x-coordinate of the upper-left corner of the rectangle, in user units.</param>
    /// <param name="y">The y-coordinate of the upper-left corner of the rectangle, in user units.</param>
    /// <param name="width">The width of the rectangle, in user units. Must be greater than or equal to zero.</param>
    /// <param name="height">The height of the rectangle, in user units. Must be greater than or equal to zero.</param>
    /// <returns>A builder object for further configuration of the newly added rectangle element.</returns>
    public SvgRectBuilder AddRect(double x, double y, double width, double height)
    {
        var rect = new SvgRect();
        _root.Children.Add(rect);

        return new SvgRectBuilder(this, rect)
            .WithX(x)
            .WithY(y)
            .WithWidth(width)
            .WithHeight(height);
    }

    /// <summary>
    /// Adds a new circle element to the SVG document with the specified center coordinates and radius.
    /// </summary>
    /// <param name="cx">The x-coordinate of the center of the circle, in user units.</param>
    /// <param name="cy">The y-coordinate of the center of the circle, in user units.</param>
    /// <param name="r">The radius of the circle, in user units. Must be non-negative.</param>
    /// <returns>A builder for the newly added SVG circle element, allowing further configuration.</returns>
    public SvgCircleBuilder AddCircle(double cx, double cy, double r)
    {
        var circle = new SvgCircle();
        _root.Children.Add(circle);

        return new SvgCircleBuilder(this, circle)
            .WithCx(cx)
            .WithCy(cy)
            .WithRadius(r);
    }

    /// <summary>
    /// Adds a new ellipse element to the SVG document with the specified center coordinates and radii.
    /// </summary>
    /// <param name="cx">The x-axis coordinate of the center of the ellipse.</param>
    /// <param name="cy">The y-axis coordinate of the center of the ellipse.</param>
    /// <param name="rx">The radius of the ellipse along the x-axis. Must be non-negative.</param>
    /// <param name="ry">The radius of the ellipse along the y-axis. Must be non-negative.</param>
    /// <returns>A builder for further configuration of the newly added ellipse element.</returns>
    public SvgEllipseBuilder AddEllipse(double cx, double cy, double rx, double ry)
    {
        var ellipse = new SvgEllipse();
        _root.Children.Add(ellipse);

        return new SvgEllipseBuilder(this, ellipse)
            .WithCx(cx)
            .WithCy(cy)
            .WithRx(rx)
            .WithRy(ry);
    }

    /// <summary>
    /// Adds a new line element to the SVG document with the specified start and end coordinates.
    /// </summary>
    /// <param name="x1">The x-coordinate of the start point of the line.</param>
    /// <param name="y1">The y-coordinate of the start point of the line.</param>
    /// <param name="x2">The x-coordinate of the end point of the line.</param>
    /// <param name="y2">The y-coordinate of the end point of the line.</param>
    /// <returns>A builder for the newly added SVG line element, allowing further configuration.</returns>
    public SvgLineBuilder AddLine(double x1, double y1, double x2, double y2)
    {
        var line = new SvgLine();
        _root.Children.Add(line);

        return new SvgLineBuilder(this, line)
            .WithX1(x1)
            .WithX2(x2)
            .WithY1(y1)
            .WithY2(y2);
    }

    /// <summary>
    /// Adds a polyline element to the SVG document using the specified collection of points.
    /// </summary>
    /// <remarks>Each point in the collection is converted to SVG coordinate format. The polyline is appended
    /// to the root of the SVG document.</remarks>
    /// <param name="points">A collection of tuples representing the X and Y coordinates of each point in the polyline. The order of points
    /// determines the shape of the polyline.</param>
    /// <returns>A builder object for further configuration of the added polyline element.</returns>
    public SvgPolylineBuilder AddPolyline(IEnumerable<(double X, double Y)> points)
    {
        var polyline = new SvgPolyline();
        _root.Children.Add(polyline);

        return new SvgPolylineBuilder(this, polyline)
            .WithPoints(points);
    }

    /// <summary>
    /// Adds a polygon element to the SVG document using the specified collection of points.
    /// </summary>
    /// <param name="points">An enumerable collection of tuples representing the X and Y coordinates of each vertex of the polygon. The order
    /// of points determines the shape of the polygon.</param>
    /// <returns>A builder object for further configuration of the added polygon element.</returns>
    public SvgPolygonBuilder AddPolygon(IEnumerable<(double X, double Y)> points)
    {
        var polygon = new SvgPolygon();
        _root.Children.Add(polygon);

        return new SvgPolygonBuilder(this, polygon)
            .WithPoints(points);
    }

    /// <summary>
    /// Adds a new SVG path element to the current SVG document and returns a builder for configuring the path.
    /// </summary>
    /// <remarks>Use the returned <see cref="SvgPathBuilder"/> to define the geometry and attributes of the
    /// path. The path is added as a child of the root SVG element.</remarks>
    /// <returns>A <see cref="SvgPathBuilder"/> instance for further configuration of the newly added SVG path.</returns>
    public SvgPathBuilder AddPath()
    {
        var path = new SvgPath();
        _root.Children.Add(path);

        return new SvgPathBuilder(this, path);
    }

    /// <summary>
    /// Adds a new SVG group element to the current SVG document and returns a builder for configuring the group.
    /// </summary>
    /// <remarks>Use the returned <see cref="SvgGroupBuilder"/> to set properties or add child elements to the
    /// group. This method is typically used when constructing complex SVG documents with nested groupings.</remarks>
    /// <returns>A <see cref="SvgGroupBuilder"/> instance that can be used to configure the newly added SVG group.</returns>
    public SvgGroupBuilder AddGroup()
    {
        var group = new SvgGroup();
        _root.Children.Add(group);

        return new SvgGroupBuilder(this, group);
    }

    /// <summary>
    /// Adds a new defs element to the SVG document and returns a builder for configuring its contents.
    /// </summary>
    /// <remarks>Use this method to define reusable SVG elements, such as gradients or symbols, within the SVG
    /// document. The returned builder allows you to add definitions to the &lt;defs&gt; section.</remarks>
    /// <returns>A <see cref="SvgDefsBuilder"/> instance for configuring the newly added &lt;defs&gt; element.</returns>
    public SvgDefsBuilder AddDefs()
    {
        var defs = new SvgDefs();
        _root.Children.Add(defs);

        return new SvgDefsBuilder(this, defs);
    }

    /// <summary>
    /// Sets the SVG transform attribute for the root element.
    /// </summary>
    /// <remarks>Use this method to apply transformations such as translation, rotation, or scaling to the
    /// entire SVG graphic.</remarks>
    /// <param name="transform">The transform string to apply to the root SVG element. This should be a valid SVG transform expression, such as
    /// "rotate(45)" or "scale(2)".</param>
    /// <returns>The current instance of <see cref="SvgBuilder"/>, enabling method chaining.</returns>
    public SvgBuilder WithTransform(string transform)
    {
        _root.Attributes["transform"] = transform;

        return this;
    }

    /// <summary>
    /// Adds a new text element to the SVG at the specified coordinates.
    /// </summary>
    /// <param name="x">The x-coordinate, in user units, where the text will be placed.</param>
    /// <param name="y">The y-coordinate, in user units, where the text will be placed.</param>
    /// <param name="text">The text content to display at the specified coordinates. Cannot be null.</param>
    /// <returns>A new SvgTextBuilder instance for further configuration of the added text element.</returns>
    public SvgTextBuilder AddText(double x, double y, string text)
    {
        var t = new SvgText
        {
            Text = text
        };

        t.Attributes["x"] = x.ToSvg();
        t.Attributes["y"] = y.ToSvg();

        _root.Children.Add(t);

        return new SvgTextBuilder(this, t);
    }

    /// <summary>
    /// Adds a new clipPath element to the SVG document and returns a builder for configuring the clip path.
    /// </summary>
    /// <returns></returns>
    public SvgClipPathBuilder AddClipPath()
    {
        var clipPath = new SvgClipPath();
        _root.Children.Add(clipPath);

        return new SvgClipPathBuilder(this, clipPath);
    }

    /// <summary>
    /// Adds a new polygon element to the SVG document and returns a builder for configuring the polygon.
    /// </summary>
    /// <returns>A <see cref="SvgPolygonBuilder"/> instance for configuring the newly added &lt;polygon&gt; element.</returns>
    public SvgPolygonBuilder AddPolygon()
    {
        var polygon = new SvgPolygon();
        _root.Children.Add(polygon);

        return new SvgPolygonBuilder(this, polygon);
    }

    /// <summary>
    /// Generates and returns the string representation of the current object hierarchy.
    /// </summary>
    /// <returns>A string containing the serialized representation of the object tree.</returns>
    public string Build()
    {
        var sb = new StringBuilder();
        _root.WriteTo(sb);

        return sb.ToString();
    }
}
