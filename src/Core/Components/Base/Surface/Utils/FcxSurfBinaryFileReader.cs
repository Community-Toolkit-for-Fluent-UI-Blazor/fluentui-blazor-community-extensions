using System.Security.Cryptography;
using System.Text;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the content and metadata associated with a digitally signed document, including image and document data,
/// signature proof, and cryptographic hashes.
/// </summary>
/// <remarks>This record is intended for scenarios involving digital signatures, including advanced and qualified
/// electronic signatures. It encapsulates all relevant data required for signature verification and
/// compliance.</remarks>
/// <typeparam name="TUserData">The type of user-specific data included in the signature proof payload.</typeparam>
/// <param name="ImageBytes">The image data, as a byte array, associated with the signed document. Typically used for visual representation or
/// preview.</param>
/// <param name="DocumentBytes">The original document data, as a byte array, that is being signed.</param>
/// <param name="Proof">The signature proof payload containing user-specific information and evidence of the signature.</param>
/// <param name="GlobalHash">The global hash of the document, used for integrity verification.</param>
/// <param name="EncryptedHash">The encrypted hash of the document, or null if the hash is not encrypted.</param>
/// <param name="IsEncrypted">Indicates whether the document hash is encrypted. Set to <see langword="true"/> if the hash is encrypted; otherwise,
/// <see langword="false"/>.</param>
/// <param name="IsSigned">Indicates whether the document has been signed. Set to <see langword="true"/> if the document is signed; otherwise,
/// <see langword="false"/>.</param>
/// <param name="IsQes">Indicates whether the signature is a qualified electronic signature (QES). Set to <see langword="true"/> if the
/// signature is QES; otherwise, <see langword="false"/>.</param>
public sealed record FcxSurfContent<TUserData>(
    byte[] ImageBytes,
    byte[] DocumentBytes,
    SurfaceProofPayload<TUserData> Proof,
    byte[] GlobalHash,
    byte[]? EncryptedHash,
    bool IsEncrypted,
    bool IsSigned,
    bool IsQes);

/// <summary>
/// Provides functionality to read and parse FCXSIGN files, extracting signature content, associated document data, and
/// cryptographic proof information for a specified user data type.
/// </summary>
/// <remarks>This class supports both encrypted and unencrypted FCXSIGN files. For encrypted files, an AES
/// decryption delegate must be provided to successfully extract the content. The parsed result includes image data,
/// document data, cryptographic hashes, and signature proof details. The class is sealed and cannot be
/// inherited.</remarks>
/// <typeparam name="TUserData">The type of user data included in the signature proof payload. This type parameter allows the signature proof to
/// carry custom user-specific information.</typeparam>
public sealed class FcxSurfBinaryFileReader<TUserData>
{
    /// <summary>
    /// Represents a constant magic number used to identify a specific file format or data structure.
    /// </summary>
    /// <remarks>This value is typically used to validate or recognize data by checking for a known signature
    /// at the beginning of a file or memory block. The exact meaning and usage depend on the context in which this
    /// constant is applied.</remarks>
    private const ulong MagicNumber = 0x004E474953584346;

