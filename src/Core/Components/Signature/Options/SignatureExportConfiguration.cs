namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the configuration settings used for exporting a signature, including conformity requirements, user data,
/// encryption, and export strategies.
/// </summary>
/// <remarks>This configuration class allows customization of the signature export process by specifying
/// conformity standards, user data, encryption keys, and custom exporters for images and documents. It is typically
/// used to control how signature data and related artifacts are generated and secured during export
/// operations.</remarks>
public sealed class SignatureExportConfiguration
{
    /// <summary>
    /// Gets or sets the user-defined data associated with this instance.
    /// </summary>
    public object? UserData { get; set; }

    /// <summary>
    /// Gets or sets the name of the file associated with this instance.
    /// </summary>
    public string? FileName { get; set; }

    /// <summary>
    /// Gets or sets the encryption key used for securing sensitive data.
    /// </summary>
    /// <remarks>The encryption key should be provided as a sequence of bytes. If the value is null,
    /// encryption operations may be disabled or handled differently depending on the implementation. Ensure that the
    /// key is kept secure and not exposed in logs or user interfaces.</remarks>
    public ReadOnlyMemory<byte>? EncryptionKey { get; set; }

    /// <summary>
    /// Gets or sets the signer used to apply digital signatures to proof documents.
    /// </summary>
    /// <remarks>Assign an implementation of the IProofDocumentSigner interface to enable document signing
    /// functionality. If this property is null, digital signing operations will be unavailable.</remarks>
    public IProofDocumentSigner? Signer { get; set; }

    /// <summary>
    /// Gets or sets the encrypter used to secure proof documents.
    /// </summary>
    /// <remarks>Assign an implementation of the IProofDocumentEncrypter interface to enable encryption and
    /// decryption of proof documents. If null, encryption operations may be disabled or unavailable.</remarks>
    public IProofDocumentEncrypter? Encrypter { get; set; }

    /// <summary>
    /// Gets or sets the builder used to construct proof documents with the specified payload and user data types.
    /// </summary>
    public ProofDocumentBuilder<StrokeLayerPayload, object>? ProofBuilder { get; set; }
}

