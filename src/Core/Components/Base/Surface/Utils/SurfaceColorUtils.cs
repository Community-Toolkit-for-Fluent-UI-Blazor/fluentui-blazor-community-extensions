namespace FluentUI.Blazor.Community.Components.Surface.Utils;

/// <summary>
/// Represents a utility class for manipulating surface colors.
/// </summary>
public static class SurfaceColorUtils
{
    /// <summary>
    /// Returns a darker shade of the specified hexadecimal color by applying the given darkening factor.
    /// </summary>
    /// <remarks>The method does not validate that the input string is a valid hexadecimal color beyond
    /// checking its format. Values of <paramref name="factor"/> outside the range 0.0 to 1.0 may produce unexpected
    /// results.</remarks>
    /// <param name="hexColor">A hexadecimal color string in the format "#RRGGBB" to be darkened.</param>
    /// <param name="factor">A value between 0.0 and 1.0 representing the proportion by which to darken the color. Higher values produce a
    /// darker result.</param>
    /// <returns>A hexadecimal color string representing the darkened color. If the input is not in the correct format, the
    /// original string is returned.</returns>
    public static string Darken(string hexColor, double factor)
    {
        if (string.IsNullOrWhiteSpace(hexColor))
        {
            return hexColor;
        }

        if (hexColor.StartsWith('#'))
        {
            hexColor = hexColor[1..];
        }

        if (hexColor.Length != 6)
        {
            return $"#{hexColor}";
        }

        var r = Convert.ToInt32(hexColor[..2], 16);
        var g = Convert.ToInt32(hexColor[2..4], 16);
        var b = Convert.ToInt32(hexColor[4..6], 16);

        r = (int)(r * (1 - factor));
        g = (int)(g * (1 - factor));
        b = (int)(b * (1 - factor));

        return $"#{r:X2}{g:X2}{b:X2}";
    }

    /// <summary>
    /// Lightens the specified hexadecimal color by blending it with white by the given percentage.
    /// </summary>
    /// <remarks>If the input color is not a valid 6-digit hexadecimal value, the method returns the input
    /// with a leading '#' if not already present. The method clamps the resulting color values to the valid range for
    /// RGB components.</remarks>
    /// <param name="hexColor">A hexadecimal color string in the format "#RRGGBB" or "RRGGBB" representing the color to lighten. Cannot be
    /// null, empty, or whitespace.</param>
    /// <param name="percent">A value between 0.0 and 1.0 indicating the proportion of white to blend with the original color. A value of 0
    /// returns the original color; a value of 1 returns white.</param>
    /// <returns>A hexadecimal color string representing the lightened color in the format "#RRGGBB". If the input is invalid,
    /// returns the original or minimally processed input.</returns>
    public static string Lighten(string hexColor, double percent)
    {
        if (string.IsNullOrWhiteSpace(hexColor))
        {
            return hexColor;
        }

        if (hexColor.StartsWith('#'))
        {
            hexColor = hexColor[1..];
        }

        if (hexColor.Length != 6)
        {
            return $"#{hexColor}";
        }

        var r = Convert.ToInt32(hexColor[..2], 16);
        var g = Convert.ToInt32(hexColor[2..4], 16);
        var b = Convert.ToInt32(hexColor[4..6], 16);
        var factor = percent;

        r = (int)(r + (255 - r) * factor);
        g = (int)(g + (255 - g) * factor);
        b = (int)(b + (255 - b) * factor);

        r = Math.Clamp(r, 0, 255);
        g = Math.Clamp(g, 0, 255);
        b = Math.Clamp(b, 0, 255);

        return $"#{r:X2}{g:X2}{b:X2}";
    }
}
