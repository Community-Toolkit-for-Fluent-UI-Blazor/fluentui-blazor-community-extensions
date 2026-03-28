using System.Globalization;

namespace FluentUI.Blazor.Community.Components.Extensions;

/// <summary>
/// Provides extension methods for formatting numeric values as SVG-compatible strings.
/// </summary>
/// <remarks>These methods ensure that numeric values are formatted using the invariant culture and a compact
/// representation suitable for SVG attributes. This avoids locale-specific formatting issues when generating SVG
/// markup.</remarks>
internal static class SvgFormatExtensions
{
    /// <summary>
    /// Provides a culture-independent, invariant culture instance for formatting and parsing operations.
    /// </summary>
    /// <remarks>The invariant culture is culture-insensitive and is associated with the English language but
    /// not with any country or region. Use this instance when consistent results are required regardless of the user's
    /// locale.</remarks>
    private static readonly CultureInfo s_culture = CultureInfo.InvariantCulture;

    /// <summary>
    /// Converts the specified double-precision floating-point value to its string representation formatted for use in
    /// SVG attributes.
    /// </summary>
    /// <param name="value">The double-precision floating-point value to convert to an SVG-compatible string.</param>
    /// <returns>A string representation of the value formatted with up to three decimal places, suitable for use in SVG markup.</returns>
    public static string ToSvg(this double value) => value.ToString("0.###", s_culture);
}
