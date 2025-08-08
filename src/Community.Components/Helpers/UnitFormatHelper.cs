using System.Text.RegularExpressions;

namespace FluentUI.Blazor.Community.Helpers;

public static class UnitFormatHelper
{
    private const string pattern = "auto|%|px|vh|em|vw|rem|in|cm|fr|mm|pt|pc|ch|ex|ms|deg|s|rad|grad|vmin|vmax";

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

        return Regex.IsMatch(formattedValue, pattern) ? formattedValue : $"{formattedValue}px";
    }
}
