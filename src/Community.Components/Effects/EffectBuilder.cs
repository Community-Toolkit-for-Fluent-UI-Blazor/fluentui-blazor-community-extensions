using System.Globalization;
using FluentUI.Blazor.Community.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace FluentUI.Blazor.Community;

/// <summary>
/// Represents a builder for constructing CSS filter effects.
/// </summary>
public sealed class EffectBuilder
{
    /// <summary>
    /// Represents the list of effects to be applied.
    /// </summary>
    private readonly List<string> _effects = [];

    /// <summary>
    /// Represents the culture info for formatting.
    /// </summary>
    private static readonly CultureInfo _invariantCulture = CultureInfo.InvariantCulture;

    /// <summary>
    /// Adds a blur effect to the current effect builder with the specified intensity and unit.
    /// </summary>
    /// <param name="value">The intensity of the blur effect. Must be a non-negative finite number.</param>
    /// <param name="unit">The unit of measurement for the blur intensity. Defaults to <see cref="LengthUnit.Pixels"/>.</param>
    /// <returns>The current <see cref="EffectBuilder"/> instance, allowing for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is negative, not a number, or infinite.</exception>
    public EffectBuilder AddBlur(double value, LengthUnit unit = LengthUnit.Pixels)
    {
        if (value < 0 || double.IsNaN(value) || double.IsInfinity(value))
        {
            throw new ArgumentException("Value must be a valid number.", nameof(value));
        }

        _effects.Add($"blur({new CssLength(value, unit)})");

        return this;
    }

