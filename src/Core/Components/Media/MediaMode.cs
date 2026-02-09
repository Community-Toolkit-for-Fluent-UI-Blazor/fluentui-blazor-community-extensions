namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies the mode of media playback, either audio or video.
/// </summary>
/// <remarks>Use this enumeration to determine the type of media being processed or played. The selected mode
/// influences the behavior of media-related operations.</remarks>
public enum MediaMode
{
    /// <summary>
    /// Represents audio data and provides methods for audio playback and manipulation.
    /// </summary>
    /// <remarks>This class may include methods for loading audio files, controlling playback, and adjusting
    /// audio properties such as volume and pitch. Ensure that audio files are in a supported format for proper
    /// playback.</remarks>
    Audio,

    /// <summary>
    /// Represents a video file and provides properties and methods to manage playback and access metadata.
    /// </summary>
    /// <remarks>This class enables loading, playing, pausing, and stopping video playback. It also exposes
    /// metadata such as duration, resolution, and format, allowing developers to retrieve information about the video
    /// content. Use this class to integrate video functionality into applications that require media playback or
    /// metadata inspection.</remarks>
    Video
}
