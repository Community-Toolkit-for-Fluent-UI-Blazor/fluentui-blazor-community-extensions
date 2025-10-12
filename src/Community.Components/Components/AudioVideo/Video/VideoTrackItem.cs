using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an video track with properties for title, artist, source URL, and cover URL.
/// </summary>
public sealed class VideoTrackItem : ComponentBase, IAsyncDisposable
{
    /// <summary>
    /// Gets or sets the source URL of the audio track.
    /// </summary>
    [Parameter]
    public string? Source { get; set; }

    /// <summary>
    /// Gets or sets the audio metadata associated with the track.
    /// </summary>
    [Parameter]
    public VideoMetadata? Metadata { get; set; }

    /// <summary>
    /// Gets or sets the chapters associated with the track.
    /// </summary>
    [Parameter]
    public List<Chapter> Chapters { get; set; } = [];

    /// <summary>
    /// Gets or sets the collection of video media sources to be played.
    /// </summary>
    /// <remarks>
    /// When multiple sources are provided, the parameter <see cref="Source"/> is used as the default or fallback source.
    /// 
    /// </remarks>
    [Parameter]
    public List<VideoMediaSource> Sources { get; set; } = [];

    /// <summary>
    /// Gets or sets the parent of this track item.
    /// </summary>
    [CascadingParameter]
    private FluentCxVideo? Parent { get; set; }

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (Parent is not null)
        {
            await Parent.AddTrackAsync(this);
        }
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (Parent is not null)
        {
            await Parent.RemoveTrackAsync(this);
        }
    }

    /// <summary>
    /// Retrieves the source URL corresponding to the specified quality, or a default source if no quality is selected.
    /// </summary>
    /// <param name="selectedQuality">The quality identifier for which to retrieve the source URL. Specify -1 to select the default or first available
    /// source.</param>
    /// <returns>A string containing the source URL for the specified quality, or the default source URL if no matching quality
    /// is found. Returns null if no sources are available.</returns>
    internal string? GetSource(int selectedQuality)
    {
        if (selectedQuality == -1)
        {
            return Source ?? Sources.FirstOrDefault()?.SourceUrl;

        }
        else
        {
            return Sources.FirstOrDefault(x => x.Quality == selectedQuality)?.SourceUrl ?? Source;
        }
    }
}
