namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the event args for a recorded audio.
/// </summary>
/// <param name="OriginalData">Original data of the audio.</param>
public record RecordedAudioEventArgs(byte[] OriginalData)
{
    /// <summary>
    /// Gets or sets the converted audio data.
    /// </summary>
    public RecordedAudio? Audio { get; set; }
}
