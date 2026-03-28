namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a builder for constructing and configuring SVG mask elements within an SVG document.
/// </summary>
/// <remarks>Use this class to fluently add shapes and configure properties for an SVG mask. The builder pattern
/// enables chaining of element additions and configuration calls, facilitating the creation of complex SVG mask
/// definitions. Instances of this class are typically created as part of a larger SVG building workflow and are not
/// intended for reuse across multiple masks.</remarks>
public sealed class SvgMaskBuilder
    : SvgElementBuilderBase<SvgMaskBuilder, SvgMask>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SvgCircleBuilder"/> class with the specified parent builder and circle element.
    /// </summary>
    /// <param name="parent">The parent SvgBuilder that this builder is associated with. Cannot be null.</param>
    /// <param name="mask">The SvgMask element to configure with this builder. Cannot be null.</param>
    public SvgMaskBuilder(SvgBuilder parent, SvgMask mask)
        : base(parent, mask)
    {
    }

    /// <summary>
    /// Adds a rectangle element to the current SVG mask with the specified position and size.
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

