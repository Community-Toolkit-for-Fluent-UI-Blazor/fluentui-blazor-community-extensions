using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a minimal video player component with customizable play and pause labels and play/pause event handling.
/// </summary>
public partial class MinimalVideoPlayer
{
    /// <summary>
    /// Reference to the underlying video element.
    /// </summary>
    private Video? _videoReference;

    /// <summary>
    /// Initializes a new instance of the <see cref="MinimalVideoPlayer"/> class with a unique identifier.
    /// </summary>
    public MinimalVideoPlayer()
    {
        Id = $"minimal-video-player-{Identifier.NewId()}";
    }

    /// <summary>
    /// Gets or sets the event callback that is triggered when the play/pause button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<bool> OnPlayPause { get; set; }

    /// <summary>
    /// Gets or sets the label for the play button.
    /// </summary>
    [Parameter]
    public string PlayLabel { get; set; } = "Play";

    /// <summary>
    /// Gets or sets the label for the pause button.
    /// </summary>
    [Parameter]
    public string PauseLabel { get; set; } = "Pause";

    /// <summary>
    /// Gets or sets the callback that is invoked when the video element is ready for interaction.
    /// </summary>
    /// <remarks>Use this callback to perform actions when the underlying video element has been initialized
    /// and is available for manipulation. The callback receives an <see cref="ElementReference"/> to the video element,
    /// which can be used for JavaScript interop or direct DOM operations.</remarks>
    [Parameter]
    public EventCallback<ElementReference> VideoReady { get; set; }

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
