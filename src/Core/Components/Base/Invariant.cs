using System.Globalization;

namespace FluentUI.Blazor.Community.Components.Components.Base;

/// <summary>
/// Provides extension methods for culture-invariant string formatting.
/// </summary>
/// <remarks>Use these methods to ensure consistent string representations regardless of the current culture
/// settings.</remarks>
internal static class Invariant
{
    /// <summary>
    /// Converts the value to its string representation using invariant culture formatting.
    /// </summary>
    /// <typeparam name="T">The type of the value that implements <see cref="IFormattable"/>.</typeparam>
    /// <param name="value">The value to convert.</param>
    /// <returns>A string representation of the value formatted using <see cref="CultureInfo.InvariantCulture"/>.</returns>
    public static string ToString<T>(this T value) where T : IFormattable
    {
        return value.ToString(null, CultureInfo.InvariantCulture);
    }
}
