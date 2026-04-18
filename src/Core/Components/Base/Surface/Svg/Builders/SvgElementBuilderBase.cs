using FluentUI.Blazor.Community.Components.Extensions;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a base class for building SVG element components with a fluent API.
/// </summary>
/// <remarks>This class is intended to be inherited by specific SVG element builder implementations to enable
/// fluent configuration and construction of SVG elements within a parent SvgBuilder context.</remarks>
/// <typeparam name="TBuilder">The type of the concrete builder class implementing the fluent API.</typeparam>
/// <typeparam name="TElement">The type of the SVG element being constructed. Must derive from SvgElement.</typeparam>
public abstract class SvgElementBuilderBase<TBuilder, TElement> : ISvgAnimatable<TBuilder>
    where TBuilder : class
    where TElement : SvgElement
{
    /// <summary>
    /// Represents the parent SvgBuilder instance associated with this object.
    /// </summary>
    private readonly SvgBuilder _parent;

    /// <summary>
    /// Represents the underlying SVG circle element used by this component.
    /// </summary>
    private readonly TElement _element;

    /// <summary>
    /// Initializes a new instance of the <see cref="SvgElementBuilderBase{TBuilder, TElement}"/> class with the specified parent builder and circle element.
    /// </summary>
    /// <param name="parent">The parent SvgBuilder that this builder is associated with. Cannot be null.</param>
    /// <param name="element">The SVG element to configure with this builder. Cannot be null.</param>
    public SvgElementBuilderBase(SvgBuilder parent, TElement element)
    {
        _parent = parent;
        _element = element;
    }

    /// <summary>
    /// Gets the parent <see cref="SvgBuilder"/> instance associated with the current builder.
    /// </summary>
    protected SvgBuilder Parent => _parent;

    /// <summary>
    /// Gets the underlying element associated with the component.
    /// </summary>
    protected internal TElement Element => _element;

    /// <summary>
    /// Sets the fill color attribute for the underlying element.
    /// </summary>
    /// <remarks>This method enables fluent configuration of the element's fill color. The specified color
    /// will be assigned to the 'fill' attribute of the element, affecting its rendering in supported contexts such as
    /// SVG or styled components.</remarks>
    /// <param name="color">The fill color to apply. This value is typically a CSS color string such as a color name, hex code, or RGB
    /// value.</param>
    /// <returns>The current builder instance with the updated fill color attribute.</returns>
    public TBuilder WithFill(string? color)
    {
        if (!string.IsNullOrEmpty(color))
        {
            _element.Attributes["fill"] = color;
        }

        return (TBuilder)(object)this;
    }

    /// <summary>
    /// Sets the svg 'id' attribute for the underlying element being built.
    /// </summary>
    /// <remarks>Use this method to specify a unique identifier for the generated HTML element, which can be
    /// useful for styling, scripting, or accessibility purposes.</remarks>
    /// <param name="id">The value to assign to the 'id' attribute. Can be null or empty to remove the attribute.</param>
    /// <returns>The builder instance with the updated 'id' attribute, enabling method chaining.</returns>
    public TBuilder WithId(string id)
    {
        _element.Attributes["id"] = id;

        return (TBuilder)(object)this;
    }

    /// <summary>
    /// Sets the 'clip-path' attribute for the underlying element being built, referencing a clip path defined elsewhere in the SVG document.
    /// </summary>
    /// <param name="clipPathId">The identifier of the clip path to reference, without the 'url(#...)' syntax.
    ///  This should correspond to the 'id' attribute of a &lt;clipPath&gt; element defined in the SVG document.</param>    
    /// <returns>The builder instance with the updated 'clip-path' attribute, enabling method chaining.</returns>
    public TBuilder WithClipPath(string? clipPathId)
    {
        if (!string.IsNullOrEmpty(clipPathId))
        {
            _element.Attributes["clip-path"] = $"url(#{clipPathId})";
        }

        return (TBuilder)(object)this;
    }

    /// <summary>
    /// Sets the stroke color attribute for the element being built.
    /// </summary>
    /// <param name="color">The color value to apply to the stroke attribute. This can be any valid CSS color string.</param>
    /// <returns>The current builder instance with the updated stroke color.</returns>
    public TBuilder WithStroke(string? color)
    {
        if (!string.IsNullOrEmpty(color))
        {
            _element.Attributes["stroke"] = color;
        }

        return (TBuilder)(object)this;
    }

    /// <summary>
    /// Sets the stroke width for the SVG element being built.
    /// </summary>
    /// <param name="width">The width of the stroke to apply, in user units. Must be a non-negative value.</param>
    /// <returns>The builder instance with the updated stroke width, enabling method chaining.</returns>
    public TBuilder WithStrokeWidth(double? width)
    {
        var stringWidth = width?.ToSvg();

        if (!string.IsNullOrEmpty(stringWidth))
        {
            _element.Attributes["stroke-width"] = stringWidth;
        }

        return (TBuilder)(object)this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="array"></param>
    /// <returns></returns>
    public TBuilder WithStrokeDashArray(double[]? array)
    {
        if (array?.Length > 0)
        {
            _element.Attributes["stroke-dasharray"] = string.Join(' ', array);
        }

        return (TBuilder)(object)this;
    }

    /// <summary>
    /// Sets the opacity value for the SVG element being built.
    /// </summary>
    /// <remarks>Use this method to control the transparency of the SVG element. Values outside the range of
    /// 0.0 to 1.0 may result in unexpected rendering behavior.</remarks>
    /// <param name="value">The opacity value to apply. Must be between 0.0 (fully transparent) and 1.0 (fully opaque).</param>
    /// <returns>The current builder instance with the updated opacity value.</returns>
    public TBuilder WithOpacity(double value)
    {
        _element.Attributes["opacity"] = value.ToSvg();

        return (TBuilder)(object)this;
    }

    /// <summary>
    /// Sets the CSS class attribute for the underlying element and returns the builder instance for method chaining.
    /// </summary>
    /// <remarks>Use this method to specify one or more CSS classes for the element being built. This enables
    /// fluent configuration of the element's appearance.</remarks>
    /// <param name="className">The CSS class or classes to assign to the element. This value replaces any existing class attribute.</param>
    /// <returns>The current builder instance with the updated class attribute.</returns>
    public TBuilder WithClass(string className)
    {
        _element.Attributes["class"] = className;

        return (TBuilder)(object)this;
    }

    /// <summary>
    /// Sets the SVG transform attribute for the current element and returns the builder instance for method chaining.
    /// </summary>
    /// <remarks>Use this method to apply transformations to SVG elements when constructing them with the
    /// builder. The method supports fluent chaining of additional configuration methods.</remarks>
    /// <param name="transform">The value to assign to the SVG transform attribute. This string defines the transformation to apply, such as
    /// translation, rotation, or scaling.</param>
    /// <returns>The current builder instance with the updated transform attribute, enabling fluent configuration.</returns>
    public TBuilder WithTransform(string transform)
    {
        _element.Attributes["transform"] = transform;

        return (TBuilder)(object)this;
    }

    /// <summary>
    /// Sets the value of the 'transform-box' attribute for the SVG group element.
    /// </summary>
    /// <remarks>The 'transform-box' attribute affects how transformations are applied to the SVG element.
    /// Refer to the SVG specification for valid values and their effects.</remarks>
    /// <param name="value">The value to assign to the 'transform-box' attribute. This determines how the SVG element's bounding box is
    /// calculated for transformations. Common values include 'fill-box', 'stroke-box', and 'view-box'.</param>
    /// <returns>The current instance to allow method chaining.</returns>
    public TBuilder WithTransformBox(string value)
    {
        Element.Attributes["transform-box"] = value;

        return (TBuilder)(object)this;
    }

    /// <summary>
    /// Sets the CSS 'transform-origin' attribute for the SVG group element.
    /// </summary>
    /// <param name="value">The value to assign to the 'transform-origin' attribute. This defines the point around which transformations are
    /// applied. Cannot be null.</param>
    /// <returns>The current instance to allow method chaining.</returns>
    public TBuilder WithTransformOrigin(string value)
    {
        Element.Attributes["transform-origin"] = value;

        return (TBuilder)(object)this;
    }

    /// <summary>
    /// Adds or updates an attribute with the specified name and value on the underlying element and returns the builder
    /// instance for method chaining.
    /// </summary>
    /// <remarks>If an attribute with the specified name already exists, its value is overwritten. This method
    /// supports fluent API usage by returning the builder instance.</remarks>
    /// <param name="name">The name of the attribute to add or update. Cannot be null.</param>
    /// <param name="value">The value to assign to the attribute. Can be null or empty.</param>
    /// <returns>The current builder instance with the updated attribute, enabling fluent configuration.</returns>
    public TBuilder WithAttribute(string name, string? value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            _element.Attributes[name] = value;
        }

        return (TBuilder)(object)this;
    }

    /// <summary>
    /// Adds or updates the 'pointer-events' attribute for the underlying element.
    /// </summary>
    /// <param name="value">The value to assign to the attribute. Can be null or empty.</param>
    /// <returns>The current builder instance with the updated attribute, enabling fluent configuration.</returns>
    protected internal TBuilder WithPointerEvents(string value)
    {
        return WithAttribute("pointer-events", value);
    }

    /// <summary>
    /// Adds or updates an attribute with the specified name and double-precision value for the current builder
    /// instance.
    /// </summary>
    /// <remarks>If the value is not null, it is converted to a string representation suitable for SVG
    /// attributes before being applied.</remarks>
    /// <param name="name">The name of the attribute to add or update. Cannot be null.</param>
    /// <param name="value">The value to assign to the attribute, or null to remove the attribute if it exists.</param>
    /// <returns>The current builder instance with the specified attribute set.</returns>
    protected internal TBuilder WithAttribute(string name, double? value)
    {
        return WithAttribute(name, value?.ToSvg());
    }

    /// <summary>
    /// Adds or updates an attribute with the specified name and double-precision value for the current builder
    /// instance.
    /// </summary>
    /// <remarks>If the value is not null, it is converted to a string representation suitable for SVG
    /// attributes before being applied.</remarks>
    /// <param name="name">The name of the attribute to add or update. Cannot be null.</param>
    /// <param name="value">The value to assign to the attribute, or null to remove the attribute if it exists.</param>
    /// <returns>The current builder instance with the specified attribute set.</returns>
    protected internal TBuilder WithAttribute(string name, int value)
    {
        return WithAttribute(name, value.ToSvg());
    }

    /// <summary>
    /// Adds or updates an attribute with the specified name and double-precision value for the current builder
    /// instance.
    /// </summary>
    /// <remarks>If the value is not null, it is converted to a string representation suitable for SVG
    /// attributes before being applied.</remarks>
    /// <param name="name">The name of the attribute to add or update. Cannot be null.</param>
    /// <param name="value">The value to assign to the attribute, or null to remove the attribute if it exists.</param>
    /// <returns>The current builder instance with the specified attribute set.</returns>
    protected internal TBuilder WithAttribute(string name, double value)
    {
        return WithAttribute(name, value.ToSvg());
    }

    /// <summary>
    /// Adds or updates an attribute with the specified name and double-precision value for the current builder
    /// instance.
    /// </summary>
    /// <remarks>If the value is not null, it is converted to a string representation suitable for SVG
    /// attributes before being applied.</remarks>
    /// <param name="name">The name of the attribute to add or update. Cannot be null.</param>
    /// <param name="value">The value to assign to the attribute, or null to remove the attribute if it exists.</param>
    /// <returns>The current builder instance with the specified attribute set.</returns>
    protected internal TBuilder WithAttribute(string name, int? value)
    {
        return WithAttribute(name, value?.ToSvg());
    }

    /// <summary>
    /// Adds or updates an attribute with the specified name and value on the underlying element and returns the builder
    /// instance for method chaining.
    /// </summary>
    /// <remarks>If an attribute with the specified name already exists, its value is overwritten. This method
    /// supports fluent API usage by returning the builder instance.</remarks>
    /// <param name="name">The name of the attribute to add or update. Cannot be null.</param>
    /// <param name="value">The value to assign to the attribute. Can be null or empty.</param>
    /// <returns>The current builder instance with the updated attribute, enabling fluent configuration.</returns>
    protected internal TBuilder WithAttribute<T>(string name, T value)
    {
        var attrValue = value?.ToString();

        if (!string.IsNullOrEmpty(attrValue))
        {
            _element.Attributes[name] = attrValue;
        }

        return (TBuilder)(object)this;
    }

    /// <inheritdoc />
    public TBuilder AddAnimate(
        string attribute,
        Action<SvgAnimateBuilder> config)
    {
        var anim = new SvgAnimate();
        anim.Attributes["attributeName"] = attribute;

        Element.Children.Add(anim);

        var builder = new SvgAnimateBuilder(Parent, anim);
        config(builder);

        return (TBuilder)(object)this;
    }

    /// <inheritdoc />
    public TBuilder AddAnimateTransform(
        string type,
        Action<SvgAnimateTransformBuilder> configure)
    {
        var anim = new SvgAnimateTransform();
        anim.Attributes["attributeName"] = "transform";
        anim.Attributes["type"] = type;

        Element.Children.Add(anim);

        var builder = new SvgAnimateTransformBuilder(Parent, anim);
        configure(builder);

        return (TBuilder)(object)this;
    }

    /// <inheritdoc />
    public TBuilder AddAnimateMotion(
        Action<SvgAnimateMotionBuilder> configure)
    {
        var anim = new SvgAnimateMotion();
        Element.Children.Add(anim);

        var builder = new SvgAnimateMotionBuilder(Parent, anim);
        configure(builder);

        return (TBuilder)(object)this;
    }

    /// <summary>
    /// Closes the current SVG element and returns to the parent builder context.
    /// </summary>
    /// <remarks>Use this method to complete the definition of a nested SVG element and continue building the
    /// parent element. This enables fluent, hierarchical SVG construction.</remarks>
    /// <returns>The parent <see cref="SvgBuilder"/> instance, allowing for fluent chaining of SVG element construction.</returns>
    public virtual SvgBuilder Close() => _parent;
}
