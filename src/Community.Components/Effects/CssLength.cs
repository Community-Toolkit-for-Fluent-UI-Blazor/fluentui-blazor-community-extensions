using System.Globalization;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a CSS length value with a unit.
/// </summary>
public readonly struct CssLength
{
    /// <summary>
    /// Represents the numeric value of the length.
    /// </summary>
    private readonly double _value;

    /// <summary>
    /// Represents the unit of the length.
    /// </summary>
    private readonly LengthUnit _unit;

    /// <summary>
    /// Represents the culture info for formatting.
    /// </summary>
    private static readonly CultureInfo _culture = CultureInfo.InvariantCulture;

    /// <summary>
    /// Initializes a new instance of the <see cref="CssLength"/> struct.
    /// </summary>
    /// <param name="value">Value of the length.</param>
    /// <param name="unit">Unit of the length.</param>
    /// <exception cref="ArgumentException"></exception>
    public CssLength(double value, LengthUnit unit = LengthUnit.Pixels)
    {
        if (double.IsNaN(value) || double.IsInfinity(value) || value < 0)
        {
            throw new ArgumentException("Value must be a valid number.", nameof(value));
        }

        _value = value;
        _unit = unit;
    }

    /// <inheritdoc />
    public override string? ToString() => $"{_value.ToString("0.##", _culture)}{_unit.ToAttributeValue()}";
}
