namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a builder for constructing SVG clip path elements by adding shapes such as rectangles and paths.
/// </summary>
/// <remarks>Use this builder to define the contents of an SVG clip path in a fluent manner. After adding the
/// desired shapes, call Close to return to the parent builder and continue constructing the SVG document.</remarks>
public sealed class SvgClipPathBuilder : SvgElementBuilderBase<SvgClipPathBuilder, SvgClipPath>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SvgClipPathBuilder"/> class with the specified parent builder and clip path
    /// definition.
    /// </summary>
    /// <param name="parent">The parent SvgBuilder that this builder is associated with. Cannot be null.</param>
    /// <param name="clip">The SvgClipPath object that defines the clipping path to be used. Cannot be null.</param>
    public SvgClipPathBuilder(SvgBuilder parent, SvgClipPath clip)
        : base(parent, clip)
    {
    }

    /// <summary>
    /// Adds a rectangle element to the current SVG clip path with the specified position and size.
    /// </summary>
    /// <param name="x">The x-coordinate of the upper-left corner of the rectangle, in user units.</param>
    /// <param name="y">The y-coordinate of the upper-left corner of the rectangle, in user units.</param>
    /// <param name="width">The width of the rectangle, in user units. Must be non-negative.</param>
    /// <param name="height">The height of the rectangle, in user units. Must be non-negative.</param>
    /// <returns>A builder for further configuration of the newly added rectangle element.</returns>
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
    /// Adds a new SVG path to the current clip and returns a builder for configuring the path.
    /// </summary>
    /// <remarks>Use the returned <see cref="SvgPathBuilder"/> to define the geometry and attributes of the
    /// new path. The path is added to the collection of children in the current clip.</remarks>
    /// <returns>A <see cref="SvgPathBuilder"/> instance for further configuration of the newly added SVG path.</returns>
    public SvgPathBuilder AddPath()
    {
        var path = new SvgPath();
        Element.Children.Add(path);

        return new SvgPathBuilder(Parent, path);
    }
}
