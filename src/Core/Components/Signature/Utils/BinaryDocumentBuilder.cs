namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to build a binary document from a specified payload using export options.
/// </summary>
/// <typeparam name="TPayload">The type of the payload to be included in the binary document.</typeparam>
internal sealed class BinaryDocumentBuilder<TPayload>
    : IDocumentPayloadBuilder<TPayload>
{
    /// <inheritdoc />
    public async ValueTask<byte[]> BuildAsync(SurfacePayload<TPayload> payload, ExportOptions options)
    {
        return await BinaryUtils.WriteAsync(payload, options);
    }
}
