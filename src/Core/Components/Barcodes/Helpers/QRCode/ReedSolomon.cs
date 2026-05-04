namespace FluentUI.Blazor.Community.Components.Helpers;

/// <summary>
/// Provides static methods and data for Reed-Solomon error correction code generation over GF(256).
/// </summary>
/// <remarks>This class is intended for internal use in QR code and similar applications that require Reed-Solomon
/// encoding. It includes precomputed generator polynomials for common QR code error correction levels and exposes
/// methods to generate error correction codewords for a given data sequence.</remarks>
internal static class ReedSolomon
{
    private const int Primitive = 0x11D;

    /// <summary>
    /// Performs finite field multiplication of two bytes using a fixed primitive polynomial.
    /// </summary>
    /// <remarks>This method implements multiplication in a Galois field, commonly used in cryptographic and
    /// error-correcting algorithms. The result may differ from standard integer multiplication due to the use of a
    /// primitive polynomial.</remarks>
    /// <param name="x">The first operand to multiply in the finite field operation.</param>
    /// <param name="y">The second operand to multiply in the finite field operation.</param>
    /// <returns>A byte representing the product of the two operands in the finite field.</returns>
    private static byte Multiply(byte x, byte y)
    {
        var z = 0;

        for (var i = 7; i >= 0; i--)
        {
            z = (z << 1) ^ (((z >> 7) & 1) * Primitive);

            if (((y >> i) & 1) != 0)
            {
                z ^= x;
            }
        }

        return (byte)z;
    }

    /// <summary>
    /// Generates the polynomial divisor coefficients for a given degree, typically used in error correction algorithms
    /// such as Reed-Solomon coding.
    /// </summary>
    /// <remarks>The returned coefficients are ordered from the highest to the lowest degree. This method is
    /// commonly used in the context of generating error correction codes for data integrity.</remarks>
    /// <param name="degree">The degree of the polynomial divisor to compute. Must be a positive integer greater than zero.</param>
    /// <returns>A byte array containing the coefficients of the computed polynomial divisor. The array length equals the
    /// specified degree.</returns>
    private static byte[] ComputeDivisor(int degree)
    {
        var result = new byte[degree];
        result[degree - 1] = 1;

        byte root = 1;

        for (var i = 0; i < degree; i++)
        {
            for (var j = 0; j < result.Length; j++)
            {
                result[j] = Multiply(result[j], root);

                if (j + 1 < result.Length)
                {
                    result[j] ^= result[j + 1];
                }
            }

            root = Multiply(root, 0x02);
        }

        return result;
    }

    /// <summary>
    /// Calculates the remainder of a polynomial division over a finite field using the provided data and divisor
    /// arrays.
    /// </summary>
    /// <remarks>This method is commonly used in error-detecting codes such as CRC or Reed-Solomon encoding,
    /// where polynomial division is performed over a finite field. The input arrays must not be null.</remarks>
    /// <param name="data">The input byte array representing the dividend polynomial coefficients, ordered from highest to lowest degree.</param>
    /// <param name="divisor">The byte array representing the divisor polynomial coefficients, ordered from highest to lowest degree. The
    /// length of this array determines the length of the returned remainder.</param>
    /// <returns>A byte array containing the remainder coefficients resulting from the polynomial division. The length of the
    /// array matches the length of the divisor.</returns>
    private static byte[] ComputeRemainder(byte[] data, byte[] divisor)
    {
        var result = new byte[divisor.Length];

        foreach (var b in data)
        {
            var factor = (byte)(b ^ result[0]);

            // Shift left
            Array.Copy(result, 1, result, 0, result.Length - 1);
            result[^1] = 0;

            if (factor != 0)
            {
                for (var i = 0; i < result.Length; i++)
                {
                    result[i] ^= Multiply(divisor[i], factor);
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Generates error correction code (ECC) bytes for the specified data using the given ECC length.
    /// </summary>
    /// <param name="data">The input data for which to generate error correction codes. Cannot be null.</param>
    /// <param name="eccCount">The number of ECC bytes to generate. Must be between 1 and 255, inclusive.</param>
    /// <returns>A byte array containing the generated ECC bytes for the input data.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="data"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="eccCount"/> is less than 1 or greater than 255.</exception>
    public static byte[] GenerateECC(byte[] data, int eccCount)
    {
        ArgumentNullException.ThrowIfNull(data);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(eccCount, 255);
        ArgumentOutOfRangeException.ThrowIfLessThan(eccCount, 1);

        var divisor = ComputeDivisor(eccCount);

        return ComputeRemainder(data, divisor);
    }
}
