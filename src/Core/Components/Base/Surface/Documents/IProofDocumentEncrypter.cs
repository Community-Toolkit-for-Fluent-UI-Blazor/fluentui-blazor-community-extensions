namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines methods for encrypting and decrypting proof documents using an optional master key.
/// </summary>
/// <remarks>Implementations of this interface provide asynchronous operations for securing document data. The
/// optional master key parameter allows for additional encryption context or security. The interface is intended for
/// use in scenarios where document confidentiality and integrity are required.</remarks>
public interface IProofDocumentEncrypter
{
    /// <summary>
    /// Asynchronously encrypts the specified data using the provided key.
    /// </summary>
    /// <param name="data">A read-only memory buffer containing the data to be encrypted.</param>
    /// <param name="key">An optional read-only memory buffer containing the encryption key.</param>
    /// <returns>A value task that represents the asynchronous operation. The result contains a byte array with the encrypted
    /// data.</returns>
    ValueTask<byte[]> EncryptAsync(ReadOnlyMemory<byte> data, ReadOnlyMemory<byte> key);

    /// <summary>
    /// Decrypts the specified encrypted data asynchronously using the provided key.
    /// </summary>
    /// <param name="data">A read-only memory buffer containing the encrypted data to decrypt.</param>
    /// <param name="key">An optional read-only memory buffer containing the decryption key.</param>
    /// <returns>A value task that represents the asynchronous operation. The result contains a byte array with the decrypted
    /// data.</returns>
    ValueTask<byte[]> DecryptAsync(ReadOnlyMemory<byte> data, ReadOnlyMemory<byte> key);
}
