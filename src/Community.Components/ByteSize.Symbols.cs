namespace FluentUI.Blazor.Community;

/// <summary>
/// Provides decimal (SI) unit constants, properties, and methods for the <see cref="ByteSize"/> struct.
/// </summary>
public partial struct ByteSize
{
    /// <summary>
    /// The number of bytes in one kilobyte (KB). 1 KB = 1,000 bytes.
    /// </summary>
    public const long BytesInKiloByte = 1_000;

    /// <summary>
    /// The number of bytes in one megabyte (MB). 1 MB = 1,000,000 bytes.
    /// </summary>
    public const long BytesInMegaByte = 1_000_000;

    /// <summary>
    /// The number of bytes in one gigabyte (GB). 1 GB = 1,000,000,000 bytes.
    /// </summary>
    public const long BytesInGigaByte = 1_000_000_000;

    /// <summary>
    /// The number of bytes in one terabyte (TB). 1 TB = 1,000,000,000,000 bytes.
    /// </summary>
    public const long BytesInTeraByte = 1_000_000_000_000;

    /// <summary>
    /// The number of bytes in one petabyte (PB). 1 PB = 1,000,000,000,000,000 bytes.
    /// </summary>
    public const long BytesInPetaByte = 1_000_000_000_000_000;

    /// <summary>
    /// The number of bytes in one exabyte (EB). 1 EB = 1,000,000,000,000,000,000 bytes.
    /// </summary>
    public const long BytesInExaByte = 1_000_000_000_000_000_000;

    /// <summary>
    /// The symbol for kilobyte (KB).
    /// </summary>
    public const string KiloByteSymbol = "KB";

    /// <summary>
    /// The symbol for megabyte (MB).
    /// </summary>
    public const string MegaByteSymbol = "MB";

    /// <summary>
    /// The symbol for gigabyte (GB).
    /// </summary>
    public const string GigaByteSymbol = "GB";

    /// <summary>
    /// The symbol for terabyte (TB).
    /// </summary>
    public const string TeraByteSymbol = "TB";

    /// <summary>
    /// The symbol for petabyte (PB).
    /// </summary>
    public const string PetaByteSymbol = "PB";

    /// <summary>
    /// The symbol for exabyte (EB).
    /// </summary>
    public const string ExaByteSymbol = "EB";

    /// <summary>
    /// Gets the value in kilobytes (KB).
    /// </summary>
    public double KiloBytes => Bytes / BytesInKiloByte;

    /// <summary>
    /// Gets the value in megabytes (MB).
    /// </summary>
    public double MegaBytes => Bytes / BytesInMegaByte;

    /// <summary>
    /// Gets the value in gigabytes (GB).
    /// </summary>
    public double GigaBytes => Bytes / BytesInGigaByte;

    /// <summary>
    /// Gets the value in terabytes (TB).
    /// </summary>
    public double TeraBytes => Bytes / BytesInTeraByte;

    /// <summary>
    /// Gets the value in petabytes (PB).
    /// </summary>
    public double PetaBytes => Bytes / BytesInPetaByte;

    /// <summary>
    /// Gets the value in exabytes (EB).
    /// </summary>
    public double ExaBytes => Bytes / BytesInExaByte;

    /// <summary>
    /// Creates a <see cref="ByteSize"/> from the specified number of kilobytes (KB).
    /// </summary>
    /// <param name="value">The number of kilobytes.</param>
    /// <returns>A new <see cref="ByteSize"/> instance.</returns>
    public static ByteSize FromKiloBytes(double value)
    {
        return new ByteSize(value * BytesInKiloByte);
    }

    /// <summary>
    /// Creates a <see cref="ByteSize"/> from the specified number of megabytes (MB).
    /// </summary>
    /// <param name="value">The number of megabytes.</param>
    /// <returns>A new <see cref="ByteSize"/> instance.</returns>
    public static ByteSize FromMegaBytes(double value)
    {
        return new ByteSize(value * BytesInMegaByte);
    }

