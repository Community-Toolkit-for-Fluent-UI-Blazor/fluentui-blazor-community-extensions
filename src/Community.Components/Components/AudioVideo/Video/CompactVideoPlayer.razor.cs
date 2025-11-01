using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a compact audio player component that provides basic audio playback functionality.
/// </summary>
public sealed partial class CompactVideoPlayer
    : FluentComponentBase
{
    /// <summary>
    /// Value indicating whether to show the playback controls.
    /// </summary>
    private bool _showControls;

    /// <summary>
    /// Reference to the underlying video element.
    /// </summary>
    private Video? _videoReference;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompactVideoPlayer"/> class.
    /// </summary>
    public CompactVideoPlayer()
    {
        Id = $"compact-player-{Identifier.NewId()}";
    }

    /// <summary>
    /// Gets or sets the current audio track to be played.
    /// </summary>
    [Parameter]
    public VideoTrackItem? CurrentTrack { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the play/pause button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<bool> OnPlayPauseChanged { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the video element is ready for interaction.
    /// </summary>
    /// <remarks>Use this callback to perform actions when the underlying video element has been initialized
    /// and is available for manipulation. The callback receives an <see cref="ElementReference"/> to the video element,
    /// which can be used for JavaScript interop or direct DOM operations.</remarks>
    [Parameter]
    public EventCallback<ElementReference> VideoReady { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the previous button is disabled.
    /// </summary>
    [Parameter]
    public bool IsPreviousDisabled { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the previous button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnPrevious { get; set; }

    /// <summary>
    /// Gets or sets the label for the previous button.
    /// </summary>
    [Parameter]
    public string? PreviousLabel { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the next button is disabled.
    /// </summary>
    [Parameter]
    public bool IsNextDisabled { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the next button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnNext { get; set; }

    /// <summary>
    /// Gets or sets the label for the next button.
    /// </summary>
    [Parameter]
    public string? NextLabel { get; set; }

    /// <summary>
    /// Gets or sets the label for the play button.
    /// </summary>
    [Parameter]
    public string? PlayLabel { get; set; }

    /// <summary>
    /// Gets or sets the label for the pause button.
    /// </summary>
    [Parameter]
    public string? PauseLabel { get; set; }

    /// <summary>
    /// Gets or sets the total duration of the current track, in seconds.
    /// </summary>
    [Parameter]
    public double Duration { get; set; }

    /// <summary>
    /// Gets or sets the current playback time of the track, in seconds.
    /// </summary>
    [Parameter]
    public double CurrentTime { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the player is floating.
    /// </summary>
    [Parameter]
    public bool IsFloating { get; set; }

    /// <summary>
    /// Gets the style string that includes the opacity for the playback controls based on the current state.
    /// </summary>
    private string? InternalStyle => new StyleBuilder(Style)
        .AddStyle("--compact-video-player-controls-opacity", _showControls ? "1" : "0")
        .Build();

    /// <summary>
    /// Handles the pointer enter event by updating the component's state.
    /// </summary>
    /// <param name="e">The event arguments associated with the pointer enter event.</param>
    private void OnPointerEnter(PointerEventArgs e)
    {
        _showControls = true;
        StateHasChanged();
    }

    /// <summary>
    /// Handles the pointer leave event by updating the component's state.
    /// </summary>
    /// <param name="e">The event data associated with the pointer leave event.</param>
    private void OnPointerLeave(PointerEventArgs e)
    {
        _showControls = false;
        StateHasChanged();
    }

    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            if (VideoReady.HasDelegate && _videoReference is not null)
            {
                await VideoReady.InvokeAsync(_videoReference.Element);
            }
        }
    }
}

