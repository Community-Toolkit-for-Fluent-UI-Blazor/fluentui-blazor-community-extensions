using System.Text;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to import surfaces from FCX SURF binary files, supporting optional AES decryption and QES
/// signature execution.
/// </summary>
/// <remarks>This importer extends the base surface importer to handle FCX SURF binary files, allowing for custom
/// decryption and signature workflows as needed. The class is sealed and intended for scenarios where binary file
/// import with optional cryptographic operations is required.</remarks>
/// <typeparam name="TUserData">The type of user data associated with the surface proof payload.</typeparam>
/// <typeparam name="TPayload">The type of payload produced by the surface importer.</typeparam>
/// <param name="surfaceImporter">An instance of a surface importer used to process the payload from the binary file.</param>
/// <param name="aesDecrypt">An optional asynchronous function that decrypts the input byte array using AES. If null, decryption is not
/// performed.</param>
/// <param name="qesExecutor">An optional asynchronous function that executes a QES signature operation on the surface proof payload. If null,
/// signature execution is not performed.</param>
public sealed class FcxSurfBinaryFileImporter<TUserData, TPayload>(
    ISurfaceImporter<TPayload> surfaceImporter,
    Func<byte[], ValueTask<byte[]>>? aesDecrypt = null,
    Func<SurfaceProofPayload<TUserData>, ValueTask>? qesExecutor = null)
    : SurfaceImporter<TPayload>
{
    /// <summary>
    /// Provides access to the binary file reader used for reading surface data with user-defined metadata.
    /// </summary>
    /// <remarks>This field is initialized with a new instance of the generic binary file reader and is
    /// intended for internal use within the class to facilitate reading operations. The type parameter specifies the
    /// user data associated with each surface record.</remarks>
    private readonly FcxSurfBinaryFileReader<TUserData> _reader = new();

    /// <inheritdoc />
    public override bool IsValidInput(string input)
    {
        return _reader.TryValidate(input);
    }

    /// <inheritdoc />
    protected override async ValueTask<ImportSurfaceResult<TPayload>> ParseAsync(string input)
    {
        try
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            var content = await _reader.ReadAsync(bytes, aesDecrypt, qesExecutor);
            var document = Encoding.UTF8.GetString(content.DocumentBytes);

            return await surfaceImporter.ImportAsync(document);
        }
        catch (Exception ex)
        {
            return new ImportSurfaceResult<TPayload>(ErrorMessage: ex.Message);
        }
    }
}
