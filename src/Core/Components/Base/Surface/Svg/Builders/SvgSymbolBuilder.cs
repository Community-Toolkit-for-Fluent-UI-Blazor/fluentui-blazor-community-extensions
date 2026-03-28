using FluentUI.Blazor.Community.Components.Extensions;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a builder for constructing and configuring an SVG &lt;symbol&gt; element, enabling the addition of child elements
/// and the specification of symbol attributes in a fluent manner.
/// </summary>
/// <remarks>Use this builder to define reusable SVG symbols, set their viewBox, and add child shapes such as
/// rectangles. The builder pattern allows for chaining configuration methods before returning to the parent SVG builder
/// context.</remarks>
public sealed class SvgSymbolBuilder : SvgElementBuilderBase<SvgSymbolBuilder, SvgSymbol>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SvgSymbolBuilder"/> class with the specified parent builder and circle element.
    /// </summary>
    /// <param name="parent">The parent SvgBuilder that this builder is associated with. Cannot be null.</param>
    /// <param name="symbol"></param>
    public SvgSymbolBuilder(SvgBuilder parent, SvgSymbol symbol)
        : base(parent, symbol)
    {
    }

    /// <summary>
    /// Sets the viewBox attribute for the SVG symbol using the specified position and size.
    /// </summary>
    /// <remarks>The viewBox defines the position and dimension of the SVG viewport for the symbol. This
    /// method enables fluent configuration of the SVG symbol's viewBox.</remarks>
    /// <param name="x">The x-coordinate of the viewBox's origin.</param>
    /// <param name="y">The y-coordinate of the viewBox's origin.</param>
    /// <param name="width">The width of the viewBox.</param>
    /// <param name="height">The height of the viewBox.</param>
    /// <returns>The current instance of the SvgSymbolBuilder with the updated viewBox attribute.</returns>
    public SvgSymbolBuilder WithViewBox(double x, double y, double width, double height)
    {
        Element.Attributes["viewBox"] = $"{x.ToSvg()} {y.ToSvg()} {width.ToSvg()} {height.ToSvg()}";

        return this;
    }

    /// <summary>
    /// Adds a rectangle element to the current SVG symbol with the specified position and dimensions.
    /// </summary>
    /// <param name="x">The x-coordinate of the upper-left corner of the rectangle, in user units.</param>
    /// <param name="y">The y-coordinate of the upper-left corner of the rectangle, in user units.</param>
    /// <param name="width">The width of the rectangle, in user units. Must be greater than or equal to zero.</param>
    /// <param name="height">The height of the rectangle, in user units. Must be greater than or equal to zero.</param>
    /// <returns>A new SvgRectBuilder instance for further configuration of the added rectangle.</returns>
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
