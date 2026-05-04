using System.Text;
using FluentUI.Blazor.Community.Components.Enums;
using FluentUI.Blazor.Community.Components.Helpers;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides static methods for encoding data, determining encoding modes and QR code versions, and constructing the bit
/// streams and matrix structures required for QR code generation according to the QR code specification.
/// </summary>
/// <remarks>This class encapsulates the core logic for QR code data preparation, including mode resolution, data
/// encoding, error correction, and matrix construction. It supports multiple encoding modes (numeric, alphanumeric,
/// byte), automatic mode selection, and error correction level handling. All methods are static and intended for
/// internal use within the QR code generation process. The class does not perform input validation beyond what is
/// required for correct encoding; callers are responsible for ensuring that input data meets the requirements of the
/// selected encoding mode.</remarks>
internal static class QRCodec
{
    /// <summary>
    /// Represents the set of valid alphanumeric characters used for encoding in certain barcode or QR code standards.
    /// </summary>
    /// <remarks>This character set includes digits, uppercase English letters, and a selection of special
    /// characters commonly supported by alphanumeric encoding modes in barcode and QR code specifications.</remarks>
    private const string AlphaTable = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ $%*+-./:";

    /// <summary>
    /// Determines the most appropriate QR encoding mode for the specified data based on its content and the requested
    /// encoding mode.
    /// </summary>
    /// <remarks>If the requested encoding mode is set to auto, the method analyzes the data to determine
    /// whether it can be encoded using the numeric or alphanumeric modes for greater efficiency. If neither is
    /// suitable, the byte mode is selected.</remarks>
    /// <param name="data">The string data to be encoded in the QR code. The content of this string is analyzed to select the encoding mode
    /// if the requested mode is set to auto.</param>
    /// <param name="requested">The preferred encoding mode to use. If set to auto, the method selects the optimal encoding mode based on the
    /// data; otherwise, the specified mode is used.</param>
    /// <returns>A value from the QREncodingMode enumeration indicating the encoding mode to use for the provided data.</returns>
    public static QREncodingMode ResolveEncodingMode(string data, QREncodingMode requested)
    {
        if (requested != QREncodingMode.Auto)
        {
            return requested;
        }

        var numeric = true;

        foreach (var ch in data)
        {
            if (ch < '0' || ch > '9')
            {
                numeric = false;
                break;
            }
        }

        if (numeric)
        {
            return QREncodingMode.Numeric;
        }

        var alphanumeric = true;

        foreach (var ch in data)
        {
            if (!AlphaTable.ToUpperInvariant().Contains(ch))
            {
                alphanumeric = false;
                break;
            }
        }

        if (alphanumeric)
        {
            return QREncodingMode.Alphanumeric;
        }

        var kanji = true;

        foreach (var ch in data)
        {
            var bytes = Encoding.GetEncoding("shift_jis").GetBytes([ch]);
            var sjis = (bytes[0] << 8) | bytes[1];

            if (!((sjis >= 0x8140 && sjis <= 0x9FFC) ||
                  (sjis >= 0xE040 && sjis <= 0xEBBF)))
            {
                kanji = false;
                break;
            }
        }

        if (kanji)
        {
            return QREncodingMode.Kanji;
        }

        return QREncodingMode.Byte;
    }

    /// <summary>
    /// Encodes the specified data string into a bit sequence using the given QR encoding mode.
    /// </summary>
    /// <remarks>If the encoding mode is Alphanumeric, the input data is converted to uppercase before
    /// encoding. For unsupported encoding modes, the method defaults to byte encoding.</remarks>
    /// <param name="data">The data string to encode. The content and allowed characters depend on the selected encoding mode.</param>
    /// <param name="mode">The QR encoding mode that determines how the data string is processed and encoded.</param>
    /// <returns>A BitList containing the encoded representation of the input data according to the specified encoding mode.</returns>
    public static BitList EncodeData(string data, QREncodingMode mode)
    {
        return mode switch
        {
            QREncodingMode.Numeric => EncodeNumeric(data),
            QREncodingMode.Alphanumeric => EncodeAlphanumeric(data.ToUpperInvariant()),
            QREncodingMode.Byte => EncodeByte(data),
            QREncodingMode.Kanji => EncodeKanji(data),
            _ => EncodeByte(data)
        };
    }

