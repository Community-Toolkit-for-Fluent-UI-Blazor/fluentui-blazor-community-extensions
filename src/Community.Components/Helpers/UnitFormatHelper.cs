using System.Text.RegularExpressions;

namespace FluentUI.Blazor.Community.Helpers;

/// <summary>
/// Represents a helper class for formatting CSS unit values.
/// </summary>
public static class UnitFormatHelper
{
    /// <summary>
    /// Represents a regex pattern to match valid CSS unit values.
    /// </summary>
    private const string pattern = "auto|%|px|vh|em|vw|rem|in|cm|fr|mm|pt|pc|ch|ex|ms|deg|s|rad|grad|vmin|vmax";

    /// <summary>
    /// Formats the specified value as a CSS unit string.
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    /// <param name="value">Value to format.</param>
    /// <returns>Returns the formatted value as a CSS unit string.</returns>
    public static string Format<T>(T value)
    {
        if (value is null)
        {
            return string.Empty;
        }

        var formattedValue = value.ToString();

        if (string.IsNullOrWhiteSpace(formattedValue))
        {
            return string.Empty;
        }

        var match = Regex.IsMatch(formattedValue, pattern);

        return match ? formattedValue : $"{formattedValue}px";
    }
}
