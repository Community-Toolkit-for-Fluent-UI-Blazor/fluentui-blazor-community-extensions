namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the conformity level of a digital signature, indicating the standards or requirements that the signature adheres to.
/// </summary>
public enum SignatureConformity
{
    /// <summary>
    /// Represents the first level of the signature conformity.
    /// </summary>
    /// <remarks>
    /// The signature is compliant with the basic requirements for electronic signatures.
    /// </remarks>
    SES,

    /// <summary>
    /// Represents the second level of the signature conformity.
    /// </summary>
    /// <remarks>
    /// The signature is compliant with advanced requirements, including additional security measures and validation processes.
    /// </remarks>
    AES,

    /// <summary>
    /// Represents the third level of the signature conformity.
    /// </summary>
    /// <remarks>
    /// The signature is compliant with qualified requirements, which may include the use of a qualified certificate and a secure signature creation device, ensuring the highest level of security and legal recognition.
    /// To use this level of conformity, you must implement your own <see cref="IProofDocumentSigner"/> service to include the necessary logic for handling qualified signatures, as this level is not natively supported by the default implementation.
    /// </remarks>
    QES
}
