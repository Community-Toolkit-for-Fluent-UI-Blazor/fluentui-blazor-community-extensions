using System.Globalization;
using FluentUI.Blazor.Community.Components;

namespace FluentUI.Blazor.Community.Helpers;

/// <summary>
/// Provides utility methods for working with colors, including conversions between color formats, validation of color
/// values, and generation of color gradients and palettes.
/// </summary>
/// <remarks>This class includes methods for validating and normalizing hexadecimal color codes, converting
/// between RGB, HSL, and hexadecimal formats, and generating color gradients and palettes based on various strategies.
/// It is designed to simplify common color manipulation tasks in applications that work with color data. <para> The
/// methods in this class are static and thread-safe, making it suitable for use in multi-threaded environments.
/// </para></remarks>
public static class ColorUtils
{
    /// <summary>
    /// Determines whether the specified string represents a valid hexadecimal color code.
    /// </summary>
    /// <remarks>A valid hexadecimal color code consists of either 3 or 6 hexadecimal digits, optionally
    /// prefixed with a '#' character. The method ignores leading and trailing whitespace.</remarks>
    /// <param name="hex">The string to validate. This can optionally include a leading '#' character.</param>
    /// <returns><see langword="true"/> if the string is a valid hexadecimal color code with 3 or 6 digits; otherwise, <see
    /// langword="false"/>.</returns>
    public static bool IsValidHex(string? hex)
    {
        if (string.IsNullOrWhiteSpace(hex))
        {
            return false;
        }

        hex = hex.Trim();

        if (hex.StartsWith('#'))
        {
            hex = hex[1..];
        }

        return (hex.Length is 3 or 6) && int.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out _);
    }

    /// <summary>
    /// Determines whether the specified string represents a valid hexadecimal color code  or a recognized CSS color
    /// name.
    /// </summary>
    /// <remarks>A valid hexadecimal color code must start with a '#' character followed by 3 or 6 
    /// hexadecimal digits. Recognized CSS color names are determined by the  <c>CssColorNamesUtils</c>
    /// utility.</remarks>
    /// <param name="value">The string to validate. This can be a hexadecimal color code  (e.g., "#FFFFFF") or a CSS color name (e.g.,
    /// "red").</param>
    /// <returns><see langword="true"/> if the string is a valid hexadecimal color code or a recognized  CSS color name;
    /// otherwise, <see langword="false"/>.</returns>
    public static bool IsValidHexOrCssName(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        if (IsValidHex(value))
        {
            return true;
        }

        return CssColorNamesUtils.TryGetHex(value, out _);
    }

    /// <summary>
    /// Normalizes a color value to its hexadecimal representation.
    /// </summary>
    /// <remarks>This method trims whitespace from the input and attempts to interpret it as a valid
    /// hexadecimal color code or a CSS color name. If the input is already a valid hexadecimal color code, it is
    /// normalized to the standard format. If the input is a recognized CSS color name, it is converted to its
    /// corresponding hexadecimal value.</remarks>
    /// <param name="value">The input color value, which can be a hexadecimal color code, a CSS color name, or a string to be normalized.</param>
    /// <returns>A normalized hexadecimal color code in the format "#RRGGBB". If the input is null, empty, or invalid, the method
    /// returns "#000000".</returns>
    public static string NormalizeToHex(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "#000000";
        }

        value = value.Trim();

        if (CssColorNamesUtils.TryGetHex(value, out var hexFromName))
        {
            return NormalizeHex(hexFromName);
        }

        if (IsValidHex(value))
        {
            return NormalizeHex(value);
        }

        return "#000000";
    }

    /// <summary>
    /// Normalizes a hexadecimal color code to a standard format.
    /// </summary>
    /// <remarks>If the input is in short form (e.g., "#123"), it will be expanded to long form (e.g.,
    /// "#112233"). The method ensures the returned value always starts with a "#" and is in uppercase.</remarks>
    /// <param name="hex">The hexadecimal color code to normalize. This can be in short form (e.g., "#123"), long form (e.g., "#112233"),
    /// or null/whitespace.</param>
    /// <returns>A normalized hexadecimal color code in uppercase long form (e.g., "#112233").  Returns "#000000" if <paramref
    /// name="hex"/> is null, empty, or consists only of whitespace.</returns>
    public static string NormalizeHex(string? hex)
    {
        if (string.IsNullOrWhiteSpace(hex))
        {
            return "#000000";
        }

        hex = hex.Trim();

        if (!hex.StartsWith('#'))
        {
            hex = "#" + hex;
        }

        if (hex.Length == 4)
        {
            var r = hex[1];
            var g = hex[2];
            var b = hex[3];

            return $"#{r}{r}{g}{g}{b}{b}".ToUpperInvariant();
        }

        return hex.ToUpperInvariant();
    }

    /// <summary>
    /// Converts a hexadecimal color code to its RGB components.
    /// </summary>
    /// <remarks>The method normalizes the input hexadecimal string by removing the '#' character before
    /// parsing.</remarks>
    /// <param name="hex">A string representing the hexadecimal color code. The string must start with a '#' character followed by six
    /// hexadecimal digits (e.g., "#FF5733").</param>
    /// <returns>A tuple containing the red, green, and blue components of the color, each as an integer in the range 0 to 255.</returns>
    public static (int R, int G, int B) HexToRgb(string hex)
    {
        hex = NormalizeHex(hex)[1..];
        var r = int.Parse(hex.AsSpan(0, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        var g = int.Parse(hex.AsSpan(2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        var b = int.Parse(hex.AsSpan(4, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);

        return (r, g, b);
    }

    /// <summary>
    /// Converts a hexadecimal color code to its RGB string representation.
    /// </summary>
    /// <param name="hex">The hexadecimal color code, starting with a '#' character, followed by 6 or 3 hexadecimal digits.</param>
    /// <returns>A string representing the RGB values in the format "R,G,B", where R, G, and B are integers between 0 and 255.</returns>
    public static string HexToRgbString(string hex)
    {
        if (CssColorNamesUtils.TryGetHex(hex, out var hexFromName))
        {
            hex = hexFromName!;
        }

        var (r, g, b) = HexToRgb(hex);

        return $"{r},{g},{b}";
    }

    /// <summary>
    /// Converts RGB color values to a hexadecimal color code.
    /// </summary>
    /// <param name="r">The red component of the color, ranging from 0 to 255.</param>
    /// <param name="g">The green component of the color, ranging from 0 to 255.</param>
    /// <param name="b">The blue component of the color, ranging from 0 to 255.</param>
    /// <returns>A string representing the hexadecimal color code, prefixed with '#'.</returns>
    public static string RgbToHex(int r, int g, int b)
    {
        return $"#{r:X2}{g:X2}{b:X2}";
    }

    /// <summary>
    /// Converts a hexadecimal color code to its equivalent HSL (Hue, Saturation, Lightness) representation.
    /// </summary>
    /// <remarks>The input hexadecimal color code must be in the format "#RRGGBB" or "#RRGGBBAA". If the alpha
    /// channel is included, it is ignored in the conversion.</remarks>
    /// <param name="hex">The hexadecimal color code, starting with a '#' character, followed by 6 or 8 hexadecimal digits.</param>
    /// <returns>A tuple containing the HSL representation of the color: <list type="bullet"> <item><description><c>H</c>: The
    /// hue, in degrees, ranging from 0 to 360.</description></item> <item><description><c>S</c>: The saturation, as a
    /// percentage, ranging from 0 to 1.</description></item> <item><description><c>L</c>: The lightness, as a
    /// percentage, ranging from 0 to 1.</description></item> </list></returns>
    public static (double H, double S, double L) HexToHsl(string hex)
    {
        var (r, g, b) = HexToRgb(hex);

        return RgbToHsl(r, g, b);
    }

    /// <summary>
    /// Converts a color from HSL (Hue, Saturation, Lightness) format to its hexadecimal color representation.
    /// </summary>
    /// <remarks>The method internally converts the HSL values to RGB and then formats the RGB values as a
    /// hexadecimal string.</remarks>
    /// <param name="h">The hue of the color, specified as a value between 0 and 360 degrees.</param>
    /// <param name="s">The saturation of the color, specified as a value between 0.0 and 1.0.</param>
    /// <param name="l">The lightness of the color, specified as a value between 0.0 and 1.0.</param>
    /// <returns>A string representing the color in hexadecimal format, prefixed with a '#' character.</returns>
    public static string HslToHex(double h, double s, double l)
    {
        var (r, g, b) = HslToRgb(h, s, l);

        return RgbToHex(r, g, b);
    }

    /// <summary>
    /// Converts an RGB color value to its equivalent HSL (Hue, Saturation, Lightness) representation.
    /// </summary>
    /// <remarks>The method normalizes the hue to ensure it is always within the range 0 to 360 degrees.
    /// Saturation and lightness values are clamped to the range 0 to 1.</remarks>
    /// <param name="r">The red component of the color, in the range 0 to 255.</param>
    /// <param name="g">The green component of the color, in the range 0 to 255.</param>
    /// <param name="b">The blue component of the color, in the range 0 to 255.</param>
    /// <returns>A tuple containing the HSL representation of the color: <list type="bullet"> <item><description><c>H</c> (Hue):
    /// The hue of the color, in degrees, in the range 0 to 360.</description></item> <item><description><c>S</c>
    /// (Saturation): The saturation of the color, in the range 0 to 1.</description></item> <item><description><c>L</c>
    /// (Lightness): The lightness of the color, in the range 0 to 1.</description></item> </list></returns>
    public static (double H, double S, double L) RgbToHsl(int r, int g, int b)
    {
        var rd = r / 255.0;
        var gd = g / 255.0;
        var bd = b / 255.0;
        var max = Math.Max(rd, Math.Max(gd, bd));
        var min = Math.Min(rd, Math.Min(gd, bd));
        var h = 0d;
        var s = 0d;
        var l = (max + min) / 2.0;

        if (Math.Abs(max - min) < 1e-9)
        {
            h = 0d;
        }
        else
        {
            var d = max - min;
            s = l > 0.5 ? d / (2.0 - max - min) : d / (max + min);

            if (max == rd)
            {
                h = (gd - bd) / d + (gd < bd ? 6 : 0);
            }
            else if (max == gd)
            {
                h = (bd - rd) / d + 2;
            }
            else
            {
                h = (rd - gd) / d + 4;
            }

            h *= 60.0;
        }

        return (NormalizeHue(h), Clamp01(s), Clamp01(l));
    }

    /// <summary>
    /// Converts a color from HSL (Hue, Saturation, Lightness) color space to RGB (Red, Green, Blue) color space.
    /// </summary>
    /// <remarks>This method normalizes the hue value to ensure it falls within the valid range [0, 360] and
    /// clamps the saturation and lightness values to the range [0, 1]. The resulting RGB values are calculated based on
    /// the HSL-to-RGB conversion formula and are clamped to the range [0, 255].</remarks>
    /// <param name="h">The hue of the color, in degrees. Must be in the range [0, 360].</param>
    /// <param name="s">The saturation of the color, as a value between 0 and 1. Must be in the range [0, 1].</param>
    /// <param name="l">The lightness of the color, as a value between 0 and 1. Must be in the range [0, 1].</param>
    /// <returns>A tuple containing the RGB representation of the color: <list type="bullet"> <item><description><c>R</c>: The
    /// red component, as an integer in the range [0, 255].</description></item> <item><description><c>G</c>: The green
    /// component, as an integer in the range [0, 255].</description></item> <item><description><c>B</c>: The blue
    /// component, as an integer in the range [0, 255].</description></item> </list></returns>
    public static (int R, int G, int B) HslToRgb(double h, double s, double l)
    {
        h = NormalizeHue(h);
        s = Clamp01(s);
        l = Clamp01(l);

        var c = (1 - Math.Abs(2 * l - 1)) * s;
        var x = c * (1 - Math.Abs((h / 60.0) % 2 - 1));
        var m = l - c / 2.0;

        var r1 = 0d;
        var g1 = 0d;
        var b1 = 0d;

        if (h < 60)
        {
            r1 = c;
            g1 = x;
        }
        else if (h < 120)
        {
            r1 = x;
            g1 = c;
        }
        else if (h < 180)
        {
            g1 = c;
            b1 = x;
        }
        else if (h < 240)
        {
            g1 = x;
            b1 = c;
        }
        else if (h < 300)
        {
            r1 = x;
            b1 = c;
        }
        else
        {
            r1 = c;
            b1 = x;
        }

        var r = (int)Math.Round((r1 + m) * 255);
        var g = (int)Math.Round((g1 + m) * 255);
        var b = (int)Math.Round((b1 + m) * 255);

        return (Clamp255(r), Clamp255(g), Clamp255(b));
    }

    /// <summary>
    /// Generates a list of unique random hexadecimal color codes.
    /// </summary>
    /// <remarks>The method ensures that all generated color codes are unique and case-insensitive. The format
    /// of each color code is a six-digit hexadecimal value prefixed with a hash symbol (#), representing RGB
    /// values.</remarks>
    /// <param name="count">The number of unique hexadecimal color codes to generate. Must be a non-negative integer.</param>
    /// <returns>A list of strings, where each string is a unique hexadecimal color code in the format "#RRGGBB".</returns>
    public static List<string> GenerateRandomHex(int count)
    {
        var rng = new Random();
        var list = new List<string>(count);
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        while (list.Count < count)
        {
            var hex = $"#{rng.Next(0x1000000):X6}";

            if (seen.Add(hex))
            {
                list.Add(hex);
            }
        }

        return list;
    }

    /// <summary>
    /// Generates a gradient of colors based on the specified base color, number of steps, and gradient strategy.
    /// </summary>
    /// <remarks>The gradient generation strategy determines how the colors are interpolated: <list
    /// type="bullet"> <item> <description><see cref="GradientStrategy.Shades"/>: Adjusts the lightness of the base
    /// color to create darker or lighter shades.</description> </item> <item> <description><see
    /// cref="GradientStrategy.Tints"/>: Adjusts both saturation and lightness to create tints of the base
    /// color.</description> </item> <item> <description><see cref="GradientStrategy.Saturation"/>: Adjusts the
    /// saturation of the base color while keeping lightness constant.</description> </item> <item> <description><see
    /// cref="GradientStrategy.HueShift"/>: Shifts the hue of the base color to create a range of colors around the
    /// color wheel.</description> </item> </list> If the <paramref name="opts"/> parameter specifies <see
    /// cref="GenerationOptions.Reverse"/> as <see langword="true"/>, the resulting gradient will be reversed.</remarks>
    /// <param name="baseHex">The base color in hexadecimal format (e.g., "#FF5733").</param>
    /// <param name="steps">The number of colors to generate in the gradient. Must be greater than zero.</param>
    /// <param name="strategy">The strategy used to generate the gradient, such as shades, tints, saturation, or hue shift.</param>
    /// <param name="opts">Optional configuration for gradient generation, including lightness, saturation, and reversal options. If null,
    /// default options are used.</param>
    /// <returns>A list of hexadecimal color strings representing the generated gradient. The list will contain distinct colors,
    /// and its order may be reversed based on the specified options.</returns>
    public static List<string> GenerateGradient(
        string baseHex,
        int steps,
        GradientStrategy strategy,
        GenerationOptions? opts = null)
    {
        var (h, s, l) = HexToHsl(baseHex);
        var list = new List<string>(steps);
        opts ??= new GenerationOptions();

        switch (strategy)
        {
            case GradientStrategy.Shades:
                {
                    for (var i = 0; i < steps; i++)
                    {
                        var t = InterpolateEasingValue(i, steps, opts);
                        var l2 = Lerp(opts.LightnessMin, opts.LightnessMax, t);
                        list.Add(HslToHex(h, s, l2));
                    }
                }

                break;

            case GradientStrategy.Tints:
                {
                    for (var i = 0; i < steps; i++)
                    {
                        var t = InterpolateEasingValue(i, steps, opts);
                        var s2 = Lerp(opts.SaturationMax, opts.SaturationMin, t);
                        var l2 = Lerp(Math.Max(0.3, opts.LightnessMin), Math.Min(0.9, opts.LightnessMax), t);
                        list.Add(HslToHex(h, Clamp01(s2), Clamp01(l2)));
                    }
                }

                break;

            case GradientStrategy.Saturation:
                {
                    for (var i = 0; i < steps; i++)
                    {
                        var t = InterpolateEasingValue(i, steps, opts);
                        var s2 = Lerp(opts.SaturationMin, opts.SaturationMax, t);
                        list.Add(HslToHex(h, Clamp01(s2), l));
                    }
                }

                break;

            case GradientStrategy.HueShift:
                {
                    for (var i = 0; i < steps; i++)
                    {
                        var t = InterpolateEasingValue(i, steps, opts);
                        var h2 = NormalizeHue(h + (t - 0.5) * 240.0);
                        list.Add(HslToHex(h2, s, l));
                    }
                }

                break;
        }

        if (opts.Reverse)
        {
            list.Reverse();
        }

        return [.. list.Distinct(StringComparer.OrdinalIgnoreCase)];
    }

    /// <summary>
    /// Generates a gradient of colors between two specified hex color values.
    /// </summary>
    /// <remarks>The gradient is generated by interpolating the HSL (Hue, Saturation, Lightness) values of the
    /// start and end colors. The interpolation can be customized using the <paramref name="opts"/> parameter.</remarks>
    /// <param name="startHex">The starting color of the gradient, represented as a hex string (e.g., "#FF0000").</param>
    /// <param name="endHex">The ending color of the gradient, represented as a hex string (e.g., "#0000FF").</param>
    /// <param name="steps">The number of colors to generate in the gradient. Must be greater than zero.</param>
    /// <param name="opts">Optional configuration for gradient generation, such as saturation and lightness constraints,  and whether to
    /// reverse the gradient. If null, default options are used.</param>
    /// <returns>A list of hex color strings representing the gradient. The list contains <paramref name="steps"/> colors, 
    /// starting with <paramref name="startHex"/> and ending with <paramref name="endHex"/>.</returns>
    public static List<string> GenerateCustomGradient(string startHex, string endHex, int steps, GenerationOptions? opts = null)
    {
        var (h1, s1, l1) = HexToHsl(startHex);
        var (h2, s2, l2) = HexToHsl(endHex);
        var list = new List<string>(steps);
        opts ??= new GenerationOptions();

        for (var i = 0; i < steps; i++)
        {
            var t = InterpolateEasingValue(i, steps, opts);
            var h = ShortestHueLerp(h1, h2, t);
            var s = Lerp(s1, s2, t);
            var l = Lerp(l1, l2, t);

            s = Clamp01(Lerp(opts.SaturationMin, opts.SaturationMax, s));
            l = Clamp01(Lerp(opts.LightnessMin, opts.LightnessMax, l));

            list.Add(HslToHex(h, s, l));
        }

        if (opts.Reverse)
        {
            list.Reverse();
        }

        return list;
    }

    /// <summary>
    /// Generates a color scheme based on the specified base color, palette mode, and number of steps.
    /// </summary>
    /// <remarks>The generated color scheme varies depending on the specified <paramref name="mode"/>: <list
    /// type="bullet"> <item> <term>Complementary</term> <description>Generates a scheme with complementary colors split
    /// evenly across the specified steps.</description> </item> <item> <term>Analogous</term> <description>Generates a
    /// scheme with colors adjacent to the base color on the color wheel.</description> </item> <item>
    /// <term>Triadic</term> <description>Generates a scheme with three evenly spaced colors on the color
    /// wheel.</description> </item> <item> <term>SplitComplementary</term> <description>Generates a scheme with colors
    /// split from the complementary color.</description> </item> <item> <term>Monochrome</term> <description>Generates
    /// a scheme with shades of the base color.</description> </item> <item> <term>Warm</term> <description>Generates a
    /// scheme with warm colors (e.g., reds and yellows).</description> </item> <item> <term>Cool</term>
    /// <description>Generates a scheme with cool colors (e.g., blues and cyans).</description> </item> <item>
    /// <term>Pastel</term> <description>Generates a scheme with soft, light colors.</description> </item> <item>
    /// <term>Neon</term> <description>Generates a scheme with bright, vibrant colors.</description> </item> <item>
    /// <term>Greyscale</term> <description>Generates a scheme with shades of grey.</description> </item> <item>
    /// <term>AccessibilitySafe</term> <description>Generates a predefined scheme of colors optimized for
    /// accessibility.</description> </item> </list> The method ensures that the returned list contains distinct colors,
    /// ignoring case.</remarks>
    /// <param name="baseHex">The base color in hexadecimal format (e.g., "#FF5733").</param>
    /// <param name="mode">The <see cref="ColorPaletteMode"/> that determines the type of color scheme to generate.</param>
    /// <param name="steps">The number of colors to include in the generated scheme. Must be greater than zero.</param>
    /// <param name="opts">Optional configuration for color generation, such as gradient strategy or blending options. If null, default
    /// options are used.</param>
    /// <returns>A list of hexadecimal color strings representing the generated color scheme. The list will contain distinct
    /// colors.</returns>
    public static List<string> GenerateScheme(string baseHex, ColorPaletteMode mode, int steps, GenerationOptions? opts = null)
    {
        var (h, s, l) = HexToHsl(baseHex);
        opts ??= new GenerationOptions();
        var list = new List<string>();

        switch (mode)
        {
            case ColorPaletteMode.Complementary:
                {
                    list.AddRange(GenerateGradient(baseHex, steps / 2, GradientStrategy.Shades, opts));
                    list.AddRange(GenerateGradient(HslToHex(h + 180, s, l), steps - steps / 2, GradientStrategy.Shades, opts));
                }

                break;

            case ColorPaletteMode.Analogous:
                {
                    list.AddRange(GenerateGradient(HslToHex(h - 30, s, l), steps / 3, GradientStrategy.Shades, opts));
                    list.AddRange(GenerateGradient(baseHex, steps / 3, GradientStrategy.Shades, opts));
                    list.AddRange(GenerateGradient(HslToHex(h + 30, s, l), steps - 2 * (steps / 3), GradientStrategy.Shades, opts));
                }

                break;

            case ColorPaletteMode.Triadic:
                {
                    list.AddRange(GenerateGradient(baseHex, steps / 3, GradientStrategy.Shades, opts));
                    list.AddRange(GenerateGradient(HslToHex(h + 120, s, l), steps / 3, GradientStrategy.Shades, opts));
                    list.AddRange(GenerateGradient(HslToHex(h + 240, s, l), steps - 2 * (steps / 3), GradientStrategy.Shades, opts));
                }

                break;

            case ColorPaletteMode.SplitComplementary:
                {
                    list.AddRange(GenerateGradient(baseHex, steps / 3, GradientStrategy.Shades, opts));
                    list.AddRange(GenerateGradient(HslToHex(h + 150, s, l), steps / 3, GradientStrategy.Shades, opts));
                    list.AddRange(GenerateGradient(HslToHex(h + 210, s, l), steps - 2 * (steps / 3), GradientStrategy.Shades, opts));
                }

                break;

            case ColorPaletteMode.Monochrome:
                {
                    list = GenerateGradient(baseHex, steps, GradientStrategy.Shades, opts);
                }

                break;

            case ColorPaletteMode.Warm:
                {
                    list = GenerateCustomGradient("#FF0000", "#FFD700", steps, opts);
                }

                break;

            case ColorPaletteMode.Cool:
                {
                    list = GenerateCustomGradient("#0000FF", "#00FFFF", steps, opts);
                }

                break;

            case ColorPaletteMode.Pastel:
                {
                    for (var i = 0; i < steps; i++)
                    {
                        var t = i / (double)(steps - 1);
                        list.Add(HslToHex(h + t * 360, Clamp01(Lerp(0.35, 0.55, t)), Clamp01(Lerp(0.7, 0.9, t))));
                    }
                }

                break;

            case ColorPaletteMode.Neon:
                {
                    for (var i = 0; i < steps; i++)
                    {
                        var t = i / (double)(steps - 1);
                        list.Add(HslToHex(h + t * 360, 1.0, 0.5));
                    }
                }

                break;

            case ColorPaletteMode.Greyscale:
                {
                    for (var i = 0; i < steps; i++)
                    {
                        var l2 = i / (double)(steps - 1);
                        list.Add(HslToHex(0, 0, l2));
                    }
                }

                break;

            case ColorPaletteMode.AccessibilitySafe:
                {
                    list = ["#66c2a5", "#fc8d62", "#8da0cb", "#e78ac3", "#a6d854", "#ffd92f", "#e5c494", "#b3b3b3"];
                }

                break;

            default:
                {
                    list = GenerateGradient(baseHex, steps, GradientStrategy.Shades, opts);
                }

                break;
        }

        return [.. list.Distinct(StringComparer.OrdinalIgnoreCase)];
    }

    /// <summary>
    /// Calculates the interpolated value based on the current step, total steps, and easing options.
    /// </summary>
    /// <remarks>The method supports multiple easing functions, including exponential and sine-based easing.
    /// If <paramref name="steps"/> is 1, the method returns 1.0 regardless of the easing function.</remarks>
    /// <param name="i">The current step index, where 0 represents the first step.</param>
    /// <param name="steps">The total number of steps. Must be greater than 0.</param>
    /// <param name="opts">The options specifying the easing function to apply.</param>
    /// <returns>A double representing the interpolated value, adjusted according to the specified easing function.</returns>
    private static double InterpolateEasingValue(int i, int steps, GenerationOptions opts)
    {
        var t = steps <= 1 ? 1.0 : i / (steps - 1.0);

        return opts.Easing switch
        {
            ColorPaletteEasing.ExponentialIn => Math.Pow(t, 2.0),
            ColorPaletteEasing.ExponentialOut => 1 - Math.Pow(1 - t, 2.0),
            ColorPaletteEasing.Sine => (1 - Math.Cos(Math.PI * t)) / 2.0,
            _ => t
        };
    }

    /// <summary>
    /// Linearly interpolates between two values based on a weighting factor.
    /// </summary>
    /// <param name="a">The starting value of the interpolation.</param>
    /// <param name="b">The ending value of the interpolation.</param>
    /// <param name="t">The interpolation factor, where 0.0 returns <paramref name="a"/> and 1.0 returns <paramref name="b"/>. Values
    /// outside the range [0.0, 1.0] will result in extrapolation.</param>
    /// <returns>The interpolated value, calculated as a weighted average of <paramref name="a"/> and <paramref name="b"/>.</returns>
    private static double Lerp(double a, double b, double t) => a + (b - a) * t;

    /// <summary>
    /// Interpolates between two hue values along the shortest path on the color wheel.
    /// </summary>
    /// <remarks>This method ensures that the interpolation follows the shortest path on the hue circle,
    /// accounting for the circular nature of hue values. The result is always normalized to the range [0,
    /// 360).</remarks>
    /// <param name="h1">The starting hue, in degrees, where 0 ≤ <paramref name="h1"/> &lt; 360.</param>
    /// <param name="h2">The ending hue, in degrees, where 0 ≤ <paramref name="h2"/> &lt; 360.</param>
    /// <param name="t">A value between 0.0 and 1.0 representing the interpolation factor.  A value of 0.0 returns <paramref
    /// name="h1"/>, and a value of 1.0 returns <paramref name="h2"/>.</param>
    /// <returns>The interpolated hue, in degrees, normalized to the range [0, 360).</returns>
    private static double ShortestHueLerp(double h1, double h2, double t)
    {
        h1 = NormalizeHue(h1);
        h2 = NormalizeHue(h2);

        var d = h2 - h1;

        if (d > 180)
        {
            d -= 360;
        }

        if (d < -180)
        {
            d += 360;
        }

        return NormalizeHue(h1 + d * t);
    }

    /// <summary>
    /// Clamps the specified value to the range [0.0, 1.0].
    /// </summary>
    /// <param name="value">The value to clamp.</param>
    /// <returns>The clamped value, which will be between 0.0 and 1.0, inclusive.</returns>
    internal static double Clamp01(double value) => Math.Min(1.0, Math.Max(0.0, value));

    /// <summary>
    /// Clamps the specified integer value to the range of 0 to 255, inclusive.
    /// </summary>
    /// <param name="value">The integer value to clamp.</param>
    /// <returns>The clamped value, which will be 0 if <paramref name="value"/> is less than 0,  255 if <paramref name="value"/> is
    /// greater than 255, or <paramref name="value"/> itself if it is within the range.</returns>
    private static int Clamp255(int value) => Math.Min(255, Math.Max(0, value));

    /// <summary>
    /// Normalizes a hue value to ensure it falls within the range of 0 to 360 degrees.
    /// </summary>
    /// <param name="h">The hue value, in degrees, which may be any real number.</param>
    /// <returns>The normalized hue value, in the range [0, 360).</returns>
    private static double NormalizeHue(double h)
    {
        h %= 360.0;

        if (h < 0)
        {
            h += 360.0;
        }

        return h;
    }
}
