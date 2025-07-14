namespace FluentUI.Blazor.Community.Helpers;

internal static class PathHelper
{
    public static string[] GetSegments(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return [];
        }

        return path.Trim().Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.RemoveEmptyEntries);
    }
}
