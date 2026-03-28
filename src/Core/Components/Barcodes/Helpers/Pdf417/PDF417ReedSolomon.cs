using FluentUI.Blazor.Community.Components.Components.Barcodes.Symbologies.Tables;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Helpers.Pdf417;

/// <summary>
/// Provides methods for generating Reed-Solomon error correction codewords for PDF417 barcodes.
/// </summary>
/// <remarks>This class supports the creation of error correction codewords as specified by the PDF417 barcode
/// standard. It is intended for internal use within PDF417 encoding workflows and is not designed for direct use by
/// application developers.</remarks>
internal static class Pdf417ReedSolomon
{
    /// <summary>
    /// Contains the supported word counts for error correction code (ECC) levels.
    /// </summary>
    /// <remarks>Each value in the array represents the number of ECC words available for a corresponding ECC
    /// level. These values are typically used to determine the amount of error correction data to allocate or process
    /// for different levels of data integrity.</remarks>
    private static readonly int[] EccWordCount =
    [
        2, 4, 8, 16, 32, 64, 128, 256, 512
    ];

    /// <summary>
    /// Represents the modulus value used for calculations within the class.
    /// </summary>
    /// <remarks>This constant is typically used in modular arithmetic operations. The specific value of 929
    /// may be relevant to the algorithm or encoding scheme implemented by the class.</remarks>
    private const int MOD = 929;

    /// <summary>
    /// Gets the number of error correction codewords associated with the specified PDF417 error correction level.
    /// </summary>
    /// <param name="level">The error correction level for which to retrieve the number of error correction codewords.</param>
    /// <returns>The number of error correction codewords for the specified error correction level.</returns>
    public static int GetECCWordCount(PDF417ErrorCorrectionLevel level) => EccWordCount[(int)level];

    /// <summary>
    /// Generates error correction codewords for the specified data using the PDF417 error correction algorithm.
    /// </summary>
    /// <remarks>The generated error correction codewords should be appended to the original data codewords in
    /// reverse order when constructing the final codeword sequence for PDF417 barcodes.</remarks>
    /// <param name="data">The input data codewords for which error correction codewords are to be generated.</param>
    /// <param name="eccCount">The number of error correction codewords to generate. Must be a positive integer.</param>
    /// <param name="level">The error correction level to use, which determines the strength of the error correction. Must correspond to a
    /// valid entry in the error correction tables.</param>
    /// <returns>An array of integers containing the generated error correction codewords. The length of the array is equal to
    /// eccCount.</returns>
    public static int[] GenerateECC(
        int[] data,
        int eccCount,
        int level)
    {
        var table = Pdf417ErrorCorrectionTables.ErrorCorrectionTables[level];
        var ecc = new int[eccCount];
        var last = eccCount - 1;
        var dataLen = data[0];

        for (var i = 0; i < dataLen; i++)
        {
            var temp = (data[i] + ecc[last]) % MOD;

            for (var j = last; j > 0; j--)
            {
                ecc[j] = (MOD + ecc[j - 1] - temp * table[j]) % MOD;
            }

            ecc[0] = (MOD - temp * table[0]) % MOD;
        }

        for (var j = last; j >= 0; j--)
        {
            ecc[j] = (MOD - ecc[j]) % MOD;
        }

        return ecc;
    }
}
