using System.Globalization;
using System.Numerics;
using System.Text;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Helpers.Pdf417;

/// <summary>
/// Provides core encoding operations for generating PDF417 barcode codewords, including data encoding, error correction
/// computation, and final codeword assembly.
/// </summary>
/// <remarks>This class contains static methods used internally to process input data into the codeword sequences
/// required for PDF417 barcode generation. It handles segmentation of input data, mode switching, and integration of
/// error correction codewords. The methods are intended for use within the PDF417 encoding workflow and are not
/// designed for direct use by external consumers.</remarks>
internal static class Pdf417EncoderCore
{
    /// <summary>
    /// Provides a cached reference to the invariant culture, which is culture-insensitive and associated with the
    /// English language but not with any country or region.
    /// </summary>
    /// <remarks>The invariant culture is used in operations that require culture-independent results, such as
    /// formatting and parsing operations that must behave consistently regardless of the user's locale.</remarks>
    private static readonly CultureInfo s_invariantCulture = CultureInfo.InvariantCulture;

    /// <summary>
    /// Specifies the available subcategories for classification or processing operations.
    /// </summary>
    /// <remarks>Use this enumeration to indicate the type of subcategory being referenced, such as
    /// alphabetic, lowercase, mixed case, or punctuation. The specific meaning of each value depends on the context in
    /// which the enumeration is used.</remarks>
    private enum Sub { Alpha, Lower, Mixed, Punct }

    /// <summary>
    /// Provides a mapping of supported mixed-mode characters to their corresponding integer values.
    /// </summary>
    /// <remarks>This dictionary is typically used for encoding or decoding operations where each character in
    /// the mixed-mode set is associated with a unique integer value. The mapping includes digits, special characters,
    /// and control characters such as carriage return and tab.</remarks>
    private static readonly Dictionary<char, int> MixedMap = new()
    {
        ['0'] = 0,
        ['1'] = 1,
        ['2'] = 2,
        ['3'] = 3,
        ['4'] = 4,
        ['5'] = 5,
        ['6'] = 6,
        ['7'] = 7,
        ['8'] = 8,
        ['9'] = 9,
        ['&'] = 10,
        ['\r'] = 11,
        ['\t'] = 12,
        [','] = 13,
        [':'] = 14,
        ['#'] = 15,
        ['-'] = 16,
        ['.'] = 17,
        ['$'] = 18,
        ['/'] = 19,
        ['+'] = 20,
        ['%'] = 21,
        ['*'] = 22,
        ['='] = 23,
        ['^'] = 24
    };

    /// <summary>
    /// Provides a mapping of punctuation characters to their corresponding integer codes.
    /// </summary>
    /// <remarks>This dictionary is typically used to efficiently look up integer values associated with
    /// specific punctuation characters. The mapping can be useful for parsing, tokenization, or encoding scenarios
    /// where punctuation needs to be identified or categorized.</remarks>
    private static readonly Dictionary<char, int> PunctMap = new()
    {
        [';'] = 0,
        ['<'] = 1,
        ['>'] = 2,
        ['@'] = 3,
        ['['] = 4,
        ['\\'] = 5,
        [']'] = 6,
        ['_'] = 7,
        ['`'] = 8,
        ['~'] = 9,
        ['!'] = 10,
        [','] = 13,
        [':'] = 14,
        ['\n'] = 15,
        ['-'] = 16,
        ['.'] = 17,
        ['$'] = 18,
        ['/'] = 19,
        ['"'] = 20,
        ['|'] = 21,
        ['*'] = 22,
        ['('] = 23,
        [')'] = 24,
        ['?'] = 25,
        ['{'] = 26,
        ['}'] = 27,
        ['\''] = 28
    };

