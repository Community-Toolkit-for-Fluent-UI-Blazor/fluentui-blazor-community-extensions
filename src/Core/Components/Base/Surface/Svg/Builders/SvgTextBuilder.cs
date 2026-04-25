using FluentUI.Blazor.Community.Components.Enums;
using FluentUI.Blazor.Community.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a builder for configuring and creating SVG &lt;text&gt; elements with fluent syntax.
/// </summary>
/// <remarks>Use this class to set common SVG text attributes such as font family, font size, text anchor, and
/// dominant baseline when constructing SVG graphics. The builder pattern enables chaining of configuration methods for
/// concise and readable SVG element creation.</remarks>
public sealed class SvgTextBuilder : SvgElementBuilderBase<SvgTextBuilder, SvgText>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SvgTextBuilder"/> class
    ///  with the specified parent builder and text element.
    /// </summary>
    /// <param name="parent">The parent <see cref="SvgBuilder"/> that this builder is associated with. Cannot be null.</param>
    /// <param name="text">The <see cref="SvgText"/> element to configure with this builder. Cannot be null.</param>
    public SvgTextBuilder(SvgBuilder parent, SvgText text) : base(parent, text)
    { }

    /// <summary>
    /// Sets the x-coordinate of the text element.
    /// </summary>
    /// <param name="x">The horizontal coordinate value.</param>
    /// <returns>The current instance for method chaining.</returns>
    public SvgTextBuilder WithX(double x)
    {
        Element.Attributes["x"] = x.ToSvg();

        return this;
    }

    /// <summary>
    /// Sets the Y coordinate of the text element.
    /// </summary>
    /// <param name="y">The Y coordinate value.</param>
    /// <returns>The current <see cref="SvgTextBuilder"/> instance for method chaining.</returns>
    public SvgTextBuilder WithY(double y)
    {
        Element.Attributes["y"] = y.ToSvg();

        return this;
    }

    /// <summary>
    /// Sets the font family for the SVG text element.
    /// </summary>
    /// <param name="family">The name of the font family to apply to the text. This value is assigned to the 'font-family' attribute of the
    /// SVG element.</param>
    /// <returns>The current instance of <see cref="SvgTextBuilder"/>, enabling method chaining.</returns>
    public SvgTextBuilder WithFontFamily(string family)
    {
        Element.Attributes["font-family"] = family;

        return this;
    }

    /// <summary>
    /// Sets the font size for the text element in the SVG output.
    /// </summary>
    /// <param name="size">The font size to apply, specified as a double. The value must be greater than zero.</param>
    /// <returns>The current instance of <see cref="SvgTextBuilder"/>, enabling method chaining.</returns>
    public SvgTextBuilder WithFontSize(double size)
    {
        Element.Attributes["font-size"] = size.ToSvg();

        return this;
    }

    /// <summary>
    /// Sets the font size attribute for the SVG text element.
    /// </summary>
    /// <param name="size">The font size to apply to the SVG text element. This value can be any valid CSS font-size string, such as
    /// "16px", "1.5em", or "large". If null or empty, the font size is not modified.</param>
    /// <returns>The current instance of <see cref="SvgTextBuilder"/>, enabling method chaining.</returns>
    public SvgTextBuilder WithFontSize(string? size)
    {
        if (string.IsNullOrEmpty(size))
        {
            return this;
        }

        Element.Attributes["font-size"] = size;

        return this;
    }

    /// <summary>
    /// Sets the SVG 'text-anchor' attribute for the text element.
    /// </summary>
    /// <remarks>The 'text-anchor' attribute determines how the text is aligned relative to a given point.
    /// This method allows for fluent configuration of the text element's alignment.</remarks>
    /// <param name="anchor">The value to assign to the 'text-anchor' attribute. Common values include 'start', 'middle', or 'end'.</param>
    /// <returns>The current instance of <see cref="SvgTextBuilder"/>, enabling method chaining.</returns>
    public SvgTextBuilder WithTextAnchor(SvgTextAnchor anchor)
    {
        Element.Attributes["text-anchor"] = anchor.ToString().ToLowerInvariant();

        return this;
    }

    /// <summary>
    /// Sets the 'dominant-baseline' attribute for the SVG text element.
    /// </summary>
    /// <remarks>Use this method to control the vertical alignment of text within an SVG element. The effect
    /// of the 'dominant-baseline' value depends on the SVG rendering context and the specific value provided.</remarks>
    /// <param name="baseline">The value to assign to the 'dominant-baseline' attribute. Common values include 'auto', 'middle', 'hanging', and
    /// others as defined by the SVG specification.</param>
    /// <returns>The current instance of <see cref="SvgTextBuilder"/>, enabling method chaining.</returns>
    public SvgTextBuilder WithDominantBaseline(string baseline)
    {
        Element.Attributes["dominant-baseline"] = baseline;

        return this;
    }

    /// <summary>
    /// Sets a rotation transformation for the SVG text element around a specified point (x, y) by a given angle in degrees.
    /// </summary>
    /// <param name="x">The x-coordinate of the point around which to rotate the text. This value is used in the 'transform' attribute of the SVG element.</param>
    /// <param name="y">The y-coordinate of the point around which to rotate the text. This value is used in the 'transform' attribute of the SVG element.</param>
    /// <param name="angle">The angle in degrees to rotate the text around the specified point. This value is used in the 'transform' attribute of the SVG element.</param>
    /// <returns>The current instance of <see cref="SvgTextBuilder"/>, enabling method chaining.</returns>
    public SvgTextBuilder WithRotation(double angle, double x, double y)
    {
        Element.Attributes["transform"] = $"rotate({angle.ToSvg()} {x.ToSvg()} {y.ToSvg()})";

        return this;
    }

    /// <summary>
    /// Sets the font weight for the text element being built.
    /// </summary>
    /// <param name="fontWeight">The font weight to apply to the text element. Specifies the thickness of the characters, such as normal or bold.</param>
    /// <returns>The current instance of <see cref="SvgTextBuilder"/> with the specified font weight applied.</returns>
    /// <exception cref="NotImplementedException">The method is not implemented.</exception>
    public SvgTextBuilder WithFontWeight(TextWeight fontWeight)
    {
        Element.Attributes["font-weight"] = fontWeight switch
        {
            TextWeight.Regular => "400",
            TextWeight.Semibold => "600",
            TextWeight.Medium => "500",
            TextWeight.Bold => "700",
            _ => throw new NotImplementedException($"Font weight '{fontWeight}' is not implemented.")
        };

        return this;
    }
}
