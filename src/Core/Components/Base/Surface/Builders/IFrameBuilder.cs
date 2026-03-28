namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines methods for associating and retrieving payloads by key within a layer.
/// </summary>
/// <remarks>Implementations of this interface allow clients to store and access typed payloads using unique
/// string keys. Payloads are expected to implement the ILayerPayload interface. If a key is reused, the existing
/// payload is replaced. Retrieval returns null if the key is not present or if the payload cannot be cast to the
/// requested type.</remarks>
public interface IFrameBuilder
{
    /// <summary>
    /// Gets a read-only collection of payloads associated with the current layer, indexed by their string keys.
    /// </summary>
    /// <remarks>Each entry in the dictionary represents a named payload that can be accessed by its key. The
    /// returned dictionary is read-only and cannot be modified directly.</remarks>
    IReadOnlyDictionary<string, object> Payloads { get; }

    /// <summary>
    /// Associates the specified payload with the given key in the layer.
    /// </summary>
    /// <remarks>If a payload is already associated with the specified key, it will be replaced by the new
    /// payload.</remarks>
    /// <param name="key">The unique identifier for the payload to set. Cannot be null.</param>
    /// <param name="payload">The payload to associate with the specified key. Cannot be null.</param>
    void Set(string key, ILayerPayload payload);

    /// <summary>
    /// Retrieves the payload associated with the specified key, if it exists.
    /// </summary>
    /// <typeparam name="TPayload">The type of the payload to retrieve. Must implement the ILayerPayload interface.</typeparam>
    /// <param name="key">The key that identifies the payload to retrieve. Cannot be null.</param>
    /// <returns>The payload of type TPayload associated with the specified key, or null if no such payload exists.</returns>
    TPayload? Get<TPayload>(string key) where TPayload : class, ILayerPayload;

    /// <summary>
    /// Clears all payloads from the layer, removing all key-payload associations.
    /// </summary>
    void Clear();
}
