namespace FluentUI.Blazor.Community.Helpers;

/// <summary>
/// Represents a helper class for path operations.
/// </summary>
internal static class PathHelper
{
    /// <summary>
    /// Gets the segments of the specified path.
    /// </summary>
    /// <param name="path">Path to segment.</param>
    /// <returns>Returns an array of <see cref="string"/> which represent the segments of the path.</returns>
    public static string[] GetSegments(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return [];
        }

        return path.Trim().Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.RemoveEmptyEntries);
    }
}