    /// <summary>
    /// Encodes the specified input string into a sequence of PDF417 codewords using appropriate encoding modes.
    /// </summary>
    /// <remarks>The encoding process automatically selects between numeric and byte modes based on the
    /// content of the input string. The resulting codewords can be used for generating a PDF417 barcode.</remarks>
    /// <param name="data">The input string to encode into PDF417 codewords. Cannot be null.</param>
    /// <returns>A read-only list of integers representing the encoded PDF417 codewords for the input data.</returns>
    public static IReadOnlyList<int> EncodeData(string data)
    {
        var codewords = new List<int>
        {
            0
        };

        var segments = CreateSegments(data);
        var first = true;

        Pdf417EncodingMode? currentMode = null;

        foreach (var segment in segments)
        {
            if (segment.Mode != currentMode)
            {
                if (!(first && segment.Mode == Pdf417EncodingMode.Text))
                {
                    switch (segment.Mode)
                    {
                        case Pdf417EncodingMode.Text:
                            codewords.Add(900);
                            break;

                        case Pdf417EncodingMode.Numeric:
                            codewords.Add(902);
                            break;

                        case Pdf417EncodingMode.Byte:
                            codewords.Add(901);
                            break;
                    }

                    currentMode = segment.Mode;
                }
            }

            first = false;

            switch (segment.Mode)
            {
                case Pdf417EncodingMode.Numeric:
                    EncodeNumeric(segment.Content, codewords);
                    break;

                case Pdf417EncodingMode.Byte:
                    EncodeByte(segment.Content, codewords);
                    break;

                case Pdf417EncodingMode.Text:
                    EncodeText(segment.Content, codewords);
                    break;
            }
        }

        return codewords;
    }

    /// <summary>
    /// Builds the full array of PDF417 codewords by combining the data codewords, padding codewords, and error correction codewords
    ///  into a single sequence suitable for matrix generation.
    /// </summary>
    /// <param name="data">The codewords representing the encoded data to be included in the PDF417 barcode. Cannot be null.</param>
    /// <param name="rows">The number of rows in the PDF417 barcode matrix.</param>
    /// <param name="columns">The number of columns in the PDF417 barcode matrix.</param>
    /// <param name="level">The error correction level to use when generating error correction codewords.</param>
    /// <returns>Returns an array of integers representing the complete sequence of PDF417 codewords, including data, padding, and error correction codewords, ready for matrix generation.</returns>
    public static int[] BuildFullCodewordArray(
        IReadOnlyList<int> data,
        int rows,
        int columns,
        PDF417ErrorCorrectionLevel level)
    {
        var total = rows * columns;
        var full = new int[total];

        for (var i = 0; i < data.Count; i++)
        {
            full[i] = data[i];
        }

        var eccCount = Pdf417ReedSolomon.GetECCWordCount(level);
        var padding = total - data.Count - eccCount;

        for (var i = 0; i < padding; i++)
        {
            full[data.Count + i] = 900;
        }

        var dataLength = data.Count + padding;
        full[0] = dataLength;

        var ecc = Pdf417ReedSolomon.GenerateECC(full, eccCount, (int)level);

        for (var i = 0; i < eccCount; i++)
        {
            full[dataLength + i] = ecc[eccCount - 1 - i];
        }

        return full;
    }

    /// <summary>
    /// Divides the input string into a sequence of segments optimized for PDF417 encoding modes.
    /// </summary>
    /// <remarks>Numeric sequences of 13 or more consecutive digits are segmented using the Numeric mode; all
    /// other sequences are segmented using the Byte mode. This segmentation optimizes encoding efficiency for PDF417
    /// barcodes.</remarks>
    /// <param name="data">The input string to be segmented for PDF417 encoding. Cannot be null.</param>
    /// <returns>A list of Pdf417Segment objects representing the segmented portions of the input string, each assigned an
    /// appropriate encoding mode.</returns>
    private static List<Pdf417Segment> CreateSegments(string data)
    {
        var segments = new List<Pdf417Segment>();
        var i = 0;

        while (i < data.Length)
        {
            if (char.IsDigit(data[i]))
            {
                var start = i;
                while (i < data.Length && char.IsDigit(data[i]))
                {
                    i++;
                }

                var len = i - start;
                var block = data.Substring(start, len);

                if (len >= 13)
                {
                    segments.Add(new Pdf417Segment(Pdf417EncodingMode.Numeric, block));
                }
                else
                {
                    segments.Add(new Pdf417Segment(Pdf417EncodingMode.Text, block));
                }

                continue;
            }

            var tstart = i;

            while (i < data.Length && !char.IsDigit(data[i]))
            {
                i++;
            }

            var chunk = data[tstart..i];
            SplitTextAndByte(chunk, segments);
        }

        return segments;
    }

