namespace FluentUI.Blazor.Community.Extensions;

/// <summary>
/// Provides extension methods for working with enum values.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Determines whether the specified enum <paramref name="value"/> equals any of the provided <paramref name="values"/>.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="value">The enum instance to test.</param>
    /// <param name="values">A list of candidate values to compare against.</param>
    public static bool IsOneOf<T>(this T value, params T[] values) where T : Enum
    {
        return values.Any(x => Equals(value, x));
    }
}
