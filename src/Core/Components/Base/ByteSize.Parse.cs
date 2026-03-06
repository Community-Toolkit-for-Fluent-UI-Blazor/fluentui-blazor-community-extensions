using System.Globalization;

namespace FluentUI.Blazor.Community;

public partial struct ByteSize
{
    /// <summary>
    /// Parses the specified string representation of a byte size using the current culture.
    /// </summary>
    /// <param name="s">The string to parse (e.g., "1.5 MB", "2 GiB", "1024 B").</param>
    /// <returns>A <see cref="ByteSize"/> instance representing the parsed value.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="s"/> is null or whitespace.</exception>
    /// <exception cref="FormatException">Thrown if the string is not in a valid format or contains an unsupported unit.</exception>
    public static ByteSize Parse(string s)
    {
        return Parse(s, NumberFormatInfo.CurrentInfo);
    }

    /// <summary>
    /// Parses the specified string representation of a byte size using the provided format provider.
    /// </summary>
    /// <param name="s">The string to parse (e.g., "1.5 MB", "2 GiB", "1024 B").</param>
    /// <param name="formatProvider">The format provider to use for parsing numbers.</param>
    /// <returns>A <see cref="ByteSize"/> instance representing the parsed value.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="s"/> is null or whitespace.</exception>
    /// <exception cref="FormatException">Thrown if the string is not in a valid format or contains an unsupported unit.</exception>
    public static ByteSize Parse(string s, IFormatProvider formatProvider)
    {
        return Parse(s, NumberStyles.Float | NumberStyles.AllowThousands, formatProvider);
    }

    /// <summary>
    /// Parses the specified string representation of a byte size using the provided number styles and format provider.
    /// </summary>
    /// <param name="value">The string to parse (e.g., "1.5 MB", "2 GiB", "1024 B").</param>
    /// <param name="numberStyles">The number styles to use for parsing the numeric part.</param>
    /// <param name="formatProvider">The format provider to use for parsing numbers.</param>
    /// <returns>A <see cref="ByteSize"/> instance representing the parsed value.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is null or whitespace.</exception>
    /// <exception cref="FormatException">Thrown if the string is not in a valid format or contains an unsupported unit.</exception>
    public static ByteSize Parse(
        string value,
        NumberStyles numberStyles,
        IFormatProvider formatProvider)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));

        value = value.TrimStart();
        var found = false;
        var numberFormatInfo = NumberFormatInfo.GetInstance(formatProvider);
        var decimalSeparator = Convert.ToChar(numberFormatInfo.NumberDecimalSeparator, CultureInfo.CurrentCulture);
        var groupSeparator = Convert.ToChar(numberFormatInfo.NumberGroupSeparator, CultureInfo.CurrentCulture);

        int num;

        for (num = 0; num < value.Length; num++)
        {
            if (!(char.IsDigit(value[num]) || value[num] == decimalSeparator || value[num] == groupSeparator))
            {
                found = true;
                break;
            }
        }

        if (!found)
        {
            throw new FormatException($"No byte indicator found in value '{value}'.");
        }

        var lastNumber = num;
        var numberPart = value[..lastNumber].Trim();

        if (!double.TryParse(numberPart, numberStyles, formatProvider, out var number))
        {
            throw new FormatException($"No number found in value '{value}'.");
        }

        var sizePart = value[lastNumber..].Trim();

        switch (sizePart)
        {
            case "b":
                {
                    if (number % 1 != 0) // Can't have partial bits
                    {
                        throw new FormatException($"Can't have partial bits for value '{value}'.");
                    }

                    return FromBits((long)number);
                }

            case "B":
                {
                    return FromBytes(number);
                }
        }

        return sizePart.ToLowerInvariant() switch
        {
            // Binary
            "kib" => FromKibiBytes(number),
            "mib" => FromMebiBytes(number),
            "gib" => FromGibiBytes(number),
            "tib" => FromTebiBytes(number),
            "pib" => FromPebiBytes(number),

            // Decimal
            "kb" => FromKiloBytes(number),
            "mb" => FromMegaBytes(number),
            "gb" => FromGigaBytes(number),
            "tb" => FromTeraBytes(number),
            "pb" => FromPetaBytes(number),
            _ => throw new FormatException($"Bytes of magnitude '{sizePart}' is not supported."),
        };
    }

    /// <summary>
    /// Tries to parse the specified string representation of a byte size using the current culture.
    /// </summary>
    /// <param name="value">The string to parse.</param>
    /// <param name="result">When this method returns, contains the parsed <see cref="ByteSize"/> value if parsing succeeded, or the default value if parsing failed.</param>
    /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
    public static bool TryParse(string value, out ByteSize result)
    {
        try
        {
            result = Parse(value, CultureInfo.CurrentCulture);
            return true;
        }
        catch
        {
            result = new ByteSize();
            return false;
        }
    }

    /// <summary>
    /// Tries to parse the specified string representation of a byte size using the provided number styles and format provider.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="numberStyles">The number styles to use for parsing the numeric part.</param>
    /// <param name="formatProvider">The format provider to use for parsing numbers.</param>
    /// <param name="result">When this method returns, contains the parsed <see cref="ByteSize"/> value if parsing succeeded, or the default value if parsing failed.</param>
    /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
    public static bool TryParse(string s, NumberStyles numberStyles, IFormatProvider formatProvider, out ByteSize result)
    {
        try
        {
            result = Parse(s, numberStyles, formatProvider);
            return true;
        }
        catch
        {
            result = new ByteSize();
            return false;
        }
    }
}
