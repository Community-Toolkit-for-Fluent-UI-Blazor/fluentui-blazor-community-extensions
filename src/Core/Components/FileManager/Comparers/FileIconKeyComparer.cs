namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides an equality comparer for tuples containing a string key and a FileView value, using case-insensitive
/// comparison for the key and exact comparison for the view.
/// </summary>
/// <remarks>This comparer is intended for use in collections or algorithms that require custom equality logic for
/// file icon keys, ensuring that string keys are compared without regard to case while FileView values are compared for
/// exact equality.</remarks>
internal sealed class FileIconKeyComparer : IEqualityComparer<(string Key, FileView View)>
{
    /// <summary>
    /// Provides a singleton instance of the FileIconKeyComparer for comparing file icon keys.
    /// </summary>
    /// <remarks>Use this instance to perform comparisons without creating additional comparer objects. This
    /// is useful for scenarios where a consistent comparison logic is required across the application.</remarks>
    public static readonly FileIconKeyComparer Instance = new();

    /// <inheritdoc />
    public bool Equals((string Key, FileView View) x, (string Key, FileView View) y)
    {
        return StringComparer.OrdinalIgnoreCase.Equals(x.Key, y.Key) &&
               x.View == y.View;
    }

    /// <inheritdoc />
    public int GetHashCode((string Key, FileView View) obj)
    {
        var h1 = StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Key);
        var h2 = obj.View.GetHashCode();

        return HashCode.Combine(h1, h2);
    }
}

