using System.Runtime.Versioning;
using System.Security.Cryptography;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a default implementation for signing and verifying document hashes using the NIST P-256 elliptic curve
/// digital signature algorithm (ECDSA).
/// </summary>
/// <remarks>This class is not supported on browser platforms. It implements the IProofDocumentSigner interface
/// and manages its own ECDSA instance for cryptographic operations. Use this class to generate and verify digital
/// signatures for document hashes in environments where ECDSA is supported.</remarks>
[UnsupportedOSPlatform("browser")]
internal sealed class DefaultProofDocumentSigner
    : IProofDocumentSigner
{
    /// <summary>
    /// Represents an ECDSA instance initialized with the NIST P-256 named curve for elliptic curve cryptography
    /// operations.
    /// </summary>
    /// <remarks>The NIST P-256 curve is commonly used for secure digital signatures and key generation. This
    /// field is intended for cryptographic operations requiring ECDSA with a well-known, secure curve.</remarks>
    private readonly ECDsa _ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256);

    /// <inheritdoc />
    public ValueTask<byte[]> SignHashAsync(ReadOnlyMemory<byte> hash) => new(_ecdsa.SignHash(hash.ToArray()));

    /// <inheritdoc />
    public ValueTask<bool> VerifyHashAsync(
        ReadOnlyMemory<byte> hash,
        ReadOnlyMemory<byte> signature) => new(_ecdsa.VerifyHash(hash.ToArray(), signature.ToArray()));

    /// <inheritdoc />
    public void Dispose() => _ecdsa.Dispose();
}
