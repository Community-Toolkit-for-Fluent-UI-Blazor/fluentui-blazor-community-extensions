namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a builder for configuring and adding SVG definition elements, such as gradients, clip paths, masks,
/// patterns, markers, symbols, and filters, to an SVG document's &lt;defs&gt; section.
/// </summary>
/// <remarks>Use this builder to add reusable SVG definitions that can be referenced elsewhere in the SVG
/// document. Each method creates and adds a specific type of definition element to the &lt;defs&gt; section and returns a
/// builder for further configuration of that element. This class is typically used as part of a fluent API for
/// constructing complex SVG graphics.</remarks>
public sealed class SvgDefsBuilder : SvgElementBuilderBase<SvgDefsBuilder, SvgDefs>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SvgDefsBuilder"/> class with the specified parent builder and circle element.
    /// </summary>
    /// <param name="parent">The parent SvgBuilder that this builder is associated with. Cannot be null.</param>
    /// <param name="defs">The SvgDefs element to configure with this builder. Cannot be null.</param>
    public SvgDefsBuilder(SvgBuilder parent, SvgDefs defs)
        : base(parent, defs)
    {
    }

    /// <summary>
    /// Adds a new linear gradient definition to the SVG and returns a builder for further configuration.
    /// </summary>
    /// <param name="id">The unique identifier to assign to the linear gradient element. This value is used as the 'id' attribute in the
    /// SVG definition and must be unique within the document.</param>
    /// <returns>A builder object for configuring the newly added linear gradient element.</returns>
    public SvgLinearGradientBuilder AddLinearGradient(string id)
    {
        var g = new SvgLinearGradient();
        Element.Children.Add(g);

        return new SvgLinearGradientBuilder(Parent, g)
            .WithId(id);
    }

    /// <summary>
    /// Adds a new radial gradient element with the specified identifier to the current SVG element.
    /// </summary>
    /// <param name="id">The unique identifier to assign to the radial gradient element. Cannot be null or empty.</param>
    /// <returns>A builder for configuring the newly added radial gradient element.</returns>
    public SvgRadialGradientBuilder AddRadialGradient(string id)
    {
        var g = new SvgRadialGradient();
        Element.Children.Add(g);

        return new SvgRadialGradientBuilder(Parent, g)
            .WithId(id);
    }

    /// <summary>
    /// Adds a new SVG clip path element with the specified identifier to the current SVG element.
    /// </summary>
    /// <param name="id">The unique identifier to assign to the new clip path element. Cannot be null or empty.</param>
    /// <returns>A builder for the newly added SVG clip path element, allowing further configuration.</returns>
    public SvgClipPathBuilder AddClipPath(string id)
    {
        var c = new SvgClipPath();
        Element.Children.Add(c);

        return new SvgClipPathBuilder(Parent, c)
            .WithId(id);
    }

    /// <summary>
    /// Adds a new SVG mask element with the specified identifier to the current SVG element.
    /// </summary>
    /// <remarks>Use this method to define a mask within the SVG structure. The returned builder can be used
    /// to configure the mask's properties and child elements.</remarks>
    /// <param name="id">The unique identifier to assign to the mask element. Cannot be null or empty.</param>
    /// <returns>A builder for the newly added SVG mask element, allowing further configuration.</returns>
    public SvgMaskBuilder AddMask(string id)
    {
        var m = new SvgMask();
        Element.Children.Add(m);

        return new SvgMaskBuilder(Parent, m)
            .WithId(id);
    }

    /// <summary>
    /// Adds a new SVG pattern element with the specified identifier and returns a builder for further configuration.
    /// </summary>
    /// <remarks>Use the returned builder to set additional attributes or child elements on the pattern. The
    /// pattern is added as a child to the current SVG element.</remarks>
    /// <param name="id">The unique identifier to assign to the SVG pattern element. Cannot be null or empty.</param>
    /// <returns>A builder instance for configuring the newly added SVG pattern element.</returns>
    public SvgPatternBuilder AddPattern(string id)
    {
        var p = new SvgPattern();
        Element.Children.Add(p);

        return new SvgPatternBuilder(Parent, p)
            .WithId(id);
    }

    /// <summary>
    /// Adds a new marker element with the specified identifier to the current SVG element and returns a builder for
    /// further marker configuration.
    /// </summary>
    /// <param name="id">The unique identifier to assign to the marker element. Cannot be null or empty.</param>
    /// <returns>A builder object for configuring the newly added marker element.</returns>
    public SvgMarkerBuilder AddMarker(string id)
    {
        var m = new SvgMarker();
        Element.Children.Add(m);

        return new SvgMarkerBuilder(Parent, m)
            .WithId(id);
    }

    /// <summary>
    /// Adds a new SVG symbol element with the specified identifier to the current SVG document.
    /// </summary>
    /// <remarks>Use the returned builder to set additional attributes or child elements on the symbol. The
    /// identifier must be unique within the SVG document to avoid conflicts when referencing symbols.</remarks>
    /// <param name="id">The unique identifier to assign to the new SVG symbol element. Cannot be null or empty.</param>
    /// <returns>A builder instance for further configuration of the newly added SVG symbol element.</returns>
    public SvgSymbolBuilder AddSymbol(string id)
    {
        var s = new SvgSymbol();
        Element.Children.Add(s);

        return new SvgSymbolBuilder(Parent, s)
            .WithId(id);
    }

    /// <summary>
    /// Adds a new SVG filter element with the specified identifier to the current element and returns a builder for
    /// further filter configuration.
    /// </summary>
    /// <param name="id">The unique identifier to assign to the new SVG filter element. Cannot be null.</param>
    /// <returns>A builder object for configuring the newly added SVG filter element.</returns>
    public SvgFilterBuilder AddFilter(string id)
    {
        var f = new SvgFilter();
        Element.Children.Add(f);

        return new SvgFilterBuilder(Parent, f)
            .WithId(id);
    }
}

