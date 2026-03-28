using System.Security.Cryptography;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to validate signed content using hash and digital signature verification, with optional
/// Qualified Electronic Signature (QES) validation.
/// </summary>
/// <remarks>This validator checks the integrity of image, document, and global hashes, verifies the presence and
/// correctness of a digital signature, and optionally performs QES validation if configured. It is designed for
/// scenarios where multiple layers of signature and hash validation are required, such as legal or compliance-sensitive
/// document workflows.</remarks>
/// <typeparam name="TUserData">The type of user data associated with the signature proof payload. This allows the validator to operate on custom
/// user data embedded in the signing process.</typeparam>
public sealed class FcxSurfValidator<TUserData>
{
    /// <summary>
    /// Represents the signer used to apply proof signatures to documents.
    /// </summary>
    private readonly IProofDocumentSigner _signer;

    /// <summary>
    /// Represents a delegate that validates a signature proof payload asynchronously.
    /// </summary>
    /// <remarks>The delegate should return a <see langword="true"/> value if the payload is valid; otherwise,
    /// <see langword="false"/>. If the value is <see langword="null"/>, no validation will be performed.</remarks>
    private readonly Func<SurfaceProofPayload<TUserData>, ValueTask<bool>>? _qesValidator;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="signer"></param>
    /// <param name="qesValidator"></param>
    public FcxSurfValidator(
        IProofDocumentSigner signer,
        Func<SurfaceProofPayload<TUserData>, ValueTask<bool>>? qesValidator = null)
    {
        _signer = signer;
        _qesValidator = qesValidator;
    }

    /// <summary>
    /// Asynchronously validates the provided signed content by verifying hashes, signature, and optional QES
    /// requirements.
    /// </summary>
    /// <remarks>Validation includes checking image, document, and global hashes, signature presence and
    /// correctness, and QES validation if applicable. The method returns <see langword="false"/> if any check
    /// fails.</remarks>
    /// <param name="content">The signed content to validate, including image, document, global hash, and proof information. Cannot be null.</param>
    /// <returns>A value task that represents the asynchronous validation operation. The result is <see langword="true"/> if the
    /// content is valid; otherwise, <see langword="false"/>.</returns>
    public async ValueTask<bool> ValidateAsync(FcxSurfContent<TUserData> content)
    {
        var proof = content.Proof;

        if (!SHA256.HashData(content.ImageBytes).SequenceEqual(proof.ImageHash))
        {
            return false;
        }

        if (!SHA256.HashData(content.DocumentBytes).SequenceEqual(proof.DocumentHash))
        {
            return false;
        }

        if (!content.GlobalHash.SequenceEqual(proof.GlobalHash))
        {
            return false;
        }

        var hasSignature = proof.DigitalSignature is { Length: > 0 };

        if (content.IsSigned && !hasSignature)
        {
            return false;
        }

        if (!content.IsSigned && hasSignature)
        {
            return false;
        }

        if (hasSignature)
        {
            var ok = await _signer.VerifyHashAsync(
                proof.GlobalHash,
                proof.DigitalSignature!);

            if (!ok)
            {
                return false;
            }
        }

        if (content.IsQes && _qesValidator is not null)
        {
            var ok = await _qesValidator(proof);

            if (!ok)
            {
                return false;
            }
        }

        return true;
    }
}
