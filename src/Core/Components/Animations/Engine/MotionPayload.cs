namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a container for storing and retrieving key-value pairs associated with a motion operation.
/// </summary>
/// <remarks>This class is used to encapsulate arbitrary data related to a motion, allowing values to be set and
/// retrieved by string keys. It is intended for internal use within motion-related components and is not
/// thread-safe.</remarks>
internal sealed class MotionPayload
{
    /// <summary>
    /// Represents the internal storage for key-value pairs. The keys are strings, and the values can be of any type.
    /// </summary>
    private readonly Dictionary<string, object?> _values = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Initializes a new instance of the MotionPayload class.
    /// </summary>
    public MotionPayload() { }

    /// <summary>
    /// Initialise une nouvelle instance de la classe MotionPayload avec les paires clé/valeur spécifiées.
    /// </summary>
    /// <param name="values">Le dictionnaire contenant les paires clé/valeur à utiliser pour initialiser le payload. Les clés sont comparées
    /// sans tenir compte de la casse. Ne peut pas être null.</param>
    private MotionPayload(Dictionary<string, object?> values)
    {
        _values = new Dictionary<string, object?>(values, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Gets a collection containing the keys in the dictionary.
    /// </summary>
    public ICollection<string> Keys => _values.Keys;

    /// <summary>
    /// Retrieves the value associated with the specified key and attempts to cast it to the specified type.
    /// </summary>
    /// <typeparam name="T">The type to which the stored value is cast.</typeparam>
    /// <param name="key">The key whose associated value is to be retrieved.</param>
    /// <returns>The value associated with the specified key cast to type T, or the default value of T if the key does not exist
    /// or the value cannot be cast.</returns>
    internal T? Get<T>(string key)
    {
        if (_values.TryGetValue(key, out var value) && value is T typed)
        {
            return typed;
        }

        return default;
    }

    /// <summary>
    /// Sets the value associated with the specified key. If the key already exists, its value is updated; otherwise, a
    /// new key-value pair is added.
    /// </summary>
    /// <typeparam name="T">The type of the value to associate with the specified key.</typeparam>
    /// <param name="key">The key with which the value will be associated. Cannot be null.</param>
    /// <param name="value">The value to set for the specified key.</param>
    internal void Set<T>(string key, T value) => _values[key] = value;

    /// <summary>
    /// Checks if the payload contains a value associated with the specified key.
    /// </summary>
    /// <param name="key">The key to check for existence in the payload.</param>
    /// <returns>Returns <see langword="true"/> if the payload contains a value for the specified key; otherwise, <see langword="false"/>.</returns>"
    internal bool Contains(string key) => _values.ContainsKey(key);

    /// <summary>
    /// Creates a new instance of the MotionPayload class that is a copy of the current instance.
    /// </summary>
    /// <returns>A new MotionPayload object with the same values as the current instance.</returns>
    internal MotionPayload Clone()
    {
        return new MotionPayload(_values);
    }

    /// <summary>
    /// Removes all elements from the collection.
    /// </summary>
    internal void Clear()
    {
        _values.Clear();
    }
}

