namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines methods for asynchronously signing and verifying document hashes using cryptographic signatures.
/// </summary>
/// <remarks>Implementations of this interface provide functionality for generating and validating digital
/// signatures on document hashes. These methods are intended for use in scenarios where proof of document integrity and
/// authenticity is required. The interface does not specify the cryptographic algorithm used; consumers should refer to
/// the implementing type for algorithm details.</remarks>
public interface IProofDocumentSigner
{
    /// <summary>
    /// Asynchronously generates a digital signature for the specified hash value.
    /// </summary>
    /// <param name="hash">A read-only memory buffer containing the hash to be signed. The buffer must contain the complete hash data to be
    /// signed.</param>
    /// <returns>A value task that represents the asynchronous operation. The result contains a byte array with the digital
    /// signature of the provided hash.</returns>
    ValueTask<byte[]> SignHashAsync(ReadOnlyMemory<byte> hash);

    /// <summary>
    /// Asynchronously verifies whether the specified signature is valid for the given hash.
    /// </summary>
    /// <param name="hash">A read-only memory buffer containing the hash value to verify against the signature.</param>
    /// <param name="signature">A read-only memory buffer containing the signature to be validated.</param>
    /// <returns>A value task that represents the asynchronous operation. The result is <see langword="true"/> if the signature
    /// is valid for the hash; otherwise, <see langword="false"/>.</returns>
    ValueTask<bool> VerifyHashAsync(ReadOnlyMemory<byte> hash, ReadOnlyMemory<byte> signature);
}
