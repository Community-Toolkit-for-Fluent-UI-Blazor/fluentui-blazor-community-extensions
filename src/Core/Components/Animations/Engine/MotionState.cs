namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the state of a motion animation, allowing you to store and retrieve custom properties related to the animation's progress and effects.
/// </summary>
public sealed class MotionState
{
    /// <summary>
    /// Represents the payload containing motion-related data for this instance.
    /// </summary>
    private readonly MotionPayload _payload = new();

    /// <summary>
    /// Initializes a new instance of the MotionState class with an empty payload.
    /// </summary>
    public MotionState() { }

    private MotionState(MotionPayload payload)
    {
        _payload = payload;
    }

    /// <summary>
    /// Gets a collection containing the keys of the payload.
    /// </summary>
    public ICollection<string> Keys => _payload.Keys;

    /// <summary>
    /// Gets or sets the X-coordinate value.
    /// </summary>
    public double X
    {
        get => Get<double>("x");
        set => Set("x", value);
    }

    /// <summary>
    /// Gets or sets the Y-coordinate value.
    /// </summary>
    public double Y
    {
        get => Get<double>("y");
        set => Set("y", value);
    }

    /// <summary>
    /// Gets or sets the horizontal scaling factor applied to the element.
    /// </summary>
    public double ScaleX
    {
        get => GetOrDefault("sX", 1.0);
        set => Set("sX", value);
    }

    /// <summary>
    /// Gets or sets the vertical scaling factor applied to the element.
    /// </summary>
    public double ScaleY
    {
        get => GetOrDefault("sY", 1.0);
        set => Set("sY", value);
    }

    /// <summary>
    /// Gets or sets the uniform scale factor applied to both the X and Y scales.
    /// </summary>
    public double Scale
    {
        get => (ScaleX + ScaleY) * 0.5;
        set
        {
            ScaleX = value;
            ScaleY = value;
        }
    }

    /// <summary>
    /// Gets or sets the opacity level of the element, where 0 represents fully transparent and 1 represents fully opaque.
    /// </summary>
    public double Opacity
    {
        get => GetOrDefault("o", 1.0);
        set => Set("o", value);
    }

    /// <summary>
    /// Gets or sets the rotation angle, in degrees, applied to the element.
    /// </summary>
    public double Rotation
    {
        get => Get<double>("r");
        set => Set("r", value);
    }

    /// <summary>
    /// Retrieves the value associated with the specified key and attempts to cast it to the specified type.
    /// </summary>
    /// <typeparam name="T">The type to which the value should be cast.</typeparam>
    /// <param name="key">The key whose associated value is to be retrieved. Cannot be null.</param>
    /// <returns>The value associated with the specified key, cast to type T, or null if the key does not exist or the value
    /// cannot be cast.</returns>
    private T? Get<T>(string key) => _payload.Get<T>(key);

    /// <summary>
    /// Adds or updates the value associated with the specified key in the payload.
    /// </summary>
    /// <typeparam name="T">The type of the value to store.</typeparam>
    /// <param name="key">The key with which the value will be associated. Cannot be null.</param>
    /// <param name="value">The value to associate with the specified key.</param>
    public void Set<T>(string key, T value) => _payload.Set(key, value);

    /// <summary>
    /// Retrieves the value associated with the specified key, or returns a default value if the key is not found.
    /// </summary>
    /// <typeparam name="T">The type of the value to retrieve.</typeparam>
    /// <param name="key">The key whose associated value is to be retrieved.</param>
    /// <param name="defaultValue">The value to return if the specified key does not exist or its value is null.</param>
    /// <returns>The value associated with the specified key if found; otherwise, the specified default value.</returns>
    public T GetOrDefault<T>(string key, T defaultValue = default!)
    {
        if (!_payload.Contains(key))
        {
            return defaultValue;
        }

        var v = _payload.Get<T>(key);

        return v is null ? defaultValue : v;
    }

    /// <summary>
    /// Creates a new object that is a copy of the current MotionState instance.
    /// </summary>
    /// <returns>A new MotionState object that is a copy of this instance.</returns>
    public MotionState Clone()
    {
        return new MotionState(_payload.Clone());
    }

    /// <summary>
    /// Resets the internal state by clearing all stored payload data.
    /// </summary>
    internal void Reset()
    {
        _payload.Clear();
    }
}
