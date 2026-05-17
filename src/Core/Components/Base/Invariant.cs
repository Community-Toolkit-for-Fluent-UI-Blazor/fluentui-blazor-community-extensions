using System.Globalization;

namespace FluentUI.Blazor.Community.Components.Components.Base;

internal static class Invariant
{
    public static string ToString<T>(this T value) where T : IFormattable
    {
        return value.ToString(null, CultureInfo.InvariantCulture);
    }
}
