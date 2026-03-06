using System.ComponentModel;
using System.Globalization;

namespace FluentUI.Blazor.Community;

/// <summary>
/// Represents a size in bytes and provides methods for formatting, conversion, and comparison.
/// </summary>
[TypeConverter(typeof(ByteSizeTypeConverter))]
public partial struct ByteSize : IComparable<ByteSize>, IEquatable<ByteSize>, IFormattable
{
    /// <summary>
    /// Represents the smallest possible value of <see cref="ByteSize"/>.
    /// </summary>
    public static readonly ByteSize MinValue = FromBits(long.MinValue);

    /// <summary>
    /// Represents the largest possible value of <see cref="ByteSize"/>.
    /// </summary>
    public static readonly ByteSize MaxValue = FromBits(long.MaxValue);

    /// <summary>
    /// The number of bits in one byte. Always 8.
    /// </summary>
    public const long BitsInByte = 8;

    /// <summary>
    /// The symbol for a bit ("b").
    /// </summary>
    public const string BitSymbol = "b";

    /// <summary>
    /// The symbol for a byte ("B").
    /// </summary>
    public const string ByteSymbol = "B";

    /// <summary>
    /// Gets the total number of bits represented by this instance.
    /// </summary>
    public long Bits { get; }

    /// <summary>
    /// Gets the total number of bytes represented by this instance.
    /// </summary>
    public double Bytes { get; }

    /// <summary>
    /// Gets the largest whole number binary unit symbol (IEC) that is less than or equal to the value.
    /// </summary>
    public string LargestWholeNumberBinarySymbol
    {
        get
        {
            return Math.Abs(ExabiBytes) >= 1 ? ExabiByteSymbol :
                   Math.Abs(PebiBytes) >= 1 ? PebiByteSymbol :
                   Math.Abs(TebiBytes) >= 1 ? TebiByteSymbol :
                   Math.Abs(GibiBytes) >= 1 ? GibiByteSymbol :
                   Math.Abs(MebiBytes) >= 1 ? MebiByteSymbol :
                   Math.Abs(KibiBytes) >= 1 ? KibiByteSymbol :
                   Math.Abs(Bytes) >= 1 ? ByteSymbol :
                   BitSymbol;
        }
    }

    /// <summary>
    /// Gets the largest whole number decimal unit symbol (SI) that is less than or equal to the value.
    /// </summary>
    public string LargestWholeNumberDecimalSymbol
    {
        get
        {
            return Math.Abs(ExaBytes) >= 1 ? ExaByteSymbol :
                   Math.Abs(PetaBytes) >= 1 ? PetaByteSymbol :
                   Math.Abs(TeraBytes) >= 1 ? TeraByteSymbol :
                   Math.Abs(GigaBytes) >= 1 ? GigaByteSymbol :
                   Math.Abs(MegaBytes) >= 1 ? MegaByteSymbol :
                   Math.Abs(KiloBytes) >= 1 ? KiloByteSymbol :
                   Math.Abs(Bytes) >= 1 ? ByteSymbol :
                   BitSymbol;
        }
    }

    /// <summary>
    /// Gets the value in the largest whole number binary unit (IEC) that is less than or equal to the value.
    /// </summary>
    public double LargestWholeNumberBinaryValue
    {
        get
        {
            return Math.Abs(PebiBytes) >= 1 ? PebiBytes :
                  Math.Abs(TebiBytes) >= 1 ? TebiBytes :
                  Math.Abs(GibiBytes) >= 1 ? GibiBytes :
                  Math.Abs(MebiBytes) >= 1 ? MebiBytes :
                  Math.Abs(KibiBytes) >= 1 ? KibiBytes :
                  Math.Abs(Bytes) >= 1 ? Bytes :
                  Bits;
        }
    }

