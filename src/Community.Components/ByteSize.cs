using System.ComponentModel;
using System.Globalization;

namespace FluentUI.Blazor.Community;

[TypeConverter(typeof(ByteSizeTypeConverter))]
public partial struct ByteSize : IComparable<ByteSize>, IEquatable<ByteSize>, IFormattable
{
    public static readonly ByteSize MinValue = FromBits(long.MinValue);

    public static readonly ByteSize MaxValue = FromBits(long.MaxValue);

    public const long BitsInByte = 8;
    public const string BitSymbol = "b";
    public const string ByteSymbol = "B";

    public long Bits { get; }

    public double Bytes { get; }

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

    public ByteSize(long bits)
        : this()
    {
        Bits = bits;
        Bytes = (double)bits / BitsInByte;
    }

    public ByteSize(double bytes)
        : this()
    {
        Bits = (long)Math.Ceiling(bytes * BitsInByte);
        Bytes = bytes;
    }

    public static ByteSize FromBits(long value)
    {
        return new ByteSize(value);
    }

    public static ByteSize FromBytes(double value)
    {
        return new ByteSize(value);
    }

    public override string ToString()
    {
        return ToString("0.##", CultureInfo.CurrentCulture);
    }

    public string ToString(string format)
    {
        return ToString(format, CultureInfo.CurrentCulture);
    }

    public string ToString(string? format, IFormatProvider? provider)
    {
        return ToString(format, provider, useBinaryByte: false);
    }

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
