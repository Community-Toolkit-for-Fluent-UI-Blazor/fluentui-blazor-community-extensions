using System.Drawing;

namespace FluentUI.Blazor.Community.Components.Infrastructure;

/// <summary>
/// Provides static methods for determining whether a color is considered dark based on its luminance and contrast
/// ratio.
/// </summary>
/// <remarks>This class is intended for internal use in scenarios where color brightness needs to be evaluated,
/// such as selecting appropriate foreground or background colors for readability. All methods are static and do not
/// require instantiation.</remarks>
internal static class ColorLuminance
{
    /// <summary>
    /// Calculates the relative luminance of the specified color based on its RGB components.
    /// </summary>
    /// <remarks>Relative luminance is commonly used in color contrast calculations and accessibility checks.
    /// The calculation follows the standard formula defined by the W3C for sRGB colors.</remarks>
    /// <param name="c">The color for which to compute the relative luminance.</param>
    /// <returns>A double value representing the relative luminance of the color, ranging from 0.0 (black) to 1.0 (white).</returns>
    private static double Luminance(Color c)
    {
        var r = c.R / 255.0;
        var g = c.G / 255.0;
        var b = c.B / 255.0;

        return 0.2126 * ToLinear(r) +
               0.7152 * ToLinear(g) +
               0.0722 * ToLinear(b);
    }

    /// <summary>
    /// Converts a gamma-encoded sRGB component value to its linear representation.
    /// </summary>
    /// <remarks>This conversion is commonly used in color calculations where linear color space is required,
    /// such as luminance or blending operations. The input value should represent a normalized sRGB channel (red,
    /// green, or blue) in the range 0.0 to 1.0.</remarks>
    /// <param name="val">The gamma-encoded sRGB component value to convert. Must be in the range 0.0 to 1.0.</param>
    /// <returns>The linear sRGB component value corresponding to the specified gamma-encoded input.</returns>
    private static double ToLinear(double val)
    {
        return val <= 0.03928 ? val / 12.92 : Math.Pow((val + 0.055) / 1.055, 2.4);
    }

    /// <summary>
    /// Calculates the contrast ratio between two luminance values according to the WCAG formula.
    /// </summary>
    /// <remarks>The contrast ratio is commonly used to determine the accessibility of text and background
    /// color combinations. Higher ratios indicate greater contrast and improved readability.</remarks>
    /// <param name="val1">The first luminance value to compare. Must be a non-negative number.</param>
    /// <param name="val2">The second luminance value to compare. Must be a non-negative number.</param>
    /// <returns>A double representing the contrast ratio between the two luminance values. The value will be greater than or
    /// equal to 1.</returns>
    private static double ContrastRatio(double val1, double val2)
    {
        var max = Math.Max(val1, val2);
        var min = Math.Min(val1, val2);

        return (max + 0.05) / (min + 0.05);
    }

    /// <summary>
    /// Determines whether the specified color is considered dark based on its luminance and contrast against black and
    /// white backgrounds.
    /// </summary>
    /// <remarks>This method uses luminance and contrast ratio calculations to assess whether a color is
    /// visually dark. This can be useful for selecting appropriate foreground or background colors to ensure sufficient
    /// contrast and readability.</remarks>
    /// <param name="color">The color to evaluate for darkness. The perceived brightness of this color is used to determine if it is
    /// classified as dark.</param>
    /// <returns>true if the color is considered dark; otherwise, false.</returns>
    public static bool IsDarkColor(Color color)
    {
        var lum = Luminance(color);
        var contrastWhite = ContrastRatio(lum, 1.0);
        var constrastBlack = ContrastRatio(lum, 0);

        return contrastWhite > constrastBlack;
    }

    /// <summary>
    /// Determines whether the specified color, represented as a hexadecimal string, is considered dark.
    /// </summary>
    /// <param name="hexColor">A string containing the color in hexadecimal format (e.g., "#RRGGBB" or "#RGB"). The string must be a valid HTML
    /// color code.</param>
    /// <returns>true if the color is classified as dark; otherwise, false.</returns>
    public static bool IsDarkColor(string hexColor)
    {
        var color = ColorTranslator.FromHtml(hexColor);

        return IsDarkColor(color);
    }
}
