using System.Globalization;

namespace FluentUI.Blazor.Community;

/// <summary>
/// Provides binary (IEC) unit constants, properties, and methods for the <see cref="ByteSize"/> struct.
/// </summary>
public partial struct ByteSize
{
    /// <summary>
    /// The number of bytes in one kibibyte (KiB). 1 KiB = 1,024 bytes.
    /// </summary>
    public const long BytesInKibiByte = 1_024;

    /// <summary>
    /// The number of bytes in one mebibyte (MiB). 1 MiB = 1,024 KiB = 1,048,576 bytes.
    /// </summary>
    public const long BytesInMebiByte = 1_024 * BytesInKibiByte;

    /// <summary>
    /// The number of bytes in one gibibyte (GiB). 1 GiB = 1,024 MiB = 1,073,741,824 bytes.
    /// </summary>
    public const long BytesInGibiByte = 1_024 * BytesInMebiByte;

    /// <summary>
    /// The number of bytes in one tebibyte (TiB). 1 TiB = 1,024 GiB = 1,099,511,627,776 bytes.
    /// </summary>
    public const long BytesInTebiByte = 1_024 * BytesInGibiByte;

    /// <summary>
    /// The number of bytes in one pebibyte (PiB). 1 PiB = 1,024 TiB = 1,125,899,906,842,624 bytes.
    /// </summary>
    public const long BytesInPebiByte = 1_024 * BytesInTebiByte;

    /// <summary>
    /// The number of bytes in one exbibyte (EiB). 1 EiB = 1,024 PiB = 1,152,921,504,606,846,976 bytes.
    /// </summary>
    public const long BytesInExabiByte = 1_024 * BytesInPebiByte;

    /// <summary>
    /// The symbol for kibibyte (KiB).
    /// </summary>
    public const string KibiByteSymbol = "KiB";

    /// <summary>
    /// The symbol for mebibyte (MiB).
    /// </summary>
    public const string MebiByteSymbol = "MiB";

    /// <summary>
    /// The symbol for gibibyte (GiB).
    /// </summary>
    public const string GibiByteSymbol = "GiB";

    /// <summary>
    /// The symbol for tebibyte (TiB).
    /// </summary>
    public const string TebiByteSymbol = "TiB";

    /// <summary>
    /// The symbol for pebibyte (PiB).
    /// </summary>
    public const string PebiByteSymbol = "PiB";

    /// <summary>
    /// The symbol for exbibyte (EiB).
    /// </summary>
    public const string ExabiByteSymbol = "EiB";

    /// <summary>
    /// Gets the value in kibibytes (KiB).
    /// </summary>
    public readonly double KibiBytes => Bytes / BytesInKibiByte;

    /// <summary>
    /// Gets the value in mebibytes (MiB).
    /// </summary>
    public readonly double MebiBytes => Bytes / BytesInMebiByte;

    /// <summary>
    /// Gets the value in gibibytes (GiB).
    /// </summary>
    public readonly double GibiBytes => Bytes / BytesInGibiByte;

    /// <summary>
    /// Gets the value in tebibytes (TiB).
    /// </summary>
    public readonly double TebiBytes => Bytes / BytesInTebiByte;

    /// <summary>
    /// Gets the value in pebibytes (PiB).
    /// </summary>
    public readonly double PebiBytes => Bytes / BytesInPebiByte;

    /// <summary>
    /// Gets the value in exbibytes (EiB).
    /// </summary>
    public readonly double ExabiBytes => Bytes / BytesInExabiByte;

    /// <summary>
    /// Creates a <see cref="ByteSize"/> from the specified number of kibibytes (KiB).
    /// </summary>
    /// <param name="value">The number of kibibytes.</param>
    /// <returns>A new <see cref="ByteSize"/> instance.</returns>
    public static ByteSize FromKibiBytes(double value)
    {
        return new ByteSize(value * BytesInKibiByte);
    }

    /// <summary>
    /// Creates a <see cref="ByteSize"/> from the specified number of mebibytes (MiB).
    /// </summary>
    /// <param name="value">The number of mebibytes.</param>
    /// <returns>A new <see cref="ByteSize"/> instance.</returns>
    public static ByteSize FromMebiBytes(double value)
    {
        return new ByteSize(value * BytesInMebiByte);
    }

    /// <summary>
    /// Creates a <see cref="ByteSize"/> from the specified number of gibibytes (GiB).
    /// </summary>
    /// <param name="value">The number of gibibytes.</param>
    /// <returns>A new <see cref="ByteSize"/> instance.</returns>
    public static ByteSize FromGibiBytes(double value)
    {
        return new ByteSize(value * BytesInGibiByte);
    }

