namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to generate a unique integer identifier for a given payload type based on its name.
/// </summary>
/// <remarks>This class is intended for internal use to support type identification scenarios, such as
/// serialization or mapping. The generated identifier is consistent for the same type across application runs, provided
/// the type name does not change.</remarks>
internal static class PayloadTypeId
{
    /// <summary>
    /// Calculates a hash code based on the fully qualified name of the specified type parameter.
    /// </summary>
    /// <remarks>This method uses the FNV-1a hashing algorithm to generate a hash from the type's full name.
    /// If the full name is unavailable, the simple name is used instead. The hash can be used for type identification
    /// or as a key in scenarios where a unique value per type is required.</remarks>
    /// <typeparam name="T">The type whose name is used to generate the hash code.</typeparam>
    /// <returns>A 32-bit unsigned integer representing the hash code of the type's name.</returns>
    public static uint FromType<T>()
    {
        var name = typeof(T).FullName ?? typeof(T).Name;

        const uint fnvPrime = 16777619u;
        var hash = 2166136261u;

        foreach (var c in name)
        {
            hash ^= c;
            hash *= fnvPrime;
        }

        return hash;
    }
}