    /// <summary>
    /// Gets the value in the largest whole number decimal unit (SI) that is less than or equal to the value.
    /// </summary>
    public double LargestWholeNumberDecimalValue
    {
        get
        {
            return Math.Abs(PetaBytes) >= 1 ? PetaBytes :
                 Math.Abs(TebiBytes) >= 1 ? TeraBytes :
                 Math.Abs(GibiBytes) >= 1 ? GigaBytes :
                 Math.Abs(MebiBytes) >= 1 ? MegaBytes :
                 Math.Abs(KibiBytes) >= 1 ? KiloBytes :
                 Math.Abs(Bytes) >= 1 ? Bytes :
                 Bits;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ByteSize"/> struct from a number of bits.
    /// </summary>
    /// <param name="bits">The number of bits.</param>
    public ByteSize(long bits)
        : this()
    {
        Bits = bits;
        Bytes = (double)bits / BitsInByte;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ByteSize"/> struct from a number of bytes.
    /// </summary>
    /// <param name="bytes">The number of bytes.</param>
    public ByteSize(double bytes)
        : this()
    {
        Bits = (long)Math.Ceiling(bytes * BitsInByte);
        Bytes = bytes;
    }

    /// <summary>
    /// Creates a <see cref="ByteSize"/> from the specified number of bits.
    /// </summary>
    /// <param name="value">The number of bits.</param>
    /// <returns>A new <see cref="ByteSize"/> instance.</returns>
    public static ByteSize FromBits(long value)
    {
        return new ByteSize(value);
    }

    /// <summary>
    /// Creates a <see cref="ByteSize"/> from the specified number of bytes.
    /// </summary>
    /// <param name="value">The number of bytes.</param>
    /// <returns>A new <see cref="ByteSize"/> instance.</returns>
    public static ByteSize FromBytes(double value)
    {
        return new ByteSize(value);
    }

    /// <summary>
    /// Returns a string representation of the value using the default format and current culture.
    /// </summary>
    /// <returns>A string representation of the value.</returns>
    public override string ToString()
    {
        return ToString("0.##", CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Returns a string representation of the value using the specified format and current culture.
    /// </summary>
    /// <param name="format">The format string.</param>
    /// <returns>A string representation of the value.</returns>
    public string ToString(string format)
    {
        return ToString(format, CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Returns a string representation of the value using the specified format and format provider.
    /// </summary>
    /// <param name="format">The format string.</param>
    /// <param name="provider">The format provider.</param>
    /// <returns>A string representation of the value.</returns>
    public string ToString(string? format, IFormatProvider? provider)
    {
        return ToString(format, provider, useBinaryByte: false);
    }

    /// <summary>
    /// Returns a string representation of the value using the specified format, format provider, and unit system.
    /// </summary>
    /// <param name="format">The format string.</param>
    /// <param name="provider">The format provider.</param>
    /// <param name="useBinaryByte">If true, uses binary (IEC) units; otherwise, uses decimal (SI) units.</param>
    /// <returns>A string representation of the value.</returns>
    public string ToString(string? format, IFormatProvider? provider, bool useBinaryByte)
    {
        if (!string.IsNullOrEmpty(format) && !format.Contains('#') && !format.Contains('0'))
        {
            format = "0.## " + format;
        }
        else
        {
            format ??= "0.##";
        }

        provider ??= CultureInfo.CurrentCulture;

        bool Contains(string s) => format != null && format.Contains(s, StringComparison.CurrentCultureIgnoreCase);
        string Output(double n) => n.ToString(format, provider);

        // Binary
        if (Contains("PiB"))
        {
            return Output(PebiBytes);
        }

        if (Contains("TiB"))
        {
            return Output(TebiBytes);
        }

        if (Contains("GiB"))
        {
            return Output(GibiBytes);
        }

        if (Contains("MiB"))
        {
            return Output(MebiBytes);
        }

        if (Contains("KiB"))
        {
            return Output(KibiBytes);
        }

        if (Contains("PB"))
        {
            return Output(PetaBytes);
        }

        if (Contains("TB"))
        {
            return Output(TeraBytes);
        }

        if (Contains("GB"))
        {
            return Output(GigaBytes);
        }

        if (Contains("MB"))
        {
            return Output(MegaBytes);
        }

        if (Contains("KB"))
        {
            return Output(KiloBytes);
        }

        if (format != null && format.Contains(ByteSymbol))
        {
            return Output(Bytes);
        }

        if (format != null && format.Contains(BitSymbol))
        {
            return Output(Bits);
        }

        return (Bytes, useBinaryByte) switch
        {
            (0, _) => "0 B",
            (not 0, true) => string.Format(
                CultureInfo.CurrentCulture,
                "{0} {1}",
                LargestWholeNumberBinaryValue.ToString(format, provider),
                LargestWholeNumberBinarySymbol),

            (not 0, false) => string.Format(
                CultureInfo.CurrentCulture,
                "{0} {1}",
                LargestWholeNumberDecimalValue.ToString(format, provider),
                LargestWholeNumberDecimalSymbol),
        };
    }
}