    /// <summary>
    /// Creates a <see cref="ByteSize"/> from the specified number of tebibytes (TiB).
    /// </summary>
    /// <param name="value">The number of tebibytes.</param>
    /// <returns>A new <see cref="ByteSize"/> instance.</returns>
    public static ByteSize FromTebiBytes(double value)
    {
        return new ByteSize(value * BytesInTebiByte);
    }

    /// <summary>
    /// Creates a <see cref="ByteSize"/> from the specified number of pebibytes (PiB).
    /// </summary>
    /// <param name="value">The number of pebibytes.</param>
    /// <returns>A new <see cref="ByteSize"/> instance.</returns>
    public static ByteSize FromPebiBytes(double value)
    {
        return new ByteSize(value * BytesInPebiByte);
    }

    /// <summary>
    /// Creates a <see cref="ByteSize"/> from the specified number of exbibytes (EiB).
    /// </summary>
    /// <param name="value">The number of exbibytes.</param>
    /// <returns>A new <see cref="ByteSize"/> instance.</returns>
    public static ByteSize FromExabiBytes(double value)
    {
        return new ByteSize(value * BytesInExabiByte);
    }

    /// <summary>
    /// Returns a new <see cref="ByteSize"/> by adding the specified number of kibibytes (KiB) to this instance.
    /// </summary>
    /// <param name="value">The number of kibibytes to add.</param>
    /// <returns>The result as a new <see cref="ByteSize"/>.</returns>
    public readonly ByteSize AddKibiBytes(double value)
    {
        return this + FromKibiBytes(value);
    }

    /// <summary>
    /// Returns a new <see cref="ByteSize"/> by adding the specified number of mebibytes (MiB) to this instance.
    /// </summary>
    /// <param name="value">The number of mebibytes to add.</param>
    /// <returns>The result as a new <see cref="ByteSize"/>.</returns>
    public readonly ByteSize AddMebiBytes(double value)
    {
        return this + FromMebiBytes(value);
    }

    /// <summary>
    /// Returns a new <see cref="ByteSize"/> by adding the specified number of gibibytes (GiB) to this instance.
    /// </summary>
    /// <param name="value">The number of gibibytes to add.</param>
    /// <returns>The result as a new <see cref="ByteSize"/>.</returns>
    public readonly ByteSize AddGibiBytes(double value)
    {
        return this + FromGibiBytes(value);
    }

    /// <summary>
    /// Returns a new <see cref="ByteSize"/> by adding the specified number of tebibytes (TiB) to this instance.
    /// </summary>
    /// <param name="value">The number of tebibytes to add.</param>
    /// <returns>The result as a new <see cref="ByteSize"/>.</returns>
    public readonly ByteSize AddTebiBytes(double value)
    {
        return this + FromTebiBytes(value);
    }

    /// <summary>
    /// Returns a new <see cref="ByteSize"/> by adding the specified number of pebibytes (PiB) to this instance.
    /// </summary>
    /// <param name="value">The number of pebibytes to add.</param>
    /// <returns>The result as a new <see cref="ByteSize"/>.</returns>
    public readonly ByteSize AddPebiBytes(double value)
    {
        return this + FromPebiBytes(value);
    }

    /// <summary>
    /// Returns a new <see cref="ByteSize"/> by adding the specified number of exbibytes (EiB) to this instance.
    /// </summary>
    /// <param name="value">The number of exbibytes to add.</param>
    /// <returns>The result as a new <see cref="ByteSize"/>.</returns>
    public readonly ByteSize AddExabiBytes(double value)
    {
        return this + FromExabiBytes(value);
    }

    /// <summary>
    /// Returns a string representation of the value using binary units (IEC), formatted with the current culture.
    /// </summary>
    /// <returns>A string representation in binary units (e.g., "1.5 GiB").</returns>
    public string ToBinaryString()
    {
        return ToString("0.##", CultureInfo.CurrentCulture, useBinaryByte: true);
    }

    /// <summary>
    /// Returns a string representation of the value using binary units (IEC), formatted with the specified format provider.
    /// </summary>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <returns>A string representation in binary units (e.g., "1.5 GiB").</returns>
    public string ToBinaryString(IFormatProvider formatProvider)
    {
        return ToString("0.##", formatProvider, useBinaryByte: true);
    }
}
