namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a fluent builder for configuring and adding an SVG &lt;animateTransform&gt; element to an SVG structure.
/// </summary>
public sealed class SvgAnimateTransformBuilder
{
    /// <summary>
    /// Represents the parent <see cref="SvgBuilder"/> instance associated with this builder, allowing for method chaining back to the parent builder.
    /// </summary>
    private readonly SvgBuilder _parent;

    /// <summary>
    /// Represents the SVG animateTransform element associated with this instance.
    /// </summary>
    private readonly SvgAnimateTransform _animate;

    /// <summary>
    /// Initializes a new instance of the SvgAnimateTransformBuilder class with the specified parent builder and animate
    /// transform element.
    /// </summary>
    /// <param name="parent">The parent SvgBuilder that this builder is associated with. Cannot be null.</param>
    /// <param name="animate">The SvgAnimateTransform element to configure. Cannot be null.</param>
    public SvgAnimateTransformBuilder(SvgBuilder parent, SvgAnimateTransform animate)
    {
        _parent = parent;
        _animate = animate;
    }

    /// <summary>
    /// Sets the starting value for the SVG transform animation.
    /// </summary>
    /// <param name="value">The initial value of the transform attribute to use at the beginning of the animation. This value determines the
    /// state from which the animation will start.</param>
    /// <returns>The current instance of <see cref="SvgAnimateTransformBuilder"/> to allow method chaining.</returns>
    public SvgAnimateTransformBuilder From(string value)
    {
        _animate.Attributes["from"] = value;

        return this;
    }

    /// <summary>
    /// Sets the target value for the SVG animation transformation.
    /// </summary>
    /// <param name="value">The value to which the animation will transition. This should be a valid SVG transform value.</param>
    /// <returns>The current instance of <see cref="SvgAnimateTransformBuilder"/> to allow method chaining.</returns>
    public SvgAnimateTransformBuilder To(string value)
    {
        _animate.Attributes["to"] = value;

        return this;
    }

    /// <summary>
    /// Sets the sequence of values for the SVG 'values' attribute in the animation transform element.
    /// </summary>
    /// <remarks>The 'values' attribute specifies the sequence of transformation values that the animation
    /// will use. Each value should be separated by a semicolon, following the SVG specification for animateTransform
    /// elements.</remarks>
    /// <param name="values">A string containing the semicolon-separated list of values to assign to the 'values' attribute. This string
    /// defines the sequence of transformation values for the animation.</param>
    /// <returns>The current instance of <see cref="SvgAnimateTransformBuilder"/> to allow method chaining.</returns>
    public SvgAnimateTransformBuilder Values(string values)
    {
        _animate.Attributes["values"] = values;

        return this;
    }

    /// <summary>
    /// Sets the duration of the SVG animation transform.
    /// </summary>
    /// <remarks>The duration determines how long the animation runs for each cycle. Ensure the value provided
    /// is compatible with SVG's 'dur' attribute requirements.</remarks>
    /// <param name="duration">The duration value to assign to the animation, specified as a string in a valid SVG time format (e.g., "2s" for
    /// two seconds or "500ms" for five hundred milliseconds).</param>
    /// <returns>The current instance of <see cref="SvgAnimateTransformBuilder"/> to allow method chaining.</returns>
    public SvgAnimateTransformBuilder Duration(string duration)
    {
        _animate.Attributes["dur"] = duration;

        return this;
    }

    /// <summary>
    /// Sets the value of the 'begin' attribute for the SVG animateTransform element.
    /// </summary>
    /// <remarks>The 'begin' attribute determines the start time of the animation. Refer to the SVG
    /// specification for valid formats and usage scenarios.</remarks>
    /// <param name="begin">The value to assign to the 'begin' attribute, specifying when the animation should start. This can be a time
    /// value, an event, or a list of such values as defined by the SVG specification.</param>
    /// <returns>The current instance of <see cref="SvgAnimateTransformBuilder"/> to allow method chaining.</returns>
    public SvgAnimateTransformBuilder Begin(string begin)
    {
        _animate.Attributes["begin"] = begin;

        return this;
    }

