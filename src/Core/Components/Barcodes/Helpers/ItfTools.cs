namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides utility methods for expanding pattern strings used in ITF barcode encoding.
/// </summary>
/// <remarks>This class is intended for internal use in barcode processing scenarios where pattern expansion is
/// required. It is not intended to be used directly by application code.</remarks>
internal static class ItfTools
{
    /// <summary>
    /// Expands a pattern string by replacing each 'N' character with '1' and all other characters with '111'.
    /// </summary>
    /// <remarks>This method is typically used to convert a symbolic barcode pattern into its corresponding
    /// module representation, where 'N' denotes a narrow bar and other characters denote wide bars.</remarks>
    /// <param name="patternNW">The input pattern string to expand. Each 'N' is treated as a narrow module and replaced with '1'; all other
    /// characters are treated as wide modules and replaced with '111'.</param>
    /// <returns>A string representing the expanded pattern, where each character in the input is replaced according to the
    /// expansion rules.</returns>
    public static string Expand(string patternNW)
    {
        var sb = new System.Text.StringBuilder();

        foreach (var c in patternNW)
        {
            if (c == 'N')
            {
                sb.Append('1');
            }
            else
            {
                sb.Append("111");
            }
        }

        return sb.ToString();
    }

    public static string BuildInterleavedPair(int d1, int d2)
    {
        var p1 = ItfTable.Digit[d1];
        var p2 = ItfTable.Digit[d2];

        var sb = new System.Text.StringBuilder();

        for (var i = 0; i < 5; i++)
        {
            // bar (digit 1)
            sb.Append(ItfTools.Expand(p1[i].ToString()));

            // space (digit 2)
            sb.Append(ItfTools.Expand(p2[i].ToString()).Replace('1', '0'));
        }

        return sb.ToString();
    }

    public static int ComputeChecksum(string data13)
    {
        var sum = 0;

        for (var i = 0; i < 13; i++)
        {
            var digit = data13[i] - '0';
            sum += (i % 2 == 0) ? digit * 3 : digit;
        }

        return (10 - (sum % 10)) % 10;
    }
}
