namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an animated element with properties that can be interpolated over time.
/// </summary>
/// <remarks>The <see cref="AnimatedElement"/> class provides a set of properties, such as position offsets,
/// scaling factors,  rotation, color, opacity, and a generic value, that can be animated using associated <see
/// cref="AnimationState{T}"/>  objects. Each property can be updated independently based on its animation state and
/// interpolation logic.</remarks>
public class AnimatedElement
{
    /// <summary>
    /// Reusable interpolators for different property types.
    /// </summary>
    private static readonly DoubleInterpolator LerpDouble = new();

    /// <summary>
    /// Reusable color interpolator for string-based colors.
    /// </summary>
    private static readonly StringColorInterpolator LerpColor = new();

    /// <summary>
    /// Gets or sets the unique identifier for the animated element.
    /// </summary>
    public string Id { get; set; } = default!;

    /// <summary>
    /// Gets or sets the horizontal offset of the element.
    /// </summary>
    public double OffsetX { get; set; }

    /// <summary>
    /// Gets or sets the vertical offset of the element.
    /// </summary>
    public double OffsetY { get; set; }

    /// <summary>
    /// Gets or sets the horizontal scaling factor of the element. A value of 1.0 represents no scaling.
    /// </summary>
    public double ScaleX { get; set; } = 1.0;

    /// <summary>
    /// Gets or sets the vertical scaling factor of the element. A value of 1.0 represents no scaling.
    /// </summary>
    public double ScaleY { get; set; } = 1.0;

    /// <summary>
    /// Gets or sets the rotation angle of the element in degrees.
    /// </summary>
    public double Rotation { get; set; }

    /// <summary>
    /// Gets or sets the color of the element in a string format (e.g., hex code, RGB, etc.).
    /// </summary>
    public string? Color { get; set; }

    /// <summary>
    /// Gets or sets the background color of the element in a string format (e.g., hex code, RGB, etc.).
    /// </summary>
    public string? BackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the opacity of the element, where 0.0 is fully transparent and 1.0 is fully opaque.
    /// </summary>
    public double Opacity { get; set; } = 1.0;

    /// <summary>
    /// Gets or sets a generic value associated with the element, which can be used for various purposes.
    /// </summary>
    public double Value { get; set; }

    /// <summary>
    /// Gets or sets the animation state for the horizontal offset property.
    /// </summary>
    internal AnimationState<double>? OffsetXState { get; set; }

    /// <summary>
    /// Gets or sets the animation state for the vertical offset property.
    /// </summary>
    internal AnimationState<double>? OffsetYState { get; set; }

    /// <summary>
    /// Gets or sets the animation state for the horizontal scaling factor property.
    /// </summary>
    internal AnimationState<double>? ScaleXState { get; set; }

    /// <summary>
    /// Gets or sets the animation state for the vertical scaling factor property.
    /// </summary>
    internal AnimationState<double>? ScaleYState { get; set; }

    /// <summary>
    /// Gets or sets the animation state for the rotation property.
    /// </summary>
    internal AnimationState<double>? RotationState { get; set; }

    /// <summary>
    /// Gets or sets the animation state for the color property.
    /// </summary>
    internal AnimationState<string>? ColorState { get; set; }

    /// <summary>
    /// Gets or sets the animation state for the background color property.
    /// </summary>
    internal AnimationState<string>? BackgroundColorState { get; set; }

    /// <summary>
    /// Gets or sets the animation state for the opacity property.
    /// </summary>
    internal AnimationState<double>? OpacityState { get; set; }

    /// <summary>
    /// Gets or sets the animation state for the generic value property.
    /// </summary>
    internal AnimationState<double>? ValueState { get; set; }