    /// <summary>
    /// Splits the specified input string into segments of text-encodable and byte-encoded data for PDF417 encoding.
    /// </summary>
    /// <remarks>This method analyzes the input string and determines, for each contiguous sequence of
    /// characters, whether it can be encoded as text or must be encoded as bytes. It then adds the corresponding
    /// segments to the provided list. The method does not clear the list before adding new segments.</remarks>
    /// <param name="chunk">The input string to be analyzed and split into text and byte segments.</param>
    /// <param name="segments">The list to which the resulting PDF417 segments are added. Must not be null.</param>
    private static void SplitTextAndByte(string chunk, List<Pdf417Segment> segments)
    {
        var i = 0;

        while (i < chunk.Length)
        {
            if (IsTextEncodable(chunk[i]))
            {
                var start = i;

                while (i < chunk.Length && IsTextEncodable(chunk[i]))
                {
                    i++;
                }

                segments.Add(new Pdf417Segment(Pdf417EncodingMode.Text, chunk[start..i]));
            }
            else
            {
                var start = i;

                while (i < chunk.Length && !IsTextEncodable(chunk[i]) && !char.IsDigit(chunk[i]))
                {
                    i++;
                }

                segments.Add(new Pdf417Segment(Pdf417EncodingMode.Byte, chunk[start..i]));
            }
        }
    }

