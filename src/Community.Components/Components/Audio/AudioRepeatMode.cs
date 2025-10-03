namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies the repeat mode for audio playback.
/// </summary>
public enum AudioRepeatMode
{
    /// <summary>
    /// The track will play once and stop.
    /// </summary>
    SingleOnce,

    /// <summary>
    /// The track will loop continuously.
    /// </summary>
    SingleLoop,

    /// <summary>
    /// The playlist will play once and stop.
    /// </summary>
    PlaylistOnce,

    /// <summary>
    /// The playlist will loop continuously.
    /// </summary>
    PlaylistLoop
}
