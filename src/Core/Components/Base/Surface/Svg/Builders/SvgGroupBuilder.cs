using FluentUI.Blazor.Community.Components.Extensions;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a builder for configuring and composing SVG group (&lt;g&gt;) elements, allowing the addition of child shapes and
/// transformations in a fluent manner.
/// </summary>
/// <remarks>Use this builder to construct complex SVG groupings by adding rectangles, paths, circles, and
/// applying transformations. The builder maintains a reference to its parent builder, enabling hierarchical SVG
/// composition. All methods return builder instances to support fluent chaining.</remarks>
public sealed class SvgGroupBuilder : SvgElementBuilderBase<SvgGroupBuilder, SvgGroup>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SvgGroupBuilder"/> class with the
    ///  specified parent builder and group element.
    /// </summary>
    /// <param name="parent">The parent SvgBuilder that this builder is associated with. Cannot be null.</param>
    /// <param name="group">The SvgCircle element to configure with this builder. Cannot be null.</param>
    public SvgGroupBuilder(SvgBuilder parent, SvgGroup group)
        : base(parent, group)
    {
    }

    /// <summary>
    /// Adds a new SVG group element as a child of the current element and returns a builder for further configuration.
    /// </summary>
    /// <remarks>Use the returned <see cref="SvgGroupBuilder"/> to add child elements or set attributes on the
    /// new group. This method simplifies the creation of nested SVG group structures.</remarks>
    /// <returns>A <see cref="SvgGroupBuilder"/> instance for configuring the newly added group element.</returns>
    public SvgGroupBuilder AddGroup()
    {
        var group = new SvgGroup();
        Element.Children.Add(group);

        return new SvgGroupBuilder(Parent, group);
    }

    /// <summary>
    /// Adds a rectangle element to the current SVG group at the specified position and size.
    /// </summary>
    /// <param name="x">The x-coordinate of the upper-left corner of the rectangle, in user units.</param>
    /// <param name="y">The y-coordinate of the upper-left corner of the rectangle, in user units.</param>
    /// <param name="width">The width of the rectangle, in user units. Must be greater than or equal to zero.</param>
    /// <param name="height">The height of the rectangle, in user units. Must be greater than or equal to zero.</param>
    /// <returns>A builder for further configuring the newly added rectangle element.</returns>
    public SvgRectBuilder AddRect(double x, double y, double width, double height)
    {
        var rect = new SvgRect();
        Element.Children.Add(rect);

        return new SvgRectBuilder(Parent, rect)
            .WithX(x)
            .WithY(y)
            .WithWidth(width)
            .WithHeight(height);
    }

    /// <summary>
    /// Adds a new SVG path element as a child and returns a builder for configuring the path.
    /// </summary>
    /// <returns>A <see cref="SvgPathBuilder"/> instance for further configuration of the newly added SVG path.</returns>
    public SvgPathBuilder AddPath()
    {
        var path = new SvgPath();
        Element.Children.Add(path);

        return new SvgPathBuilder(Parent, path);
    }

    /// <summary>
    /// Adds a new circle element to the current SVG group with the specified center coordinates and radius.
    /// </summary>
    /// <param name="cx">The x-coordinate of the center of the circle, in user units.</param>
    /// <param name="cy">The y-coordinate of the center of the circle, in user units.</param>
    /// <param name="r">The radius of the circle, in user units. Must be non-negative.</param>
    /// <returns>A builder for the newly added SVG circle element, allowing further configuration.</returns>
    public SvgCircleBuilder AddCircle(double cx, double cy, double r)
    {
        var circle = new SvgCircle();
        Element.Children.Add(circle);

        return new SvgCircleBuilder(Parent, circle)
            .WithCx(cx)
            .WithCy(cy)
            .WithRadius(r);
    }

    /// <summary>
    /// Adds a new text element to the SVG at the specified coordinates with the given value.
    /// </summary>
    /// <param name="x">The horizontal position, in SVG coordinate space, where the text element will be placed.</param>
    /// <param name="y">The vertical position, in SVG coordinate space, where the text element will be placed.</param>
    /// <param name="value">The string value to display within the text element.</param>
    /// <returns>A builder object for further configuration of the newly added SVG text element.</returns>
    public SvgTextBuilder AddText(double x, double y, string? value)
    {
        var text = new SvgText()
        {
            Text = value
        };

        text.Attributes["x"] = x.ToSvg();
        text.Attributes["y"] = y.ToSvg();

        Element.Children.Add(text);

        return new SvgTextBuilder(Parent, text);
    }

    /// <summary>
    /// Adds a new polygon element to the current SVG group.
    /// </summary>
    /// <returns>A <see cref="SvgPolygonBuilder"/> instance for configuring the newly added &lt;polygon&gt; element.</returns>
    public SvgPolygonBuilder AddPolygon()
    {
        var polygon = new SvgPolygon();
        Element.Children.Add(polygon);

        return new SvgPolygonBuilder(Parent, polygon);
    }
}
