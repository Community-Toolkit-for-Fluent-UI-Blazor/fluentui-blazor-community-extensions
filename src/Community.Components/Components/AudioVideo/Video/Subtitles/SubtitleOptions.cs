namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents options for configuring subtitle display settings.
/// </summary>
public record SubtitleOptions
{
    /// <summary>
    /// Gets or sets the background style for subtitles.
    /// </summary>
    public SubtitleBackground Background { get; set; } = SubtitleBackground.Solid;

    /// <summary>
    /// Gets the background color to use, specified as a hexadecimal color code.
    /// </summary>
    /// <remarks>The color code should be in the format "#RRGGBB" or "#AARRGGBB". If null, a default color may
    /// be used by the consumer.</remarks>
    public string? BackgroundColor { get; set; } = "#000000";
}
