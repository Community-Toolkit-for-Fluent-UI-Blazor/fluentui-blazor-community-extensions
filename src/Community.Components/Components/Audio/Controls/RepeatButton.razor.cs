using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a button that allows users to cycle through different audio repeat modes.
/// </summary>
public partial class RepeatButton : FluentComponentBase
{
    /// <summary>
    /// Represents the icon for the "Single Once" repeat mode.
    /// </summary>
    private static readonly Icon SingleOnceIcon = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size24.ArrowRepeatAllOff();

    /// <summary>
    /// Represents the icon for the "Single Loop" repeat mode.
    /// </summary>
    private static readonly Icon SingleLoopIcon = new Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size24.ArrowRepeat1();

    /// <summary>
    /// Represents the icon for the "Playlist Once" repeat mode.
    /// </summary>
    private static readonly Icon PlaylistOnceIcon = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size24.ArrowRepeatAll();

    /// <summary>
    /// Represents the icon for the "Playlist Loop" repeat mode.
    /// </summary>
    private static readonly Icon PlaylistLoopIcon = new Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size24.ArrowSync();

    /// <summary>
    /// Represents the current repeat mode of the audio player.
    /// </summary>
    private AudioRepeatMode _repeatMode = AudioRepeatMode.SingleOnce;

    /// <summary>
    /// Initializes a new instance of the <see cref="RepeatButton"/> class.
    /// </summary>
    public RepeatButton()
    {
        Id = $"repeat-button-{Identifier.NewId()}";
    }

    /// <summary>
    /// Gets the icon corresponding to the current repeat mode.
    /// </summary>
    private Icon RepeatIcon { get; set; } = SingleOnceIcon;

    /// <summary>
    /// Gets or sets the event callback that is invoked when the repeat mode changes.
    /// </summary>
    [Parameter]
    public EventCallback<AudioRepeatMode> OnRepeatModeChanged { get; set; }

    /// <summary>
    /// Gets or sets the label for the "Single Once" repeat mode.
    /// </summary>
    [Parameter]
    public string SingleOnceLabel { get; set; } = "Play Single Once";

    /// <summary>
    /// Gets or sets the label for the "Single Loop" repeat mode.
    /// </summary>
    [Parameter]
    public string SingleLoopLabel { get; set; } = "Repeat Single";

    /// <summary>
    /// Gets or sets the label for the "Playlist Once" repeat mode.
    /// </summary>
    [Parameter]
    public string PlaylistOnceLabel { get; set; } = "Play Playlist Once";

    /// <summary>
    /// Gets or sets the label for the "Playlist Loop" repeat mode.
    /// </summary>
    [Parameter]
    public string PlaylistLoopLabel { get; set; } = "Repeat Playlist";

    /// <summary>
    /// Gets the label corresponding to the current repeat mode.
    /// </summary>
    private string RepeatLabel => _repeatMode switch
    {
        AudioRepeatMode.SingleOnce => SingleOnceLabel ?? "Play Single Once",
        AudioRepeatMode.SingleLoop => SingleLoopLabel ?? "Repeat Single",
        AudioRepeatMode.PlaylistOnce => PlaylistOnceLabel ?? "Play Playlist Once",
        AudioRepeatMode.PlaylistLoop => PlaylistLoopLabel ?? "Repeat Playlist",
        _ => SingleOnceLabel ?? "Play Single Once"
    };

    /// <summary>
    /// Cycles through the available audio repeat modes in a predefined order.
    /// </summary>
    /// <remarks>The repeat modes transition in the following sequence:  <see
    /// cref="AudioRepeatMode.SingleOnce"/> → <see cref="AudioRepeatMode.SingleLoop"/> →  <see
    /// cref="AudioRepeatMode.PlaylistOnce"/> → <see cref="AudioRepeatMode.PlaylistLoop"/> →  <see
    /// cref="AudioRepeatMode.SingleOnce"/>. After updating the repeat mode, the component's state is
    /// refreshed.</remarks>
    private async Task OnChangeRepeatModeAsync()
    {
        _repeatMode = _repeatMode switch
        {
            AudioRepeatMode.SingleOnce => AudioRepeatMode.SingleLoop,
            AudioRepeatMode.SingleLoop => AudioRepeatMode.PlaylistOnce,
            AudioRepeatMode.PlaylistOnce => AudioRepeatMode.PlaylistLoop,
            AudioRepeatMode.PlaylistLoop => AudioRepeatMode.SingleOnce,
            _ => AudioRepeatMode.SingleOnce
        };

        RepeatIcon = _repeatMode switch
        {
            AudioRepeatMode.SingleOnce => SingleOnceIcon,
            AudioRepeatMode.SingleLoop => SingleLoopIcon,
            AudioRepeatMode.PlaylistOnce => PlaylistOnceIcon,
            AudioRepeatMode.PlaylistLoop => PlaylistLoopIcon,
            _ => SingleOnceIcon
        };

        if (OnRepeatModeChanged.HasDelegate)
        {
            await OnRepeatModeChanged.InvokeAsync(_repeatMode);
        }
    }
}
