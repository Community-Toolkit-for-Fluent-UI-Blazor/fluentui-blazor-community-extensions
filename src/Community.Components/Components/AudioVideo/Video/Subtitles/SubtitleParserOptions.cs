namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies options for controlling subtitle parsing behavior, including formatting, line merging, normalization, and
/// text constraints.
/// </summary>
/// <remarks>Use this record to customize how subtitles are parsed and processed. Adjust the options to match the
/// requirements of your target playback system or user preferences.</remarks>
/// <param name="KeepFormating">Indicates whether to preserve original subtitle formatting, such as italics or bold. Set to <see langword="true"/>
/// to retain formatting; otherwise, formatting will be removed.</param>
/// <param name="MergeLines">Indicates whether consecutive subtitle lines should be merged into a single line. Set to <see langword="true"/> to
/// combine lines; otherwise, lines remain separate.</param>
/// <param name="Normalization">Specifies the type of text normalization to apply to subtitle content. Use <see cref="TextNormalization.None"/> to
/// disable normalization.</param>
/// <param name="MaxLineLength">The maximum allowed length, in characters, for each subtitle line. Set to 0 to disable line length restriction.</param>
/// <param name="MaxCharsPerSecond">The maximum number of characters per second permitted in subtitle text. Set to 0 to disable this constraint.</param>
public record SubtitleParserOptions(bool KeepFormating = true,
                                    bool MergeLines = true,
                                    TextNormalization Normalization = TextNormalization.None,
                                    int MaxLineLength = 0,
                                    int MaxCharsPerSecond = 0)
{
}
