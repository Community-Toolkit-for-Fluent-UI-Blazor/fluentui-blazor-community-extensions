namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines a contract for building a binary export of a surface payload using specified export options.
/// </summary>
/// <typeparam name="TPayload">The type of the payload data contained within the surface payload to be exported.</typeparam>
public interface IDocumentPayloadBuilder<TPayload>
{
    /// <summary>
    /// Generates a binary representation of the specified surface payload using the provided export options.
    /// </summary>
    /// <param name="payload">The surface payload to be exported. Cannot be null.</param>
    /// <param name="options">The export options that determine the format and settings for the generated binary data. Cannot be null.</param>
    /// <returns>A byte array containing the exported binary data. The array will be empty if the payload contains no exportable
    /// content.</returns>
    ValueTask<byte[]> BuildAsync(SurfacePayload<TPayload> payload, ExportOptions options);
}
