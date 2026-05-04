using System.Security.Cryptography;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to build, sign, and optionally encrypt proof documents from a payload and user data,
/// supporting various signature conformity modes.
/// </summary>
/// <remarks>This builder coordinates the rendering, document creation, signing, and encryption processes required
/// to produce a finalized proof document. It supports different signature conformity levels, such as AES and QES, and
/// enforces platform-specific restrictions for signing and encryption. Use this class when you need to generate a
/// secure, verifiable document from structured payload and user data, with optional cryptographic
/// protections.</remarks>
/// <typeparam name="TPayload">The type of the payload data to be included in the proof document.</typeparam>
/// <typeparam name="TUserData">The type of the user-specific data to be embedded in the proof document.</typeparam>
public sealed class ProofDocumentBuilder<TPayload, TUserData>
{
    /// <summary>
    /// Gets the default instance of the proof document builder configured with standard signing and encryption
    /// implementations.
    /// </summary>
    /// <remarks>This property provides a preconfigured builder using the default proof document signer and
    /// AES-based encrypter. It is intended for typical usage scenarios where custom signing or encryption is not
    /// required. Not supported on browser platforms.</remarks>
    public static ProofDocumentBuilder<TPayload, TUserData> Default { get; } = new(
        new DefaultProofDocumentSigner(),
        new DefaultProofDocumentEncrypter());

    /// <summary>
    /// Provides access to the proof document signer used for signing operations.
    /// </summary>
    private readonly IProofDocumentSigner _signer;

    /// <summary>
    /// Provides encryption services for proof documents.
    /// </summary>
    private readonly IProofDocumentEncrypter _encrypter;

    /// <summary>
    /// Initialize a new instance of the <see cref="ProofDocumentBuilder{TPayload, TUserData}" /> class with the specified signer and encrypter.
    /// </summary>
    /// <param name="signer">Provides the signing functionality to create digital signatures for the proof document. Must be compatible with the required signature conformity levels.</param>
    /// <param name="encrypter">Provides the encryption functionality to secure the proof document when AES conformity is required. Must be compatible with the required signature conformity levels.</param>
    public ProofDocumentBuilder(
        IProofDocumentSigner signer,
        IProofDocumentEncrypter encrypter)
    {
        _signer = signer;
        _encrypter = encrypter;
    }

    /// <summary>
    /// Builds a signed and optionally encrypted binary file from the provided image, document, payload, and user data
    /// according to the specified signature conformity.
    /// </summary>
    /// <remarks>If the conformity mode is set to AES and an encryption key is provided, the resulting file is
    /// encrypted using AES. If the conformity mode is QES, the file is signed with a qualified electronic signature.
    /// The method performs all necessary hashing, signing, and encryption steps based on the specified
    /// conformity.</remarks>
    /// <param name="imageBytes">The image data as a byte array to include in the binary file.</param>
    /// <param name="documentBytes">The document data as a byte array to include in the binary file.</param>
    /// <param name="payloadBytes">The payload data as a byte array to include in the binary file.</param>
    /// <param name="userData">The user-specific data to embed in the binary file.</param>
    /// <param name="conformity">The signature conformity mode that determines the signing and encryption behavior.</param>
    /// <param name="encryptionKey">An optional encryption key used to encrypt the file when the conformity mode requires AES encryption. If null,
    /// the file is not encrypted.</param>
    /// <returns>A byte array containing the constructed binary file, signed and encrypted as specified by the conformity mode.</returns>
    public async ValueTask<byte[]> BuildAsync(
        byte[] imageBytes,
        byte[] documentBytes,
        byte[] payloadBytes,
        TUserData userData,
        SignatureConformity conformity,
        ReadOnlyMemory<byte>? encryptionKey)
    {
        ValidateEnvironment(conformity);

        var payloadHash = Sha256(payloadBytes);
        var documentHash = Sha256(documentBytes);
        var imageHash = Sha256(imageBytes);

        var raw = await FcxSurfBinaryFileWriter<TUserData>.BuildAsync(
            imageBytes,
            documentBytes,
            payloadHash,
            documentHash,
            imageHash,
            userData,
            conformity == SignatureConformity.QES);

        // 5. Signature
        byte[]? signature = null;

        if (conformity is SignatureConformity.AES or SignatureConformity.QES)
        {
            signature = await _signer.SignHashAsync(raw.GlobalHash);
        }

        // 6. Build signed (still unencrypted)
        var signed = await FcxSurfBinaryFileWriter<TUserData>.BuildAsync(
            imageBytes,
            documentBytes,
            payloadHash,
            documentHash,
            imageHash,
            userData,
            conformity == SignatureConformity.QES,
            raw.GlobalHash,
            signature);

        // 7. AES encryption
        if (conformity == SignatureConformity.AES && encryptionKey.HasValue)
        {
            // Encrypt signed file
            var encrypted = await _encrypter.EncryptAsync(signed.FileBytes, encryptionKey.Value);

            // encrypted = [IV | CIPHERTEXT | HMAC]
            var iv = encrypted.AsSpan(0, 16).ToArray();
            var hmac = encrypted.AsSpan(encrypted.Length - 32, 32).ToArray();
            var cipherText = encrypted.AsSpan(16, encrypted.Length - 48).ToArray();

            // Wrap into FCXSIGN AES format
            var wrapped = FcxSurfBinaryFileWriter<TUserData>.WrapEncrypted(
                iv,
                cipherText,
                hmac,
                signature != null,
                conformity == SignatureConformity.QES);

            return wrapped.FileBytes;
        }

        return signed.FileBytes;
    }

    /// <summary>
    /// Computes the SHA-256 hash value for the specified byte array.
    /// </summary>
    /// <param name="data">The input data to compute the hash for. Cannot be null.</param>
    /// <returns>A byte array containing the SHA-256 hash of the input data.</returns>
    private static byte[] Sha256(byte[] data)
    {
        return SHA256.HashData(data);
    }

    /// <summary>
    /// Validates that the current environment and configuration are compatible with the specified signature conformity
    /// requirements.
    /// </summary>
    /// <remarks>This method ensures that only supported combinations of signers and encrypters are used based
    /// on the execution environment and the required signature conformity. It should be called before performing
    /// operations that depend on these configurations.</remarks>
    /// <param name="conformity">The signature conformity level to validate against. Determines which environment and configuration checks are
    /// performed.</param>
    /// <exception cref="PlatformNotSupportedException">Thrown if the current environment is a browser and an unsupported signer or encrypter is configured.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the specified signature conformity is QES and an unsupported signer or encrypter is configured.</exception>
    private void ValidateEnvironment(SignatureConformity conformity)
    {
        if (System.OperatingSystem.IsBrowser())
        {
            if (_signer is DefaultProofDocumentSigner)
            {
                throw new PlatformNotSupportedException("Default signer is not supported in browser.");
            }

            if (_encrypter is DefaultProofDocumentEncrypter)
            {
                throw new PlatformNotSupportedException("AES encryption is not supported in browser.");
            }
        }

        if (conformity == SignatureConformity.QES)
        {
            if (_encrypter is DefaultProofDocumentEncrypter)
            {
                throw new InvalidOperationException("AES encryption cannot be used in QES mode.");
            }

            if (_signer is DefaultProofDocumentSigner)
            {
                throw new InvalidOperationException("Default signer cannot be used in QES mode.");
            }
        }
    }
}
