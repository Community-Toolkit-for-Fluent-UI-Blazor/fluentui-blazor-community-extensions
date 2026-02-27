namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides methods for comparing payload objects for equality using reference and value semantics.
/// </summary>
public static class PayloadComparer
{
    /// <summary>
    /// Determines whether two payload objects are equal by first checking for reference equality,
    ///  then nullability, and finally using value equality if both objects are non-null.
    /// </summary>
    /// <typeparam name="T">The type of the payload objects, which must be a reference type and implement IEquatable&lt;T&gt;.</typeparam>
    /// <param name="a">First payload object to compare.</param>
    /// <param name="b">Second payload object to compare.</param>
    /// <returns>Returns <see langword="true" /> if the payloads are considered equal; otherwise, <see langword="false" />.</returns>
    public static bool AreEqual<T>(T? a, T? b)
        where T : class, IEquatable<T>
    {
        if (ReferenceEquals(a, b))
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }
}