    /// <summary>
    /// Determines whether the specified character can be represented as plain text without requiring special encoding.
    /// </summary>
    /// <remarks>This method considers letters, digits, whitespace, and common punctuation as encodable
    /// without special handling. Characters outside these ranges may require encoding to be safely represented in
    /// text.</remarks>
    /// <param name="c">The character to evaluate for text encodability.</param>
    /// <returns>true if the character can be represented as plain text; otherwise, false.</returns>
    private static bool IsTextEncodable(char c)
    {
        if (c == ' ')
        {
            return true;
        }

        if (c >= 'A' && c <= 'Z')
        {
            return true;
        }

        if (c >= 'a' && c <= 'z')
        {
            return true;
        }

        if ("0123456789&\r\t,:#-.$/+%*=^".Contains(c))
        {
            return true;
        }

        if (";<>@[\\]_`~!\n|()*?{}'\"".Contains(c))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Encodes the specified string as a sequence of UTF-8 bytes and appends the resulting byte values to the provided
    /// output list.
    /// </summary>
    /// <param name="content">The string to encode as UTF-8 bytes.</param>
    /// <param name="output">The list to which the encoded byte values are appended.</param>
    private static void EncodeByte(string content, List<int> output)
    {
        var bytes = Encoding.UTF8.GetBytes(content);

        foreach (var b in bytes)
        {
            output.Add(b);
        }
    }

    /// <summary>
    /// Encodes a numeric string into a sequence of codewords using base 900 encoding and appends the result to the
    /// specified output list.
    /// </summary>
    /// <remarks>This method processes the input string in blocks of up to 44 digits, encoding each block as a
    /// series of integers suitable for barcode or similar applications. The output list is modified in place and will
    /// contain additional codewords after the method completes.</remarks>
    /// <param name="content">The numeric string to encode. Must consist only of digit characters ('0'-'9').</param>
    /// <param name="output">The list to which the encoded codewords are appended.</param>
    private static void EncodeNumeric(string content, List<int> output)
    {
        var i = 0;

        while (i < content.Length)
        {
            var len = Math.Min(44, content.Length - i);
            var block = content.Substring(i, len);
            i += len;

            var s = $"1{block}";
            var value = BigInteger.Parse(s, s_invariantCulture);
            var stack = new Stack<int>();

            while (value > 0)
            {
                value = BigInteger.DivRem(value, 900, out var rem);
                stack.Push((int)rem);
            }

            foreach (var cw in stack)
            {
                output.Add(cw);
            }
        }
    }

    /// <summary>
    /// Encodes the specified text content into a sequence of codewords and appends the result to the provided output
    /// list.
    /// </summary>
    /// <remarks>Characters that cannot be directly encoded are handled using a fallback mechanism, which may
    /// result in additional codewords being added to the output. The method processes the input text in pairs of
    /// characters when packing codewords.</remarks>
    /// <param name="content">The text content to encode. Each character in the string is processed and converted into one or more codewords.</param>
    /// <param name="output">The list to which the encoded codewords are appended. The list is modified by this method.</param>
    private static void EncodeText(string content, List<int> output)
    {
        var tmp = new List<int>();
        var mode = Sub.Alpha;

        foreach (var ch in content)
        {
            if (!TryEncodeChar(ch, ref mode, tmp))
            {
                output.Add(901);
                EncodeByte(ch.ToString(), output);
                output.Add(900);
                mode = Sub.Alpha;
            }
        }

        for (var i = 0; i < tmp.Count; i += 2)
        {
            if (i + 1 < tmp.Count)
            {
                output.Add(tmp[i] * 30 + tmp[i + 1]);
            }
            else
            {
                output.Add(tmp[i] * 30 + 29);
            }
        }
    }

    /// <summary>
    /// Attempts to encode a single character into its corresponding code sequence based on the current encoding mode.
    /// </summary>
    /// <remarks>This method updates the encoding mode as needed to encode the specified character. If the
    /// character cannot be encoded using the supported modes or mappings, the method returns false and no changes are
    /// made to the mode or output list for that character.</remarks>
    /// <param name="ch">The character to encode.</param>
    /// <param name="mode">The current encoding mode. This value may be updated to reflect a mode shift required to encode the character.</param>
    /// <param name="tmp">A list to which the encoded integer values for the character are appended.</param>
    /// <returns>true if the character was successfully encoded; otherwise, false.</returns>
    private static bool TryEncodeChar(char ch, ref Sub mode, List<int> tmp)
    {
        if (ch >= 'A' && ch <= 'Z')
        {
            if (mode != Sub.Alpha)
            {
                tmp.Add(mode == Sub.Lower ? 27 : 28);
                mode = Sub.Alpha;
            }

            tmp.Add(ch - 'A');
            return true;
        }

        if (ch == ' ')
        {
            tmp.Add(26);
            return true;
        }

        if (ch >= 'a' && ch <= 'z')
        {
            if (mode != Sub.Lower)
            {
                tmp.Add(mode == Sub.Alpha ? 27 : 28);
                mode = Sub.Lower;
            }

            tmp.Add(ch - 'a');
            return true;
        }

        if (MixedMap.TryGetValue(ch, out var m))
        {
            if (mode != Sub.Mixed)
            {
                tmp.Add(28);
                mode = Sub.Mixed;
            }

            tmp.Add(m);
            return true;
        }

        if (PunctMap.TryGetValue(ch, out var p))
        {
            tmp.Add(29);
            tmp.Add(p);

            return true;
        }

        return false;
    }

    /// <summary>
    /// Determines the appropriate PDF417 error correction level based on the provided data codewords and the specified
    /// error correction level.
    /// </summary>
    /// <remarks>If the errorLevel parameter is set to Auto, the method selects an error correction level
    /// based on the number of data codewords, following PDF417 barcode specification guidelines.</remarks>
    /// <param name="dataCw">A read-only list of integers representing the data codewords to be encoded.</param>
    /// <param name="errorLevel">The desired error correction level. If set to Auto, the level is determined based on the number of data
    /// codewords.</param>
    /// <param name="errorCorrectionLength">An output parameter that will contain the number of error correction codewords corresponding to the selected error correction level.</param>
    /// <returns>A PDF417ErrorCorrectionLevel value representing the selected error correction level for the given data.</returns>
    internal static PDF417ErrorCorrectionLevel GetErrorCorrectionLevel(
        IReadOnlyList<int> dataCw,
        PDF417ErrorCorrectionLevel errorLevel,
        out int errorCorrectionLength)
    {
        if (errorLevel != PDF417ErrorCorrectionLevel.Auto)
        {
            errorCorrectionLength = 1 << (int)errorLevel + 1;
            return errorLevel;
        }

        var count = dataCw.Count;

        if (count <= 40)
        {
            errorCorrectionLength = 1 << (int)PDF417ErrorCorrectionLevel.Level2 + 1;

            return PDF417ErrorCorrectionLevel.Level2;
        }
        else if (count <= 160)
        {
            errorCorrectionLength = 1 << (int)PDF417ErrorCorrectionLevel.Level3 + 1;

            return PDF417ErrorCorrectionLevel.Level3;
        }
        else if (count <= 320)
        {
            errorCorrectionLength = 1 << (int)PDF417ErrorCorrectionLevel.Level4 + 1;

            return PDF417ErrorCorrectionLevel.Level4;
        }
        else if (count <= 863)
        {
            errorCorrectionLength = 1 << (int)PDF417ErrorCorrectionLevel.Level5 + 1;

            return PDF417ErrorCorrectionLevel.Level5;
        }

        errorCorrectionLength = 1 << (int)PDF417ErrorCorrectionLevel.Level6 + 1;
        return PDF417ErrorCorrectionLevel.Level6;
    }
}
