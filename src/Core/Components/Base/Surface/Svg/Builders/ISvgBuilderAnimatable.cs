namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an interface for SVG elements that can be animated.
/// </summary>
public interface ISvgAnimatable<TBuilder>
{
    /// <summary>
    /// Adds an SVG animate element for the specified attribute and configures it using the provided builder action.
    /// </summary>
    /// <remarks>Use this method to define custom animations for SVG attributes by specifying the attribute to
    /// animate and configuring the animation parameters through the builder. Multiple animations can be added by
    /// calling this method multiple times with different attributes.</remarks>
    /// <param name="attribute">The name of the SVG attribute to animate. This value cannot be null or empty.</param>
    /// <param name="config">An action that configures the animation using the provided <see cref="SvgAnimateBuilder"/> instance.</param>
    TBuilder AddAnimate(string attribute, Action<SvgAnimateBuilder> config);

    /// <summary>
    /// Sets the opacity level for the current element.
    /// </summary>
    /// <param name="value">The opacity value to apply. Must be between 0.0 (fully transparent) and 1.0 (fully opaque).</param>
    TBuilder WithOpacity(double? value);

    /// <summary>
    /// Adds an attribute with the specified name and value.
    /// </summary>
    /// <param name="name">The name of the attribute to add. Cannot be null or empty.</param>
    /// <param name="value">The value to assign to the attribute. Can be null or empty.</param>
    TBuilder WithAttribute(string name, string value);

    /// <summary>
    /// Adds an SVG animateTransform element for the specified transform type and configures it using the provided builder action.
    /// </summary>
    /// <param name="type">The type of transformation to animate. This value cannot be null or empty.</param>
    /// <param name="configure">An action that configures the animation using the provided <see cref="SvgAnimateTransformBuilder"/> instance.</param>
    /// <returns>The current builder instance with the added animateTransform element.</returns>
    TBuilder AddAnimateTransform(string type, Action<SvgAnimateTransformBuilder> configure);

    /// <summary>
    /// Adds an &lt;animateMotion&gt; SVG element to the current builder and allows configuration of its properties.
    /// </summary>
    /// <remarks>Use this method to animate the motion of an SVG element along a specified path. The <paramref
    /// name="configure"/> action is used to set up the animation's attributes and behavior.</remarks>
    /// <param name="configure">A delegate that receives an instance of <see cref="SvgAnimateMotionBuilder"/> to configure the &lt;animateMotion&gt;
    /// element. Cannot be null.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    TBuilder AddAnimateMotion(Action<SvgAnimateMotionBuilder> configure);

    /// <summary>
    /// Sets the CSS transform to apply to the component being built.
    /// </summary>
    /// <remarks>Use this method to customize the visual transformation of the component. Common values
    /// include standard CSS transform functions such as 'rotate(45deg)', 'scale(1.5)', or 'translateX(10px)'.</remarks>
    /// <param name="transform">The CSS transform string to apply. This value determines the transformation applied to the component's
    /// rendering, such as scaling, rotating, or translating. Cannot be null.</param>
    /// <returns>The builder instance with the specified transform applied, enabling method chaining.</returns>
    TBuilder WithTransform(string transform);
}
