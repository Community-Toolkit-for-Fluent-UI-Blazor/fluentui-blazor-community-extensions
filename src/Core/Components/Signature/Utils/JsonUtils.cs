using System.Text.Json;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides utility methods for serializing and deserializing surface payloads to and from JSON format using predefined
/// serialization options.
/// </summary>
/// <remarks>This class is intended for internal use to facilitate consistent JSON serialization and
/// deserialization of surface-related data structures. All methods are static and thread-safe.</remarks>
internal static class JsonUtils
{
    /// <summary>
    /// Provides default JSON serialization options with indented output and camel case property naming.
    /// </summary>
    /// <remarks>These options can be used to ensure consistent JSON formatting and property naming
    /// conventions across serialization and deserialization operations.</remarks>
    private static readonly JsonSerializerOptions s_jsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    /// <summary>
    /// Asynchronously serializes the specified payload to a JSON byte array according to the provided export options.
    /// </summary>
    /// <typeparam name="TPayload">The type of the content contained within the payload.</typeparam>
    /// <param name="payload">The payload containing the data to be serialized.</param>
    /// <param name="options">The options that determine which elements of the payload are included in the serialized output.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a byte array with the serialized
    /// JSON data.</returns>
    internal static async ValueTask<byte[]> WriteAsync<TPayload>(
        SurfacePayload<TPayload> payload,
        ExportOptions options)
    {
        var jsonObject = new Dictionary<string, object?>();

        if (options.IncludeView)
        {
            jsonObject["view"] = payload.View;
        }

        if (options.IncludeBackground)
        {
            jsonObject["background"] = payload.Background;
        }

        if (options.IncludeGrid)
        {
            jsonObject["grid"] = payload.Grid;
        }

        if (options.IncludeAxes)
        {
            jsonObject["axes"] = payload.Axes;
        }

        jsonObject["content"] = payload.Content;

        if (options.IncludeWatermark)
        {
            jsonObject["watermark"] = payload.Watermark;
        }

        using var ms = new MemoryStream();
        await JsonSerializer.SerializeAsync(ms, jsonObject, s_jsonOptions).ConfigureAwait(false);

        return ms.ToArray();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="payload"></param>
    /// <returns></returns>
    internal static async ValueTask<byte[]> WriteAsync<T>(T payload)
    {
        using var ms = new MemoryStream();
        await JsonSerializer.SerializeAsync(ms, payload, s_jsonOptions).ConfigureAwait(false);

        return ms.ToArray();
    }

    /// <summary>
    /// Asynchronously reads a JSON payload from the specified stream and deserializes it into a SurfacePayload object.
    /// </summary>
    /// <remarks>The method expects the JSON to contain properties such as 'view', 'background', 'grid',
    /// 'axes', 'content', and 'watermark'. Only the properties present in the JSON will be deserialized and assigned to
    /// the corresponding members of the SurfacePayload object.</remarks>
    /// <typeparam name="TPayload">The type of the content payload to deserialize from the JSON data.</typeparam>
    /// <param name="stream">The stream containing the JSON data to read and deserialize. The stream must be readable and positioned at the
    /// start of the JSON payload.</param>
    /// <returns>A ValueTask that represents the asynchronous read operation. The result contains a SurfacePayload object
    /// populated with the deserialized data.</returns>
    internal static async ValueTask<SurfacePayload<TPayload>> ReadAsync<TPayload>(Stream stream)
    {
        using var doc = await JsonDocument.ParseAsync(stream).ConfigureAwait(false);
        var root = doc.RootElement;

        var payload = new SurfacePayload<TPayload>();

        if (root.TryGetProperty("view", out var viewProp))
        {
            payload.View = viewProp.Deserialize<ViewPayload>(s_jsonOptions);
        }

        if (root.TryGetProperty("background", out var bgProp))
        {
            payload.Background = bgProp.Deserialize<BackgroundPayload>(s_jsonOptions);
        }

        if (root.TryGetProperty("grid", out var gridProp))
        {
            payload.Grid = gridProp.Deserialize<GridPayload>(s_jsonOptions);
        }

        if (root.TryGetProperty("axes", out var axesProp))
        {
            payload.Axes = axesProp.Deserialize<AxesPayload>(s_jsonOptions);
        }

        if (root.TryGetProperty("content", out var contentProp))
        {
            payload.Content = contentProp.Deserialize<TPayload>(s_jsonOptions);
        }

        if (root.TryGetProperty("watermark", out var wmProp))
        {
            payload.Watermark = wmProp.Deserialize<WatermarkPayload>(s_jsonOptions);
        }

        return payload;
    }

    /// <summary>
    /// Asynchronously deserializes a signature proof payload of the specified type from a byte array containing JSON
    /// data.
    /// </summary>
    /// <typeparam name="T">The type of the payload to deserialize from the JSON data.</typeparam>
    /// <param name="proofBytes">A byte array containing the JSON-encoded signature proof payload.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a deserialized instance of
    /// SignatureProofPayload{T} if successful; otherwise, null.</returns>
    internal static async ValueTask<T?> ReadAsync<T>(byte[] proofBytes)
    {
        using var stream = new MemoryStream(proofBytes);

        return await JsonSerializer.DeserializeAsync<T>(stream, s_jsonOptions);
    }
}
