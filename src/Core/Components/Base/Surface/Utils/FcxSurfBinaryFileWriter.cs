using System.Security.Cryptography;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to build and sign FCXSIGN binary files, including support for embedding user data and
/// generating a global hash.
/// </summary>
/// <typeparam name="TUserData">The type of user-defined data to include in the signature payload. This allows custom metadata or information to be
/// associated with the signed file.</typeparam>
internal class FcxSurfBinaryFileWriter<TUserData>
{
    /// <summary>
    /// Represents the result of a file signing operation, including the signed file bytes and the computed global hash.
    /// </summary>
    /// <param name="FileBytes">The byte array containing the signed file data.</param>
    /// <param name="GlobalHash">The byte array representing the global hash value computed during the signing process.</param>
    public sealed record FcxSurfResult(
        byte[] FileBytes,
        byte[] GlobalHash);

    /// <summary>
    /// Represents the magic number identifier used to validate or recognize a specific file format or data structure.
    /// </summary>
    /// <remarks>This constant can be used to verify the integrity or type of data by comparing it against the
    /// expected magic number value.</remarks>
    private const ulong MagicNumber = 0x004E474953584346;

    /// <summary>
    /// Builds the FCXSIGN binary file (header + index + sections).
    /// </summary>
    internal static async ValueTask<FcxSurfResult> BuildAsync(
        byte[] imageBytes,
        byte[] documentBytes,
        byte[] payloadHash,
        byte[] documentHash,
        byte[] imageHash,
        TUserData userData,
        bool isQes = false,
        byte[]? globalHash = null,
        byte[]? digitalSignature = null)
    {
        var proofBytes = await JsonUtils.WriteAsync(new SurfaceProofPayload<TUserData>
        {
            UserData = userData,
            PayloadHash = payloadHash,
            DocumentHash = documentHash,
            ImageHash = imageHash,
            GlobalHash = globalHash ?? [],
            DigitalSignature = digitalSignature

        });
        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms);

        // HEADER (16 bytes)
        bw.Write(MagicNumber);
        bw.Write((byte)1);

        bw.Write((byte)0);

        byte flags = 0;

        if (digitalSignature != null)
        {
            flags |= 0b00000010;
        }

        if (isQes)
        {
            flags |= 0b00000100;
        }

        bw.Write(flags);
        bw.Write(new byte[5]); // reserved

        // INDEX placeholder
        var indexPos = ms.Position;
        bw.Write(0L);
        bw.Write(0L);
        bw.Write(0L);
        bw.Write(0L);
        bw.Write(0L);
        bw.Write(0L);

        var imageOffset = ms.Position;
        bw.Write(imageBytes);

        var imageLength = ms.Position - imageOffset;
        var docOffset = ms.Position;

        bw.Write(documentBytes);

        var docLength = ms.Position - docOffset;
        var proofOffset = ms.Position;
        bw.Write(proofBytes);

        var proofLength = ms.Position - proofOffset;

        bw.Flush();

        ms.Position = indexPos;
        bw.Write(imageOffset);

        bw.Write(imageLength);
        bw.Write(docOffset);
        bw.Write(docLength);
        bw.Write(proofOffset);
        bw.Write(proofLength);
        bw.Flush();

        var final = ms.ToArray();

        return new(final, SHA256.HashData(final));
    }

    /// <summary>
    /// Wraps an AES-encrypted block into a valid FCXSIGN file.
    /// </summary>
    internal static FcxSurfResult WrapEncrypted(
        byte[] iv,
        byte[] cipherText,
        byte[] hmac,
        bool isSigned,
        bool isQes)
    {
        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms);

        // HEADER (16 bytes)
        bw.Write(MagicNumber);
        bw.Write((byte)1); // major
        bw.Write((byte)0); // minor

        byte flags = 0b00000001; // AES encrypted

        if (isSigned)
        {
            flags |= 0b00000010;
        }

        if (isQes)
        {
            flags |= 0b00000100;
        }

        bw.Write(flags);

        bw.Write(new byte[5]); // reserved

        // BODY
        bw.Write(iv);
        bw.Write(cipherText);
        bw.Write(hmac);

        bw.Flush();

        var final = ms.ToArray();
        var globalHash = SHA256.HashData(final);

        return new(final, globalHash);
    }
}
