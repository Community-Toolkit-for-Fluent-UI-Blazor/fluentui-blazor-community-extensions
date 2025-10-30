namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies the rendering mode for an audio player.
/// </summary>
/// <remarks>The rendering mode determines the layout and appearance of the audio player. Use the appropriate mode
/// based on the desired user interface and available screen space.</remarks>
public enum AudioPlayerView
{
    /// <summary>
    /// Represents the normal rendering mode of the audio player, which includes all standard controls and features.
    /// </summary>
    Default,

    /// <summary>
    /// Represents the compact rendering mode of the audio player, which provides a smaller layout with essential controls only.
    /// </summary>
    Compact,

    /// <summary>
    /// Represents the floating rendering mode of the audio player, which allows the player to be positioned independently of other content.
    /// </summary>
    Floating,

    /// <summary>
    /// Represents the minimal rendering mode of the audio player, which offers a very simplified interface with only the most basic controls.
    /// </summary>
    Minimal
}