    /// <summary>
    /// Sets the value of the 'end' attribute for the SVG animate transform element.
    /// </summary>
    /// <param name="end">The time value or event that determines when the animation should end. This can be a clock value, an event base,
    /// or a semicolon-separated list of such values, as defined by the SVG specification.</param>
    /// <returns>The current instance of <see cref="SvgAnimateTransformBuilder"/> to allow method chaining.</returns>
    public SvgAnimateTransformBuilder End(string end)
    {
        _animate.Attributes["end"] = end;

        return this;
    }

    /// <summary>
    /// Sets the number of times the animation will repeat during its duration.
    /// </summary>
    /// <remarks>The value of <paramref name="count"/> should conform to the SVG specification for the
    /// 'repeatCount' attribute. Using 'indefinite' causes the animation to repeat continuously.</remarks>
    /// <param name="count">A string specifying the repeat count for the animation. This can be a numeric value or the keyword 'indefinite'
    /// to repeat the animation indefinitely.</param>
    /// <returns>The current instance of <see cref="SvgAnimateTransformBuilder"/> to allow method chaining.</returns>
    public SvgAnimateTransformBuilder RepeatCount(string count)
    {
        _animate.Attributes["repeatCount"] = count;

        return this;
    }

    /// <summary>
    /// Sets the fill color for the SVG animate transform element.
    /// </summary>
    /// <param name="fill">The fill color to apply. This can be any valid CSS color value, such as a color name, hexadecimal, RGB, or RGBA
    /// value.</param>
    /// <returns>The current instance of <see cref="SvgAnimateTransformBuilder"/> to allow method chaining.</returns>
    public SvgAnimateTransformBuilder Fill(string fill)
    {
        _animate.Attributes["fill"] = fill;

        return this;
    }

    /// <summary>
    /// Specifies the sequence of time values at which the animation's keyframes occur.
    /// </summary>
    /// <remarks>The number of values in <paramref name="keyTimes"/> must match the number of values specified
    /// in the corresponding 'values' or 'keySplines' attributes, if present. Invalid or mismatched input may result in
    /// incorrect animation behavior.</remarks>
    /// <param name="keyTimes">A semicolon-separated list of time values, expressed as percentages or absolute values, that define when each
    /// keyframe of the animation is reached. The format must conform to the SVG 'keyTimes' attribute specification.</param>
    /// <returns>The current instance of <see cref="SvgAnimateTransformBuilder"/> to allow method chaining.</returns>
    public SvgAnimateTransformBuilder KeyTimes(string keyTimes)
    {
        _animate.Attributes["keyTimes"] = keyTimes;

        return this;
    }

    /// <summary>
    /// Sets the key splines that define the pacing of the animation between keyframes.
    /// </summary>
    /// <remarks>The 'keySplines' parameter must match the number of animation intervals defined by the
    /// 'keyTimes' attribute minus one. Each spline is specified as four numbers separated by spaces or commas,
    /// representing the control points of a cubic Bézier curve.</remarks>
    /// <param name="keySplines">A string containing a list of control points for cubic Bézier curves, formatted as required by the SVG
    /// 'keySplines' attribute. Each set of four values defines one spline segment.</param>
    /// <returns>The current instance of <see cref="SvgAnimateTransformBuilder"/>, enabling method chaining.</returns>
    public SvgAnimateTransformBuilder KeySplines(string keySplines)
    {
        _animate.Attributes["keySplines"] = keySplines;

        return this;
    }

    /// <summary>
    /// Closes the current SVG element and returns control to the parent builder.
    /// </summary>
    /// <remarks>Use this method to complete the definition of the current SVG element and continue building
    /// the parent element. This enables fluent construction of nested SVG structures.</remarks>
    /// <returns>The parent <see cref="SvgBuilder"/> instance, allowing for method chaining or further SVG construction.</returns>
    public SvgBuilder Close() => _parent;
}
