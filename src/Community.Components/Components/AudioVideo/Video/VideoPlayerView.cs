namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the rendering mode for a video player.
/// </summary>
public enum VideoPlayerView
{
    /// <summary>
    /// Represents the normal rendering mode of the video player, which includes all standard controls and features.
    /// </summary>
    Default,

    /// <summary>
    /// Represents the fullscreen rendering mode of the video player, which expands the video to occupy the entire screen.
    /// </summary>
    Fullscreen,

    /// <summary>
    /// Represents the compact rendering mode of the video player, which provides a smaller layout with essential controls only.
    /// </summary>
    Compact,

    /// <summary>
    /// Represents the floating rendering mode of the video player, which allows the player to be positioned independently of other content.
    /// </summary>
    Floating,

    /// <summary>
    /// Represents the minimal rendering mode of the video player, which includes only the most basic controls and features.
    /// </summary>
    Minimal
}
