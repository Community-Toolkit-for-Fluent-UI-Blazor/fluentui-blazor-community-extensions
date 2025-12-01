using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a button component that toggles between play and pause states, displaying the appropriate icon and label
/// based on the current state.
/// </summary>
/// <remarks>Use this component to allow users to start or pause playback in a user interface. The button's label
/// and icon update automatically according to its state. You can customize the labels for play and pause states using
/// the <see cref="PlayLabel"/> and <see cref="PauseLabel"/> properties. To respond to state changes, handle the <see
/// cref="OnPlayChanged"/> event. The button can be disabled by setting <see cref="IsDisabled"/> to <see
/// langword="true"/>.</remarks>
public partial class PlayOrPauseButton
    : FluentComponentBase
{
    /// <summary>
    /// Represents the icon displayed when shuffling is disabled.
    /// </summary>
    private static readonly Icon PlayIcon = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size24.Play();

    /// <summary>
    /// Represents the icon displayed when shuffling is enabvled.
    /// </summary>
    private static readonly Icon PauseIcon = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size24.Pause();

    /// <summary>
    /// Indicates whether the collection is currently being shuffled.
    /// </summary>
    private bool _isPlaying;

    /// <summary>
    /// Gets or sets the event callback that is invoked when the play state changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> OnPlayChanged { get; set; }

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
    /// Gets the label for the current state of the button.
    /// </summary>
    private string PlayOrPauseLabel => _isPlaying ? PauseLabel ?? "Pause" : PlayLabel ?? "Play";

    /// <summary>
    /// Initializes a new instance of the <see cref="PlayOrPauseButton"/> class.
    /// </summary>
    public PlayOrPauseButton()
    {
        Id = $"play-pause-button-{Identifier.NewId()}";
    }

    /// <summary>
    /// Gets or sets a value indicating whether the button is disabled.
    /// </summary>
    [Parameter]
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Occurs when the shuffle button is clicked.
    /// </summary>
    /// <returns>Returns a task which toggles on / off the shuffle</returns>
    private async Task OnTogglePlayOrPauseAsync()
    {
        _isPlaying = !_isPlaying;

        if (OnPlayChanged.HasDelegate)
        {
            await OnPlayChanged.InvokeAsync(_isPlaying);
        }
    }

    /// <summary>
    /// Sets the play/pause state of the button.
    /// </summary>
    /// <param name="isPlaying">Value indicating if the button is in playing state.</param>
    internal void SetPlayPauseState(bool isPlaying)
    {
        _isPlaying = isPlaying;
        StateHasChanged();
    }
}
