using FluentUI.Blazor.Community.Components.Enums;
using FluentUI.Blazor.Community.Components.Extensions;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a builder for configuring and adding SVG &lt;animate&gt; elements using a fluent interface.
/// </summary>
public sealed class SvgAnimateBuilder
{
    /// <summary>
    /// Represents the parent SvgBuilder instance associated with this object.
    /// </summary>
    private readonly SvgBuilder _parent;

    /// <summary>
    /// Represents the SVG animate element associated with this instance.
    /// </summary>
    private readonly SvgAnimate _animate;

    /// <summary>
    /// Initializes a new instance of the <see cref="SvgAnimateBuilder"/> class with the specified parent builder and animate element.
    /// </summary>
    /// <param name="parent">The parent <see cref="SvgBuilder"/> that this builder is associated with. Cannot be null.</param>
    /// <param name="animate">The <see cref="SvgAnimate"/> element to be configured by this builder. Cannot be null.</param>
    public SvgAnimateBuilder(SvgBuilder parent, SvgAnimate animate)
    {
        _parent = parent;
        _animate = animate;
    }

    /// <summary>
    /// Sets the starting value of the animation for the associated SVG element.
    /// </summary>
    /// <param name="value">The initial value from which the animation will begin. This value is assigned to the 'from' attribute of the SVG
    /// animate element.</param>
    /// <returns>The current instance of <see cref="SvgAnimateBuilder"/>, enabling method chaining.</returns>
    public SvgAnimateBuilder From(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return this;
        }

        _animate.Attributes["from"] = value;

