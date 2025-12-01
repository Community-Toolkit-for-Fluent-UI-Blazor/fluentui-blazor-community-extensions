namespace FluentUI.Blazor.Community.Extensions;

/// <summary>
/// Provides extension methods for working with enumeration (enum) values.
/// </summary>
/// <remarks>This static class contains utility methods that extend the functionality of enum types, enabling more
/// expressive and convenient operations when handling enumerations.</remarks>
public static class EnumExtensions
{
    /// <summary>
    /// Determines whether the specified enum value matches any value in the provided list.
    /// </summary>
    /// <remarks>This method provides a convenient way to check if an enum value is one of several specified
    /// values. The comparison uses the default equality comparer for the enum type.</remarks>
    /// <typeparam name="T">The enum type to compare.</typeparam>
    /// <param name="value">The enum value to check for a match.</param>
    /// <param name="values">An array of enum values to compare against. Can contain zero or more values.</param>
    /// <returns>true if the specified value is equal to any value in the list; otherwise, false.</returns>
    public static bool IsOneOf<T>(this T value, params T[] values) where T : Enum
    {
        return values.Any(x => Equals(value, x));
    }
}
