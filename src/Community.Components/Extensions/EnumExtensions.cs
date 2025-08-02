namespace FluentUI.Blazor.Community.Extensions;

public static class EnumExtensions
{
    public static bool IsOneOf<T>(this T value, params T[] values) where T : Enum
    {
        return values.Any(x => Equals(value, x));
    }
}