        return this;
    }

    /// <summary>
    /// Sets the target value for the SVG animation's 'to' attribute.
    /// </summary>
    /// <param name="value">The value to assign to the 'to' attribute of the SVG animate element. Represents the final value of the
    /// animation.</param>
    /// <returns>The current instance of <see cref="SvgAnimateBuilder"/> to allow method chaining.</returns>
    public SvgAnimateBuilder To(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return this;
        }

        _animate.Attributes["to"] = value;

        return this;
    }

    /// <summary>
    /// Sets the starting value of the animation for the associated SVG element.
    /// </summary>
    /// <param name="value">The initial value from which the animation will begin. This value is assigned to the 'from' attribute of the SVG
    /// animate element.</param>
    /// <returns>The current instance of <see cref="SvgAnimateBuilder"/>, enabling method chaining.</returns>
    public SvgAnimateBuilder From(double value)
    {
        _animate.Attributes["from"] = value.ToSvg();

        return this;
    }

    /// <summary>
    /// Sets the target value for the SVG animation's 'to' attribute.
    /// </summary>
    /// <param name="value">The value to assign to the 'to' attribute of the SVG animate element. Represents the final value of the
    /// animation.</param>
    /// <returns>The current instance of <see cref="SvgAnimateBuilder"/> to allow method chaining.</returns>
    public SvgAnimateBuilder To(double value)
    {
        _animate.Attributes["to"] = value.ToSvg();

        return this;
    }

    /// <summary>
    /// Sets the sequence of values for the SVG animation attribute.
    /// </summary>
    /// <remarks>Use this method to define the key values that the SVG animation will interpolate between. The
    /// values should be compatible with the target SVG attribute and follow the SVG specification for the 'values'
    /// attribute.</remarks>
    /// <param name="values">A semicolon-separated list of values that the animation will use during its progression. The format and meaning
    /// of each value depend on the animated SVG attribute.</param>
    /// <returns>The current instance of <see cref="SvgAnimateBuilder"/> to allow method chaining.</returns>
    public SvgAnimateBuilder Values(string values)
    {
        _animate.Attributes["values"] = values;

        return this;
    }

    /// <summary>
    /// Sets the sequence of values for the SVG animation attribute.
    /// </summary>
    /// <remarks>Use this method to define the key values that the SVG animation will interpolate between. The
    /// values should be compatible with the target SVG attribute and follow the SVG specification for the 'values'
    /// attribute.</remarks>
    /// <param name="values">A variable number of string arguments representing the values that the animation will use during its progression.</param>
    /// <returns>The current instance of <see cref="SvgAnimateBuilder"/> to allow method chaining.</returns>
    public SvgAnimateBuilder Values(params string[] values)
    {
        return Values(string.Join(";", values));
    }

    /// <summary>
    /// Sets the duration of the SVG animation.
    /// </summary>
    /// <remarks>The duration determines how long the animation runs before completing or repeating. Ensure
    /// the value provided is valid according to the SVG specification.</remarks>
    /// <param name="duration">The duration value to assign to the animation, specified as a string. The value should follow the SVG 'dur'
    /// attribute format, such as '2s' for two seconds or '500ms' for five hundred milliseconds.</param>
    /// <returns>The current instance of <see cref="SvgAnimateBuilder"/>, enabling method chaining.</returns>
    public SvgAnimateBuilder Duration(string duration)
    {
        _animate.Attributes["dur"] = duration;

        return this;
    }

    /// <summary>
    /// Sets the duration of the SVG animation.
    /// </summary>
    /// <remarks>The duration determines how long the animation runs before completing or repeating. Ensure
    /// the value provided is valid according to the SVG specification.</remarks>
    /// <param name="duration">The duration value to assign to the animation, specified as a string. The value should follow the SVG 'dur'
    /// attribute format, such as '2s' for two seconds or '500ms' for five hundred milliseconds.</param>
    /// <returns>The current instance of <see cref="SvgAnimateBuilder"/>, enabling method chaining.</returns>
    public SvgAnimateBuilder Duration(TimeSpan duration)
    {
        _animate.Attributes["dur"] = $"{duration.TotalMilliseconds}ms";

        return this;
    }

    /// <summary>
    /// Sets the time at which the animation should begin.
    /// </summary>
    /// <remarks>The 'begin' parameter accepts values such as clock values (e.g., '2s'), event-based triggers
    /// (e.g., 'click'), or semicolon-separated lists for multiple start times, following the SVG specification for the
    /// 'begin' attribute.</remarks>
    /// <param name="begin">A string specifying when the animation will start. This can be a time value, an event, or a combination as
    /// defined by the SVG 'begin' attribute.</param>
    /// <returns>The current instance of <see cref="SvgAnimateBuilder"/> to allow method chaining.</returns>
    public SvgAnimateBuilder Begin(string begin)
    {
        _animate.Attributes["begin"] = begin;

        return this;
    }

    /// <summary>
    /// Sets the time at which the animation should begin.
    /// </summary>
    /// <remarks>The 'begin' parameter accepts values such as clock values (e.g., '2s'), event-based triggers
    /// (e.g., 'click'), or semicolon-separated lists for multiple start times, following the SVG specification for the
    /// 'begin' attribute.</remarks>
    /// <param name="begin">A string specifying when the animation will start. This can be a time value, an event, or a combination as
    /// defined by the SVG 'begin' attribute.</param>
    /// <returns>The current instance of <see cref="SvgAnimateBuilder"/> to allow method chaining.</returns>
    public SvgAnimateBuilder Begin(TimeSpan begin)
    {
        _animate.Attributes["begin"] = $"{begin.TotalMilliseconds}ms";

        return this;
    }

    /// <summary>
    /// Sets the value of the 'end' attribute for the SVG animate element.
    /// </summary>
    /// <remarks>The 'end' attribute determines when the animation will stop. Refer to the SVG specification
    /// for valid timing expressions.</remarks>
    /// <param name="end">The value to assign to the 'end' attribute. Specifies when the animation should end, using a valid SVG timing
    /// expression.</param>
    /// <returns>The current instance of <see cref="SvgAnimateBuilder"/> to allow method chaining.</returns>
    public SvgAnimateBuilder End(string end)
    {
        _animate.Attributes["end"] = end;

        return this;
    }

    /// <summary>
    /// Sets the value of the 'end' attribute for the SVG animate element.
    /// </summary>
    /// <remarks>The 'end' attribute determines when the animation will stop. Refer to the SVG specification
    /// for valid timing expressions.</remarks>
    /// <param name="end">The value to assign to the 'end' attribute. Specifies when the animation should end, using a valid SVG timing
    /// expression.</param>
    /// <returns>The current instance of <see cref="SvgAnimateBuilder"/> to allow method chaining.</returns>
    public SvgAnimateBuilder End(TimeSpan end)
    {
        _animate.Attributes["end"] = $"{end.TotalMilliseconds}ms";

        return this;
    }

    /// <summary>
    /// Sets the number of times the animation will repeat during its duration.
    /// </summary>
    /// <remarks>If the value is set to "indefinite", the animation will repeat continuously. Otherwise, the
    /// value should be a positive number representing the number of iterations.</remarks>
    /// <param name="count">The repeat count value to assign. This can be a specific number of iterations or the string "indefinite" to
    /// repeat the animation indefinitely. Cannot be null.</param>
    /// <returns>The current instance of <see cref="SvgAnimateBuilder"/> to allow method chaining.</returns>
    public SvgAnimateBuilder RepeatCount(string count)
    {
        _animate.Attributes["repeatCount"] = count;

        return this;
    }

    /// <summary>
    /// Sets the fill state for the SVG animation element.
    /// </summary>
    /// <param name="fill">The fill state to apply to the SVG element.</param>
    /// <returns>The current instance of <see cref="SvgAnimateBuilder"/> to allow method chaining.</returns>
    public SvgAnimateBuilder Fill(string fill)
    {
        _animate.Attributes["fill"] = fill;

        return this;
    }

    /// <summary>
    /// Sets the fill state for the SVG animation element.
    /// </summary>
    /// <param name="fill">The fill state to apply to the SVG element.</param>
    /// <returns>The current instance of <see cref="SvgAnimateBuilder"/> to allow method chaining.</returns>
    public SvgAnimateBuilder Fill(SvgAnimationFillMode fill = SvgAnimationFillMode.Freeze)
    {
        return Fill(fill.ToString().ToLowerInvariant());
    }

    /// <summary>
    /// Sets a custom attribute for the SVG animate element.
    /// </summary>
    /// <param name="attribute">The name of the attribute to set on the SVG animate element.</param>
    /// <param name="value">The value to assign to the specified attribute.</param>
    /// <returns>Returns the current instance of <see cref="SvgAnimateBuilder"/> to allow method chaining.</returns>
    public SvgAnimateBuilder WithAttribute(string attribute, string? value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            _animate.Attributes[attribute] = value;
        }

        return this;
    }

    /// <summary>
    /// Specifies the sequence of time values at which the animation's keyframes occur.
    /// </summary>
    /// <remarks>The number of values in <paramref name="keyTimes"/> must match the number of values specified
    /// in the 'values' attribute of the animation. Invalid or mismatched input may result in incorrect animation
    /// behavior.</remarks>
    /// <param name="keyTimes">A semicolon-separated list of time values, expressed as percentages or absolute values, that define when each
    /// keyframe of the animation is reached. The format must match the requirements of the SVG 'keyTimes' attribute.</param>
    /// <returns>The current instance of <see cref="SvgAnimateBuilder"/> to allow method chaining.</returns>
    public SvgAnimateBuilder KeyTimes(string keyTimes)
    {
        _animate.Attributes["keyTimes"] = keyTimes;

        return this;
    }

    /// <summary>
    /// Specifies the sequence of time values at which the animation's keyframes occur.
    /// </summary>
    /// <remarks>The number of values in <paramref name="keyTimes"/> must match the number of values specified
    /// in the 'values' attribute of the animation. Invalid or mismatched input may result in incorrect animation
    /// behavior.</remarks>
    /// <param name="keyTimes">A double array representing the key times for the animation, where each value corresponds to a keyframe.</param>
    /// <returns>The current instance of <see cref="SvgAnimateBuilder"/> to allow method chaining.</returns>
    public SvgAnimateBuilder KeyTimes(params double[] keyTimes)
    {
        return KeyTimes(string.Join(";", keyTimes.Select(kt => kt.ToSvg())));
    }

    /// <summary>
    /// Sets the key splines attribute for the animation, defining the pacing of the animation between keyframes using
    /// cubic Bézier curves.
    /// </summary>
    /// <remarks>The keySplines value must match the number of intervals defined by the keyTimes attribute, if
    /// used. Each spline segment controls the acceleration and deceleration between two keyframes.</remarks>
    /// <param name="keySplines">A string that specifies the key splines for the animation, formatted as a list of control points for cubic
    /// Bézier curves. Each set of four numbers defines one spline segment.</param>
    /// <returns>The current instance of the <see cref="SvgAnimateBuilder"/>, enabling method chaining.</returns>
    public SvgAnimateBuilder KeySplines(string keySplines)
    {
        _animate.Attributes["keySplines"] = keySplines;

        return this;
    }

    /// <summary>
    /// Sets a JavaScript method to be called when the animation ends.
    /// </summary>
    /// <param name="jsMethod">Method name of the JavaScript function to be called when the animation ends.</param>
    /// <param name="chartId">The unique identifier of the chart, used for scoping the JavaScript function call.</param>
    /// <param name="groupId">The unique identifier of the group within the chart, used for scoping the JavaScript function call.</param>
    /// <param name="itemId">The unique identifier of the item within the group, used for scoping the JavaScript function call.</param>
    /// <returns>The current instance of <see cref="SvgAnimateBuilder"/> to allow method chaining.</returns>
    public SvgAnimateBuilder OnEnd(string jsMethod, string chartId, string groupId, string itemId)
    {
        _animate.Attributes["onend"] = $"{jsMethod}('{chartId}', '{groupId}', '{itemId}')";

        return this;
    }

    /// <summary>
    /// Closes the current SVG element and returns control to the parent builder.
    /// </summary>
    /// <remarks>Use this method to complete the definition of a nested SVG element and continue building the
    /// parent SVG structure. This enables fluent chaining when constructing complex SVG documents.</remarks>
    /// <returns>The parent <see cref="SvgBuilder"/> instance, allowing for method chaining or further SVG construction.</returns>
    public SvgBuilder Close() => _parent;
}

