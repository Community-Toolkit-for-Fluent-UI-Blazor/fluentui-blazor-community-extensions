using FluentUI.Blazor.Community.Components.Extensions;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a builder for configuring and constructing SVG pattern elements using a fluent interface.
/// </summary>
/// <remarks>Use this class to define SVG patterns by specifying attributes such as view box, size, and pattern
/// units, and by adding child elements like rectangles. The builder pattern enables chaining of configuration methods
/// for concise and readable SVG pattern creation. Once configuration is complete, call Close to return to the parent
/// builder and continue constructing the SVG document.</remarks>
public sealed class SvgPatternBuilder : SvgElementBuilderBase<SvgPatternBuilder, SvgPattern>
{
    /// <summary>
    /// Initializes a new instance of the SvgPatternBuilder class with the specified parent builder and pattern
    /// definition.
    /// </summary>
    /// <param name="parent">The parent SvgBuilder that this builder is associated with. Cannot be null.</param>
    /// <param name="pattern">The SvgPattern instance that defines the pattern to be built. Cannot be null.</param>
    public SvgPatternBuilder(SvgBuilder parent, SvgPattern pattern)
        : base(parent, pattern)
    {
    }

    /// <summary>
    /// Sets the viewBox attribute for the SVG pattern using the specified position and size.
    /// </summary>
    /// <remarks>Use this method to define the coordinate system and dimensions for the SVG pattern's viewBox,
    /// which determines how the pattern content is scaled and positioned within the pattern element.</remarks>
    /// <param name="x">The x-coordinate of the viewBox's origin.</param>
    /// <param name="y">The y-coordinate of the viewBox's origin.</param>
    /// <param name="width">The width of the viewBox.</param>
    /// <param name="height">The height of the viewBox.</param>
    /// <returns>The current instance of the SvgPatternBuilder with the updated viewBox attribute.</returns>
    public SvgPatternBuilder WithViewBox(double x, double y, double width, double height)
    {
        Element.SetAttribute("viewBox", $"{x.ToSvg()} {y.ToSvg()} {width.ToSvg()} {height.ToSvg()}");

        return this;
    }

    /// <summary>
    /// Sets the width and height of the SVG pattern element.
    /// </summary>
    /// <remarks>Use this method to specify the dimensions of the SVG pattern. Setting appropriate width and
    /// height is required for correct rendering of the pattern in SVG graphics.</remarks>
    /// <param name="width">The width of the pattern, in user units. Must be a non-negative value.</param>
    /// <param name="height">The height of the pattern, in user units. Must be a non-negative value.</param>
    /// <returns>The current instance of <see cref="SvgPatternBuilder"/> with the updated size.</returns>
    public SvgPatternBuilder WithSize(double width, double height)
    {
        Element.SetAttribute("width", width.ToSvg());
        Element.SetAttribute("height", height.ToSvg());

        return this;
    }

    /// <summary>
    /// Sets the value of the 'patternUnits' attribute for the SVG pattern element being built.
    /// </summary>
    /// <param name="units">The coordinate system to use for the pattern's content units. Common values are "userSpaceOnUse" or
    /// "objectBoundingBox".</param>
    /// <returns>The current instance of <see cref="SvgPatternBuilder"/> to allow method chaining.</returns>
    public SvgPatternBuilder WithPatternUnits(string units)
    {
        Element.SetAttribute("patternUnits", units);

        return this;
    }

    /// <summary>
    /// Adds a rectangle element to the current SVG pattern with the specified position and size.
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
}
