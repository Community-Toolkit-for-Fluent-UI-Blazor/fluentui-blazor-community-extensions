namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a builder for configuring and adding SVG &lt;animateMotion&gt; elements to an SVG document using a fluent
/// interface.
/// </summary>
public sealed class SvgAnimateMotionBuilder
{
    /// <summary>
    /// Represents the parent SVG builder to which this animateMotion element belongs, allowing for fluent chaining back to the parent context.
    /// </summary>
    private readonly SvgBuilder _parent;

    /// <summary>
    /// Represents the underlying SVG animate motion element associated with this instance.
    /// </summary>
    private readonly SvgAnimateMotion _animate;

    /// <summary>
    /// Initializes a new instance of the <see cref="SvgAnimateMotionBuilder"/> class with the specified parent builder and animate
    /// motion element.
    /// </summary>
    /// <param name="parent">The parent SvgBuilder that this builder is associated with. Cannot be null.</param>
    /// <param name="animate">The SvgAnimateMotion element to configure with this builder. Cannot be null.</param>
    public SvgAnimateMotionBuilder(SvgBuilder parent, SvgAnimateMotion animate)
    {
        _parent = parent;
        _animate = animate;
    }

    /// <summary>
    /// Specifies the motion path for the animation using an SVG path data string.
    /// </summary>
    /// <remarks>The path parameter should follow the SVG path data syntax as defined by the SVG
    /// specification. Invalid or malformed path data may result in unexpected animation behavior.</remarks>
    /// <param name="path">The SVG path data that defines the trajectory along which the animation will move. Must be a valid SVG path
    /// string.</param>
    /// <returns>The current instance of <see cref="SvgAnimateMotionBuilder"/> to allow method chaining.</returns>
    public SvgAnimateMotionBuilder Path(string path)
    {
        _animate.SetAttribute("path", path);

        return this;
    }

    /// <summary>
    /// Sets the duration of the SVG animation motion element.
    /// </summary>
    /// <remarks>The duration determines how long the animation runs before completing or repeating, depending
    /// on other animation attributes. Supplying an invalid duration string may result in incorrect SVG
    /// behavior.</remarks>
    /// <param name="duration">The duration value to assign to the animation, specified as a string. This can be a time value such as '2s' for
    /// two seconds or '500ms' for five hundred milliseconds. The format must be valid according to the SVG
    /// specification.</param>
    /// <returns>The current instance of <see cref="SvgAnimateMotionBuilder"/> to allow method chaining.</returns>
    public SvgAnimateMotionBuilder Duration(string duration)
    {
        _animate.SetAttribute("dur", duration);

        return this;
    }

    /// <summary>
    /// Sets the value of the 'begin' attribute for the SVG animateMotion element.
    /// </summary>
    /// <remarks>The 'begin' attribute determines the start time of the animation. Refer to the SVG
    /// specification for valid formats and usage scenarios.</remarks>
    /// <param name="begin">The value that specifies when the animation should begin. This can be a time value, an event, or a list of such
    /// values as defined by the SVG specification.</param>
    /// <returns>The current instance of <see cref="SvgAnimateMotionBuilder"/> to allow method chaining.</returns>
    public SvgAnimateMotionBuilder Begin(string begin)
    {
        _animate.SetAttribute("begin", begin);

        return this;
    }

    /// <summary>
    /// Sets the repeat count for the SVG animateMotion element.
    /// </summary>
    /// <remarks>The repeat count determines how many times the animation will play. Use "indefinite" to make
    /// the animation loop continuously.</remarks>
    /// <param name="count">The number of times the animation should repeat. Accepts a numeric value for a specific number of repetitions or
    /// the string "indefinite" to repeat the animation indefinitely.</param>
    /// <returns>The current instance of <see cref="SvgAnimateMotionBuilder"/> to allow method chaining.</returns>
    public SvgAnimateMotionBuilder RepeatCount(string count)
    {
        _animate.SetAttribute("repeatCount", count);

        return this;
    }

    /// <summary>
    /// Sets the fill color for the animated SVG element.
    /// </summary>
    /// <param name="fill">The fill color to apply, specified as a valid CSS color string. This value determines the color used to fill the
    /// SVG shape during animation.</param>
    /// <returns>The current instance of <see cref="SvgAnimateMotionBuilder"/> to allow method chaining.</returns>
    public SvgAnimateMotionBuilder Fill(string fill)
    {
        _animate.SetAttribute("fill", fill);
        return this;
    }

    /// <summary>
    /// Adds an &lt;mpath&gt; element to the animation, specifying a motion path reference for the animated object.
    /// </summary>
    /// <remarks>Use this method to associate a motion path with the animation by referencing an existing SVG
    /// path element. The referenced path must exist in the SVG document for the animation to function
    /// correctly.</remarks>
    /// <param name="href">The URI reference to the path element that defines the motion path. This should be a valid fragment identifier
    /// or URL pointing to an existing path element.</param>
    /// <returns>The current instance of <see cref="SvgAnimateMotionBuilder"/>, enabling method chaining.</returns>
    public SvgAnimateMotionBuilder AddMPath(string href)
    {
        var mpath = new SvgMPath();
        mpath.SetAttribute("href", href);

        _animate.Children.Add(mpath);

        return this;
    }

    /// <summary>
    /// Closes the current SVG element and returns control to the parent builder.
    /// </summary>
    /// <remarks>Use this method to complete the definition of a nested SVG element and continue building the
    /// parent SVG structure. This enables a fluent interface for constructing complex SVG documents.</remarks>
    /// <returns>The parent <see cref="SvgBuilder"/> instance, allowing for method chaining or further SVG construction.</returns>
    public SvgBuilder Close() => _parent;
}
