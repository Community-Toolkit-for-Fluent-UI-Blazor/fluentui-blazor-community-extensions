namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the converted recorded audio.
/// </summary>
/// <param name="AudioData">Converted audio data.</param>
/// <param name="ContentType">New content type of the audio.</param>
public record RecordedAudio(byte[] AudioData, string ContentType)
{
}
