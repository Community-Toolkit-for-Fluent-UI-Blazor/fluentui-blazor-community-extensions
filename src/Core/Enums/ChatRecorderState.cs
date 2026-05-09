namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// The state of the chat recorder.
/// </summary>
public enum ChatRecorderState
{
    /// <summary>
    /// Idle state. No recording is in progress.
    /// </summary>
    Idle,

    /// <summary>
    /// Recording state. The recorder is actively capturing audio input.
    /// </summary>
    Recording,

    /// <summary>
    /// Processing state. The recorded audio is being processed or prepared for further actions.
    /// </summary>
    Processing,

    /// <summary>
    /// Completed state. The recording process has finished, and the audio data is ready for use.
    /// </summary>
    Completed
}
