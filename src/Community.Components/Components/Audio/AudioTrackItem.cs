using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an audio track with properties for title, artist, source URL, and cover URL.
/// </summary>
public sealed class AudioTrackItem : ComponentBase, IDisposable
{
    /// <summary>
    /// Gets or sets the title of the audio track.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the artist of the audio track.
    /// </summary>
    [Parameter]
    public string? Artist { get; set; }

    /// <summary>
    /// Gets or sets the source URL of the audio track.
    /// </summary>
    [Parameter]
    public string? Source { get; set; }

    /// <summary>
    /// Gets or sets the cover URL of the audio track.
    /// </summary>
    [Parameter]
    public string? Cover { get; set; }

    /// <summary>
    /// Gets or sets the parent <see cref="FluentCxAudio"/> component.
    /// </summary>
    [CascadingParameter]
    private FluentCxAudio? Parent { get; set; }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Parent?.AddTrack(this);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Parent?.RemoveTrack(this);
    }   
}