    /// <summary>
    /// Encodes a string of Kanji characters into a bit sequence using the QR Code Kanji mode encoding.
    /// </summary>
    /// <remarks>This method is intended for use with QR Code generation where Kanji mode is supported. The
    /// input string should only contain characters that can be encoded in the QR Code Kanji mode, which are typically
    /// double-byte characters in the Shift_JIS character set.</remarks>
    /// <param name="data">The string containing Kanji characters to encode. Each character must be representable in Shift_JIS encoding.</param>
    /// <returns>A BitList containing the encoded bit sequence representing the input Kanji characters.</returns>
    private static BitList EncodeKanji(string data)
    {
        var bits = new BitList();
        var sjis = Encoding.GetEncoding("shift_jis");

        foreach (var ch in data)
        {
            var bytes = sjis.GetBytes([ch]);
            var value = (bytes[0] << 8) | bytes[1];

            int adjusted;

            if (value >= 0x8140 && value <= 0x9FFC)
            {
                adjusted = value - 0x8140;
            }
            else
            {
                adjusted = value - 0xC140;
            }

            var encoded = ((adjusted >> 8) * 0xC0) + (adjusted & 0xFF);

            bits.Add(encoded, 13);
        }

        return bits;
    }

    /// <summary>
    /// Encodes the specified string into a sequence of bits using UTF-8 encoding.
    /// </summary>
    /// <remarks>Each character in the input string is encoded as one or more bytes using UTF-8, and each byte
    /// is represented as 8 bits in the resulting BitList.</remarks>
    /// <param name="data">The string to encode as a sequence of bits. Cannot be null.</param>
    /// <returns>A BitList containing the bits representing the UTF-8 encoded bytes of the input string.</returns>
    private static BitList EncodeByte(string data)
    {
        var bits = new BitList();
        var bytes = Encoding.UTF8.GetBytes(data);

        foreach (var b in bytes)
        {
            bits.Add(b, 8);
        }

        return bits;
    }

    /// <summary>
    /// Encodes the specified string into a sequence of bits using the alphanumeric mode encoding as defined by the QR
    /// code specification.
    /// </summary>
    /// <remarks>Characters not present in the supported alphanumeric set will result in an encoding value of
    /// -1, which may cause incorrect output. The alphanumeric mode supports a limited set of characters as defined by
    /// the QR code standard.</remarks>
    /// <param name="data">The input string to encode. Each character must be present in the supported alphanumeric character set.</param>
    /// <returns>A BitList containing the encoded bit sequence representing the input data in alphanumeric mode.</returns>
    private static BitList EncodeAlphanumeric(string data)
    {
        var bits = new BitList();

        var i = 0;

        while (i < data.Length)
        {
            if (i + 1 < data.Length)
            {
                var v1 = AlphaTable.IndexOf(data[i]);
                var v2 = AlphaTable.IndexOf(data[i + 1]);
                var value = v1 * 45 + v2;
                bits.Add(value, 11);
                i += 2;
            }
            else
            {
                var v = AlphaTable.IndexOf(data[i]);
                bits.Add(v, 6);
                i += 1;
            }
        }

        return bits;
    }

    /// <summary>
    /// Encodes a numeric string into a sequence of bits using a compact representation suitable for QR code numeric
    /// mode.
    /// </summary>
    /// <remarks>The input string is processed in groups of up to three digits, with each group encoded into a
    /// fixed number of bits according to QR code numeric mode specifications. The method does not validate that the
    /// input contains only numeric characters; providing non-digit characters may result in incorrect
    /// encoding.</remarks>
    /// <param name="data">The numeric string to encode. Each character must be a digit ('0'-'9').</param>
    /// <returns>A BitList containing the encoded bit sequence representing the numeric input.</returns>
    private static BitList EncodeNumeric(string data)
    {
        var bits = new BitList();

        var i = 0;

        while (i < data.Length)
        {
            var remaining = data.Length - i;

            if (remaining >= 3)
            {
                var value = (data[i] - '0') * 100 +
                            (data[i + 1] - '0') * 10 +
                            (data[i + 2] - '0');
                bits.Add(value, 10);
                i += 3;
            }
            else if (remaining == 2)
            {
                var value = (data[i] - '0') * 10 +
                            (data[i + 1] - '0');
                bits.Add(value, 7);
                i += 2;
            }
            else
            {
                var value = (data[i] - '0');
                bits.Add(value, 4);
                i += 1;
            }
        }

        return bits;
    }

