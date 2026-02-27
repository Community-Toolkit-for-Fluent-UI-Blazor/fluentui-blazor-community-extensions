namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to build a JSON-encoded payload from a surface payload object of the specified type.
/// </summary>
/// <remarks>This builder serializes the payload using the default settings of the System.Text.Json serializer.
/// The resulting byte array represents the UTF-8 encoded JSON document. This class is intended for internal use and is
/// not thread-safe.</remarks>
/// <typeparam name="TPayload">The type of the payload data to be serialized into JSON.</typeparam>
internal sealed class JsonDocumentPayloadBuilder<TPayload>
    : IDocumentPayloadBuilder<TPayload>
{
    /// <inheritdoc />
    public ValueTask<byte[]> BuildAsync(
        SurfacePayload<TPayload> payload,
        ExportOptions options)
    {
        return JsonUtils.WriteAsync(payload, options);
    }
}
