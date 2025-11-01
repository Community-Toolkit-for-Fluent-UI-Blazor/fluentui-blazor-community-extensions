namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the state of subtitle management, including the currently selected subtitle language.
/// </summary>
internal sealed class VideoState
{
    /// <summary>
    /// Gets or sets the options used to configure subtitle display and behavior.
    /// </summary>
    public SubtitleOptions SubtitleOptions { get; set; } = new SubtitleOptions();

    /// <summary>
    /// Gets or sets the index of the currently selected quality option.
    /// </summary>
    /// <remarks>A value of -1 indicates that default quality option is selected.</remarks>
    public int SelectedQuality { get; set; } = -1;
}