    /// <summary>
    /// Determines whether the specified byte array represents a valid file according to the expected format and version
    /// requirements.
    /// </summary>
    /// <remarks>This method checks for a specific magic number, version, and validates the integrity of
    /// internal data ranges. It returns false if the file does not meet the expected format or if any validation step
    /// fails.</remarks>
    /// <param name="fileBytes">The byte array containing the file data to validate. Must not be null and should contain at least the minimum
    /// required header length.</param>
    /// <returns>true if the byte array is recognized as a valid file format and passes all structural checks; otherwise, false.</returns>
    private static bool IsValid(byte[] fileBytes)
    {
        try
        {
            if (fileBytes.Length < 16 + 48)
            {
                return false;
            }

            using var ms = new MemoryStream(fileBytes);
            using var br = new BinaryReader(ms);

            var magic = br.ReadUInt64();

            if (magic != MagicNumber)
            {
                return false;
            }

            var major = br.ReadByte();
            var minor = br.ReadByte();

            if (major != 1 || minor != 0)
            {
                return false;
            }

            var flags = br.ReadByte();
            br.ReadBytes(5);

            var imageOffset = br.ReadInt64();
            var imageLength = br.ReadInt64();
            var docOffset = br.ReadInt64();
            var docLength = br.ReadInt64();
            var proofOffset = br.ReadInt64();
            var proofLength = br.ReadInt64();

            if (!IsRangeValid(imageOffset, imageLength, fileBytes.Length))
            {
                return false;
            }

            if (!IsRangeValid(docOffset, docLength, fileBytes.Length))
            {
                return false;
            }

            if (!IsRangeValid(proofOffset, proofLength, fileBytes.Length))
            {
                return false;
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Determines whether the specified range, defined by an offset and length, is valid within a total size.
    /// </summary>
    /// <param name="offset">The zero-based starting position of the range. Must be greater than or equal to 0.</param>
    /// <param name="length">The length of the range. Must be greater than or equal to 0.</param>
    /// <param name="total">The total size within which the range must fit. The sum of offset and length must not exceed this value.</param>
    /// <returns>true if the range is valid and fits within the total size; otherwise, false.</returns>
    private static bool IsRangeValid(long offset, long length, int total)
    {
        if (offset < 0 ||
            length < 0 ||
            offset + length > total)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Attempts to validate the specified input string and indicates whether the validation was successful.
    /// </summary>
    /// <param name="input">The input string to validate. Cannot be null.</param>
    /// <returns>true if the input is valid; otherwise, false.</returns>
    public bool TryValidate(string input)
    {
        var fileBytes = Encoding.UTF8.GetBytes(input);

        return IsValid(fileBytes);
    }

    /// <summary>
    /// Asynchronously reads and parses an FCXSIGN file from the specified byte array, optionally decrypting its
    /// contents if encrypted.
    /// </summary>
    /// <remarks>If the file is encrypted, the aesDecrypt delegate is used to decrypt the file's payload
    /// before parsing. The method verifies the file's integrity and format before returning the parsed
    /// content.</remarks>
    /// <param name="fileBytes">The byte array containing the contents of the FCXSIGN file to read.</param>
    /// <param name="aesDecrypt">An optional delegate used to decrypt the file contents if the file is encrypted. If the file is encrypted, this
    /// delegate must be provided; otherwise, an exception is thrown.</param>
    /// <param name="qesExecutor">An optional delegate that executes specific logic for qualified electronic signatures (QES) if the signature is identified as a QES.</param>
    /// <returns>A task that represents the asynchronous read operation. The task result contains a parsed FcxSignContent object
    /// representing the file's contents.</returns>
    /// <exception cref="InvalidDataException">Thrown if the file format is invalid, such as when the magic number does not match or the encrypted payload is
    /// malformed.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the file is encrypted and the aesDecrypt delegate is not provided.</exception>
    public async ValueTask<FcxSurfContent<TUserData>> ReadAsync(
        byte[] fileBytes,
        Func<byte[], ValueTask<byte[]>>? aesDecrypt = null,
        Func<SurfaceProofPayload<TUserData>, ValueTask>? qesExecutor = null)
    {
        using var ms = new MemoryStream(fileBytes);
        using var br = new BinaryReader(ms);

        // HEADER
        var magic = br.ReadUInt64();

        if (magic != MagicNumber)
        {
            throw new InvalidDataException("Invalid FCXSIGN magic number.");
        }

        var major = br.ReadByte();
        var minor = br.ReadByte();
        var flags = br.ReadByte();
        br.ReadBytes(5); // reserved

        var isEncrypted = (flags & 0b00000001) != 0;
        var isSigned = (flags & 0b00000010) != 0;
        var isQes = (flags & 0b00000100) != 0;

        if (isEncrypted)
        {
            if (aesDecrypt is null)
            {
                throw new InvalidOperationException("AES decryption delegate is required for encrypted FCXSIGN files.");
            }

            // Hash of the crypted file.
            var encryptedHash = SHA256.HashData(fileBytes);

            // Read IV + remaining payload (ciphertext + HMAC)
            var iv = br.ReadBytes(16);
            var remaining = br.ReadBytes((int)(ms.Length - ms.Position));

            // Split remaining into ciphertext and HMAC (last 32 bytes)
            if (remaining.Length < 32)
            {
                throw new InvalidDataException("Invalid encrypted FCXSIGN payload.");
            }

            var cipherText = remaining.AsSpan(0, remaining.Length - 32).ToArray();
            var hmac = remaining.AsSpan(remaining.Length - 32, 32).ToArray();

            // Rebuild the block for the decrypter: [IV | CIPHERTEXT | HMAC]
            var encryptedBlock = new byte[iv.Length + cipherText.Length + hmac.Length];
            Buffer.BlockCopy(iv, 0, encryptedBlock, 0, iv.Length);
            Buffer.BlockCopy(cipherText, 0, encryptedBlock, iv.Length, cipherText.Length);
            Buffer.BlockCopy(hmac, 0, encryptedBlock, iv.Length + cipherText.Length, hmac.Length);

            var clearBytes = await aesDecrypt(encryptedBlock);

            // Hash of the clear file (base of the signature)
            var clearHash = SHA256.HashData(clearBytes);

            return await ReadClearAsync(
                clearBytes,
                clearHash,
                encryptedHash,
                isEncrypted: true,
                isSigned: isSigned,
                isQes: isQes,
                qesExecutor);
        }
        else
        {
            // Clear file: the clear hash = hash of the file
            var clearHash = SHA256.HashData(fileBytes);

            return await ReadClearAsync(
                fileBytes,
                clearHash,
                encryptedHash: null,
                isEncrypted: false,
                isSigned: isSigned,
                isQes: isQes,
                qesExecutor);
        }
    }

    /// <summary>
    /// Asynchronously reads and parses a clear-signed content structure from the specified file bytes, extracting
    /// image, document, and proof data along with signature metadata.
    /// </summary>
    /// <remarks>The method expects the input byte array to follow a specific binary format, including header
    /// and indexed sections for image, document, and proof data. The caller is responsible for ensuring the input
    /// conforms to this structure.</remarks>
    /// <param name="fileBytes">The byte array containing the file data to be parsed. Must include the expected header and content structure.</param>
    /// <param name="clearHash">The hash of the clear (unencrypted) content. Used to verify the integrity of the clear data.</param>
    /// <param name="encryptedHash">The hash of the encrypted content, if available. May be null if the content is not encrypted.</param>
    /// <param name="isEncrypted">true if the content is encrypted; otherwise, false.</param>
    /// <param name="isSigned">true if the content is signed; otherwise, false.</param>
    /// <param name="isQes">true if the signature is a qualified electronic signature (QES); otherwise, false.</param>
    /// <param name="qesExecutor">An optional delegate that executes QES-specific logic if the signature is a qualified electronic signature.</param>
    /// <returns>A ValueTask that represents the asynchronous operation. The result contains an FcxSignContent instance with the
    /// extracted image, document, proof, and signature metadata.</returns>
    private static async ValueTask<FcxSurfContent<TUserData>> ReadClearAsync(
        byte[] fileBytes,
        byte[] clearHash,
        byte[]? encryptedHash,
        bool isEncrypted,
        bool isSigned,
        bool isQes,
        Func<SurfaceProofPayload<TUserData>, ValueTask>? qesExecutor)
    {
        using var ms = new MemoryStream(fileBytes);
        using var br = new BinaryReader(ms);

        // Skip header (16 bytes)
        ms.Position = 16;

        // INDEX
        var imageOffset = br.ReadInt64();
        var imageLength = br.ReadInt64();
        var docOffset = br.ReadInt64();
        var docLength = br.ReadInt64();
        var proofOffset = br.ReadInt64();
        var proofLength = br.ReadInt64();

        // IMAGE
        ms.Position = imageOffset;
        var imageBytes = br.ReadBytes((int)imageLength);

        // DOCUMENT
        ms.Position = docOffset;
        var documentBytes = br.ReadBytes((int)docLength);

        // PROOF
        ms.Position = proofOffset;
        var proofBytes = br.ReadBytes((int)proofLength);

        var proof = await JsonUtils.ReadAsync<SurfaceProofPayload<TUserData>>(proofBytes);

        if (isQes)
        {
            if (qesExecutor is null)
            {
                throw new InvalidOperationException("QES execution delegate is required for QES signatures.");
            }

            await qesExecutor(proof!);
        }

        return new FcxSurfContent<TUserData>(
            imageBytes,
            documentBytes,
            proof!,
            clearHash,
            encryptedHash,
            isEncrypted,
            isSigned,
            isQes);
    }
}