    /// <summary>
    /// Adds a brightness adjustment effect to the current effect builder.
    /// </summary>
    /// <param name="value">The brightness adjustment value. Must be a non-negative finite number.</param>
    /// <returns>The current <see cref="EffectBuilder"/> instance, allowing for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is less than 0, is <see cref="double.NaN"/>, or is <see
    /// cref="double.PositiveInfinity"/> or <see cref="double.NegativeInfinity"/>.</exception>
    public EffectBuilder AddBrightness(double value)
    {
        if (value < 0 || double.IsNaN(value) || double.IsInfinity(value))
        {
            throw new ArgumentException("Value must be a valid number.", nameof(value));
        }

        _effects.Add($"brightness({value.ToString("0.##", _invariantCulture)})");

        return this;
    }

    /// <summary>
    /// Adds a contrast adjustment effect with the specified value to the current effect builder.
    /// </summary>
    /// <param name="value">The contrast adjustment value. Must be a finite number greater than or equal to 0.</param>
    /// <returns>The current <see cref="EffectBuilder"/> instance, allowing for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is less than 0, is <see cref="double.NaN"/>, or is infinite.</exception>
    public EffectBuilder AddContrast(double value)
    {
        if (value < 0 || double.IsNaN(value) || double.IsInfinity(value))
        {
            throw new ArgumentException("Value must be a valid number.", nameof(value));
        }

        _effects.Add($"contrast({value.ToString("0.##", _invariantCulture)})");

        return this;
    }

    /// <summary>
    /// Adds a grayscale effect to the current effect builder with the specified intensity.
    /// </summary>
    /// <param name="value">A value between 0 and 1 (inclusive) that specifies the intensity of the grayscale effect,  where 0 represents no
    /// effect and 1 represents full grayscale.</param>
    /// <returns>The current <see cref="EffectBuilder"/> instance, allowing for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is less than 0, greater than 1, or is not a finite number.</exception>
    public EffectBuilder AddGrayscale(double value)
    {
        if (value < 0 || value > 1 || double.IsNaN(value) || double.IsInfinity(value))
        {
            throw new ArgumentException("Value must be between 0 and 1.", nameof(value));
        }

        _effects.Add($"grayscale({value.ToString("0.##", _invariantCulture)})");

        return this;
    }

    /// <summary>
    /// Adds a hue rotation effect to the current effect builder.
    /// </summary>
    /// <param name="angle">The angle by which to rotate the hue. Must be a valid finite number.</param>
    /// <param name="unit">The unit of the angle, such as degrees or radians. The default is <see cref="AngleUnit.Degrees"/>.</param>
    /// <returns>The current <see cref="EffectBuilder"/> instance, allowing for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="angle"/> is not a valid finite number.</exception>
    public EffectBuilder AddHueRotate(double angle, AngleUnit unit = AngleUnit.Degrees)
    {
        if (double.IsNaN(angle) || double.IsInfinity(angle))
        {
            throw new ArgumentException("Angle must be a valid number.", nameof(angle));
        }

        _effects.Add($"hue-rotate({angle.ToString(_invariantCulture)}{unit.ToAttributeValue()})");

        return this;
    }

    /// <summary>
    /// Adds an invert effect with the specified intensity to the effect builder.
    /// </summary>
    /// <param name="value">The intensity of the invert effect, specified as a value between 0 and 1, inclusive. A value of 0 applies no
    /// inversion, while a value of 1 applies full inversion.</param>
    /// <returns>The current <see cref="EffectBuilder"/> instance, allowing for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is less than 0, greater than 1, or is not a finite number.</exception>
    public EffectBuilder AddInvert(double value)
    {
        if (value < 0 || value > 1 || double.IsNaN(value) || double.IsInfinity(value))
        {
            throw new ArgumentException("Value must be between 0 and 1.", nameof(value));
        }

        _effects.Add($"invert({value.ToString("0.##", _invariantCulture)})");

        return this;
    }

    /// <summary>
    /// Adds an opacity effect to the current effect builder with the specified value.
    /// </summary>
    /// <param name="value">A value between 0 and 1 (inclusive) representing the opacity level, where 0 is fully transparent and 1 is fully
    /// opaque.</param>
    /// <returns>The current <see cref="EffectBuilder"/> instance, allowing for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is less than 0, greater than 1, or is not a finite number.</exception>
    public EffectBuilder AddOpacity(double value)
    {
        if (value < 0 || value > 1 || double.IsNaN(value) || double.IsInfinity(value))
        {
            throw new ArgumentException("Value must be between 0 and 1.", nameof(value));
        }

        _effects.Add($"opacity({value.ToString("0.##", _invariantCulture)})");

        return this;
    }

    /// <summary>
    /// Adds a saturation effect with the specified intensity to the effect builder.
    /// </summary>
    /// <param name="value">The intensity of the saturation effect. Must be a non-negative finite number.</param>
    /// <returns>The current <see cref="EffectBuilder"/> instance, allowing for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is less than 0, is <see cref="double.NaN"/>, or is infinite.</exception>
    public EffectBuilder AddSaturate(double value)
    {
        if (value < 0 || double.IsNaN(value) || double.IsInfinity(value))
        {
            throw new ArgumentException("Value must be a valid number.", nameof(value));
        }

        _effects.Add($"saturate({value.ToString("0.##", _invariantCulture)})");

        return this;
    }

    /// <summary>
    /// Adds a sepia tone effect to the current effect chain with the specified intensity.
    /// </summary>
    /// <param name="value">The intensity of the sepia effect, ranging from 0 to 1. A value of 0 applies no sepia effect,  while a value of
    /// 1 applies the maximum sepia effect.</param>
    /// <returns>The current <see cref="EffectBuilder"/> instance, allowing for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is less than 0, greater than 1, or is not a finite number.</exception>
    public EffectBuilder AddSepia(double value)
    {
        if (value < 0 || value > 1 || double.IsNaN(value) || double.IsInfinity(value))
        {
            throw new ArgumentException("Value must be between 0 and 1.", nameof(value));
        }

        _effects.Add($"sepia({value.ToString("0.##", _invariantCulture)})");

        return this;
    }

    /// <summary>
    /// Adds a drop shadow effect to the current effect builder.
    /// </summary>
    /// <remarks>This method appends a CSS-compatible drop shadow effect to the internal list of effects.  The
    /// resulting shadow is defined by the specified offsets, blur radius, and color.</remarks>
    /// <param name="offsetX">The horizontal offset of the shadow. This value is specified as a <see cref="CssLength"/>.</param>
    /// <param name="offsetY">The vertical offset of the shadow. This value is specified as a <see cref="CssLength"/>.</param>
    /// <param name="blurRadius">The blur radius of the shadow. This value is specified as a <see cref="CssLength"/> and must be non-negative.</param>
    /// <param name="color">The color of the shadow, specified as an <see cref="RgbaColor"/>.</param>
    /// <returns>The current <see cref="EffectBuilder"/> instance, allowing for method chaining.</returns>
    public EffectBuilder AddDropShadow(CssLength offsetX, CssLength offsetY, CssLength blurRadius, RgbaColor color)
    {
        _effects.Add($"drop-shadow({offsetX} {offsetY} {blurRadius} {color})");
        return this;
    }

    /// <summary>
    /// Builds and returns a string representation of the effects, separated by spaces.
    /// </summary>
    /// <remarks>If no effects are present, the method returns <see langword="null"/>.</remarks>
    /// <returns>A string containing the effects separated by spaces, or <see langword="null"/> if no effects are present.</returns>
    public string? Build()
    {
        if (_effects.Count == 0)
        {
            return null;
        }

        return string.Join(" ", _effects);
    }
}
