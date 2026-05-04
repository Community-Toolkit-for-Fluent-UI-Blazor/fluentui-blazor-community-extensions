using FluentUI.Blazor.Community.Components.Enums;
using FluentUI.Blazor.Community.Components.Extensions;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a builder for configuring and constructing an SVG linear gradient element using a fluent interface.
/// </summary>
/// <remarks>Use this builder to define the start and end points, spread method, and color stops of a linear
/// gradient in SVG markup. The builder supports method chaining for concise and readable gradient definitions. Once
/// configuration is complete, call Close() to return to the parent SVG builder and continue constructing the SVG
/// document.</remarks>
public sealed class SvgLinearGradientBuilder : SvgElementBuilderBase<SvgLinearGradientBuilder, SvgLinearGradient>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SvgLinearGradientBuilder"/> class with the specified parent builder and circle element.
    /// </summary>
    /// <param name="parent">The parent SvgBuilder that this builder is associated with. Cannot be null.</param>
    /// <param name="gradient">The <see cref="SvgLinearGradient" /> element to configure with this builder. Cannot be null.</param>
    public SvgLinearGradientBuilder(SvgBuilder parent, SvgLinearGradient gradient)
        : base(parent, gradient)
    {
    }

    /// <summary>
    /// Sets the starting coordinates for the linear gradient in the SVG element.
    /// </summary>
    /// <param name="x1">The x-coordinate of the gradient's starting point, specified as a double value.</param>
    /// <param name="y1">The y-coordinate of the gradient's starting point, specified as a double value.</param>
    /// <returns>The current instance of <see cref="SvgLinearGradientBuilder"/> to allow method chaining.</returns>
    public SvgLinearGradientBuilder From(double x1, double y1)
    {
        return WithAttribute("x1", x1.ToSvg())
              .WithAttribute("y1", y1.ToSvg());
    }

    /// <summary>
    /// Sets the ending coordinates of the linear gradient and returns a new builder instance with the updated
    /// attributes.
    /// </summary>
    /// <param name="x2">The x-coordinate of the gradient's end point, specified in user space or as a percentage.</param>
    /// <param name="y2">The y-coordinate of the gradient's end point, specified in user space or as a percentage.</param>
    /// <returns>A new instance of the builder with the 'x2' and 'y2' attributes set to the specified values.</returns>
    public SvgLinearGradientBuilder To(double x2, double y2)
    {
        return WithAttribute("x2", x2.ToSvg())
              .WithAttribute("y2", y2.ToSvg());
    }

    /// <summary>
    /// Sets the spread method for the linear gradient element.
    /// </summary>
    /// <remarks>The spread method determines how the gradient is rendered outside the defined range. Refer to
    /// the SVG specification for valid values and their effects.</remarks>
    /// <param name="mode">The spread method to apply to the gradient.</param>
    /// <returns>The current instance of <see cref="SvgLinearGradientBuilder"/>, enabling method chaining.</returns>
    public SvgLinearGradientBuilder WithSpreadMethod(SvgSpreadMode mode)
    {
        return WithAttribute("spreadMethod", mode.ToString().ToLowerInvariant());
    }

    /// <summary>
    /// Adds a color stop to the linear gradient at the specified offset.
    /// </summary>
    /// <param name="offset">The position of the color stop within the gradient, specified as a value between 0.0 and 1.0, where 0.0
    /// represents the start and 1.0 represents the end of the gradient.</param>
    /// <param name="color">The color of the stop, specified as a CSS color string (e.g., "#FF0000" or "red").</param>
    /// <param name="opacity">The opacity of the color stop, specified as a value between 0.0 (fully transparent) and 1.0 (fully opaque). If
    /// null, the stop will use the default opacity.</param>
    /// <returns>The current instance of <see cref="SvgLinearGradientBuilder"/>, allowing for method chaining.</returns>
    public SvgLinearGradientBuilder AddStop(double offset, string color, double? opacity = null)
    {
        var stop = new SvgStop();
        stop.SetAttribute("offset", offset.ToSvg());
        stop.SetAttribute("stop-color", color);

        if (opacity.HasValue)
        {
            stop.SetAttribute("stop-opacity", opacity.Value.ToSvg());
        }

        Element.Children.Add(stop);

        return this;
    }
}