    /// <summary>
    /// Updates the current state of the object by interpolating its properties based on the specified time.
    /// </summary>
    /// <remarks>This method updates the properties of the object, such as offsets, scale, rotation, color,
    /// opacity,  and value, by interpolating their respective states if they are not yet completed. Each property is 
    /// updated independently based on its associated state and interpolation function.</remarks>
    /// <param name="now">The current time used to evaluate the interpolation states.</param>
    public void Update(DateTime now)
    {
        if (OffsetXState != null)
        {
            OffsetX = OffsetXState.Interpolate(now, LerpDouble);
        }

        if (OffsetYState != null)
        {
            OffsetY = OffsetYState.Interpolate(now, LerpDouble);
        }

        if (ScaleXState != null)
        {
            ScaleX = ScaleXState.Interpolate(now, LerpDouble);
        }

        if (ScaleYState != null)
        {
            ScaleY = ScaleYState.Interpolate(now, LerpDouble);
        }

        if (RotationState != null)
        {
            Rotation = RotationState.Interpolate(now, LerpDouble);
        }

        if (ColorState != null)
        {
            Color = ColorState.Interpolate(now, LerpColor);
        }

        if (OpacityState != null)
        {
            Opacity = OpacityState.Interpolate(now, LerpDouble);
        }

        if (ValueState != null)
        {
            Value = ValueState.Interpolate(now, LerpDouble);
        }

        if (BackgroundColorState != null)
        {
            BackgroundColor = BackgroundColorState.Interpolate(now, LerpColor);
        }
    }

    /// <summary>
    /// Computes the differences between the current <see cref="AnimatedElement"/> instance and a specified previous
    /// instance.
    /// </summary>
    /// <remarks>The method compares the following properties: <c>OffsetX</c>, <c>OffsetY</c>, <c>ScaleX</c>,
    /// <c>ScaleY</c>, <c>Rotation</c>, <c>Color</c>, <c>Opacity</c>, and <c>Value</c>. Only properties with differing
    /// values are included in the returned dictionary.</remarks>
    /// <param name="previous">The previous <see cref="AnimatedElement"/> instance to compare against. Cannot be <c>null</c>.</param>
    /// <returns>A dictionary containing the properties that differ between the current instance and the <paramref
    /// name="previous"/> instance. The keys represent the property names, and the values represent the current values
    /// of those properties. If no differences are found, an empty dictionary is returned.</returns>
    public Dictionary<string, object?> GetDiff(AnimatedElement previous)
    {
        var diff = new Dictionary<string, object?>();

        if (OffsetX != previous.OffsetX)
        {
            diff["offsetX"] = OffsetX;
        }

        if (OffsetY != previous.OffsetY)
        {
            diff["offsetY"] = OffsetY;
        }

        if (ScaleX != previous.ScaleX)
        {
            diff["scaleX"] = ScaleX;
        }

        if (ScaleY != previous.ScaleY)
        {
            diff["scaleY"] = ScaleY;
        }

        if (Rotation != previous.Rotation)
        {
            diff["rotation"] = Rotation;
        }

        if (!string.IsNullOrEmpty(Color) &&
            !string.IsNullOrEmpty(previous.Color) &&
            !string.Equals(Color, previous.Color))
        {
            diff["color"] = Color;
        }

        if (!string.IsNullOrEmpty(BackgroundColor) &&
            !string.IsNullOrEmpty(previous.BackgroundColor) &&
            !string.Equals(BackgroundColor, previous.BackgroundColor))
        {
            diff["backgroundColor"] = BackgroundColor;
        }

        if (Opacity != previous.Opacity)
        {
            diff["opacity"] = Opacity;
        }

        if (Value != previous.Value)
        {
            diff["value"] = Value;
        }

        return diff;
    }

    /// <summary>
    /// Clones the current <see cref="AnimatedElement"/> instance, creating a new instance with the same property values.
    /// </summary>
    /// <returns>Returns the cloned element.</returns>
    public AnimatedElement Clone()
    {
        return new AnimatedElement()
        {
            Id = Id,
            OffsetX = OffsetX,
            OffsetY = OffsetY,
            Opacity = Opacity,
            Rotation = Rotation,
            ScaleX = ScaleX,
            ScaleY = ScaleY,
            BackgroundColor = BackgroundColor,
            Color = Color,
            Value = Value
        };
    }
}