    /// <summary>
    /// Determines the appropriate QR code version that can accommodate the specified data length and error correction
    /// level.
    /// </summary>
    /// <param name="rawBits">The raw bits to encode in the QR code. Must be non-negative.</param>
    /// <param name="requested">The requested QR code version. If set to QRVersion.Auto, the smallest suitable version is selected
    /// automatically.</param>
    /// <param name="ecc">The error correction level to use when determining the QR code version.</param>
    /// <param name="mode">The encoding mode used for the data. Affects the number of bits required for the character count indicator,
    ///  which is part of the overhead.</param>
    /// <returns>A QRVersion value representing the version that can fit the specified data and error correction level.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the requested version cannot accommodate the data with the specified error correction level, or if no
    /// QR code version can fit the data.</exception>
    public static QRVersion ResolveVersion(
        BitList rawBits,
        QRVersion requested,
        QREncodingMode mode,
        QRErrorCorrectionLevel ecc)
    {
        if (requested != QRVersion.Auto)
        {
            if (!CanFit(requested, rawBits, mode, ecc))
            {
                throw new InvalidOperationException($"Data does not fit in QR version {requested} with ECC {ecc}.");
            }

            return requested;
        }

        for (var v = 1; v <= 40; v++)
        {
            var version = (QRVersion)v;

            if (CanFit(version, rawBits, mode, ecc))
            {
                return version;
            }
        }

        throw new InvalidOperationException("Data too large to fit in any QR version (1–40).");
    }

    /// <summary>
    /// Determines whether the specified data length can be encoded in a QR code of the given version and error
    /// correction level.
    /// </summary>
    /// <param name="version">The QR code version to evaluate. Specifies the size and data capacity of the QR code symbol.</param>
    /// <param name="rawBits">The data, in bits, to be encoded in the QR code.</param>
    /// <param name="ecc">The error correction level to use when evaluating capacity. Higher levels provide greater error resilience but
    /// reduce available data capacity.</param>
    /// <param name="mode">The encoding mode used for the data. Affects the number of bits required for the character count indicator,
    ///  which is part of the overhead.</param>
    /// <returns>true if the data length and required overhead fit within the data capacity of the specified QR code version and
    /// error correction level; otherwise, false.</returns>
    private static bool CanFit(
        QRVersion version,
        BitList rawBits,
        QREncodingMode mode,
        QRErrorCorrectionLevel ecc)
    {
        var info = QRVersionInfo.Versions[(int)version - 1];
        var dataCodewords = info.Ecc[ecc].Sum(b => b.BlockCount * b.DataCodewords);
        var capacityBits = dataCodewords * 8;

        // Overhead: mode + count
        var bits = 4;
        bits += CountBits(version, mode);
        bits += rawBits.Length;

        // Terminator (max 4 bits)
        var remaining = capacityBits - bits;
        bits += Math.Min(4, Math.Max(0, remaining));

        // Align to byte
        bits = (bits + 7) & ~7;

        return bits <= capacityBits;
    }

    /// <summary>
    /// Calculates the number of bits required to encode the character count indicator for a given QR code version and
    /// encoding mode.
    /// </summary>
    /// <remarks>The bit count varies according to the QR code specification, depending on both the version
    /// and the encoding mode. This value is essential for correctly formatting QR code data segments.</remarks>
    /// <param name="version">The QR code version for which to determine the bit count. Determines the range used for the calculation.</param>
    /// <param name="mode">The encoding mode specifying the data type being encoded. Affects the number of bits required for the character
    /// count indicator.</param>
    /// <returns>The number of bits needed to represent the character count indicator for the specified version and encoding
    /// mode.</returns>
    private static int CountBits(
        QRVersion version,
        QREncodingMode mode)
    {
        var v = (int)version;

        return mode switch
        {
            QREncodingMode.Numeric => v <= 9 ? 10 : v <= 26 ? 12 : 14,
            QREncodingMode.Alphanumeric => v <= 9 ? 9 : v <= 26 ? 11 : 13,
            QREncodingMode.Byte => v <= 9 ? 8 : 16,
            QREncodingMode.Kanji => v <= 9 ? 8 : v <= 26 ? 10 : 12,
            _ => 16
        };
    }

    /// <summary>
    /// Adds the necessary error correction codewords to the data bits according to the specified QR code version and error.
    /// </summary>
    /// <param name="dataBits">The data bits to which error correction codewords will be added.</param>
    /// <param name="version">The QR code version for which to generate error correction codewords.</param>
    /// <param name="ecc">The error correction level to use when generating codewords. Higher levels produce more codewords for greater error resilience.</param>
    /// <returns>Returns a byte array containing the interleaved data and error correction codewords ready for placement in the QR code matrix.</returns>
    public static byte[] AddErrorCorrection(
        BitList dataBits,
        QRVersion version,
        QRErrorCorrectionLevel ecc)
    {
        var dataCodewords = BitsToCodewords(dataBits);
        var info = QRVersionInfo.Versions[(int)version - 1];
        var eccBlocks = info.Ecc[ecc];
        var blocks = SplitIntoBlocks(dataCodewords, eccBlocks);
        var rsBlocks = new List<byte[]>();
        var blockIndex = 0;

        foreach (var group in eccBlocks)
        {
            for (var i = 0; i < group.BlockCount; i++)
            {
                var eccBytes = ReedSolomon.GenerateECC(blocks[blockIndex], group.EccCodewords);
                rsBlocks.Add(eccBytes);
                blockIndex++;
            }
        }

        return Interleave(blocks, rsBlocks);
    }

