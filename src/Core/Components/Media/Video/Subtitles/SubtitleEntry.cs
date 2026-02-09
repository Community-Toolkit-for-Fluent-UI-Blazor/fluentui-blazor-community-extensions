namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a single subtitle entry with timing and text information.
/// </summary>
public record SubtitleEntry
{
    /// <summary>
    /// Gets the index of the subtitle entry.
    /// </summary>
    public long Index { get; init; }

    /// <summary>
    /// Gets the start time of the subtitle entry in seconds.
    /// </summary>
    public double Start { get; init; }

    /// <summary>
    /// Gets the end time of the subtitle entry in seconds.
    /// </summary>
    public double End { get; init; }

    /// <summary>
    /// Gets the text of the subtitle entry.
    /// </summary>
    public string Text { get; init; } = string.Empty;
}
