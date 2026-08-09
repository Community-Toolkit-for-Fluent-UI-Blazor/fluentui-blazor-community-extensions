namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a collection of key-value pairs used to define a set of motion variant properties.
/// </summary>
/// <remarks>Use this class to store and retrieve strongly-typed values associated with string keys, typically for
/// configuring motion or animation behaviors in UI components. The class provides type-safe methods for setting and
/// retrieving values, and supports method chaining for fluent configuration.</remarks>
public sealed class MotionVariant
{
    /// <summary>
    /// Represents the values for the variant.
    /// </summary>
    private readonly Dictionary<string, object?> _values = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Gets a read-only dictionary containing the current set of values associated with this instance.
    /// </summary>
    public IReadOnlyDictionary<string, object?> Values => _values;

    /// <summary>
    /// Sets the value associated with the specified key, using the provided generic type.
    /// </summary>
    /// <remarks>If the key already exists, its value is overwritten. This method supports fluent
    /// configuration by returning the same instance.</remarks>
    /// <typeparam name="T">The type of the value to associate with the specified key.</typeparam>
    /// <param name="key">The key with which the value will be associated. Cannot be null.</param>
    /// <param name="value">The value to set for the specified key.</param>
    /// <returns>The current instance of the MotionVariant, enabling method chaining.</returns>
    public MotionVariant Set<T>(string key, T value)
    {
        _values[key] = value;

        return this;
    }

    /// <summary>
    /// Attempts to retrieve the value associated with the specified key and cast it to the specified type.
    /// </summary>
    /// <remarks>Use this method to safely attempt retrieval and type conversion of a value without throwing
    /// an exception if the key is not present or the value is of a different type.</remarks>
    /// <typeparam name="T">The type to which the value should be cast if found.</typeparam>
    /// <param name="key">The key whose associated value is to be retrieved.</param>
    /// <param name="value">When this method returns, contains the value associated with the specified key, cast to type <typeparamref
    /// name="T" />, if the key is found and the value is of the correct type; otherwise, the default value for type
    /// <typeparamref name="T" />. This parameter is passed uninitialized.</param>
    /// <returns><see langword="true"/> if the key exists and the value can be cast to type <typeparamref name="T"/>; otherwise,
    /// <see langword="false"/>.</returns>
    public bool TryGet<T>(string key, out T? value)
    {
        if (_values.TryGetValue(key, out var obj) && obj is T cast)
        {
            value = cast;
            return true;
        }

        value = default;
        return false;
    }
}