    /// <summary>
    /// Converts a sequence of bits to an array of bytes, grouping every eight bits into a single byte.
    /// </summary>
    /// <remarks>Bits are packed in order, with the first bit in the most significant position of the first
    /// byte. If the number of bits is not a multiple of eight, the remaining bits are placed in the least significant
    /// positions of the final byte.</remarks>
    /// <param name="bits">The sequence of bits to convert to codewords. Cannot be null.</param>
    /// <returns>An array of bytes representing the input bits, where each byte contains up to eight bits from the input
    /// sequence. The last byte may contain fewer than eight bits if the total number of bits is not a multiple of
    /// eight.</returns>
    private static byte[] BitsToCodewords(BitList bits)
    {
        var list = new List<byte>();
        var array = bits.ToArray();
        var count = array.Length;

        for (var i = 0; i < count; i += 8)
        {
            var value = 0;

            for (var b = 0; b < 8 && i + b < count; b++)
            {
                value = (value << 1) | (array[i + b] ? 1 : 0);
            }

            list.Add((byte)value);
        }

        return [.. list];
    }

    /// <summary>
    /// Divides the input data into multiple blocks according to the specified error correction block information.
    /// </summary>
    /// <remarks>The number and size of the resulting blocks are determined by the properties of each element
    /// in the eccBlocks array. The method processes the data sequentially, assigning codewords to each block as
    /// specified.</remarks>
    /// <param name="data">The byte array containing the data to be split into blocks.</param>
    /// <param name="eccBlocks">An array of error correction block information that specifies the number and size of each block.</param>
    /// <returns>A list of byte arrays, where each array represents a block of data as defined by the error correction block
    /// information.</returns>
    private static List<byte[]> SplitIntoBlocks(byte[] data, QREccInfo[] eccBlocks)
    {
        var blocks = new List<byte[]>();
        var offset = 0;

        foreach (var block in eccBlocks)
        {
            for (var i = 0; i < block.BlockCount; i++)
            {
                var slice = data.AsSpan(offset, block.DataCodewords).ToArray();
                blocks.Add(slice);
                offset += block.DataCodewords;
            }
        }

        return blocks;
    }

    /// <summary>
    /// Interleaves the bytes from the specified data and error correction code (ECC) blocks into a single byte array.
    /// </summary>
    /// <remarks>The method processes all data blocks first, interleaving their bytes by position, and then
    /// processes all ECC blocks in the same manner. If blocks have different lengths, shorter blocks contribute fewer
    /// bytes to the result.</remarks>
    /// <param name="dataBlocks">The list of byte arrays representing the data blocks to be interleaved. Each array may have a different length.</param>
    /// <param name="eccBlocks">The list of byte arrays representing the error correction code (ECC) blocks to be interleaved. Each array may
    /// have a different length.</param>
    /// <returns>A byte array containing the interleaved bytes from the data and ECC blocks. The data bytes are interleaved
    /// first, followed by the ECC bytes.</returns>
    private static byte[] Interleave(List<byte[]> dataBlocks, List<byte[]> eccBlocks)
    {
        var result = new List<byte>();
        var maxData = dataBlocks.Max(b => b.Length);
        var maxEcc = eccBlocks.Max(b => b.Length);

        // Interleave data
        for (var i = 0; i < maxData; i++)
        {
            foreach (var block in dataBlocks)
            {
                if (i < block.Length)
                {
                    result.Add(block[i]);
                }
            }
        }

        // Interleave ECC
        for (var i = 0; i < maxEcc; i++)
        {
            foreach (var block in eccBlocks)
            {
                if (i < block.Length)
                {
                    result.Add(block[i]);
                }
            }
        }

        return [.. result];
    }

