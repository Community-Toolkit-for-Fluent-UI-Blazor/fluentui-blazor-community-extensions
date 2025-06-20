using System.Globalization;

namespace FluentUI.Blazor.Community;

public partial struct ByteSize
{
    public static ByteSize Parse(string s)
    {
        return Parse(s, NumberFormatInfo.CurrentInfo);
    }

    public static ByteSize Parse(string s, IFormatProvider formatProvider)
    {
        return Parse(s, NumberStyles.Float | NumberStyles.AllowThousands, formatProvider);
    }

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
