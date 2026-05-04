using System.Diagnostics;
using System.Globalization;
using FluentUI.Blazor.Community.Components.ColorSpace.Converters;

namespace FluentUI.Blazor.Community.Components.ColorSpace.Spaces;

/// <summary>
/// Represents a color in the sRGB color space with 8 bits per channel (sRGB8), which is commonly used for digital images and displays.
/// </summary>
[DebuggerDisplay("{ToString()}")]
public readonly struct Srgb8 : IEquatable<Srgb8>
{
    /// <summary>
    /// Gets the red component value of the color.
    /// </summary>
    public byte R { get; }

    /// <summary>
    /// Gets the green component value of the color.
    /// </summary>
    public byte G { get; }

    /// <summary>
    /// Gets the blue component value of the color.
    /// </summary>
    public byte B { get; }

    /// <summary>
    /// Gets the alpha (opacity) component value of the color, which ranges from 0 (fully transparent) to 255 (fully opaque).
    /// </summary>
    public byte A { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Srgb8"/> struct with the specified red, green, blue, and alpha component values.
    /// </summary>
    /// <param name="r">The red component value of the color. Valid values are 0 through 255.</param>
    /// <param name="g">The green component value of the color. Valid values are 0 through 255.</param>
    /// <param name="b">The blue component value of the color. Valid values are 0 through 255.</param>
    /// <param name="a">The alpha component value of the color. Valid values are 0 through 255, where 0 is fully transparent and 255 is fully opaque.</param>
    public Srgb8(byte r, byte g, byte b, byte a = 255)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    /// <summary>
    /// Creates a new Srgb8 color from the specified red, green, and blue component values.
    /// </summary>
    /// <param name="r">The red component of the color. Valid values are from 0 to 255.</param>
    /// <param name="g">The green component of the color. Valid values are from 0 to 255.</param>
    /// <param name="b">The blue component of the color. Valid values are from 0 to 255.</param>
    /// <returns>A Srgb8 instance representing the color defined by the specified red, green, and blue values.</returns>
    public static Srgb8 FromRgb(byte r, byte g, byte b) => new(r, g, b);

    /// <summary>
    /// Creates a new Srgb8 color from the specified alpha, red, green, and blue component values.
    /// </summary>
    /// <param name="a">The alpha component value of the color. Valid values are 0 through 255, where 0 is fully transparent and 255 is
    /// fully opaque.</param>
    /// <param name="r">The red component value of the color. Valid values are 0 through 255.</param>
    /// <param name="g">The green component value of the color. Valid values are 0 through 255.</param>
    /// <param name="b">The blue component value of the color. Valid values are 0 through 255.</param>
    /// <returns>A Srgb8 color constructed from the specified alpha, red, green, and blue values.</returns>
    public static Srgb8 FromArgb(byte a, byte r, byte g, byte b) => new(r, g, b, a);

    /// <inheritdoc />
    public bool Equals(Srgb8 other) => R == other.R && G == other.G && B == other.B && A == other.A;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Srgb8 other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(R, G, B, A);

    /// <summary>
    /// Determines whether two Srgb8 instances are equal.
    /// </summary>
    /// <remarks>This operator compares the values of the two Srgb8 instances for equality.</remarks>
    /// <param name="left">The first Srgb8 instance to compare.</param>
    /// <param name="right">The second Srgb8 instance to compare.</param>
    /// <returns><see langword="true"/> if the specified instances are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(Srgb8 left, Srgb8 right) => left.Equals(right);

    /// <summary>
    /// Determines whether two Srgb8 instances are not equal.
    /// </summary>
    /// <param name="left">The first Srgb8 instance to compare.</param>
    /// <param name="right">The second Srgb8 instance to compare.</param>
    /// <returns>true if the specified Srgb8 instances are not equal; otherwise, false.</returns>
    public static bool operator !=(Srgb8 left, Srgb8 right) => !left.Equals(right);

    /// <inheritdoc />
    public override string ToString() => $"#{R:X2}{G:X2}{B:X2}{A:X2}";

    /// <summary>
    /// Returns a new Srgb8 instance with each color channel clamped to the valid byte range.
    /// </summary>
    /// <remarks>Use this method to ensure that all color channel values are within the valid range for 8-bit
    /// color components. This is useful when calculations or assignments may result in values outside the standard byte
    /// range.</remarks>
    /// <returns>A new Srgb8 object whose R, G, B, and A channel values are constrained to the range 0 to 255.</returns>
    public Srgb8 Clamp()
    {
        return new Srgb8(
            Math.Clamp(R, byte.MinValue, byte.MaxValue),
            Math.Clamp(G, byte.MinValue, byte.MaxValue),
            Math.Clamp(B, byte.MinValue, byte.MaxValue),
            Math.Clamp(A, byte.MinValue, byte.MaxValue)
        );
    }

    /// <summary>
    /// Enhances the luminance of the color by a specified factor while preserving the hue and chroma,
    /// using the Oklab color space for the transformation.
    /// </summary>
    /// <param name="factor">The factor by which to boost the luminance. A value greater than 1 increases luminance, while a value between 0 and 1 decreases it.</param>
    /// <param name="workingSpace">The RGB working space to use for the color conversion.</param>
    /// <returns>Returns a new <see cref="Srgb8"/> color with the luminance boosted by the specified factor.</returns>
    public Srgb8 WithLuminanceBoost(double factor, RgbWorkingSpace workingSpace)
    {
        var xyz = ColorSpaceConverters.ToXyz(this, workingSpace);
        var lab = ColorSpaceConverters.ToOklab(xyz);
        var lch = ColorSpaceConverters.ToOklch(lab);
        var newL = Math.Clamp(lch.L * factor, 0, 1);
        var boosted = new Oklch(newL, lch.C, lch.H);
        var boostedLab = ColorSpaceConverters.FromOklch(boosted);
        var boostedXyz = ColorSpaceConverters.FromOklab(boostedLab);

        var srgb = ColorSpaceConverters.ToSrgb8(
            boostedXyz,
            workingSpace.XyzToRgbMatrix,
            workingSpace.Profile.Gamma
        );

        return srgb;
    }

    /// <summary>
    /// Creates a new Srgb8 color by combining the specified color with a new alpha value.
    /// </summary>
    /// <param name="a">The alpha component to assign to the new color.</param>
    /// <returns>A new Srgb8 color with the same red, green, and blue components as the original color, and the specified alpha
    /// value.</returns>
    public Srgb8 WithAlpha(byte a) => new(R, G, B, a);

    /// <summary>
    /// Returns a new color that is a lighter version of the specified color by increasing its lightness by the given
    /// percentage.
    /// </summary>
    /// <remarks>The method adjusts the lightness in the HSL color space, preserving the original hue,
    /// saturation, and alpha values. If the percentage is 0, the original color is returned. If the percentage is 100,
    /// the result is fully white with the original alpha.</remarks>
    /// <param name="percent">The percentage by which to increase the lightness of the color. Must be between 0 and 100.</param>
    /// <returns>A new Srgb8 color that is lighter than the original color by the specified percentage.</returns>
    public Srgb8 Lighten(double percent)
    {
        var hsl = ColorSpaceConverters.ToHsl(RgbLinear.FromSrgb8(this));
        var l = hsl.L + (1 - hsl.L) * (percent / 100);

        return ColorSpaceConverters.FromHsl(new Hsl(hsl.H, hsl.S, l, A / 255.0)).ToSrgb8();
    }

    /// <summary>
    /// Returns a new color that is a darker version of the specified sRGB color by reducing its lightness by the given
    /// percentage.
    /// </summary>
    /// <remarks>The method converts the input color to the HSL color space, decreases its lightness, and then
    /// converts it back to sRGB. The alpha channel is preserved. If percent is 0, the original color is returned; if
    /// percent is 100, the result is fully black with the original alpha.</remarks>
    /// <param name="percent">The percentage by which to decrease the lightness of the color. Must be between 0 and 100.</param>
    /// <returns>A new Srgb8 color that is darker than the original color by the specified percentage.</returns>
    public Srgb8 Darken(double percent)
    {
        var hsl = ColorSpaceConverters.ToHsl(RgbLinear.FromSrgb8(this));
        var l = hsl.L * (1 - percent / 100);

        return ColorSpaceConverters.FromHsl(new Hsl(hsl.H, hsl.S, l, A / 255.0)).ToSrgb8();
    }

    /// <summary>
    /// Blends two colors by interpolating between them using the specified percentage.
    /// </summary>
    /// <remarks>The blending is performed linearly on each color channel, including the alpha channel. Values
    /// of percent less than 0 or greater than 100 will extrapolate beyond the input colors.</remarks>
    /// <param name="b">The color to blend.</param>
    /// <param name="percent">The percentage of the blend, where 0 returns the first color and 100 returns the second color. Values between 0
    /// and 100 produce a linear interpolation.</param>
    /// <returns>A new Srgb8 color representing the blended result of the two input colors.</returns>
    public Srgb8 Blend(Srgb8 b, double percent)
    {
        var t = percent / 100.0;

        return new Srgb8(
            (byte)(R * (1 - t) + b.R * t),
            (byte)(G * (1 - t) + b.G * t),
            (byte)(B * (1 - t) + b.B * t),
            (byte)(A * (1 - t) + b.A * t)
        );
    }

    /// <summary>
    /// Creates a new color by inverting the red, green, and blue channels of the specified color while preserving its
    /// alpha channel.
    /// </summary>
    /// <returns>A new Srgb8 color with inverted red, green, and blue channels and the original alpha channel.</returns>
    public Srgb8 Invert() => new((byte)(255 - R), (byte)(255 - G), (byte)(255 - B), A);

    /// <summary>
    /// Returns either the specified light or dark color based on the luminance of the input color and a given
    /// threshold.
    /// </summary>
    /// <remarks>This method is useful for selecting a contrasting color (such as for text or icons) based on
    /// the brightness of a background color. The luminance is calculated using the standard relative luminance formula
    /// for sRGB colors.</remarks>
    /// <param name="light">The color to return if the luminance of the input color is less than or equal to the threshold.</param>
    /// <param name="dark">The color to return if the luminance of the input color is greater than the threshold.</param>
    /// <param name="threshold">The luminance threshold used to determine whether to return the light or dark color. Must be between 0.0 and
    /// 1.0. The default value is 0.5.</param>
    /// <returns>The light color if the input color's luminance is less than or equal to the threshold; otherwise, the dark
    /// color.</returns>
    public Srgb8 Contrast(Srgb8 light, Srgb8 dark, double threshold = 0.5)
    {
        var lin = RgbLinear.FromSrgb8(this);
        var luminance = 0.2126 * lin.R + 0.7152 * lin.G + 0.0722 * lin.B;

        return luminance > threshold ? dark : light;
    }

    /// <summary>
    /// Parses a string representation of a color in hexadecimal, RGB function, or CSS named color format into an Srgb8
    /// structure.
    /// </summary>
    /// <remarks>The method supports parsing colors specified in hexadecimal notation, CSS rgb() functions,
    /// and standard CSS named colors. The comparison for named colors is case-insensitive.</remarks>
    /// <param name="value">The string containing the color to parse. Supported formats include hexadecimal notation (e.g., "#FF0000"), CSS
    /// rgb() functions (e.g., "rgb(255,0,0)"), or CSS named colors (e.g., "red").</param>
    /// <returns>An Srgb8 structure that represents the parsed color.</returns>
    /// <exception cref="ArgumentException">Thrown if the value parameter is null, empty, or consists only of white-space characters.</exception>
    /// <exception cref="FormatException">Thrown if the value parameter is not in a recognized color format.</exception>
    public static Srgb8 Parse(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Color string is null or empty.", nameof(value));
        }

        value = value.Trim();

        if (value[0] == '#')
        {
            return ParseHex(value);
        }

        if (value.StartsWith("rgb", StringComparison.OrdinalIgnoreCase))
        {
            return ParseRgbFunction(value);
        }

        if (Srgb8Colors.TryGetColor(value.ToLowerInvariant(), out var named))
        {
            return named;
        }

        throw new FormatException($"Unrecognized color format: '{value}'.");
    }

    /// <summary>
    /// Parses a hexadecimal color string in the formats #RGB, #RGBA, #RRGGBB, or #RRGGBBAA
    /// and returns the corresponding Srgb8 color.
    /// </summary>
    /// <param name="hex">The hex color string to parse. It may optionally start with a '#'
    /// character and must be in one of the following formats: #RGB, #RGBA, #RRGGBB, or #RRGGBBAA.</param>
    /// <returns>The corresponding Srgb8 color.</returns>
    /// <exception cref="FormatException">Thrown when the hex string is not in a valid format.</exception>
    private static Srgb8 ParseHex(string hex)
    {
        hex = hex.TrimStart('#');

        if (hex.Length == 3)
        {
            var r = Convert.ToByte(new string(hex[0], 2), 16);
            var g = Convert.ToByte(new string(hex[1], 2), 16);
            var b = Convert.ToByte(new string(hex[2], 2), 16);

            return new Srgb8(r, g, b, 255);
        }

        if (hex.Length == 4)
        {
            var r = Convert.ToByte(new string(hex[0], 2), 16);
            var g = Convert.ToByte(new string(hex[1], 2), 16);
            var b = Convert.ToByte(new string(hex[2], 2), 16);
            var a = Convert.ToByte(new string(hex[3], 2), 16);

            return new Srgb8(r, g, b, a);
        }

        if (hex.Length == 6)
        {
            var r = Convert.ToByte(hex[..2], 16);
            var g = Convert.ToByte(hex.Substring(2, 2), 16);
            var b = Convert.ToByte(hex.Substring(4, 2), 16);

            return new Srgb8(r, g, b, 255);
        }

        if (hex.Length == 8)
        {
            var r = Convert.ToByte(hex[..2], 16);
            var g = Convert.ToByte(hex.Substring(2, 2), 16);
            var b = Convert.ToByte(hex.Substring(4, 2), 16);
            var a = Convert.ToByte(hex.Substring(6, 2), 16);

            return new Srgb8(r, g, b, a);
        }

        throw new FormatException($"Invalid hex color: #{hex}");
    }

    private static Srgb8 ParseRgbFunction(string input)
    {
        var open = input.IndexOf('(');
        var close = input.IndexOf(')');

        if (open < 0 || close < 0 || close <= open)
        {
            throw new FormatException($"Invalid rgb/rgba format: {input}");
        }

        var args = input.Substring(open + 1, close - open - 1)
                        .Split(',', StringSplitOptions.TrimEntries);

        if (args.Length < 3)
        {
            throw new FormatException($"Invalid rgb/rgba format: {input}");
        }

        var r = ParseRgbComponent(args[0]);
        var g = ParseRgbComponent(args[1]);
        var b = ParseRgbComponent(args[2]);
        byte a = 255;

        if (args.Length >= 4)
        {
            var alpha = args[3];

            if (alpha.EndsWith('%'))
            {
                var v = double.Parse(alpha.TrimEnd('%'), CultureInfo.InvariantCulture);
                a = (byte)Math.Round(255 * (v / 100.0));
            }
            else
            {
                var v = double.Parse(alpha, CultureInfo.InvariantCulture);
                a = (byte)Math.Round(v * 255.0);
            }
        }

        return new Srgb8(r, g, b, a);
    }

    /// <summary>
    /// Parses a string representing an RGB color component and returns its byte value.
    /// </summary>
    /// <remarks>The method supports both integer and percentage formats for RGB components. When a percentage
    /// is provided, it is converted to the equivalent byte value by scaling it to the 0–255 range. The input string
    /// must be a valid integer or percentage; otherwise, a format exception may be thrown.</remarks>
    /// <param name="s">A string containing the RGB component value, either as an integer (e.g., "128") or as a percentage (e.g.,
    /// "50%"). Leading and trailing whitespace is ignored.</param>
    /// <returns>A byte value corresponding to the parsed RGB component. If the input is a percentage, the value is scaled to the
    /// 0–255 range.</returns>
    private static byte ParseRgbComponent(string s)
    {
        s = s.Trim();

        if (s.EndsWith('%'))
        {
            var v = double.Parse(s.TrimEnd('%'), CultureInfo.InvariantCulture);

            return (byte)Math.Round(255 * (v / 100.0));
        }

        return byte.Parse(s, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Attempts to parse a color value from the specified string representation.
    /// </summary>
    /// <remarks>This method supports parsing colors from common CSS formats, including hexadecimal strings
    /// (with a leading '#'), rgb()/rgba() functions, and standard CSS color names. Parsing is case-insensitive for
    /// named colors and function names. If the input is null, empty, or not in a recognized format, the method returns
    /// false and sets the output parameter to its default value.</remarks>
    /// <param name="input">The string containing the color to parse. Supported formats include hexadecimal notation (e.g., "#RRGGBB"), CSS
    /// rgb()/rgba() functions, and named CSS colors. May be null or whitespace.</param>
    /// <param name="color">When this method returns, contains the parsed color if the conversion succeeded, or the default value if the
    /// conversion failed.</param>
    /// <returns>true if the input string was successfully parsed as a color; otherwise, false.</returns>
    public static bool TryParse(string? input, out Srgb8 color)
    {
        color = default;

        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        input = input.Trim();

        if (input[0] == '#')
        {
            return TryParseHex(input, out color);
        }

        if (input.StartsWith("rgb", StringComparison.OrdinalIgnoreCase))
        {
            return TryParseRgbFunction(input, out color);
        }

        if (Srgb8Colors.TryGetColor(input, out var named))
        {
            color = named;

            return true;
        }

        return false;
    }

    /// <summary>
    /// Try to parse a hexadecimal color string in the formats #RGB, #RGBA, #RRGGBB, or #RRGGBBAA
    /// </summary>
    /// <param name="hex">The hexadecimal color string to parse. May start with a '#' character.</param>
    /// <param name="color">When this method returns, contains the parsed color if the conversion succeeded, or the default value if the conversion failed.</param>
    /// <returns>true if the input string was successfully parsed as a hexadecimal color; otherwise, false.</returns>
    private static bool TryParseHex(string hex, out Srgb8 color)
    {
        color = default;

        hex = hex.TrimStart('#');

        return hex.Length switch
        {
            3 => TryParseHex3(hex, out color),
            4 => TryParseHex4(hex, out color),
            6 => TryParseHex6(hex, out color),
            8 => TryParseHex8(hex, out color),
            _ => false,
        };
    }

    /// <summary>
    /// Attempts to parse a 3-digit hexadecimal color string into an equivalent Srgb8 color value.
    /// </summary>
    /// <remarks>Each digit in the input string is expanded to an 8-bit color component by duplicating its
    /// value (e.g., 'A' becomes 0xAA). The method does not perform input validation; callers should ensure the input is
    /// valid.</remarks>
    /// <param name="hex">A string containing exactly three hexadecimal digits representing the red, green, and blue color components.</param>
    /// <param name="color">When this method returns, contains the Srgb8 color value equivalent to the hexadecimal input if parsing
    /// succeeds.</param>
    /// <returns>true if the input string was successfully parsed as a 3-digit hexadecimal color; otherwise, false.</returns>
    private static bool TryParseHex3(string hex, out Srgb8 color)
    {
        color = new Srgb8(
            Expand(hex[0]),
            Expand(hex[1]),
            Expand(hex[2])
        );
        return true;

        static byte Expand(char c) => (byte)((HexToByte(c) << 4) | HexToByte(c));
    }

    /// <summary>
    /// Attempts to parse a 3-digit hexadecimal color string into an equivalent 8-bit sRGB color value.
    /// </summary>
    /// <remarks>Each hexadecimal digit is expanded to 8 bits by repeating its value. For example, 'FAB' is
    /// interpreted as 'FFAA BB'. The input string must be exactly three characters long and contain only valid
    /// hexadecimal digits.</remarks>
    /// <param name="hex">A string containing exactly three hexadecimal digits representing the red, green, and blue color components.</param>
    /// <param name="color">When this method returns, contains the parsed sRGB color value if parsing succeeded; otherwise, the default
    /// value.</param>
    /// <returns>true if the string was successfully parsed as a 3-digit hexadecimal color; otherwise, false.</returns>
    private static bool TryParseHex4(string hex, out Srgb8 color)
    {
        color = new Srgb8(
            Expand(hex[0]),
            Expand(hex[1]),
            Expand(hex[2])
        );

        return true;

        static byte Expand(char c)
            => (byte)((HexToByte(c) << 4) | HexToByte(c));
    }

    /// <summary>
    /// Attempts to parse a 6-character hexadecimal color string into an Srgb8 color value.
    /// </summary>
    /// <param name="hex">A string containing exactly six hexadecimal characters representing the red, green, and blue components of a
    /// color.</param>
    /// <param name="color">When this method returns, contains the parsed Srgb8 color value if parsing succeeds; otherwise, the default
    /// value.</param>
    /// <returns>true if the string was successfully parsed into an Srgb8 color; otherwise, false.</returns>
    private static bool TryParseHex6(string hex, out Srgb8 color)
    {
        color = new Srgb8(
            (byte)((HexToByte(hex[0]) << 4) | HexToByte(hex[1])),
            (byte)((HexToByte(hex[2]) << 4) | HexToByte(hex[3])),
            (byte)((HexToByte(hex[4]) << 4) | HexToByte(hex[5]))
        );
        return true;
    }

    /// <summary>
    /// Attempts to parse a hexadecimal color string in the 8-bit per channel RGB format into an Srgb8 color value.
    /// </summary>
    /// <remarks>The input string must be exactly six hexadecimal digits. No validation is performed on the
    /// input length or character validity.</remarks>
    /// <param name="hex">A string containing exactly six hexadecimal characters representing the red, green, and blue color channels (in
    /// the format RRGGBB).</param>
    /// <param name="color">When this method returns, contains the parsed Srgb8 color value if parsing succeeded; otherwise, the default
    /// value.</param>
    /// <returns>true if the string was successfully parsed into an Srgb8 color; otherwise, false.</returns>
    private static bool TryParseHex8(string hex, out Srgb8 color)
    {
        color = new Srgb8(
            (byte)((HexToByte(hex[0]) << 4) | HexToByte(hex[1])),
            (byte)((HexToByte(hex[2]) << 4) | HexToByte(hex[3])),
            (byte)((HexToByte(hex[4]) << 4) | HexToByte(hex[5]))
        );
        return true;
    }

    /// <summary>
    /// Converts a hexadecimal character to its corresponding integer value.
    /// </summary>
    /// <remarks>This method does not throw an exception for invalid input; instead, it returns 0 for any
    /// character that is not a valid hexadecimal digit.</remarks>
    /// <param name="c">The hexadecimal character to convert. Valid values are '0'-'9', 'a'-'f', or 'A'-'F'.</param>
    /// <returns>An integer value from 0 to 15 that corresponds to the specified hexadecimal character. Returns 0 if the
    /// character is not a valid hexadecimal digit.</returns>
    private static int HexToByte(char c)
    {
        return c switch
        {
            >= '0' and <= '9' => c - '0',
            >= 'a' and <= 'f' => c - 'a' + 10,
            >= 'A' and <= 'F' => c - 'A' + 10,
            _ => 0
        };
    }

    /// <summary>
    /// Attempts to parse an RGB color value from a string in the CSS 'rgb()' function format.
    /// </summary>
    /// <remarks>The input string must contain at least three comma-separated components within parentheses.
    /// Parsing fails if the format is invalid or any component cannot be parsed as an RGB value.</remarks>
    /// <param name="input">The input string containing the RGB function to parse. Expected to be in the format 'rgb(r, g, b)' where r, g,
    /// and b are integer values.</param>
    /// <param name="color">When this method returns, contains the parsed RGB color if parsing succeeded; otherwise, contains the default
    /// value.</param>
    /// <returns>true if the input string was successfully parsed as an RGB color; otherwise, false.</returns>
    private static bool TryParseRgbFunction(string input, out Srgb8 color)
    {
        color = default;

        var open = input.IndexOf('(');
        var close = input.IndexOf(')');

        if (open < 0 || close < 0 || close <= open)
        {
            return false;
        }

        var args = input.Substring(open + 1, close - open - 1)
                        .Split(',', StringSplitOptions.RemoveEmptyEntries);

        if (args.Length < 3)
        {
            return false;
        }

        if (!TryParseRgbComponent(args[0], out var r))
        {
            return false;
        }

        if (!TryParseRgbComponent(args[1], out var g))
        {
            return false;
        }

        if (!TryParseRgbComponent(args[2], out var b))
        {
            return false;
        }

        color = new Srgb8(r, g, b);
        return true;
    }

    /// <summary>
    /// Attempts to parse an RGB color component value from a string representation, supporting both integer and
    /// percentage formats.
    /// </summary>
    /// <remarks>The method accepts values in either integer form (0–255) or as a percentage (0%–100%),
    /// converting percentages to the corresponding byte value in the 0–255 range. Values outside the valid range are
    /// clamped to 0 or 255.</remarks>
    /// <param name="raw">The string containing the RGB component value to parse. May be an integer (e.g., "128") or a percentage (e.g.,
    /// "50%"). Leading and trailing whitespace are ignored.</param>
    /// <param name="value">When this method returns, contains the parsed byte value of the RGB component if parsing succeeded; otherwise,
    /// contains 0. This parameter is passed uninitialized.</param>
    /// <returns>true if the string was successfully parsed as an RGB component; otherwise, false.</returns>
    private static bool TryParseRgbComponent(string raw, out byte value)
    {
        raw = raw.Trim();

        if (raw.EndsWith('%'))
        {
            if (double.TryParse(raw.TrimEnd('%'), out var pct))
            {
                value = (byte)Math.Clamp((pct / 100.0) * 255.0, 0, 255);
                return true;
            }
        }

        if (int.TryParse(raw, out var v))
        {
            value = (byte)Math.Clamp(v, 0, 255);
            return true;
        }

        value = 0;
        return false;
    }
}