    /// <summary>
    /// Creates and returns a two-dimensional matrix representing the QR code structure for the specified QR version.
    /// </summary>
    /// <param name="version">The QR version for which to build the matrix. Determines the size and structure of the resulting QR code matrix.</param>
    /// <param name="codewords">An optional byte array of codewords to be placed in the QR code matrix. If null, the matrix will be initialized without data placement.</param>
    /// <param name="ecc">The error correction level to apply when building the matrix. This affects the placement of data and error correction codewords, as well as the selection of the best mask pattern.</param>
    /// <returns>A two-dimensional array of nullable Boolean values representing the QR code matrix for the specified version.
    /// Each element indicates the presence or absence of a module; <see langword="null"/> values may represent
    /// uninitialized or reserved areas.</returns>
    public static bool[,] BuildMatrix(
        QRVersion version,
        byte[] codewords,
        QRErrorCorrectionLevel ecc)
    {
        return new QRMatrix(version, codewords, ecc).Matrix;
    }

    /// <summary>
    /// Calculates the total number of data bits available for encoding in a QR code for the specified version and error
    /// correction level.
    /// </summary>
    /// <remarks>The returned value represents the total data capacity in bits, excluding bits used for error
    /// correction. Use this value to determine the maximum amount of data that can be encoded in a QR code with the
    /// given parameters.</remarks>
    /// <param name="version">The QR code version to evaluate. Determines the size and data capacity of the QR code.</param>
    /// <param name="ecc">The error correction level to use. Affects the amount of space reserved for error correction and, consequently,
    /// the available data capacity.</param>
    /// <returns>The number of data bits available for encoding in the specified QR code version and error correction level.</returns>
    private static int GetDataCapacityBits(QRVersion version, QRErrorCorrectionLevel ecc)
    {
        var info = QRVersionInfo.Versions[(int)version - 1];
        var dataCodewords = 0;

        foreach (var block in info.Ecc[ecc])
        {
            dataCodewords += block.BlockCount * block.DataCodewords;
        }

        return dataCodewords * 8;
    }

    /// <summary>
    /// Returns the bit pattern that represents the specified QR encoding mode.
    /// </summary>
    /// <remarks>The returned bit pattern is used in QR code generation to indicate the encoding mode in the
    /// QR symbol's data stream.</remarks>
    /// <param name="mode">The QR encoding mode for which to retrieve the corresponding bit pattern.</param>
    /// <returns>An integer containing the bit pattern associated with the specified encoding mode.</returns>
    private static int GetModeBits(QREncodingMode mode)
    {
        return mode switch
        {
            QREncodingMode.Numeric => 0b0001,
            QREncodingMode.Alphanumeric => 0b0010,
            QREncodingMode.Byte => 0b0100,
            QREncodingMode.Kanji => 0b1000,
            _ => 0b0100
        };
    }

    /// <summary>
    ///Build the final binary stream for a QR Code symbol, adding mode indicators, the character count, data bits,
    /// the terminator, byte alignment, and padding according to the QR Code specification.
    /// </summary>
    /// <remarks>The generated binary stream complies with the data capacity, byte alignment, and padding requirements defined
    ///  by the QR Code standard. This method should be used when preparing data for encoding a QR Code symbol
    ///  of the specified version and error correction level.</remarks>
    /// <param name="dataBits">The bit sequence representing the data to be encoded in the QR Code.</param>
    /// <param name="charCount">The number of data characters to encode, used to generate the appropriate character count field.</param>
    /// <param name="version">The version of the QR Code, which determines the capacity and structure of the symbol.</param>
    /// <param name="mode">The QR Code encoding mode used for the data (for example, numeric, alphanumeric, binary).</param>
    /// <param name="ecc">The level of error correction to be applied to the QR Code.</param>
    /// <returns>An instance of BitList containing the final binary stream ready to be used for QR Code generation.</returns>
    public static BitList BuildFinalBitStream(
        BitList dataBits,
        int charCount,
        QRVersion version,
        QREncodingMode mode,
        QRErrorCorrectionLevel ecc)
    {
        var capacityBits = GetDataCapacityBits(version, ecc);
        var bits = new BitList();

        // 1. Mode indicator (4 bits)
        bits.Add(GetModeBits(mode), 4);

        // 2. Character count
        var countBits = CountBits(version, mode);
        bits.Add(charCount, countBits);

        // 3. Data bits
        foreach (var b in dataBits.ToArray())
        {
            bits.Add(b);
        }

        // 4. Terminator
        var remaining = capacityBits - bits.Length;
        var terminator = Math.Min(4, remaining);

        if (terminator > 0)
        {
            bits.Add(0, terminator);
        }

        // 5. Align to byte boundary
        while (bits.Length % 8 != 0)
        {
            bits.Add(false);
        }

        // 6. Padding 0xEC / 0x11
        var padBytes = new[] { 0xEC, 0x11 };
        var padIndex = 0;

        while (bits.Length < capacityBits)
        {
            bits.Add(padBytes[padIndex], 8);
            padIndex ^= 1;
        }

        return bits;
    }
}
