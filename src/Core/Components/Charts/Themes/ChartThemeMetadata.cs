namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Represents the metadata information for a chart theme, including its name, author, version, description, tags, and source.
/// </summary>
public sealed record ChartThemeMetadata
{
    /// <summary>
    /// Gets the name of the theme.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Gets the name of the author of the theme.
    /// </summary>
    public string? Author { get; init; }

    /// <summary>
    /// Gets the version of the theme.
    /// </summary>
    public string Version { get; init; } = "1.0";

    /// <summary>
    /// Gets the description of the theme.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Gets the tags metadata for the theme.
    /// </summary>
    public IReadOnlyList<string> Tags { get; init; } = [];

    /// <summary>
    /// Gets the source or location of the theme.
    /// </summary>
    public string? Source { get; init; }
}
