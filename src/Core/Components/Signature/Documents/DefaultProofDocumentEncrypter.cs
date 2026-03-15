using System.Security.Cryptography;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a default implementation of the IProofDocumentEncrypter interface for encrypting and decrypting proof
/// documents using a symmetric key and HMAC-based authentication.
/// </summary>
/// <remarks>This class uses a combination of SHA-256, HMAC-SHA256, and a custom XOR-based stream cipher to
/// provide confidentiality and integrity for proof document data. The encryption and decryption methods require a
/// symmetric key of at least 32 bytes. The implementation ensures that encrypted data is authenticated using an HMAC
/// tag to prevent tampering. Instances of this class are thread-safe and can be reused across multiple
/// operations.</remarks>
public sealed class DefaultProofDocumentEncrypter : IProofDocumentEncrypter
{
    /// <inheritdoc />
    public ValueTask<byte[]> EncryptAsync(ReadOnlyMemory<byte> data, ReadOnlyMemory<byte> key)
    {
        if (key.Length < 32)
        {
            throw new ArgumentOutOfRangeException(nameof(key));
        }

        var encKey = SHA256.HashData(Concat(key, "ENC"u8));
        var macKey = SHA256.HashData(Concat(key, "MAC"u8));

        var iv = new byte[16];
        RandomNumberGenerator.Fill(iv);

        var cipher = XorStream(data.Span, encKey, iv);
        var ivAndCipher = new byte[iv.Length + cipher.Length];

        Buffer.BlockCopy(iv, 0, ivAndCipher, 0, iv.Length);
        Buffer.BlockCopy(cipher, 0, ivAndCipher, iv.Length, cipher.Length);

        using var hmac = new HMACSHA256(macKey);
        var tag = hmac.ComputeHash(ivAndCipher);

        var result = new byte[ivAndCipher.Length + tag.Length];
        Buffer.BlockCopy(ivAndCipher, 0, result, 0, ivAndCipher.Length);
        Buffer.BlockCopy(tag, 0, result, ivAndCipher.Length, tag.Length);

        return ValueTask.FromResult(result);
    }

    /// <inheritdoc />
    public ValueTask<byte[]> DecryptAsync(ReadOnlyMemory<byte> data, ReadOnlyMemory<byte> key)
    {
        if (key.Length < 32)
        {
            throw new ArgumentOutOfRangeException(nameof(key));
        }

        var encKey = SHA256.HashData(Concat(key, "ENC"u8));
        var macKey = SHA256.HashData(Concat(key, "MAC"u8));
        var ivLength = 16;
        var tagLength = 32;

        if (data.Length < ivLength + tagLength)
        {
            throw new CryptographicException("Invalid data.");
        }

        var ivAndCipherLength = data.Length - tagLength;
        var ivAndCipher = data[..ivAndCipherLength].ToArray();
        var tag = data.Slice(ivAndCipherLength, tagLength).ToArray();

        using var hmac = new HMACSHA256(macKey);
        var expected = hmac.ComputeHash(ivAndCipher);

        if (!CryptographicOperations.FixedTimeEquals(tag, expected))
        {
            throw new CryptographicException("HMAC validation failed.");
        }

        var iv = ivAndCipher.AsSpan(0, ivLength).ToArray();
        var cipher = ivAndCipher.AsSpan(ivLength).ToArray();

        var plain = XorStream(cipher, encKey, iv);

        return ValueTask.FromResult(plain);
    }

    /// <summary>
    /// Apply a simple XOR-based stream cipher to the input data using a key and an initialization vector (IV).
    /// </summary>
    /// <param name="data">Data to be encrypted or decrypted.</param>
    /// <param name="key">The key used for encryption or decryption. Must be 32 bytes long for AES-256.</param>
    /// <param name="iv">The initialization vector (IV) used to ensure unique encryption for the same plaintext.</param>
    /// <returns></returns>
    private static byte[] XorStream(ReadOnlySpan<byte> data, byte[] key, byte[] iv)
    {
        var output = new byte[data.Length];
        var counter = 0u;

        for (var i = 0; i < data.Length; i += 32)
        {
            var block = SHA256.HashData(Concat3(key, iv, BitConverter.GetBytes(counter)));
            counter++;

            var len = Math.Min(32, data.Length - i);

            for (var j = 0; j < len; j++)
            {
                output[i + j] = (byte)(data[i + j] ^ block[j]);
            }
        }

        return output;
    }

    /// <summary>
    /// Concatenates three byte arrays into a single array.
    /// </summary>
    /// <param name="key">The first byte array to concatenate. Cannot be null.</param>
    /// <param name="iv">The second byte array to concatenate. Cannot be null.</param>
    /// <param name="counter">The third byte array to concatenate. Cannot be null.</param>
    /// <returns>A new byte array containing the elements of 'a' followed by the elements of 'b' and then 'c', in order.</returns>
    private static byte[] Concat3(byte[] key, byte[] iv, byte[] counter)
    {
        var result = new byte[key.Length + iv.Length + counter.Length];
        Buffer.BlockCopy(key, 0, result, 0, key.Length);
        Buffer.BlockCopy(iv, 0, result, key.Length, iv.Length);
        Buffer.BlockCopy(counter, 0, result, key.Length + iv.Length, counter.Length);
        return result;
    }

    /// <summary>
    /// Concatenates the contents of a read-only memory buffer and a read-only span of bytes into a single byte array.
    /// </summary>
    /// <param name="a">The first sequence of bytes to include in the concatenated result.</param>
    /// <param name="b">The second sequence of bytes to append after the contents of <paramref name="a"/>.</param>
    /// <returns>A new byte array containing the bytes from <paramref name="a"/> followed by the bytes from <paramref name="b"/>.</returns>
    private static byte[] Concat(ReadOnlyMemory<byte> a, ReadOnlySpan<byte> b)
    {
        var result = new byte[a.Length + b.Length];
        a.CopyTo(result);
        b.CopyTo(result.AsSpan(a.Length));

        return result;
    }
}

