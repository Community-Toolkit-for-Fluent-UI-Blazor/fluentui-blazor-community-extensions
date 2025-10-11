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
}
