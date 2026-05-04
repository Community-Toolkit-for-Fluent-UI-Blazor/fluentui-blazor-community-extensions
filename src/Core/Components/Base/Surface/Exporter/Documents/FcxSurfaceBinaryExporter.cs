namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to export surface payloads to a binary format using configurable image and document
/// exporters, supporting user data and proof document generation.
/// </summary>
/// <remarks>This exporter coordinates multiple export strategies, allowing for flexible export scenarios that may
/// include images, documents, and proof data. It is designed for asynchronous operations and can be extended with
/// custom payload and user data types.</remarks>
/// <typeparam name="TUserData">The type of user data associated with the proof document during export.</typeparam>
/// <typeparam name="TPayload">The type of the payload data contained in the surface to be exported.</typeparam>
/// <param name="conformity">The signature conformity level that dictates the signing and encryption requirements for the exported data.</param>
/// <param name="encryptionKey">An optional encryption key used for AES conformity to encrypt the exported data. Must be provided if AES conformity is required.</param>
/// <param name="userData">The user-specific data to be included in the proof document during export, allowing for personalized information to be embedded in the exported file.</param>
/// <param name="imageExporter">The image exporter responsible for exporting surface images in the desired format.</param>
/// <param name="documentExporter">The document exporter used to generate and export document representations of the surface payload.</param>
/// <param name="proofDocumentBuilder">The builder that creates proof documents incorporating the payload and user data during export.</param>
public sealed class FcxSurfaceBinaryExporter<TUserData, TPayload>(
    TUserData userData,
    SignatureConformity conformity,
    ReadOnlyMemory<byte>? encryptionKey,
    SurfaceImageExporterBase<TPayload> imageExporter,
    DocumentExporterBase<TPayload> documentExporter,
    ProofDocumentBuilder<TPayload, TUserData> proofDocumentBuilder)
    : ISurfaceExporter<TPayload>
{
    /// <summary>
    /// Asynchronously exports the specified surface payload using the provided export options.
    /// </summary>
    /// <param name="fileName">The name of the file to export to, or null to use a default name.</param>
    /// <param name="payload">The surface payload containing the data to be exported.</param>
    /// <param name="options">The options that configure the export operation.</param>
    /// <returns>A ValueTask that represents the asynchronous export operation. The result contains the outcome of the export.</returns>
    public async ValueTask<ExportResult> ExportAsync(
        string? fileName,
        SurfacePayload<TPayload> payload,
        SurfaceExportOptions options)
    {
        var image = await imageExporter.ExportAsync(fileName, payload, options);
        var document = await documentExporter.ExportAsync(fileName, payload, options);
        var payloadData = await JsonUtils.WriteAsync(payload, options);

        var proofDocument = await proofDocumentBuilder.BuildAsync(
            image.Data,
            document.Data,
            payloadData,
            userData,
            conformity,
            encryptionKey);

        return new ExportResult(fileName ?? "signedDocument.fcxsurface", "application/octet-stream", proofDocument);
    }
}
