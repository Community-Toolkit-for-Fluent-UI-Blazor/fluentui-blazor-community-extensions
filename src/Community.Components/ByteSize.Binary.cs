// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Globalization;

namespace FluentUI.Blazor.Community;

public partial struct ByteSize
{
    public const long BytesInKibiByte = 1_024;
    public const long BytesInMebiByte = 1_024 * BytesInKibiByte;
    public const long BytesInGibiByte = 1_024 * BytesInMebiByte;
    public const long BytesInTebiByte = 1_024 * BytesInGibiByte;
    public const long BytesInPebiByte = 1_024 * BytesInTebiByte;
    public const long BytesInExabiByte = 1_024 * BytesInPebiByte;

    public const string KibiByteSymbol = "KiB";
    public const string MebiByteSymbol = "MiB";
    public const string GibiByteSymbol = "GiB";
    public const string TebiByteSymbol = "TiB";
    public const string PebiByteSymbol = "PiB";
    public const string ExabiByteSymbol = "EiB";

    public readonly double KibiBytes => Bytes / BytesInKibiByte;
    public readonly double MebiBytes => Bytes / BytesInMebiByte;
    public readonly double GibiBytes => Bytes / BytesInGibiByte;
    public readonly double TebiBytes => Bytes / BytesInTebiByte;
    public readonly double PebiBytes => Bytes / BytesInPebiByte;
    public readonly double ExabiBytes => Bytes / BytesInExabiByte;


    public static ByteSize FromKibiBytes(double value)
    {
        return new ByteSize(value * BytesInKibiByte);
    }

    public static ByteSize FromMebiBytes(double value)
    {
        return new ByteSize(value * BytesInMebiByte);
    }

    public static ByteSize FromGibiBytes(double value)
    {
        return new ByteSize(value * BytesInGibiByte);
    }

    public static ByteSize FromTebiBytes(double value)
    {
        return new ByteSize(value * BytesInTebiByte);
    }

    public static ByteSize FromPebiBytes(double value)
    {
        return new ByteSize(value * BytesInPebiByte);
    }

    public static ByteSize FromExabiBytes(double value)
    {
        return new ByteSize(value * BytesInExabiByte);
    }

    public readonly ByteSize AddKibiBytes(double value)
    {
        return this + FromKibiBytes(value);
    }

    public readonly ByteSize AddMebiBytes(double value)
    {
        return this + FromMebiBytes(value);
    }

    public readonly ByteSize AddGibiBytes(double value)
    {
        return this + FromGibiBytes(value);
    }

    public readonly ByteSize AddTebiBytes(double value)
    {
        return this + FromTebiBytes(value);
    }

    public readonly ByteSize AddPebiBytes(double value)
    {
        return this + FromPebiBytes(value);
    }

    public readonly ByteSize AddExabiBytes(double value)
    {
        return this + FromExabiBytes(value);
    }

    public string ToBinaryString()
    {
        return ToString("0.##", CultureInfo.CurrentCulture, useBinaryByte: true);
    }

    public string ToBinaryString(IFormatProvider formatProvider)
    {
        return ToString("0.##", formatProvider, useBinaryByte: true);
    }
}
