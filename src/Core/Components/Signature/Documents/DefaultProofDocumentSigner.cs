using System.Security.Cryptography;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a default implementation for signing and verifying document hashes using the NIST P-256 elliptic curve
/// digital signature algorithm (ECDSA).
/// </summary>
/// <remarks>This class is not supported on browser platforms. It implements the IProofDocumentSigner interface
/// and manages its own ECDSA instance for cryptographic operations. Use this class to generate and verify digital
/// signatures for document hashes in environments where ECDSA is supported.</remarks>
internal sealed class DefaultProofDocumentSigner : IProofDocumentSigner
{
    /// <summary>
    /// Represents the secret key used for signing and verifying document hashes. 
    /// </summary>
    private readonly byte[] _key;

    /// <summary>
    /// Initializes a new instance of the DefaultProofDocumentSigner class using a newly generated cryptographic key.
    /// </summary>
    /// <remarks>This constructor automatically generates a key for signing operations. Use this constructor
    /// when a new, unique key is required for each signer instance.</remarks>
    public DefaultProofDocumentSigner()
    {
        _key = GenerateKey();
    }

    /// <summary>
    /// Asynchronously computes a HMAC-SHA256 signature for the specified hash value using the configured key.
    /// </summary>
    /// <param name="hash">The hash value to sign. This value must not be empty.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the computed HMAC-SHA256 signature
    /// as a byte array.</returns>
    public ValueTask<byte[]> SignHashAsync(ReadOnlyMemory<byte> hash)
    {
        using var hmac = new HMACSHA256(_key);
        var signature = hmac.ComputeHash(hash.ToArray());

        return new ValueTask<byte[]>(signature);
    }

    /// <summary>
    /// Asynchronously verifies that the specified signature matches the provided hash using the current HMAC-SHA256
    /// key.
    /// </summary>
    /// <remarks>This method performs a constant-time comparison to help prevent timing attacks. The operation
    /// is synchronous but returned as a ValueTask for API consistency.</remarks>
    /// <param name="hash">The hash value to verify, as a read-only memory buffer of bytes.</param>
    /// <param name="signature">The expected HMAC-SHA256 signature to compare against, as a read-only memory buffer of bytes.</param>
    /// <returns>A value task that represents the asynchronous operation. The result is <see langword="true"/> if the signature
    /// matches the hash; otherwise, <see langword="false"/>.</returns>
    public ValueTask<bool> VerifyHashAsync(
        ReadOnlyMemory<byte> hash,
        ReadOnlyMemory<byte> signature)
    {
        using var hmac = new HMACSHA256(_key);
        var expected = hmac.ComputeHash(hash.ToArray());
        var ok = CryptographicOperations.FixedTimeEquals(expected, signature.Span);

        return new ValueTask<bool>(ok);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        CryptographicOperations.ZeroMemory(_key);
    }

    /// <summary>
    /// Generates a new 256-bit cryptographic key using a secure random number generator.
    /// </summary>
    /// <remarks>The returned key is suitable for use in cryptographic operations that require a securely
    /// generated random key, such as encryption or signing. Each call produces a different key.</remarks>
    /// <returns>A byte array containing the generated 256-bit key.</returns>
    private static byte[] GenerateKey()
    {
        var key = new byte[32];
        RandomNumberGenerator.Fill(key);

        return key;
    }
}
