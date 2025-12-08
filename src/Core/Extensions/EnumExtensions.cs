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
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> matches any element in <paramref name="values"/>; otherwise, <see langword="false"/>.
    /// If no <paramref name="values"/> are supplied the method returns <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// var result = myEnumValue.IsOneOf(MyEnum.OptionA, MyEnum.OptionB);
    /// </code>
    /// </example>
    public static bool IsOneOf<T>(this T value, params T[] values) where T : Enum
    {
        return values.Any(x => Equals(value, x));
    }
}
