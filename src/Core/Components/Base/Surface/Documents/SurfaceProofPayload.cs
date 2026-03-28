namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a payload containing cryptographic hashes and user data for signature proof operations.
/// </summary>
/// <remarks>This class encapsulates multiple hash values and user-specific data used in digital signature
/// verification scenarios. It is intended for internal use where signature proof and validation are required.</remarks>
/// <typeparam name="TUserData">The type of user data associated with the signature proof payload.</typeparam>
public sealed class SurfaceProofPayload<TUserData>
{
    /// <summary>
    /// Gets or sets the user-defined data associated with this instance.
    /// </summary>
    public TUserData UserData { get; init; } = default!;

    /// <summary>
    /// Gets or sets the hash value of the payload data.
    /// </summary>
    public byte[] PayloadHash { get; init; } = [];

    /// <summary>
    /// Gets the hash value of the document as a byte array.
    /// </summary>
    public byte[] DocumentHash { get; init; } = [];

    /// <summary>
    /// Gets the hash value of the image as a byte array.
    /// </summary>
    public byte[] ImageHash { get; init; } = [];

    /// <summary>
    /// Gets the hash value of the global hash.
    /// </summary>
    public byte[] GlobalHash { get; init; } = [];

    /// <summary>
    /// Gets the hash value of the digital signature.
    /// </summary>
    public byte[]? DigitalSignature { get; init; } = [];
}
