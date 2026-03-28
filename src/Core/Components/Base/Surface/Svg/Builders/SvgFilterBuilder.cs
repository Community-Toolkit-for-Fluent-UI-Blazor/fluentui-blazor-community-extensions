using FluentUI.Blazor.Community.Components.Extensions;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a builder for configuring SVG filter elements in a fluent manner.
/// </summary>
/// <remarks>Use this class to define and customize SVG filter attributes when constructing SVG graphics. The
/// builder pattern enables chaining of configuration methods for concise and readable filter setup. Instances of this
/// class are typically created via the parent SvgBuilder when adding filters to an SVG document.</remarks>
public sealed class SvgFilterBuilder : SvgElementBuilderBase<SvgFilterBuilder, SvgFilter>
{
    /// <summary>
    /// Initializes a new instance of the SvgFilterBuilder class with the specified parent builder and filter
    /// definition.
    /// </summary>
    /// <param name="parent">The parent SvgBuilder instance that this builder is associated with. Cannot be null.</param>
    /// <param name="filter">The SvgFilter object that defines the filter to be built. Cannot be null.</param>
    public SvgFilterBuilder(SvgBuilder parent, SvgFilter filter)
        : base(parent, filter)
    {
    }

    /// <summary>
    /// Sets the bounding rectangle for the SVG filter using the specified position and size.
    /// </summary>
    /// <remarks>Setting the bounds defines the area of the SVG element to which the filter effects will be
    /// applied. Values are typically specified as user space coordinates or percentages, depending on the SVG
    /// context.</remarks>
    /// <param name="x">The x-coordinate of the upper-left corner of the filter region, in user space units.</param>
    /// <param name="y">The y-coordinate of the upper-left corner of the filter region, in user space units.</param>
    /// <param name="width">The width of the filter region, in user space units. Must be greater than or equal to zero.</param>
    /// <param name="height">The height of the filter region, in user space units. Must be greater than or equal to zero.</param>
    /// <returns>The current instance of <see cref="SvgFilterBuilder"/>, enabling method chaining.</returns>
    public SvgFilterBuilder WithBounds(double x, double y, double width, double height)
    {
        Element.Attributes["x"] = x.ToSvg();
        Element.Attributes["y"] = y.ToSvg();
        Element.Attributes["width"] = width.ToSvg();
        Element.Attributes["height"] = height.ToSvg();

        return this;
    }

    // Tu pourras ajouter feGaussianBlur, feOffset, etc. ici
}
