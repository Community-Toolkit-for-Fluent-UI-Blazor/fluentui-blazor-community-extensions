namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Represents the state of the audio recorder.
/// </summary>
public enum RecorderState
{
    /// <summary>
    /// The audio recorder is idle and not recording.
    /// </summary>
    Idle,

    /// <summary>
    /// The audio recorder is currently recording audio.
    /// </summary>
    Recording,

    /// <summary>
    /// The audio recorder is processing the recorded audio data.
    /// </summary>
    Processing,

    /// <summary>
    /// The audio recording has been completed and is ready for further actions (e.g., saving or sending).
    /// </summary>
    Completed
}
