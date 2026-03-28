namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a builder for managing and retrieving layer payloads by key.
/// </summary>
/// <remarks>FrameBuilder enables storage and retrieval of objects implementing the ILayerPayload interface, using
/// string keys. It is typically used to associate and access payloads relevant to different layers or components within
/// a frame-based architecture. This class is sealed and not intended for inheritance.</remarks>
public sealed class FrameBuilder : IFrameBuilder
{
    /// <summary>
    /// Stores the payloads associated with each layer, keyed by layer name.
    /// </summary>
    private readonly Dictionary<string, object> _payloads = [];

    /// <inheritdoc />
    public IReadOnlyDictionary<string, object> Payloads => _payloads;

    /// <inheritdoc />
    public void Set(string key, ILayerPayload payload) => _payloads[key] = payload;

    /// <inheritdoc />
    public TPayload? Get<TPayload>(string key) where TPayload : class, ILayerPayload => _payloads.TryGetValue(key, out var value) ? value as TPayload : null;

    /// <inheritdoc />
    public void Clear()
    {
        _payloads.Clear();
    }
}
