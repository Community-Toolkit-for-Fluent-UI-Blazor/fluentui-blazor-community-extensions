using FluentUI.Blazor.Community.Components.Extensions;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a builder for configuring and constructing an SVG radial gradient element using a fluent interface.
/// </summary>
/// <remarks>Use this class to define the center, radius, and color stops of a radial gradient in SVG markup. The
/// builder pattern enables chaining of configuration methods for concise and readable gradient definitions. Once
/// configuration is complete, call Close to return to the parent SVG builder and continue constructing the SVG
/// document.</remarks>
public sealed class SvgRadialGradientBuilder : SvgElementBuilderBase<SvgRadialGradientBuilder, SvgRadialGradient>
{
    /// <summary>
    /// Initializes a new instance of the SvgRadialGradientBuilder class with the specified parent builder and radial
    /// gradient definition.
    /// </summary>
    /// <param name="parent">The parent SvgBuilder instance to which this radial gradient builder is associated. Cannot be null.</param>
    /// <param name="gradient">The SvgRadialGradient object that defines the properties of the radial gradient to be built. Cannot be null.</param>
    public SvgRadialGradientBuilder(SvgBuilder parent, SvgRadialGradient gradient)
        : base(parent, gradient)
    {
    }

    /// <summary>
    /// Sets the center coordinates of the radial gradient in the SVG element.
    /// </summary>
    /// <remarks>Use this method to position the center of the radial gradient within the SVG coordinate
    /// system. The values can be absolute or percentage-based, depending on the SVG context.</remarks>
    /// <param name="cx">The x-coordinate of the gradient's center, specified in user units or as a percentage.</param>
    /// <param name="cy">The y-coordinate of the gradient's center, specified in user units or as a percentage.</param>
    /// <returns>The current instance of <see cref="SvgRadialGradientBuilder"/> to allow method chaining.</returns>
    public SvgRadialGradientBuilder Center(double cx, double cy)
    {
        Element.SetAttribute("cx", cx.ToSvg());
        Element.SetAttribute("cy", cy.ToSvg());

        return this;
    }

    /// <summary>
    /// Sets the radius of the radial gradient in the SVG element.
    /// </summary>
    /// <param name="r">The radius value to assign to the gradient. Must be a non-negative number representing the radius in user units.</param>
    /// <returns>The current instance of <see cref="SvgRadialGradientBuilder"/> to allow method chaining.</returns>
    public SvgRadialGradientBuilder Radius(double r)
    {
        Element.SetAttribute("r", r.ToSvg());

        return this;
    }

    /// <summary>
    /// Adds a color stop to the radial gradient at the specified offset.
    /// </summary>
    /// <param name="offset">The position of the color stop within the gradient, specified as a value between 0.0 and 1.0, where 0.0
    /// represents the start and 1.0 represents the end of the gradient.</param>
    /// <param name="color">The color of the stop, specified as a CSS color string (e.g., "#FF0000" or "red").</param>
    /// <param name="opacity">The opacity of the color stop, specified as a value between 0.0 (fully transparent) and 1.0 (fully opaque). If
    /// null, the stop will use the default opacity.</param>
    /// <returns>The current instance of <see cref="SvgRadialGradientBuilder"/>, allowing for method chaining.</returns>
    public SvgRadialGradientBuilder AddStop(
        double offset,
        string color,
        double? opacity = null)
    {
        var stop = new SvgStop();
        Element.Children.Add(stop);

        stop.SetAttribute("offset", offset.ToSvg());
        stop.SetAttribute("stop-color", color);

        if (opacity.HasValue)
        {
            stop.SetAttribute("stop-opacity", opacity.Value.ToSvg());
        }

        return this;
    }
}