    /// <summary>
    /// Creates a <see cref="ByteSize"/> from the specified number of gigabytes (GB).
    /// </summary>
    /// <param name="value">The number of gigabytes.</param>
    /// <returns>A new <see cref="ByteSize"/> instance.</returns>
    public static ByteSize FromGigaBytes(double value)
    {
        return new ByteSize(value * BytesInGigaByte);
    }

    /// <summary>
    /// Creates a <see cref="ByteSize"/> from the specified number of terabytes (TB).
    /// </summary>
    /// <param name="value">The number of terabytes.</param>
    /// <returns>A new <see cref="ByteSize"/> instance.</returns>
    public static ByteSize FromTeraBytes(double value)
    {
        return new ByteSize(value * BytesInTeraByte);
    }

    /// <summary>
    /// Creates a <see cref="ByteSize"/> from the specified number of petabytes (PB).
    /// </summary>
    /// <param name="value">The number of petabytes.</param>
    /// <returns>A new <see cref="ByteSize"/> instance.</returns>
    public static ByteSize FromPetaBytes(double value)
    {
        return new ByteSize(value * BytesInPetaByte);
    }

    /// <summary>
    /// Creates a <see cref="ByteSize"/> from the specified number of exabytes (EB).
    /// </summary>
    /// <param name="value">The number of exabytes.</param>
    /// <returns>A new <see cref="ByteSize"/> instance.</returns>
    public static ByteSize FromExaBytes(double value)
    {
        return new ByteSize(value * BytesInExaByte);
    }

    /// <summary>
    /// Returns a new <see cref="ByteSize"/> by adding the specified number of kilobytes (KB) to this instance.
    /// </summary>
    /// <param name="value">The number of kilobytes to add.</param>
    /// <returns>The result as a new <see cref="ByteSize"/>.</returns>
    public ByteSize AddKiloBytes(double value)
    {
        return this + FromKiloBytes(value);
    }

    /// <summary>
    /// Returns a new <see cref="ByteSize"/> by adding the specified number of megabytes (MB) to this instance.
    /// </summary>
    /// <param name="value">The number of megabytes to add.</param>
    /// <returns>The result as a new <see cref="ByteSize"/>.</returns>
    public ByteSize AddMegaBytes(double value)
    {
        return this + FromMegaBytes(value);
    }

    /// <summary>
    /// Returns a new <see cref="ByteSize"/> by adding the specified number of gigabytes (GB) to this instance.
    /// </summary>
    /// <param name="value">The number of gigabytes to add.</param>
    /// <returns>The result as a new <see cref="ByteSize"/>.</returns>
    public ByteSize AddGigaBytes(double value)
    {
        return this + FromGigaBytes(value);
    }

    /// <summary>
    /// Returns a new <see cref="ByteSize"/> by adding the specified number of terabytes (TB) to this instance.
    /// </summary>
    /// <param name="value">The number of terabytes to add.</param>
    /// <returns>The result as a new <see cref="ByteSize"/>.</returns>
    public ByteSize AddTeraBytes(double value)
    {
        return this + FromTeraBytes(value);
    }

    /// <summary>
    /// Returns a new <see cref="ByteSize"/> by adding the specified number of petabytes (PB) to this instance.
    /// </summary>
    /// <param name="value">The number of petabytes to add.</param>
    /// <returns>The result as a new <see cref="ByteSize"/>.</returns>
    public ByteSize AddPetaBytes(double value)
    {
        return this + FromPetaBytes(value);
    }

    /// <summary>
    /// Returns a new <see cref="ByteSize"/> by adding the specified number of exabytes (EB) to this instance.
    /// </summary>
    /// <param name="value">The number of exabytes to add.</param>
    /// <returns>The result as a new <see cref="ByteSize"/>.</returns>
    public ByteSize AddExaBytes(double value)
    {
        return this + FromExaBytes(value);
    }
}
